﻿using DominosCutScreen.Server.Models;
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
        public async Task<IActionResult> MakelineCode([FromBody] int makelineCode)
        {
            _logger.LogInformation("Setting MakelineCode to {code}", makelineCode);
            _context.GetSettings().MakelineCode = makelineCode;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> OvenTime([FromBody] int ovenTime)
        {
            _logger.LogInformation("Setting OvenTime to {time}", ovenTime);
            _context.GetSettings().OvenTime = ovenTime;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GraceTime([FromBody] int graceTime)
        {
            _logger.LogInformation("Setting GraceTime to {time}", graceTime);
            _context.GetSettings().GraceTime = graceTime;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AlertInterval([FromBody] int alertInterval)
        {
            _logger.LogInformation("Setting AlertInterval to {time}", alertInterval);
            _context.GetSettings().AlertInterval = alertInterval;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UseShortAlarmSounds([FromBody] bool useShortAlarm)
        {
            _logger.LogInformation("Setting UseShortAlarmSound to {value}", useShortAlarm);
            _context.GetSettings().UseShortAlarm = useShortAlarm;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> FetchInterval([FromBody] int fetchInterval)
        {
            _logger.LogInformation("Setting FetchInterval to {time}", fetchInterval);
            _context.GetSettings().FetchInterval = fetchInterval;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("/api/[controller]/QuietTime/Enabled")]
        public async Task<IActionResult> QuietTimeEnabled([FromBody] bool isEnabled)
        {
            _logger.LogInformation("Setting QuietTime Enabled to {enabled}", isEnabled);
            _context.GetSettings().QuietTime.IsEnabled = isEnabled;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("/api/[controller]/QuietTime/Start")]
        public async Task<IActionResult> QuietTimeStart([FromBody] TimeOnly start)
        {
            _logger.LogInformation("Setting QuietTime Start to {start}", start);
            _context.GetSettings().QuietTime.Start = start;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("/api/[controller]/QuietTime/End")]
        public async Task<IActionResult> QuietTimeEnd([FromBody] TimeOnly end)
        {
            _logger.LogInformation("Setting QuietTime End to {end}", end);
            _context.GetSettings().QuietTime.End = end;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("/api/[controller]/TimedOrderAlarm/Enabled")]
        public async Task<IActionResult> TimedOrderAlarmEnabled([FromBody] bool isEnabled)
        {
            _logger.LogInformation("Setting TimedOrder Enabled to {enabled}", isEnabled);
            _context.GetSettings().TimedOrderAlarm.IsEnabled = isEnabled;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("/api/[controller]/TimedOrderAlarm/SecondsPerPizza")]
        public async Task<IActionResult> TimedOrderAlarmSecondsPerPizza([FromBody] int secondsPerPizza)
        {
            _logger.LogInformation("Setting TimedOrder SecondsPerPizza to {time}", secondsPerPizza);
            _context.GetSettings().TimedOrderAlarm.SecondsPerPizza= secondsPerPizza;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("/api/[controller]/TimedOrderAlarm/MinPizzaThreshold")]
        public async Task<IActionResult> TimedOrderAlarmMinPizzaThreshold([FromBody] int minPizzaThreshold)
        {
            _logger.LogInformation("Setting TimedOrder MinPizzaThreshold to {threshold}", minPizzaThreshold);
            _context.GetSettings().TimedOrderAlarm.MinPizzaThreshold = minPizzaThreshold;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PulseApiServer([FromBody] string pulseApiServer)
        {
            _logger.LogInformation("Setting PulseApiServer to {server}", pulseApiServer);
            _context.GetSettings().PulseApiServer = pulseApiServer;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("/api/[controller]/PostBake/{receiptCode}/ToppingCode")]
        public async Task<IActionResult> PostBakeToppingCode(string receiptCode, [FromBody] string toppingCode)
        {
            var postbake = await _context.PostBakes.FindAsync(receiptCode);
            if (postbake == null)
                return NotFound();

            postbake.ToppingCode = toppingCode;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("/api/[controller]/PostBake/{receiptCode}/ToppingDescription")]
        public async Task<IActionResult> PostBakeToppingDescription(string receiptCode, [FromBody] string toppingDescription)
        {
            var postbake = await _context.PostBakes.FindAsync(receiptCode);
            if (postbake == null)
                return NotFound();

            postbake.ToppingDescription = toppingDescription;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("/api/[controller]/PostBake/{receiptCode}/Enabled")]
        public async Task<IActionResult> PostBakeEnabled(string receiptCode, [FromBody] bool enabled)
        {
            var postbake = await _context.PostBakes.FindAsync(receiptCode);
            if (postbake == null)
                return NotFound();

            postbake.IsEnabled = enabled;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("/api/[controller]/PostBake")]
        public async Task<IActionResult> PostBakeEnabled([FromBody] PostBake newPostbake)
        {
            var postbake = await _context.PostBakes.FindAsync(newPostbake.ReceiptCode);
            if (postbake != null)
                return Conflict();

            newPostbake.IsEnabled = true;
            await _context.PostBakes.AddAsync(newPostbake);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("/api/[controller]/PostBake/{receiptCode}")]
        public async Task<IActionResult> PostBakeDelete(string receiptCode)
        {
            var postbake = await _context.PostBakes.FindAsync(receiptCode);
            if (postbake == null)
                return NotFound();

            _context.PostBakes.Remove(postbake);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
