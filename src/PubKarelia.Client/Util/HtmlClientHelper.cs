using PubKarelia.Client.Game;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
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
    public async Task<ApiResult<TValue>> SendAsync<TValue>(HttpRequestMessage message)
    {
        try
        {
            using var response = await _httpClient.SendAsync(message);
            var body = await response.Content.ReadAsStringAsync();

            // 401 Unauthorized
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return ApiResult<TValue>.Failure(HttpStatusCode.Unauthorized, body);

            // 400 Bad Request
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                return ApiResult<TValue>.Failure(HttpStatusCode.BadRequest, body);

            // 404 Not Found
            else if (response.StatusCode == HttpStatusCode.NotFound)
                return ApiResult<TValue>.Failure(HttpStatusCode.NotFound, body);

            // We have to use default keyword with generics and to be honest, it is good practice outside of them as well
            if (string.IsNullOrWhiteSpace(body))
                return ApiResult<TValue>.Success(default);

            // Wisdom from StackOverflow from 17 years ago. This stumped me but this is sadly the best solution for this current problem
            // we have to do this check because otherwise we'll serialize the data and we'll run into an issue with purely string body.
            if (typeof(TValue) == typeof(string))
            {
                // This pasta is served by:
                // https://stackoverflow.com/questions/39244449/cast-generic-type-parameter-to-a-specific-type-in-c-sharp
                // but I am actually quite stumped if there is a better way of handling this, but it'll work for now.
                return ApiResult<TValue>.Success((TValue)(object)body); 
            }


            // Could do with improvement but we'll check if response contains application/json and therefore can be serialized.
            if (response.Content.Headers.ContentType is not null && response.Content.Headers.ContentType.MediaType == "application/json")
            {
                /*
                 * TO-DO:
                 * CHECK THE SERIALIZER OPTIONS
                 * NEWTONSOFT AND NET SERIALIZER HAVE DIFFERENT BEHAVIORS WITH SPECIFIC ASPECTS
                 * AND ALL I CAN REMEMBER THAT ONE OF THEIRS' DEFAULT WAS TO SERIALIZE/DESERIALIZE CASE-INSENSITIVE
                */
                var content = JsonSerializer.Deserialize<TValue>(body);
                return ApiResult<TValue>.Success(content);
            }


            else
                throw new InvalidOperationException("Something wen't wrong.");
        }

        // Catches any other HttpRequest Exception, Try no longer throws exception through EnsureSuccessStatusCode
        catch (HttpRequestException ex)
        {
            return ApiResult<TValue>.Failure(HttpStatusCode.BadRequest, $"{ex.Message}");
        }

        // This is kinda the downside of HttpStatusCode in this situation, since we are smuggling our potential server issue as api server issue.
        // Could set it to nullable and have null for these scenarios, but is that good design?
        catch (JsonException ex)
        {
            return ApiResult<TValue>.Failure(HttpStatusCode.BadRequest, $"{ex.Message}");
        }
    }
}
