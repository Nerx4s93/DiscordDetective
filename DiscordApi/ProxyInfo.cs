namespace DiscordApi;

public sealed class ProxyInfo
{
    public required string Host { get; init; }
    public required int Port { get; init; }

    public string? Username { get; init; }
    public string? Password { get; init; }

    public bool HasCredentials =>!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
}