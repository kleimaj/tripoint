using Newtonsoft.Json;

namespace TriPoint.Models;

public class Tpl2Salesforce {
    public  class TripointProspects
    {
        [JsonProperty("prospects")]
        public required Prospect[] Prospects { get; set; }
    }

    public  class Prospect
    {
        [JsonProperty("offer_code")]
        public required string OfferCode { get; set; }

        [JsonProperty("first_name")]
        public required string FirstName { get; set; }

        [JsonProperty("last_name")]
        public required string LastName { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("state")]
        public string? State { get; set; }

        [JsonProperty("zip_code")]
        public string? ZipCode { get; set; }

        [JsonProperty("home_phone")]
        public required string HomePhone { get; set; }

        [JsonProperty("cell_phone")]
        public required string CellPhone { get; set; }

        [JsonProperty("email")]
        public required string Email { get; set; }

        [JsonProperty("loan_amount")]
        public required string LoanAmount { get; set; }
        
        [JsonProperty("lead_source")]
        public required string Team { get; set; }
    }
    public partial class TripointSalesforceResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("instance_url")]
        public Uri InstanceUrl { get; set; }

        [JsonProperty("id")]
        public Uri Id { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("issued_at")]
        public string IssuedAt { get; set; }
    }
}