using DominosCutScreen.Server.Models;
using DominosCutScreen.Shared;

using Microsoft.AspNetCore.Mvc;

namespace DominosCutScreen.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly CutBenchContext _context;

        public SettingsController(ILogger<SettingsController> logger, CutBenchContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.GetSettings());
        }

        // `HttpPut` requires an Id which we dont have, so we use `HttpPost` instead.

        [HttpPost]
        public async Task<IActionResult> MakelineServer([FromBody] string makelineServer)
        {
            _logger.LogInformation("Setting MakelineServer to {server}", makelineServer);
            _context.GetSettings().MakelineServer = makelineServer;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MakelineCode(int makelineCode)
        {
            _logger.LogInformation("Setting MakelineCode to {code}", makelineCode);
            _context.GetSettings().MakelineCode = makelineCode;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> OvenTime(int ovenTime)
        {
            _logger.LogInformation("Setting OvenTime to {time}", ovenTime);
            _context.GetSettings().OvenTime = ovenTime;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GraceTime(int graceTime)
        {
            _logger.LogInformation("Setting GraceTime to {time}", graceTime);
            _context.GetSettings().GraceTime = graceTime;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AlertInterval(int alertInterval)
        {
            _logger.LogInformation("Setting AlertInterval to {time}", alertInterval);
            _context.GetSettings().AlertInterval = alertInterval;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> FetchInterval(int fetchInterval)
        {
            _logger.LogInformation("Setting FetchInterval to {time}", fetchInterval);
            _context.GetSettings().FetchInterval = fetchInterval;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("QuietTime/Enabled")]
        public async Task<IActionResult> QuietTimeEnabled(bool isEnabled)
        {
            _logger.LogInformation("Setting QuietTime Enabled to {enabled}", isEnabled);
            _context.GetSettings().QuietTime.IsEnabled = isEnabled;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("QuietTime/Start")]
        public async Task<IActionResult> QuietTimeStart(TimeOnly start)
        {
            _logger.LogInformation("Setting QuietTime Start to {start}", start);
            _context.GetSettings().QuietTime.Start = start;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("QuietTime/End")]
        public async Task<IActionResult> QuietTimeEnd(TimeOnly end)
        {
            _logger.LogInformation("Setting QuietTime End to {end}", end);
            _context.GetSettings().QuietTime.End = end;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("TimedOrderAlarm/Enabled")]
        public async Task<IActionResult> TimedOrderAlarmEnabled(bool isEnabled)
        {
            _logger.LogInformation("Setting TimedOrder Enabled to {enabled}", isEnabled);
            _context.GetSettings().TimedOrderAlarm.IsEnabled = isEnabled;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("TimedOrderAlarm/SecondsPerPizza")]
        public async Task<IActionResult> TimedOrderAlarmSecondsPerPizza(int secondsPerPizza)
        {
            _logger.LogInformation("Setting TimedOrder SecondsPerPizza to {time}", secondsPerPizza);
            _context.GetSettings().TimedOrderAlarm.SecondsPerPizza= secondsPerPizza;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("TimedOrderAlarm/MinPizzaThreshold")]
        public async Task<IActionResult> TimedOrderAlarmMinPizzaThreshold(int minPizzaThreshold)
        {
            _logger.LogInformation("Setting TimedOrder MinPizzaThreshold to {threshold}", minPizzaThreshold);
            _context.GetSettings().TimedOrderAlarm.MinPizzaThreshold = minPizzaThreshold;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
