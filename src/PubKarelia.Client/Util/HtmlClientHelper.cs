using PubKarelia.Client.Game;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace PubKarelia.Client.Util;

// Change the name, it is not good but I don't know what to call it otherwise.
public class HtmlClientHelper
{
    private readonly HttpClient _httpClient;

    public HtmlClientHelper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Required headers are currently placed in the Main() when creating HttpClient
    // Do we need body?
    public async Task<ApiResult<T>> SendAsync<T>(HttpRequestMessage message)
    {
        try
        {
            // if message is null, we'll assign 
            using var response = await _httpClient.SendAsync(message);

            // To-do:
            // Responses can contain valuable data even in negative status codes, so maybe we redesign but do it later.
            #region rethinkthiswholething
            // 401 Unauthorized
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return ApiResult<T>.Failure(HttpStatusCode.Unauthorized, "TIKO AUTH -header missing.");

            // 400 Bad Request
            // Think about this later, since body content might contain interesting details.
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return ApiResult<T>.Failure(HttpStatusCode.BadRequest, "Unable to do the action in current location.");

            else if (response.StatusCode == HttpStatusCode.NotFound)
                return ApiResult<T>.Failure(HttpStatusCode.NotFound, "Not found.");


            // catch anything else.
            response.EnsureSuccessStatusCode();
            #endregion

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
}
