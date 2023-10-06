using DominosCutScreen.Server.Services;
using DominosCutScreen.Shared;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Diagnostics;
using System.Text;
using System.Transactions;
using System.Xml.Serialization;

namespace DominosCutScreen.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MakelineController : ControllerBase
    {
        private readonly SettingsService _settings;

        public MakelineController(SettingsService settings)
        {
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

                Console.Error.WriteLine($"MakelineController.SilenceMakeline failed: {response.ReasonPhrase}");
            }
            catch (HttpRequestException e)
            {
                Console.Error.WriteLine($"MakelineController.SilenceMakeline failed: {e.Message}");
            }

            return BadRequest();
        }
    }
}
