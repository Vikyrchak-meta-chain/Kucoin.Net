using Kucoin.Net.Clients;
using Kucoin.Net.Objects;
using Kucoin.Net.Objects.Options;
using KucoinBroker.Net;
using Microsoft.Extensions.Logging;

internal class Program
{
    static bool Authenticated = false;

    // Optimized GetClient method
    static IKucoinRestBrokerClientApi GetClient()
    {
        // Retrieve environment variables
        var key = Environment.GetEnvironmentVariable("APIKEY");
        var sec = Environment.GetEnvironmentVariable("APISECRET");
        var pass = Environment.GetEnvironmentVariable("APIPASS");
        var brokerKey = Environment.GetEnvironmentVariable("ApiBrokerKey");

        // Check authentication status
        Authenticated = !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(sec);

        // Setup logger
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter("Microsoft", LogLevel.Warning)
                   .AddFilter("System", LogLevel.Warning)
                   .AddFilter("SampleApp.Program", LogLevel.Debug);
        });
        var _logger = loggerFactory.CreateLogger<KucoinRestBrokerClientApi>();

        // Create KucoinRestOptions with environment variables (if available)
        var options = new KucoinRestOptions
        {
            ApiCredentials = new KucoinApiCredentials(key, sec, pass),
            AutoTimestamp = true,
        };

        // Create and return the client
        var client = new KucoinRestClient(optionsDelegate =>
        {
            optionsDelegate.ApiCredentials = new KucoinApiCredentials(key, sec, pass);
            optionsDelegate.AutoTimestamp = true;
        });

        return new KucoinRestBrokerClientApi(_logger, null, client, options);
    }

    // Main method to test the client
    private static async Task Main(string[] args)
    {
        try
        {
            var client = GetClient();

            // Test client call (e.g., get server time)
            var result = await client.GetServerTimeAsync();
            Console.WriteLine($"Server Time: {result.Data}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
}
