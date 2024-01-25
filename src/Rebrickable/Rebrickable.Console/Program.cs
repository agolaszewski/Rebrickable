using Microsoft.Extensions.Configuration;
using Rebrickable.Api;
using System.Net.Http;

namespace Rebrickable.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
            .Build();

            var handler = new HttpClientHandler();
   
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(config["Rebrickable:Url"]);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"key {config["Rebrickable:Key"]}");

            try
            {
                var r = new RebrickableClient(httpClient);
                var xd = await r.LegoSetsReadAsync("42083-1");
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }
            
        }
    }
}