using System.Threading.Tasks;

using StackExchange.Redis;

namespace DiscordDetective.Pipeline;

internal static class RedisConnector
{
    private static readonly ConfigurationOptions _options = new ConfigurationOptions
    {
        EndPoints = { "127.0.0.1:6367" },
        AbortOnConnectFail = false,
        ConnectTimeout = 2000,
        ConnectRetry = 2
    };

    public static async Task<IConnectionMultiplexer> ConnectAsync()
    {
        return await ConnectionMultiplexer.ConnectAsync(_options);
    }
}