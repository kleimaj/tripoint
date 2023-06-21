using NewHavenLanding.Models;

namespace NewHavenLanding.Services;

public interface IMartenService {
    internal Task<bool> CreatePromocode(Promocode promocode);
    internal Task<bool> CreateCustomer(Customer customer);
    internal Task<bool> CreateCustomerLoanInfo(CustomerLoanInfo customerLoanInfo);
    
}