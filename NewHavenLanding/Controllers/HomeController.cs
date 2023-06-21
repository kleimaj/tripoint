using System.Diagnostics;
using JSNLog;
using Microsoft.AspNetCore.Mvc;
using NewHavenLanding.Models;
using NewHavenLanding.Services;

namespace NewHavenLanding.Controllers;

public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;
    private IMartenService _martenService;
    
    public HomeController(ILogger<HomeController> logger, IMartenService martenService) {
        _logger = logger;
        _martenService = martenService;
    }

    public IActionResult Index() {
        return View();
    }

    [HttpPost]
    [Route("Log")]
    public void JsnLogger([FromBody] JsnLogMessage obj) {
        foreach (var jsnError in obj.Payload)
            switch (jsnError.Level) {
                case (int)Level.TRACE:
                    _logger.LogTrace("{@LogMessage}", jsnError.Message);
                    break;
                case (int)Level.DEBUG:
                    _logger.LogDebug("{@LogMessage}", jsnError.Message);
                    break;
                case (int)Level.INFO:
                    _logger.LogInformation("{@LogMessage}", jsnError.Message);
                    break;
                case (int)Level.WARN:
                    _logger.LogWarning("{@LogMessage}", jsnError.Message);
                    break;
                case (int)Level.ERROR:
                    _logger.LogError("{@LogMessage}", jsnError.Message);
                    break;
                case (int)Level.FATAL:
                    _logger.LogCritical("{@LogMessage}", jsnError.Message);
                    break;
            }
    }
    [HttpGet]
    public async Task<IActionResult> SubmitCode(string? accessCode = null, string? referer = null)
    {
        var customerGid = Guid.NewGuid();
        var promocode = new Promocode {Id = customerGid, Code = accessCode??"Missing", Referer = referer??"Missing"};
        await _martenService.CreatePromocode(promocode);

        //Url.Action("", "Verify", new { customerGid });
        return RedirectToAction("", "Verify", new { customerGid });
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        _logger.LogError("{HttpContextTraceIdentifier}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}