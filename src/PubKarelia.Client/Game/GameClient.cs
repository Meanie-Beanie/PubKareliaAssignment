using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Net;
using PubKarelia.Client.Dto;
using PubKarelia.Client.Util;

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

    // GET
    public async Task<ApiResult<CurrentPositionDto>> CurrentPosition()
        => await _httpClient.SendAsync<CurrentPositionDto>(new HttpRequestMessage(HttpMethod.Get, ApiRoutes.CurrentPosition));

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

    // PUT

    // PlayerManager has quite a bit of checks before we even finish completing a call to the endpoint.
    public async Task<ApiResult<string>> Move(int xCoordinates, int yCoordinates)
    {
        // An issue as it is non-descriptive, it can be for many reasons. Could apply Result pattern for this or simply tuple or something
        // but we'll keep it like this for now because the scale keeps getting bigger.
        if (!_playerManager.CanMove(xCoordinates, yCoordinates))
            return ApiResult<string>.Failure(HttpStatusCode.BadRequest, "Invalid movements.");

        var uriWithParameters = ApiRoutes.Move(xCoordinates, yCoordinates);

        return await _httpClient.SendAsync<string>(new HttpRequestMessage(HttpMethod.Put, uriWithParameters));
    }

    // Help - low priority

    // History - Low priority

    // Position - Low Priority
}
