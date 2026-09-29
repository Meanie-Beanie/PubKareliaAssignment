using System;
using System.Collections.Generic;
using System.Text;

namespace PubKarelia.Client.Game;

// Will change a lot as it is being designed
// Actually, return types can widely vary and need to be uniform.
// I do not want to throw errors for each, so will be using Result -pattern
public class GameClient
{
    readonly HttpClient _httpClient;

    public GameClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    //private async Task<Result<T>> SendAsync<T>()
    //{


    //    var response = await _httpClient.SendAsync();
    //}

    // GET
    //public async Task<Result<string>> GetJoke()
    //{
    //    var response = await _httpClient.GetAsync(ApiRoutes.GetJoke);

    //    var joke = await response.Content.ReadAsStringAsync();



    //    return joke;
    //}

    //// GET
    //public async Task<string> ThrowDart()
    //{
    //    var response = await _httpClient.GetAsync(ApiRoutes.GetJoke);

    //    var joke = await response.Content.ReadAsStringAsync();

    //    return joke;
    //}

    // Help - low priority

    // History - Low priority

    // Position - Low Priority
}
