using DominosCutScreen.Shared;

using Microsoft.AspNetCore.Mvc;

namespace DominosCutScreen.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly SettingsService _settings;

        public SettingsController(ILogger<SettingsController> logger, SettingsService settings)
        {
            _logger = logger;
            _settings = settings;
        }

        // `HttpPut` requires an Id which we dont have, so we use `HttpPost` instead.

        [HttpPost]
        public IActionResult MakelineServer([FromBody] string makelineServer)
        {
            _logger.LogInformation("Setting MakelineServer to {server}", makelineServer);
            _settings.MakelineServer = makelineServer;
            return Ok();
        }

        [HttpPost]
        public IActionResult MakelineCode(int makelineCode)
        {
            _logger.LogInformation("Setting MakelineCode to {code}", makelineCode);
            _settings.MakelineCode = makelineCode;
            return Ok();
        }

        [HttpPost]
        public IActionResult OvenTime(int ovenTime)
        {
            _logger.LogInformation("Setting OvenTime to {time}", ovenTime);
            _settings.OvenTime = ovenTime;
            return Ok();
        }

        [HttpPost]
        public IActionResult GraceTime(int graceTime)
        {
            _logger.LogInformation("Setting GraceTime to {time}", graceTime);
            _settings.GraceTime = graceTime;
            return Ok();
        }

        [HttpPost]
        public IActionResult AlertInterval(int alertInterval)
        {
            _logger.LogInformation("Setting AlertInterval to {time}", alertInterval);
            _settings.AlertInterval = alertInterval;
            return Ok();
        }

        [HttpPost]
        public IActionResult FetchInterval(int fetchInterval)
        {
            _logger.LogInformation("Setting FetchInterval to {time}", fetchInterval);
            _settings.FetchInterval = fetchInterval;
            return Ok();
        }

        [HttpPost("QuietTime/Enabled")]
        public IActionResult QuietTimeEnabled(bool isEnabled)
        {
            _logger.LogInformation("Setting QuietTime Enabled to {enabled}", isEnabled);
            _settings.QuietTime.IsEnabled = isEnabled;
            return Ok();
        }

        [HttpPost("QuietTime/Start")]
        public IActionResult QuietTimeStart(TimeOnly start)
        {
            _logger.LogInformation("Setting QuietTime Start to {start}", start);
            _settings.QuietTime.Start = start;
            return Ok();
        }

        [HttpPost("QuietTime/End")]
        public IActionResult QuietTimeEnd(TimeOnly end)
        {
            _logger.LogInformation("Setting QuietTime End to {end}", end);
            _settings.QuietTime.End = end;
            return Ok();
        }

        [HttpPost("TimedOrderAlarm/Enabled")]
        public IActionResult TimedOrderAlarmEnabled(bool isEnabled)
        {
            _logger.LogInformation("Setting TimedOrder Enabled to {enabled}", isEnabled);
            _settings.TimedOrderAlarm.IsEnabled = isEnabled;
            return Ok();
        }

        [HttpPost("TimedOrderAlarm/SecondsPerPizza")]
        public IActionResult TimedOrderAlarmSecondsPerPizza(int secondsPerPizza)
        {
            _logger.LogInformation("Setting TimedOrder SecondsPerPizza to {time}", secondsPerPizza);
            _settings.TimedOrderAlarm.SecondsPerPizza= secondsPerPizza;
            return Ok();
        }

        [HttpPost("TimedOrderAlarm/MinPizzaThreshold")]
        public IActionResult TimedOrderAlarmMinPizzaThreshold(int minPizzaThreshold)
        {
            _logger.LogInformation("Setting TimedOrder MinPizzaThreshold to {threshold}", minPizzaThreshold);
            _settings.TimedOrderAlarm.MinPizzaThreshold = minPizzaThreshold;
            return Ok();
        }
    }
}
