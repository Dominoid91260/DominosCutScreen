using DominosCutScreen.Shared;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Diagnostics;
using System.Transactions;
using System.Xml.Serialization;

namespace DominosCutScreen.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MakelineController : ControllerBase
    {
        const string Address = "http://10.104.37.32:59108/makelines/2/orderHistory";

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

        [HttpGet("curl")]
        public async Task<IActionResult> GetCurl()
        {
            // Start the child process.
            ProcessStartInfo psi = new()
            {
                // Redirect the output stream of the child process.
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                FileName = "curl",
                Arguments = "-X GET " + Address,
                CreateNoWindow = true
            };

            using var p = new Process{ StartInfo = psi };
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            p.WaitForExit();

            return new JsonResult(new
            {
                text = output,
                error = error
            });
        }
    }
}
