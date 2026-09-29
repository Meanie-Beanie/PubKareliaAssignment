using PubKarelia.Client.Game;
using Microsoft.Extensions.Configuration;

namespace PubKarelia.Client;

internal class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        var apiKey = config["api_key"] ?? throw new ArgumentNullException("You have not set api_key secret in VS user secrets");

        // We'll actually set the base route through ApiRoutes
        using HttpClient httpClient = new HttpClient();

        // Since most call require it anyways, we'll set it here.
        httpClient.DefaultRequestHeaders.Add("TIKO-AUTH", apiKey);

        App app = new(httpClient, new GameClient(httpClient));

        await app.Start();
    }
}