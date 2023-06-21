using Marten;
using Marten.Linq;
using Marten.Services.Json;
using Microsoft.Extensions.Options;
using NewHavenLanding.Models;
using NewHavenLanding.Models.Settings;
using Newtonsoft.Json;
using Serilog.Core;
using Weasel.Core;

namespace NewHavenLanding.Services;

public class MartenService : IMartenService {
    private readonly DmsApiConfig _configuration;
    private readonly EmailSettings _emailSettings;
    private readonly Logger _logger;
    private readonly DocumentStore _store;
    
    public MartenService(IOptions<DmsApiConfig> dmsApiConfig, IOptions<EmailSettings> emailSettings, IDmsLoggerService dmsLoggerService, IDocumentStore documentStore) {
        _configuration = dmsApiConfig.Value;
        _emailSettings = emailSettings.Value;
        _logger = dmsLoggerService.Logger;
        _store = documentStore as DocumentStore;
        
        
        
       Console.WriteLine(JsonConvert.SerializeObject(_emailSettings) + JsonConvert.SerializeObject(_configuration));
        // _logger = //DmsLoggerService.GetLoggerConfig(_configuration.PostgresConnectionString, _emailSettings).CreateLogger();
        //_logger.Information("Email settings are {emailSettings}", JsonConvert.SerializeObject( _emailSettings));
        _logger.Information("dmsApiConfig settings are {dmsApiConfig}", JsonConvert.SerializeObject( _configuration));
        _logger.Information("PostgresService created");
        
    }
    public async Task<bool> CreatePromocode(Promocode promocode) {
        try {
           /* using var store = DocumentStore.For(opts => {
                opts.Connection(_configuration.PostgresConnectionString);
                
                opts.UseDefaultSerialization(
                    serializerType: SerializerType.SystemTextJson,
                    enumStorage: EnumStorage.AsString,
                    casing: Casing.CamelCase
                );
            });*/
           
            await using var session = _store.LightweightSession();
            session.Store(promocode);

            await session.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }catch(Exception ex) {
            _logger.Error("Unable to create customer {@ex} {@promocode}", ex, promocode);
            return false;
        }
    }
    public async Task<bool> CreateCustomer(Customer customer) {
        try {
            //using var store = DocumentStore.For(_configuration.PostgresConnectionString);
            await using var session = _store.LightweightSession();
            session.Store(customer);

            await session.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }catch(Exception ex) {
            _logger.Error("Unable to create customer {@ex} {@customer}", ex, customer);
            return false;
        }
    }
    
    public async Task<bool> CreateCustomerLoanInfo(CustomerLoanInfo customerLoanInfo) {
        try {
            //using var store = DocumentStore.For(_configuration.PostgresConnectionString);
            await using var session = _store.LightweightSession();
            session.Store(customerLoanInfo);

            await session.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }catch(Exception ex) {
            _logger.Error("Unable to create customerLoanInfo {@ex} {@customerLoanInfo}", ex, customerLoanInfo);
            return false;
        }
    }
}