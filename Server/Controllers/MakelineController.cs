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
        public IEnumerable<MakeLineOrder> GetMakelineData([FromQuery]DateTime Since)
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
    }
}
