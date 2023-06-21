using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewHavenLanding.Models;

namespace NewHavenLanding.Controllers;

public class ThankYouController : Controller {
    private readonly ILogger<ThankYouController> _logger;

    public ThankYouController(ILogger<ThankYouController> logger) {
        _logger = logger;
    }

    public IActionResult Index() {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        _logger.LogError("{HttpContextTraceIdentifier}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}