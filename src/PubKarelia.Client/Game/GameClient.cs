using System;
using System.Collections.Generic;
using System.Text;

namespace PubKarelia.Client.Game;

// Will change a lot as it is being designed

public class GameClient
{
    readonly HttpClient _httpClient;

    public GameClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<string> GetJoke()
    {
        var response = await _httpClient.GetAsync(ApiRoutes.GetJoke);

        var body = response.Content.ReadAsStringAsync();

        return "";
    }


    // Help - low priority

    // History - Low priority

    // Position - Low Priority
}
