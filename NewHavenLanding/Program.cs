using System.Collections.Generic;
using System.IO.Compression;
using Destructurama;
using FluentValidation;
using FluentValidation.AspNetCore;
using FormHelper;
using Joonasw.AspNetCore.SecurityHeaders;
using Marten;
using Marten.Services.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewHavenLanding.Models;
using NewHavenLanding.Models.Settings;
using NewHavenLanding.Services;
using NewHavenLanding.Validators;
using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using Serilog.Sinks.PostgreSQL.ColumnWriters;
using Weasel.Core;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.AspNetCore7;
using WebMarkupMin.NUglify;

var tableName = "logs";


IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase> {
    { "message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
    {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
    { "level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
    { "raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
    { "exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
    { "properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
    {"props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
    {"machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") }
};


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;

builder.Configuration.AddSecretsManager(configurator: options => {
    options.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}");
    options.KeyGenerator = (_, s) => s
        .Replace($"{env}_{appName}_", string.Empty)
        .Replace("__", ":");
});

services.AddWebMarkupMin(options => {
        options.AllowMinificationInDevelopmentEnvironment = true;
        options.AllowCompressionInDevelopmentEnvironment = true;
    })
    .AddHtmlMinification(options => {
        var settings = options.MinificationSettings;
        settings.RemoveRedundantAttributes = true;
        settings.RemoveHttpProtocolFromAttributes = true;
        settings.RemoveHttpsProtocolFromAttributes = true;

        options.CssMinifierFactory = new NUglifyCssMinifierFactory();
        options.JsMinifierFactory = new NUglifyJsMinifierFactory();
    })
    .AddHttpCompression(options => {
        options.CompressorFactories = new List<ICompressorFactory> {
            new BuiltInBrotliCompressorFactory(new BuiltInBrotliCompressionSettings {
                Level = CompressionLevel.Fastest
                
            }),
            new DeflateCompressorFactory(new DeflateCompressionSettings {
                Level = CompressionLevel.Fastest
            }),
            new GZipCompressorFactory(new GZipCompressionSettings {
                Level = CompressionLevel.Fastest
            })
        };
    })
    ;

builder.Services.AddResponseCaching();
builder.Services.AddCsp(nonceByteAmount: 32);
builder.Services.AddControllersWithViews().AddFormHelper(options => {
    options.EmbeddedFiles = true;
    options.ToastrDefaultPosition = ToastrPosition.TopRight;
});
builder.Services.Configure<DmsApiConfig>(builder.Configuration.GetSection(DmsApiConfig.Key));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(EmailSettings.Key));

builder.Services.AddMarten(options => {
    options.Connection(builder.Configuration.GetSection(DmsApiConfig.Key)["PostgresConnectionString"]);
   // options.Connection(connectionstring);
    options.AutoCreateSchemaObjects = AutoCreate.All;
    options.UseDefaultSerialization(
        serializerType: SerializerType.SystemTextJson,
        enumStorage: EnumStorage.AsString,
        casing: Casing.CamelCase
    );
    
}).OptimizeArtifactWorkflow().InitializeWith().UseLightweightSessions();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddTransient<IValidator<Customer>, CustomerValidator>();
builder.Services.AddTransient<IValidator<CustomerLoanInfo>, CustomerLoanInfoValidator>();
builder.Services.AddSingleton<IMartenService, MartenService>();
builder.Services.AddSingleton<IDmsLoggerService, DmsLoggerService>();

using var log = new LoggerConfiguration()
    .MinimumLevel.Error()
    .Enrich.FromLogContext()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Verbose)
    .Destructure.JsonNetTypes()
    .WriteTo.PostgreSQL(builder.Configuration.GetSection(DmsApiConfig.Key)["PostgresConnectionString"], tableName, columnWriters, needAutoCreateTable: true)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(log);

/*SelfLog.Enable(msg => {
    Debug.Print(msg);
    Debugger.Break();
});*/

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCsp(csp =>
{
    csp.AllowBaseUri
        .FromSelf();
    csp.AllowScripts
        .FromSelf()
        .AddNonce()
        .AllowUnsafeInline(); //Fallback for browsers that don't support nonce
    csp.AllowImages
        .FromSelf()
        .From("data:");
    csp.AllowStyles
        .FromSelf()
        .From("data:")
        .AddNonce()
        .AllowUnsafeInline(); //Fallback for browsers that don't support nonce
    csp.AllowFrames
        .FromNowhere(); 
    csp.AllowPlugins
        .FromNowhere();
    csp.AllowFraming
        .FromNowhere();
});

Log.Warning("Starting up the application");
Log.CloseAndFlush();
app.UseWebMarkupMin();

app.UseFormHelper();
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=2592000");
    }
});
app.UseResponseCaching();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();