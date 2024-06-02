using DominosCutScreen.Shared;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using System.Net.Http.Json;

namespace DominosCutScreen.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            {
                var client = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
                var result = await client.GetFromJsonAsync<SettingsService>("/api/Settings/Get");

                if (result == null)
                    throw new ApplicationException("Failed to get Settings from server");

                builder.Services.AddSingleton(result);
            }

            await builder.Build().RunAsync();
        }
    }
}