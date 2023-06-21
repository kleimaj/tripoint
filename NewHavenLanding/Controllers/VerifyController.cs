using System.Diagnostics;
using FluentValidation;
using FluentValidation.AspNetCore;
using FormHelper;
using Marten;
using Microsoft.AspNetCore.Mvc;
using NewHavenLanding.Models;
using NewHavenLanding.Services;
using Serilog;

namespace NewHavenLanding.Controllers;

public class VerifyController : Controller {
    private readonly ILogger<VerifyController> _logger;
    private IValidator<Customer> _validator;
    private IMartenService _martenService;

    public VerifyController(IValidator<Customer> validator, ILogger<VerifyController> logger, IMartenService martenService) {
        _validator = validator;
        _logger = logger;
        _martenService = martenService;
    }

    public IActionResult Index(Guid customerGid) {
        ViewBag.CustomerGid = customerGid;
        return View(new Customer{Id = customerGid});
    }

   
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        _logger.LogError("{HttpContextTraceIdentifier}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [HttpPost, FormValidator]
    public async Task<IActionResult> Save(Customer customer) {
        var result = await _validator.ValidateAsync(customer);
        if (!result.IsValid) 
        {
            result.AddToModelState(ModelState);
            return View("Index", customer);
        }
        try {

            await _martenService.CreateCustomer(customer);
            
            return FormResult.CreateSuccessResult("Customer saved.", Url.Action("", "GetStarted", new {customerGid = customer.Id}), 500);
        }
        catch
        {
            return FormResult.CreateErrorResult("An error occurred!");
        }
    }
    
}