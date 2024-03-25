using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TriPoint.Models.Jsn;

namespace TriPoint.Controllers;

public class CalculatorController : Controller {
    private readonly ILogger<FaqController> _logger;

    public CalculatorController(ILogger<FaqController> logger) {
        _logger = logger;
    }

    [HttpGet("calculator")]
    public IActionResult Index() {
        return View();
    }

    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        _logger.LogError("{HttpContextTraceIdentifier}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}