using DominosCutScreen.Server.Services;
using DominosCutScreen.Shared;

using Microsoft.AspNetCore.Mvc;

namespace DominosCutScreen.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MakelineController : ControllerBase
    {
        private readonly SettingsService _settings;
        private readonly ILogger<MakelineController> _logger;

        public MakelineController(ILogger<MakelineController> logger, SettingsService settings)
        {
            _logger = logger;
            _settings = settings;
        }

        [HttpGet("orders")]
        public IEnumerable<MakeLineOrder> GetMakelineData()
        {
            var makelineService = HttpContext.RequestServices.GetServices<IHostedService>().OfType<MakelineService>().First();
            var arrayOfOrder = makelineService.Orders;

            if (arrayOfOrder == null)
                yield break;

            foreach (var order in arrayOfOrder)
                yield return order;
        }

        [HttpGet("bump")]
        public IEnumerable<MakeLineOrderItemHistory> GetBumpHistory()
        {
            var makelineService = HttpContext.RequestServices.GetServices<IHostedService>().OfType<MakelineService>().First();
            var arrayOfHistory = makelineService.BumpHistory;

            if (arrayOfHistory == null)
                yield break;

            foreach (var history in arrayOfHistory)
                yield return history;
        }

        [HttpGet("silence")]
        public async Task<IActionResult> SilenceMakeline()
        {
            var client = new HttpClient();
            try
            {
                var response = await client.PostAsync($"{_settings.MakelineServer}/makelines/{_settings.MakelineCode}/silenceAlarm", null);
                if (response.IsSuccessStatusCode)
                    return Ok();

                _logger.LogError("MakelineController.SilenceMakeline failed: {reason}", response.ReasonPhrase);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("MakelineController.SilenceMakeline failed: {reason}", e.Message);
            }

            return BadRequest();
        }

        [HttpGet("reprint")]
        public async Task<IActionResult> ReprintOrderReceipt(int OrderNumber)
        {
            var client = new HttpClient();
            try
            {
                var response = await client.PostAsync($"{_settings.MakelineServer}/makelines/{_settings.MakelineCode}/orders/{OrderNumber}/print", null);
                if (response.IsSuccessStatusCode)
                    return Ok();

                _logger.LogError("MakelineController.ReprintOrderReceipt failed: {reason}", response.ReasonPhrase);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("MakelineController.ReprintOrderReceipt failed: {reason}", e.Message);
            }

            return BadRequest();
        }
    }
}
