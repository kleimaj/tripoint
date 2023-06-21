using System.Diagnostics;
using FluentValidation;
using FormHelper;
using Microsoft.AspNetCore.Mvc;
using NewHavenLanding.Models;
using NewHavenLanding.Services;

namespace NewHavenLanding.Controllers;

public class GetStartedController : Controller {
    private readonly ILogger<GetStartedController> _logger;
    private IValidator<CustomerLoanInfo> _validator;
    private readonly IMartenService _martenService;
    
    public GetStartedController(ILogger<GetStartedController> logger, IValidator<CustomerLoanInfo> validator, IMartenService martenService) {
        _logger = logger;
        _validator = validator;
        _martenService = martenService;
    }

    public IActionResult Index(Guid customerGid) {
        ViewBag.CustomerGid = customerGid;
        return View(new CustomerLoanInfo{Id = customerGid});
    }

   
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        _logger.LogError("{HttpContextTraceIdentifier}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    [HttpPost, FormValidator]
    public async Task<IActionResult> Save(CustomerLoanInfo customerLoanInfo) { 
        await _martenService.CreateCustomerLoanInfo(customerLoanInfo);
        
        try
        {
            return FormResult.CreateSuccessResult("Customer saved.", Url.Action("", "ThankYou"), 500);
        }
        catch
        {
            return FormResult.CreateErrorResult("An error occurred!");
        }
    }
}