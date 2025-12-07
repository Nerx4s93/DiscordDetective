using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using DiscordDetective.Px6Api.ApiModels;
using DiscordDetective.Px6Api.DTOModels;

using Nager.Country;

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

    public async Task<CountriesDTO> GetCountriesAsync(ProxyVersion proxyVersion = ProxyVersion.IPv6)
    {
        var response = await GetAsync<CountriesApi>($"{BaseUrl}/{_apiKey}/getcountry?version={(int)proxyVersion}");

        var countryProvider = new CountryProvider();
        var countriesDTO = new CountriesDTO()
        {
            Iso2Code = response.CountriesList,
            CountriesList = response.CountriesList.Select(c => countryProvider.GetCountry(c).OfficialName).ToList()
        };
        return countriesDTO;
    }

    #region Формирвоание запроса

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
