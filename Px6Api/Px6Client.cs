using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using DiscordDetective.Px6Api.ApiModels;

namespace DiscordDetective.Px6Api;

public class Px6Client : IDisposable
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    private const string BaseUrl = "https://px6.link/api";

    public Px6Client(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };
    }

    public async Task<GetCountryResponse> GetCountriesAsync(
        ProxyVersion proxyVersion = ProxyVersion.IPv6)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("version", (int)proxyVersion)
            .Build();

        var url = BuilUrl("getcountry", parameters);
        return await GetAsync<GetCountryResponse>(url);
    }

    public async Task<GetCountResponse> GetProxyCountAsync(
        string countryIso2, ProxyVersion proxyVersion = ProxyVersion.IPv6)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("country", countryIso2)
            .AddParameter("version", (int)proxyVersion)
            .Build();

        var url = BuilUrl("getcount", parameters);
        return await GetAsync<GetCountResponse>(url);
    }

    public async Task<GetProxyResponse> GetProxiesAsync(
        ProxyState state = ProxyState.All, string? description = null,
        bool noKey = false, int page = 1, int limit = 1000)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("state", state, ProxyState.All)
            .AddParameterIf(!string.IsNullOrEmpty(description), "descr", description!)
            .AddParameter("nokey", noKey, false)
            .AddParameterIf(page > 1, "page", page, 1)
            .AddParameterIf(limit != 1000, "limit", limit, 1000)
            .Build();

        var url = BuilUrl("getproxy", parameters);
        return await GetAsync<GetProxyResponse>(url);
    }

    public async Task<ApiResponse> SetProxyTypeAsync(
        List<int> proxyIds, ProxyProtocol protocol)
    {
        if (proxyIds == null || !proxyIds.Any())
        {
            throw new ArgumentException("Proxy IDs cannot be null or empty", nameof(proxyIds));
        }

        var parameters = QueryParametersBuilder.Create()
            .AddParameter("ids", proxyIds)
            .AddParameter("type", protocol.ToString().ToLower())
            .Build();

        var url = BuilUrl("settype", parameters);
        return await GetAsync<ApiResponse>(url);
    }

    public async Task<CheckResponse> CheckProxyAsync(
        int proxyId)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("ids", proxyId)
            .Build();

        var url = BuilUrl("check", parameters);
        return await GetAsync<CheckResponse>(url);
    }

    public async Task<CheckResponse> CheckProxyAsync(
        string proxyIpPortUserPass)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("proxy", proxyIpPortUserPass)
            .Build();

        var url = BuilUrl("check", parameters);
        return await GetAsync<CheckResponse>(url);
    }

    public async Task<SetDescriptionResponse> SetProxyDescriptionAsync(
        List<int> proxyIds, string newDescription)
    {
        if (proxyIds == null || !proxyIds.Any())
        {
            throw new ArgumentException("Proxy IDs cannot be null or empty", nameof(proxyIds));
        }

        var parameters = QueryParametersBuilder.Create()
            .AddParameter("ids", proxyIds)
            .AddParameter("new", newDescription)
            .Build();

        var url = BuilUrl("setdescr", parameters);
        return await GetAsync<SetDescriptionResponse>(url);
    }

    public async Task<SetDescriptionResponse> SetProxyDescriptionAsync(
        string oldDescription, string newDescription)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("old", oldDescription)
            .AddParameter("new", newDescription)
            .Build();

        var url = BuilUrl("setdescr", parameters);
        return await GetAsync<SetDescriptionResponse>(url);
    }

    #region Формирвоание запроса

    private string BuilUrl(string endpoint, string parameters)
    {
        return $"{BaseUrl}/{_apiKey}/{endpoint}{parameters}";
    }

    private async Task<T> GetAsync<T>(string url) where T : ApiResponse
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(json, _jsonOptions);

            if (!result!.IsSuccess)
            {
                throw new Px6ApiException(result.ErrorId ?? 0, result.Error);
            }

            return result;
        }
        catch (HttpRequestException ex)
        {
            throw new Px6ApiException(0, $"HTTP error: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new Px6ApiException(0, $"JSON parsing error: {ex.Message}", ex);
        }
    }

    #endregion

    public void Dispose()
    {
        _httpClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}
