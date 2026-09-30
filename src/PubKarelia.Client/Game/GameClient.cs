using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Net;

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
    private async Task<ApiResult<T>> SendAsync<T>(HttpMethod httpVerb, string requestUri)
    {
        try
        {
            HttpRequestMessage message = new HttpRequestMessage(method: httpVerb, requestUri: requestUri);

            var response = await _httpClient.SendAsync(message);

            // Should we throw or handle the error codes manually?
            // But how many different error codes are there in the api docs? Maybe map them out and figure out, since we do want to inform users 
            response.EnsureSuccessStatusCode();

            // if it has body
            var body = await response.Content.ReadAsStringAsync();

            var content = JsonSerializer.Deserialize<T>(body);

            return ApiResult<T>.Success(content);
        }
        
        catch (HttpRequestException ex)
        {
            return ApiResult<T>.Failure(HttpStatusCode.BadRequest, $"{ex.Message}"); // Does not work properly, read comments above.
        }

        // This is kinda the downside of HttpStatusCode in this situation, since we are smuggling our potential server issue as api server issue.
        catch (JsonException ex)
        {
            return ApiResult<T>.Failure(HttpStatusCode.BadRequest, $"{ex.Message}");
        }
    }

    // GET
    public async Task<ApiResult<string>> GetJoke()
        => await SendAsync<string>(HttpMethod.Get, ApiRoutes.GetJoke);


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
