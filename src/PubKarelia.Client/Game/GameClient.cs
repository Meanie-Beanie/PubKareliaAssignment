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
public class GameClient
{
    readonly HtmlClientHelper _httpClient;

    private PlayerCoordinates _PlayerCoordinates;

    private bool firstMove = true;

    public GameClient(HtmlClientHelper httpHelper, PlayerCoordinates playerCoordinates)
    {
        _httpClient = httpHelper;
        _PlayerCoordinates = playerCoordinates;

        // If there are coordinates, player has moved.
        if (_PlayerCoordinates.x != 0 || _PlayerCoordinates.y != 0)
            firstMove = false;
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

        var uriWithParameters = ApiRoutes.OrderPizza + $"?ToppingsCount={toppingsCount}";

        return await _httpClient.SendAsync<string>(new HttpRequestMessage(HttpMethod.Get, uriWithParameters));
    }

    public async Task<ApiResult<List<string>>> ReadMessages()
        => await _httpClient.SendAsync<List<string>>(new HttpRequestMessage(HttpMethod.Get, ApiRoutes.ReadMessages));

    // PUT
    public async Task<ApiResult<string>> Move(int xCoordinates, int yCoordinates)
    {
        if (firstMove)

    }

    // We have certain rules:
    // +-50 pixels allowed max movement PER direction. So 50 X and -Y is still allowed. unless first move
    // Can first mo ve go as long as possible??
    // cannot go outside below 0 or above 1000 boundaries
    private bool CheckMovement(int x, int y)
    {

    }

    // Help - low priority

    // History - Low priority

    // Position - Low Priority
}
