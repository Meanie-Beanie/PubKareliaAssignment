using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Net;
using PubKarelia.Client.Dto;

namespace PubKarelia.Client.Game;

// Will change a lot as it is being designed
// Actually, return types can widely vary and need to be uniform.
// I do not want to throw errors for each, so will be using Result -pattern
public class GameClient
{
    readonly HttpClient _httpClient;
    private int currentPosition = 0;

    private bool firstMove = false;

    public GameClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Required headers are currently placed in the Main() when creating HttpClient
    // Do we need body?
    private async Task<ApiResult<T>> SendAsync<T>(HttpRequestMessage message)
    {
        try
        {
            // if message is null, we'll assign 
            using var response = await _httpClient.SendAsync(message);

            // 401 Unauthorized
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return ApiResult<T>.Failure(HttpStatusCode.Unauthorized, "TIKO AUTH -header missing.");

            // 400 Bad Request
            // Think about this later, since body content might contain interesting details.
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return ApiResult<T>.Failure(HttpStatusCode.BadRequest, "Unable to do the action in current location.");

            // catch anything else.
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();

            // We have to use default keyword with generics and to be honest, it is good practice outside of them as well
            if (string.IsNullOrWhiteSpace(body))
                return ApiResult<T>.Success(default); 

            /*
             * TO-DO:
             * CHECK THE SERIALIZER OPTIONS
             * NEWTONSOFT AND NET SERIALIZER HAVE DIFFERENT BEHAVIORS WITH SPECIFIC ASPECTS
             * AND ALL I CAN REMEMBER THAT ONE OF THEIRS' DEFAULT WAS TO SERIALIZE/DESERIALIZE CASE-INSENSITIVE
            */
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


    public async Task<ApiResult<string>> GetJoke()
        => await SendAsync<string>(new HttpRequestMessage(HttpMethod.Get, ApiRoutes.GetJoke));

    public async Task<ApiResult<string>> ThrowDart()
        => await SendAsync<string>(new HttpRequestMessage(HttpMethod.Get, ApiRoutes.ThrowDart));

    public async Task<ApiResult<string>> GetMoominLemonade()
        => await SendAsync<string>(new HttpRequestMessage(HttpMethod.Get, ApiRoutes.GetMoominLemonade));

    public async Task<ApiResult<string>> OrderPizza(int toppingsCount = 1)
    {
        if (toppingsCount > 5 || toppingsCount < 1)
            throw new ArgumentOutOfRangeException("Only 1 to 5 toppings allowed."); // Technically could be ArgumentException? I kinda only see this for data structure limits.

        var uriWithParameters = ApiRoutes.OrderPizza + $"?ToppingsCount={toppingsCount}";

        return await SendAsync<string>(new HttpRequestMessage(HttpMethod.Get, uriWithParameters));
    }

    public async Task<ApiResult<List<string>>> ReadMessages()
        => await SendAsync<List<string>>(new HttpRequestMessage(HttpMethod.Get, ApiRoutes.ReadMessages));

    // PUT
    public async Task<ApiResult<string>> Move(int xCoordinates, int yCoordinates)
    {

    }

    // Help - low priority

    // History - Low priority

    // Position - Low Priority
}
