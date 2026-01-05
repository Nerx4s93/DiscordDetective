using System.Threading.Tasks;
using StackExchange.Redis;

namespace DiscordDetective.Pipeline;

internal static class RedisDatabaseFactory
{
    public static async Task<IDatabase> Create()
    {
        var options = new ConfigurationOptions
        {
            EndPoints = { "127.0.0.1:6367" },
            AbortOnConnectFail = false,
            ConnectTimeout = 2000,
            ConnectRetry = 2
        };

        var redis = await ConnectionMultiplexer.ConnectAsync(options);
        var database = redis.GetDatabase();

        return database;
    }
}
