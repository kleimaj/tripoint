using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using FluentValidation;
using FluentValidation.AspNetCore;
using FormHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using TriPoint.Models;
using TriPoint.Models.Const;
using TriPoint.Models.Jsn;
using TriPoint.Models.Settings;
using TriPoint.Services;

namespace TriPoint.Controllers;

public class ApplyController : Controller {
    private readonly ILogger<ApplyController> _logger;
    private readonly IMartenService _martenService;
    private readonly IValidator<Customer> _validator;
    private readonly EmailSettings _emailSettings;
    
    public ApplyController(IValidator<Customer> validator,  ILogger<ApplyController> logger,
        IMartenService martenService, IOptions<EmailSettings> emailSettings) {
        _validator = validator;
        _logger = logger;
        _martenService = martenService;
        _emailSettings = emailSettings.Value;
    }

    [HttpGet]
    public IActionResult Index() {
        return View();
    }

    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        _logger.LogError("{HttpContextTraceIdentifier}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    [FormValidator]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(Customer customer) {
        var response = new HttpResponseMessage();
        
        if (Request != null) {
            var form = await Request.ReadFormAsync();
            var token = form["cf-turnstile-response"];
            var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            customer.IpAddress = ip;
            var formData = new MultipartFormDataContent();
            const string SECRET_KEY = "0x4AAAAAAAIUXGJaJKd7EGHsvUK8FToQJk4";
            formData.Add(new StringContent(SECRET_KEY), "secret");
            formData.Add(new StringContent(token), "response");
            formData.Add(new StringContent(ip), "remoteip");

            var url = "https://challenges.cloudflare.com/turnstile/v0/siteverify";
            var client = new HttpClient();
            response = await client.PostAsJsonAsync(url, formData);
        }
        else {
           response = new HttpResponseMessage(HttpStatusCode.OK);
        }

        if (response.IsSuccessStatusCode) {
            var result = await _validator.ValidateAsync(customer);
            if (!result.IsValid) {
                result.AddToModelState(ModelState);
                return View("Index", customer);
            }
            
            try {
                postToCRM(customer);
            }
            catch (Exception e) {
                _logger.LogError("{HttpContextTraceIdentifier}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
            }       
            try {
                postToSalesforce(customer);
            }
            catch (Exception e) {
                _logger.LogError("{HttpContextTraceIdentifier}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
            }
            
            try {
                EmailContact(customer);
            }
            catch (Exception e) {
                _logger.LogError("{HttpContextTraceIdentifier}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
            }
            try {
                await _martenService.CreateCustomer(customer);
                
                return FormResult.CreateSuccessResult("Getting your Quote.",
                    Url.Action("", "Congratulations", new { customerGid = customer.Id }), 500);
            }
            catch {
                return FormResult.CreateErrorResult("An error occurred!");
            }
        }
        else {
            return FormResult.CreateErrorResult("Capatcha result failed!");
        }
    }
    
   /// <summary>
   ///  Post to CRM based on loan amount and offer code
   /// </summary>
   /// <param name="customer"></param>
   /// <exception cref="InvalidOperationException"></exception>
    internal void postToCRM(Customer customer) {
        var url = GetEndpointUrl(customer);
        
        var client = new RestClient(url);
        var request = new RestRequest($"") {
            Method = Method.Post
        };
      
        request.AddQueryParameter("first_name",customer.FirstName );
        request.AddQueryParameter("last_name", customer.LastName);
        request.AddQueryParameter("email", customer.Email);
      //  request.AddParameter("address", customer.Address);
        request.AddQueryParameter("cell_phone", customer.Cell);
      //  request.AddParameter("state", customer.State);
       // request.AddParameter("city", customer.City);
      //  request.AddParameter("zip_code", customer.Zip);
        request.AddQueryParameter("home_phone", customer.Phone);
        request.AddQueryParameter("loan_amount", customer.LoanAmount);
        request.AddQueryParameter("offer_code", string.IsNullOrEmpty(customer.Offer)?"None":customer.Offer);
       
        if (!String.IsNullOrEmpty(customer.Offer)) {
            var directMail =  _martenService.GetDirectMail(customer.Offer).Result;
            if (directMail != null) {
                request.AddQueryParameter("address", directMail.Address);
                request.AddQueryParameter("state", directMail.StateCode);
                request.AddQueryParameter("city", directMail.City);
                request.AddQueryParameter("zip_code", directMail.Zip);
            }
        }
        
        var posturl = client.BuildUri(request);
        _logger?.LogInformation(posturl.ToString());

        var response = client.Execute(request);
        if (response.IsSuccessful == false) throw new InvalidOperationException(response.ErrorMessage);
        
    }

    internal void postToSalesforce(Customer customer) {
        var url = TripointSalesforce.base_url;
        var tokenPath = TripointSalesforce.token_url;
        // request token from salesforce api
        var client = new RestClient(tokenPath);
        var request = new RestRequest("") {
            Method = Method.Post
        };
        request.AddParameter("grant_type", TripointSalesforce.grant_type);
        request.AddParameter("client_id", TripointSalesforce.client_id);
        request.AddParameter("client_secret", TripointSalesforce.client_secret);

        var token = client.Execute<Tpl2Salesforce.TripointSalesforceResponse>(request);
        token.Data = JsonConvert.DeserializeObject<Tpl2Salesforce.TripointSalesforceResponse>(token.Content);
        if (token.IsSuccessful == false) throw new InvalidOperationException(token.ErrorMessage);
        var accessToken = token.Data.AccessToken;
        // post to salesforce api
        client = new RestClient(url);
        request = new RestRequest("") {
            Method = Method.Post
        };
        request.AddHeader("Authorization", "Bearer " + accessToken);
        request.AddHeader("Content-Type", "application/json");
        var lead = new Tpl2Salesforce.Prospect {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            CellPhone = customer.Cell,
            HomePhone = customer.Phone,
            LoanAmount = customer.LoanAmount,
            Team = GetTeam(customer),
            OfferCode = string.IsNullOrEmpty(customer.Offer)?"None":customer.Offer
        };
        
        if (!String.IsNullOrEmpty(customer.Offer)) {
            var directMail =  _martenService.GetDirectMail(customer.Offer).Result;
            if (directMail != null) {
                //add directmail information to lead
                lead.Address = directMail.Address;
                lead.State = directMail.StateCode;
                lead.City = directMail.City;
                lead.ZipCode = directMail.Zip;
            }
        }
        var prospects = new Tpl2Salesforce.TripointProspects {
            Prospects = new[] {lead}
        };
        
        request.AddJsonBody(JsonConvert.SerializeObject(prospects));
        _logger.LogInformation("Posting lead to salesforce CRM: {@lead}", JsonConvert.SerializeObject(prospects));

        var leadResponse = client.Execute(request);
        _logger.LogInformation("Sending Lead: {leadResponse}", JsonConvert.SerializeObject(lead));
        if (leadResponse.IsSuccessful == false) {
            _logger.LogCritical("ERROR with Content: {leadResponse}", leadResponse.Content);
            _logger.LogCritical("ERROR with ErrorMessage: {leadResponse}", leadResponse.ErrorMessage);
            _logger.LogCritical("ERROR with ResponseStatus: {leadResponse}", leadResponse.ResponseStatus);
            throw new InvalidOperationException(leadResponse.ErrorMessage);
        }
        else {
            _logger.LogInformation("LeadResponse Content: {leadResponseId}", leadResponse.Content);
            _logger.LogInformation("LeadResponse ResponseStatus: {leadResponseId}", leadResponse.ResponseStatus);
            _logger.LogInformation("LeadResponse StatusCode: {leadResponseId}", leadResponse.StatusCode);
            _logger.LogInformation("LeadResponse ErrorMessage: {leadResponseId}", leadResponse.ErrorMessage);
        }

    }
     private string GetTeam(Customer customer) {
            var team = "TPL - Website";
           // anything under 20k goes to CLA
           //remove potential special characters from customer loan amount
           var loanAmt = customer.LoanAmount.Replace("$", "").Replace(",", "");
           _logger.LogInformation("Loan Amount: {loanAmt}", loanAmt);
           if (decimal.TryParse(loanAmt, out var loanAmount)) {
               if (loanAmount < 20000) {
                   _logger.LogInformation("Loan Amount is less than 20k");
                   team = "CLA - Website";
               }
               // everything else goes to TPL
               else {
                   _logger.LogInformation("Loan Amount is greater than 20k");
               }
           }
           else {
               _logger.LogError("Unable to parse loan amount, defaulting to tplEndpoint");
           }
   
           // if offer is not null then check if offer code belongs to cla or tpl
           if (!string.IsNullOrEmpty(customer.Offer)) {
               // if cla file then cla
               var directMail = _martenService.GetDirectMail(customer.Offer).Result;
               if (directMail != null) {
                   if (!string.IsNullOrEmpty(directMail.Team)) { 
                       if (directMail.Team.ToUpper().Contains("CLA")) {
                           team = "CLA - Website";
                       }
                       // if tpl file then tpl
                       else if (directMail.Team.ToUpper().Contains("TPL")) {
                           team = "TPL - Website";
                       }
                   }
               }
               else {
                   _logger?.LogError($"Promocode used but not found in direct mail table {customer.Offer}");
               }
           }
           _logger.LogInformation("Team: {team}");
           return team;
       }


   
    /// <summary>
    ///   Get endpoint url based on loan amount and offer code
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private string GetEndpointUrl(Customer customer) {
        var loanAmtList = LoanAmountList.loanAmtList;
        string url;
        var claEndpoint = CrmEndpoints.claEndpoint;
        var tplEndpoint = CrmEndpoints.tplEndpoint;
        // anything under 20k goes to CLA
        
        //remove potential special characters from customer loan amount
        var loanAmt = customer.LoanAmount.Replace("$", "").Replace(",", "");
        _logger.LogInformation("Loan Amount: {loanAmt}", loanAmt);
        if (decimal.TryParse(loanAmt, out var loanAmount)) {
            if (loanAmount < 20000) {
                _logger.LogInformation("Loan Amount is less than 20k");
                url = claEndpoint;
            }
            // everything else goes to TPL
            else {
                _logger.LogInformation("Loan Amount is greater than 20k");
                url = tplEndpoint;
            }
        }
        else {
            _logger.LogError("Unable to parse loan amount, defaulting to tplEndpoint");
            url = tplEndpoint;
        }

        // if offer is not null then check if offer code belongs to cla or tpl
        if (!string.IsNullOrEmpty(customer.Offer)) {
            // if cla file then cla
            var directMail = _martenService.GetDirectMail(customer.Offer).Result;
            if (directMail != null) {
                if (!string.IsNullOrEmpty(directMail.Team)) { 

                    if (directMail.Team.ToUpper().Contains("CLA")) {
                        url = claEndpoint;
                    }
                    // if tpl file then tpl
                    else if (directMail.Team.ToUpper().Contains("TPL")) {
                        url = tplEndpoint;
                    }
                }
            }
            else {
                _logger?.LogInformation($"Promocode used but not found in direct mail table {customer.Offer}");
            }
        }

        if (string.IsNullOrEmpty(url)) throw new InvalidOperationException("Endpoint url is empty");
        return url;
    }

    private bool EmailContact(Customer customerResponse) {
        
        var body = "First Name: " + customerResponse.FirstName;
        body += "\nLast Name: " + customerResponse.LastName;
        body += "\nEmail: " + customerResponse.Email;
      //  body += "\nAddress 1: " + customerResponse.Address;
      //  body += "\nAddress 2: " + customerResponse.Address2;
     //   body += "\nCity: " + customerResponse.City;
     //   body += "\nState: " + customerResponse.State;
      //  body += "\nZip Code: " + customerResponse.Zip;
        body += "\nCell Phone: " + customerResponse.Cell;
        body += "\nHome Phone: " + customerResponse.Phone;
        body += "\nloan Amount: " + customerResponse.LoanAmount;
        body += "\nOffer: " + (string.IsNullOrEmpty(customerResponse.Offer)?"None":customerResponse.Offer);
        
        body += "\nTime(PDT): " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
      
        using var message = new MailMessage();

        var emails = _emailSettings.ToEmail.Split(',');
        foreach (var email in emails) {
            message.To.Add(email);
        }
       
        message.From = new MailAddress(_emailSettings.Username);
        message.Subject = _emailSettings.Subject;
        message.Body = body;

        using var smtp = new SmtpClient();
        smtp.EnableSsl = true;

        smtp.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
        smtp.Host = _emailSettings.Host;
        smtp.Port = _emailSettings.Port;

        try {
            smtp.Send(message);
            return true;
        }
        catch (Exception ex) {
            _logger?.LogError("Unable to send email {ex}", ex);
            return false;
        }
    }
}