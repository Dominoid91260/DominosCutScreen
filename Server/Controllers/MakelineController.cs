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
        const string Address = "http://10.104.37.32:59108";
        const int MakelineCode = 2;

        private static T? DeserializeXML<T>(string xml) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xml);
            return serializer.Deserialize(reader) as T;
        }

        private static async Task<string?> MakeHTTPRequest(string Path)
        {
            string fullPath = $"{Address}/makelines/{MakelineCode}/{Path}";
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(fullPath);
                if (!response.IsSuccessStatusCode)
                {
                    // response.ReasonPhrase
                    return null;
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                // e.Message
                return null;
            }
        }

        private static async Task<T?> FetchAndDeserialize<T>(string Path) where T : class
        {
            var result = await MakeHTTPRequest(Path);

            if (result == null)
                return default;

            return DeserializeXML<T>(result);
        }

        [HttpGet("httpclient")]
        public async Task<IActionResult> GetHttpClient()
        {
            HttpResponseMessage response;
            try
            {
                var client = new HttpClient();
                response = await client.GetAsync(Address);
                if (!response.IsSuccessStatusCode)
                {
                    return new JsonResult(new
                    {
                        error = response.ReasonPhrase
                    });
                }
            }
            catch (HttpRequestException e)
            {
                return new JsonResult(new
                {
                    error = e.Message
                });
            }

            XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfMakeLineOrderItemHistory));
            using var reader = new StringReader(await response.Content.ReadAsStringAsync());
            var rootObject = serializer.Deserialize(reader) as ArrayOfMakeLineOrderItemHistory;
            if (rootObject is null)
            {
                return new JsonResult(new
                {
                    error = "Invalid XML"
                });
            }

            return new JsonResult(rootObject.Items);
        }

        [HttpGet("orders")]
        public async IAsyncEnumerable<MakeLineOrder> GetMakelineData([FromQuery]DateTime Since)
        {
            var arrayOfOrder = await FetchAndDeserialize<ArrayOfMakeLineOrder>($"orders/updates/{Since:s}");

            if (arrayOfOrder == null)
                yield break;

            foreach (var order in arrayOfOrder.Orders)
                yield return order;
        }

        [HttpGet("bump")]
        public async IAsyncEnumerable<MakeLineOrderItemHistory> GetBumpHistory()
        {
            var arrayOfHistory = await FetchAndDeserialize<ArrayOfMakeLineOrderItemHistory>("orderHistory");

            if (arrayOfHistory == null)
                yield break;

            foreach (var history in arrayOfHistory.Items)
                yield return history;
        }
    }
}
