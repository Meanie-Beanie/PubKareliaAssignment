using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

    // Required headers are currently placed in the Main() when creating HttpClient
    // Do we need body?
    private async Task<Result<T>> SendAsync<T>(HttpMethod httpVerb, string requestUri)
    {
        HttpRequestMessage message = new HttpRequestMessage(method: httpVerb, requestUri: requestUri);
        
        var response = await _httpClient.SendAsync(message);


        // if it has body
        var body = await response.Content.ReadAsStringAsync();

        var content = JsonSerializer.Deserialize<T>(body);

        return Result<T>.Success(content); ;
    }

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
