using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Px6Api.ApiModels;

namespace Px6Api;

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

    /// <summary>
    /// Используется для получения информации о сумме заказа в зависимости от версии, периода и кол-ва прокси
    /// </summary>
    /// <param name="count">Кол-во прокси</param>
    /// <param name="period">Период - кол-во дней</param>
    /// <param name="proxyVersion">Версия прокси</param>
    /// <returns></returns>
    public async Task<GetPriceResponse> GetPriceAsync(
        int count, int period, ProxyVersion proxyVersion = ProxyVersion.IPv6)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("count", count)
            .AddParameter("period", period)
            .AddParameter("version", (int)proxyVersion, (int)ProxyVersion.IPv6)
            .Build();

        var url = BuilUrl("getprice", parameters);
        return await GetAsync<GetPriceResponse>(url);
    }

    /// <summary>
    /// Используется для получения информации о доступном для приобретения кол-ве прокси определенной страны
    /// </summary>
    /// <param name="countryIso2">Код страны в формате iso2</param>
    /// <param name="proxyVersion">Версия прокси</param>
    /// <returns></returns>
    public async Task<CountResponse> GetProxyCountAsync(
        string countryIso2, ProxyVersion proxyVersion = ProxyVersion.IPv6)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("country", countryIso2)
            .AddParameter("version", (int)proxyVersion, (int)ProxyVersion.IPv6)
            .Build();

        var url = BuilUrl("getcount", parameters);
        return await GetAsync<CountResponse>(url);
    }

    /// <summary>
    /// Используется для получения информации о доступных для приобретения странах
    /// </summary>
    /// <param name="proxyVersion">Версия прокси</param>
    /// <returns></returns>
    public async Task<GetCountryResponse> GetCountriesAsync(
        ProxyVersion proxyVersion = ProxyVersion.IPv6)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("version", (int)proxyVersion, (int)ProxyVersion.IPv6)
            .Build();

        var url = BuilUrl("getcountry", parameters);
        return await GetAsync<GetCountryResponse>(url);
    }

    /// <summary>
    /// Используется для получения списка ваших прокси.
    /// </summary>
    /// <param name="state">Состояние возвращаемых прокси. Доступные значения: active - Активные, expired - Неактивные, expiring - Заканчивающиеся, all - Все (по-умолчанию)</param>
    /// <param name="description">Технический комментарий, который вы указывали при покупке прокси. Если данный параметр присутствует, то будут выбраны только те прокси, у которых присутствует данный комментарий, если же данный параметр не задан, то будут выбраны все прокси</param>
    /// <param name="noKey">При добавлении данного параметра (значение не требуется), список list будет возвращаться без ключей</param>
    /// <param name="page">Номер страницы для вывода. 1 - по-умолчанию</param>
    /// <param name="limit">Кол-во прокси для вывода в списке. 1000 - по-умолчанию (максимальное значение)</param>
    /// <returns></returns>
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

    /// <summary>
    /// Используется для изменения типа (протокола) у списка прокси.
    /// </summary>
    /// <param name="proxyIds">Перечень внутренних номеров прокси, через запятую</param>
    /// <param name="protocol">Устанавливаемый тип (протокол)</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
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

    /// <summary>
    /// Используется для обновления технического комментария у списка прокси, который был установлен при покупке (метод buy)
    /// </summary>
    /// <param name="proxyIds">Перечень внутренних номеров прокси, через запятую</param>
    /// <param name="newDescription">Технический комментарий, на который нужно изменить. Максимальная длина 50 символов</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<CountResponse> SetProxyDescriptionAsync(
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
        return await GetAsync<CountResponse>(url);
    }

    /// <summary>
    /// Используется для обновления технического комментария у списка прокси, который был установлен при покупке (метод buy)
    /// </summary>
    /// <param name="oldDescription">Технический комментарий, который нужно изменить</param>
    /// <param name="newDescription">Технический комментарий, на который нужно изменить. Максимальная длина 50 символов</param>
    /// <returns></returns>
    public async Task<CountResponse> SetProxyDescriptionAsync(
        string oldDescription, string newDescription)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("old", oldDescription)
            .AddParameter("new", newDescription)
            .Build();

        var url = BuilUrl("setdescr", parameters);
        return await GetAsync<CountResponse>(url);
    }


    /// <summary>
    /// Используется для покупки прокси
    /// </summary>
    /// <param name="count">Кол-во прокси для покупки</param>
    /// <param name="period">Период на который покупаются прокси - кол-во дней</param>
    /// <param name="country">Страна в формате iso2</param>
    /// <param name="description">Технический комментарий для списка прокси, максимальная длина 50 символов. Указание данного параметра позволит вам делать выборку списка прокси про этому параметру через метод getproxy</param>
    /// <param name="autoProlong">При добавлении данного параметра (значение не требуется), у купленных прокси будет включено автопродление</param>
    /// <param name="nokey">При добавлении данного параметра (значение не требуется), список list будет возвращаться без ключей</param>
    /// <param name="proxyVersion">Версия прокси</param>
    /// <param name="proxyProtocol">Тип прокси (протокол)</param>
    /// <returns></returns>
    public async Task<BuyResponse> BuyProxy(
        int count, int period, string country, string description, bool autoProlong, bool nokey,
        ProxyVersion proxyVersion = ProxyVersion.IPv6, ProxyProtocol proxyProtocol = ProxyProtocol.Http)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("count", count)
            .AddParameter("period", period)
            .AddParameter("country", country.ToLower())
            .AddParameter("version", (int)proxyVersion, (int)ProxyVersion.IPv6)
            .AddParameter("type", proxyProtocol, ProxyProtocol.Http)
            .AddParameterIf(!string.IsNullOrWhiteSpace(description), "descr", description)
            .AddParameterIf(autoProlong, "auto_prolong", "true")
            .AddParameterIf(nokey, "nokey", "true")
            .Build();

        var url = BuilUrl("buy", parameters);
        return await GetAsync<BuyResponse>(url);
    }

    /// <summary>
    /// Используется для продления текущих прокси
    /// </summary>
    /// <param name="period">Период продления - кол-во дней</param>
    /// <param name="ids">Перечень внутренних номеров прокси в нашей системе, через запятую</param>
    /// <param name="noKey"></param>
    /// <returns></returns>
    public async Task<ProlongResponse> ProlongProxyAsync(
        int period, List<int> proxyIds, bool noKey = false)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("period", period)
            .AddParameter("ids", proxyIds)
            .AddParameter("noKey", noKey, false)
            .Build();

        var url = BuilUrl("prolong", parameters);
        return await GetAsync<ProlongResponse>(url);
    }

    /// <summary>
    /// Используется для удаления прокси
    /// </summary>
    /// <param name="proxyIds">Перечень внутренних номеров прокси в нашей системе, через запятую</param>
    /// <returns></returns>
    public async Task<CountResponse> DeleteProxyAsync(
        List<int> proxyIds)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("ids", proxyIds)
            .Build();

        var url = BuilUrl("delete", parameters);
        return await GetAsync<CountResponse>(url);
    }

    /// <summary>
    /// Используется для удаления прокси
    /// </summary>
    /// <param name="description">Технический комментарий, который вы указывали при покупке прокси, либо через метод setdescr</param>
    /// <returns></returns>
    public async Task<CountResponse> DeleteProxyAsync(
        string description)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("descr", description)
            .Build();

        var url = BuilUrl("delete", parameters);
        return await GetAsync<CountResponse>(url);
    }

    /// <summary>
    /// Используется для проверки валидности (работоспособности) прокси
    /// </summary>
    /// <param name="proxyId">Внутренний номер прокси</param>
    /// <returns></returns>
    public async Task<CheckResponse> CheckProxyAsync(
        int proxyId)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("ids", proxyId)
            .Build();

        var url = BuilUrl("check", parameters);
        return await GetAsync<CheckResponse>(url);
    }

    /// <summary>
    /// Используется для проверки валидности (работоспособности) прокси
    /// </summary>
    /// <param name="proxyIpPortUserPass">Строка прокси в формате: ip:port:user:pass</param>
    /// <returns></returns>
    public async Task<CheckResponse> CheckProxyAsync(
        string proxyIpPortUserPass)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("proxy", proxyIpPortUserPass)
            .Build();

        var url = BuilUrl("check", parameters);
        return await GetAsync<CheckResponse>(url);
    }

    /// <summary>
    /// Используется для привязки авторизации прокси по ip
    /// </summary>
    /// <param name="ips">Список привязываемых ip-адресов</param>
    /// <returns></returns>
    public async Task<ApiResponse> IpAuthorizationAsync(
        List<string> ips)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("ip", ips)
            .Build();

        var url = BuilUrl("ipauth", parameters);
        return await GetAsync<CheckResponse>(url);
    }

    /// <summary>
    /// Используется для удаления привязки авторизации прокси по ip
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResponse> DeleteIpAuthorizationAsync()
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("ip", "delete")
            .Build();

        var url = BuilUrl("ipauth", parameters);
        return await GetAsync<CheckResponse>(url);
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
