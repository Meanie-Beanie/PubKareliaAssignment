namespace PubKarelia.Client;

internal class App
{
    readonly HttpClient _httpClient;

    public App(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


}