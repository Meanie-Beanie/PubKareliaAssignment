using PubKarelia.Client.Game;

namespace PubKarelia.Client;

internal class App
{
    readonly HttpClient _httpClient; // Maybe comes in useful, keeping it in for now.
    readonly GameClient _gameClient;

    public App(HttpClient httpClient, GameClient gameClient)
    {
        _httpClient = httpClient;
        _gameClient = gameClient;
    }

    public async Task Start()
    {
        await _gameClient.GetJoke();
    }
}