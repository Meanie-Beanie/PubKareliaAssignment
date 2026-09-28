namespace PubKarelia.Client;

internal class Program
{
    static void Main(string[] args)
    {
        string apiKey = Environment.GetEnvironmentVariable("api_key") ?? throw new ArgumentNullException("You have not set api_key secret in VS environmental secrets");

        using HttpClient client = new HttpClient()
        {
            BaseAddress = new("https://pubkareliaapi20250319124213-g2f5c0apewdcb7ae.northeurope-01.azurewebsites.net/PubKarelia")
        };

        // Since most call require it anyways, we'll set it here.
        client.DefaultRequestHeaders.Add("TIKO-AUTH", apiKey);

        App app = new(client);
    }
}