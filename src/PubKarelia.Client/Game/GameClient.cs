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

            // 401 Unauthorized
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return ApiResult<T>.Failure(HttpStatusCode.Unauthorized, "TIKO AUTH -header missing.");

            // 400 Bad Request
            // Think about this later, since body content might contain interesting details.
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return ApiResult<T>.Failure(HttpStatusCode.BadRequest, "Unable to do the action in current location.");

            var body = await response.Content.ReadAsStringAsync();

            var content = JsonSerializer.Deserialize<T>(body);
            
            return ApiResult<T>.Success(content);
        }
        
        // Catches any other HttpRequest Exception, Try no longer throws exception through EnsureSuccessStatusCode
        catch (HttpRequestException ex)
        {
            return ApiResult<T>.Failure(HttpStatusCode.BadRequest, $"{ex.Message}");
        }

        // This is kinda the downside of HttpStatusCode in this situation, since we are smuggling our potential server issue as api server issue.
        // Could set it to nullable and have null for these scenarios, but is that good design?
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
