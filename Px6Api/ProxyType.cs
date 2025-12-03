using System.ComponentModel;

namespace DiscordDetective.Px6Api;

public enum ProxyType
{
    [Description("http")]
    Https,
    [Description("socks")]
    Socks5
}