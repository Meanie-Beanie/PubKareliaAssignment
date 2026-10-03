using PubKarelia.Client.Game;
using Microsoft.Extensions.Configuration;
using PubKarelia.Client.Util;
using PubKarelia.Client.Dto;

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

        HtmlClientHelper httpHelper = new(httpClient);

        // We'll get the starting position of the player for the game.
        var startingPosition = await httpHelper.SendAsync<CurrentPositionDto>(new HttpRequestMessage(HttpMethod.Get, ApiRoutes.CurrentPosition));

        // We'll ensure we get the player position
        if (startingPosition.Data is null)
            throw new InvalidOperationException("Unable to get the player starting position.");

        var playerStartingCoordinates = new Coordinates(startingPosition.Data.X, startingPosition.Data.Y);
        PlayerManager playerManager = new PlayerManager(playerStartingCoordinates, name: "Olli");

        App app = new(httpClient, new GameClient(httpHelper, playerManager));

        await app.Start();
    }
}