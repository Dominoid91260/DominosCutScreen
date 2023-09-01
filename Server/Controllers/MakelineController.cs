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
        [HttpGet("orders")]
        public IEnumerable<MakeLineOrder> GetMakelineData()
        {
            var arrayOfOrder = HttpContext.RequestServices.GetServices<IHostedService>().OfType<MakelineService>().First().Orders;

            if (arrayOfOrder == null)
                yield break;

            foreach (var order in arrayOfOrder)
                yield return order;
        }

        [HttpGet("bump")]
        public IEnumerable<MakeLineOrderItemHistory> GetBumpHistory()
        {
            var arrayOfHistory = HttpContext.RequestServices.GetServices<IHostedService>().OfType<MakelineService>().First().BumpHistory;

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
                var response = await client.GetAsync($"{MakelineService._address}/makelines/{MakelineService._makelineCode}/silenceAlarm");
                if (response.IsSuccessStatusCode)
                    return Ok();
            }
            catch (HttpRequestException e)
            {
                Console.Error.WriteLine(e.Message);
            }

            return BadRequest();
        }
    }
}
