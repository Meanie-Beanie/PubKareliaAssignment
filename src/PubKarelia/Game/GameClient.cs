using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Net;
using PubKarelia.Client.Dto;
using PubKarelia.Client.Util;
using System.Net.Http.Json;

namespace PubKarelia.Client.Game;

// Will change a lot as it is being designed
// Actually, return types can widely vary and need to be uniform.
// I do not want to throw errors for each, so will be using Result -pattern
internal class GameClient
{
    readonly PlayerManager _playerManager;
    readonly HtmlClientHelper _httpClient;

    public GameClient(HtmlClientHelper httpHelper, PlayerManager playerManager)
    {
        _httpClient = httpHelper;
        _playerManager = playerManager;
    }

    /* GET */

    public async Task<ApiResult<string>> Help()
    => await _httpClient.SendAsync<string>(new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Help));

    /// <summary>
    /// Is not tested, who knows if it works.
    /// </summary>
    /// <param name="action">
    /// 0 = fetched jokes
    /// 1 = Dart scores
    /// 2 = WC -visits
    /// 3 = WC -flushes
    /// 4 = Drink orders
    /// 5 = Food orders
    /// 6 = Moves
    /// 7 = Writing in the guest book
    /// </param>
    /// <returns></returns>
    public async Task<ApiResult<History>> History(int action)
    {
        if (action > 7 && action < 0)
            throw new ArgumentException("Must be within 0 to 7.");

        var uriWithParameters = ApiRoutes.History(action);
        return await _httpClient.SendAsync<History>(new HttpRequestMessage(HttpMethod.Get, uriWithParameters));
    }

    public async Task<ApiResult<CurrentPositionDto>> CurrentPosition()
    {
        var result = await _httpClient.SendAsync<CurrentPositionDto>(new HttpRequestMessage(HttpMethod.Get, ApiRoutes.CurrentPosition));

        // So in my opinion, this is abit badly designed.
        // the api returns 404 IF the user has not moved from the start, why not just return OK WITH the starting coordinates.
        // I think it might be related that maybe it checks some other variable, like history or something. who knows.
        // We'll use this really bad code to deal with this situation
        if (result.StatusCode == HttpStatusCode.NotFound)
        {
            // and yes, those are the starting location from the docs. We'll fix this later.
            return ApiResult<CurrentPositionDto>.Success(new CurrentPositionDto() { X = 500, Y = 900 });
        }

        else
            return result;
    }


    public async Task<ApiResult<string>> GetJoke()
        => await _httpClient.SendAsync<string>(new HttpRequestMessage(HttpMethod.Get, ApiRoutes.GetJoke));

    public async Task<ApiResult<string>> ThrowDart()
        => await _httpClient.SendAsync<string>(new HttpRequestMessage(HttpMethod.Get, ApiRoutes.ThrowDart));

    public async Task<ApiResult<string>> GetMoominLemonade()
        => await _httpClient.SendAsync<string>(new HttpRequestMessage(HttpMethod.Get, ApiRoutes.GetMoominLemonade));

    public async Task<ApiResult<string>> OrderPizza(int toppingsCount = 1)
    {
        if (toppingsCount > 5 || toppingsCount < 1)
            throw new ArgumentOutOfRangeException("Only 1 to 5 toppings allowed."); // Technically could be ArgumentException? I kinda only see this for data structure limits.

        var uriWithParameters = ApiRoutes.OrderPizza(toppingsCount);

        return await _httpClient.SendAsync<string>(new HttpRequestMessage(HttpMethod.Get, uriWithParameters));
    }

    public async Task<ApiResult<List<string>>> ReadMessages()
        => await _httpClient.SendAsync<List<string>>(new HttpRequestMessage(HttpMethod.Get, ApiRoutes.ReadMessages));

    /* PUT */

    // PlayerManager has quite a bit of checks before we even finish completing a call to the endpoint.
    public async Task<ApiResult<string>> Move(int xCoordinates, int yCoordinates)
    {
        // An issue as it is non-descriptive, it can be for many reasons. Could apply Result pattern for this or simply tuple or something
        // but we'll keep it like this for now because the scale keeps getting bigger.
        if (!_playerManager.CanMove(new(xCoordinates, yCoordinates)))
            return ApiResult<string>.Failure(HttpStatusCode.BadRequest, "Invalid movements.");

        var uriWithParameters = ApiRoutes.Move(xCoordinates, yCoordinates);

        var result = await _httpClient.SendAsync<string>(new HttpRequestMessage(HttpMethod.Put, uriWithParameters));

        // since it succeeded, update the coordinates.
        if (result.IsSuccess)
            _playerManager.UpdatePlayerCoordinates(new(xCoordinates, yCoordinates));

        return result;
    }

    public async Task<ApiResult<string>> WC()
        => await _httpClient.SendAsync<string>(new HttpRequestMessage(HttpMethod.Put, ApiRoutes.WC));




    /* POST */
    public async Task<ApiResult<string>> WriteMessage(string title, string message, string author = "")
    {
        // We'll throw, since we are not supposed to be using this with empty details anyways and returning result I feel like gives a wrong impression.
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(message))
            throw new ArgumentException($"{nameof(title)} or {nameof(message)} cannot be empty or null.");

        // If author isn't empty, we'll use it but if it is, we'll get the player name instead.
        author = (string.IsNullOrWhiteSpace(author)) ? _playerManager.PlayerName : author;

        GuestBookEntry entry = new GuestBookEntry(title, message, author);

        using HttpRequestMessage httpMessage = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.WriteMessage);
        httpMessage.Content = JsonContent.Create(entry); // HttpContent, my archnemesis. I would rather use PostAsAsync with httpclient, but alas.

        return await _httpClient.SendAsync<string>(httpMessage);
    }

    /* DELETE */

    public async Task<ApiResult<string>> WCFlush()
        => await _httpClient.SendAsync<string>(new HttpRequestMessage(HttpMethod.Delete, ApiRoutes.WCFlush));

    public async Task<ApiResult<string>> Reset()
    => await _httpClient.SendAsync<string>(new HttpRequestMessage(HttpMethod.Delete, ApiRoutes.Reset));
}
