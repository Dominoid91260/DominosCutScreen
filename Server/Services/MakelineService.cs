using DominosCutScreen.Shared;

using System.Collections.ObjectModel;
using System.Net;
using System.Xml.Serialization;

namespace DominosCutScreen.Server.Services
{
    public class MakelineService : BackgroundService
    {
        public const string _address = "http://10.104.37.32:59108";
        public const int _makelineCode = 2;

        /// <summary>
        /// In minutes, how often should we poll the makeline for order and bump history
        /// The makeline only keeps full bumped order history for a few minutes (i think about 5)
        /// </summary>
        private const int _makelinePollInterval = 4;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly object _lock = new();
        private DateTime _lastMakelineCheck;

        public IEnumerable<MakeLineOrder> Orders { get; private set; }
        public IEnumerable<MakeLineOrderItemHistory> BumpHistory { get; private set; }

        private static T? DeserializeXML<T>(string xml) where T : class
        {
            if (xml.Length == 0)
                return default;

            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xml);

            try
            {
                return serializer.Deserialize(reader) as T;
            }
            catch (InvalidOperationException e)
            {
                Console.Error.WriteLine($"MakelineService.DeserializeXML failed: {e.Message}");
                return null;
            }
        }
        private static async Task<string?> MakeHTTPRequest(HttpClient Client, string Path)
        {
            string fullPath = $"{_address}/makelines/{_makelineCode}/{Path}";
            try
            {
                var response = await Client.GetAsync(fullPath);
                if (!response.IsSuccessStatusCode)
                {
                    Console.Error.WriteLine($"MakelineService.MakeHTTPRequest failed: {fullPath} | {response.ReasonPhrase}");
                    return null;
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Console.Error.WriteLine($"MakelineService.MakeHTTPRequest failed: {fullPath} | {e.Message}");
                return null;
            }
        }

        private static async Task<T?> FetchAndDeserialize<T>(HttpClient Client, string Path) where T : class
        {
            var result = await MakeHTTPRequest(Client, Path);

            if (result == null)
                return default;

            return DeserializeXML<T>(result);
        }

        public MakelineService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            Orders = new List<MakeLineOrder>();
            BumpHistory = new List<MakeLineOrderItemHistory>();
            _lastMakelineCheck = DateTime.Now.Date; // Make it midnight so we get as much info as possible
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var client = _httpClientFactory.CreateClient();

            if (client == null)
                return;

            while (!stoppingToken.IsCancellationRequested)
            {
                // Bump History
                var bumpHistory = await FetchAndDeserialize<ArrayOfMakeLineOrderItemHistory>(client, "orderHistory");
                if (bumpHistory != null)
                {
                    lock (_lock)
                    {
                        BumpHistory = bumpHistory.Items;
                    }
                }

                // Orders
                var orders = await FetchAndDeserialize<ArrayOfMakeLineOrder>(client, $"orders/updates/{_lastMakelineCheck:s}");
                _lastMakelineCheck = DateTime.Now;
                if (orders != null)
                {
                    lock (_lock)
                    {
                        Orders = Orders.Concat(orders.Orders).Distinct();
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(_makelinePollInterval), stoppingToken);
            }
        }
    }
}
