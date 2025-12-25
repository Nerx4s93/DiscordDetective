using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Px6Api.ApiModels;
using Px6Api.DTOModels;

namespace Px6Api;

public class Px6Client(string apiKey) : IDisposable
{
    private const string BaseUrl = "https://px6.link/api";

    private readonly HttpClient _httpClient = new();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = false,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };

    /// <summary>
    /// Используется для получения информации о сумме заказа в зависимости от версии, периода и кол-ва прокси
    /// </summary>
    /// <param name="count">Кол-во прокси</param>
    /// <param name="period">Период - кол-во дней</param>
    /// <param name="proxyVersion">Версия прокси</param>
    /// <returns></returns>
    public async Task<(User user, PriceInfo price)> GetPriceAsync(
        int count, int period, ProxyVersion proxyVersion = ProxyVersion.IPv6)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("count", count)
            .AddParameter("period", period)
            .AddParameter("version", (int)proxyVersion, (int)ProxyVersion.IPv6)
            .Build();

        var url = BuildUrl("getprice", parameters);
        var answer = await GetAsync<GetPriceResponse>(url);

        var priceInfo = new PriceInfo
        {
            Price = answer.Price,
            Count = answer.Count,
            Period = answer.Period,
            PriceSingle = answer.PriceSingle
        };
        return (answer.User, priceInfo);
    }

    /// <summary>
    /// Используется для получения информации о доступном для приобретения кол-ве прокси определенной страны
    /// </summary>
    /// <param name="countryIso2">Код страны в формате iso2</param>
    /// <param name="proxyVersion">Версия прокси</param>
    /// <returns></returns>
    public async Task<(User user, int proxiesCount)> GetProxyCountAsync(
        string countryIso2, ProxyVersion proxyVersion = ProxyVersion.IPv6)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("country", countryIso2)
            .AddParameter("version", (int)proxyVersion, (int)ProxyVersion.IPv6)
            .Build();

        var url = BuildUrl("getcount", parameters);
        var answer = await GetAsync<CountResponse>(url);

        return (answer.User, answer.Count);
    }

    /// <summary>
    /// Используется для получения информации о доступных для приобретения странах
    /// </summary>
    /// <param name="proxyVersion">Версия прокси</param>
    /// <returns></returns>
    public async Task<(User user, List<string> countries)> GetCountriesAsync(
        ProxyVersion proxyVersion = ProxyVersion.IPv6)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("version", (int)proxyVersion, (int)ProxyVersion.IPv6)
            .Build();

        var url = BuildUrl("getcountry", parameters);
        var answer = await GetAsync<GetCountryResponse>(url);

        return (answer.User, answer.CountriesList);
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
    public async Task<(User user, Dictionary<string, ProxyInfo> proxies)> GetProxiesAsync(
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

        var url = BuildUrl("getproxy", parameters);
        var answer = await GetAsync<GetProxyResponse>(url);

        return (answer.User, answer.ProxyList);
    }

    /// <summary>
    /// Используется для изменения типа (протокола) у списка прокси.
    /// </summary>
    /// <param name="proxyIds">Перечень внутренних номеров прокси, через запятую</param>
    /// <param name="protocol">Устанавливаемый тип (протокол)</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<User> SetProxyTypeAsync(List<int> proxyIds, ProxyProtocol protocol)
    {
        if (proxyIds == null || proxyIds.Count == 0)
        {
            throw new ArgumentException("Proxy IDs cannot be null or empty", nameof(proxyIds));
        }

        var parameters = QueryParametersBuilder.Create()
            .AddParameter("ids", proxyIds)
            .AddParameter("type", protocol.ToString().ToLower())
            .Build();

        var url = BuildUrl("settype", parameters);
        var answer = await GetAsync<ApiResponse>(url);

        return answer.User;
    }

    /// <summary>
    /// Используется для обновления технического комментария у списка прокси, который был установлен при покупке (метод buy)
    /// </summary>
    /// <param name="proxyIds">Перечень внутренних номеров прокси, через запятую</param>
    /// <param name="newDescription">Технический комментарий, на который нужно изменить. Максимальная длина 50 символов</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<(User user, int changedCount)> SetProxyDescriptionAsync(List<int> proxyIds, string newDescription)
    {
        if (proxyIds == null || proxyIds.Count == 0)
        {
            throw new ArgumentException("Proxy IDs cannot be null or empty", nameof(proxyIds));
        }

        var parameters = QueryParametersBuilder.Create()
            .AddParameter("ids", proxyIds)
            .AddParameter("new", newDescription)
            .Build();

        var url = BuildUrl("setdescr", parameters);
        var answer = await GetAsync<CountResponse>(url);

        return (answer.User, answer.Count);
    }

    /// <summary>
    /// Используется для обновления технического комментария у списка прокси, который был установлен при покупке (метод buy)
    /// </summary>
    /// <param name="oldDescription">Технический комментарий, который нужно изменить</param>
    /// <param name="newDescription">Технический комментарий, на который нужно изменить. Максимальная длина 50 символов</param>
    /// <returns></returns>ф
    public async Task<(User user, int changedCount)> SetProxyDescriptionAsync(string oldDescription, string newDescription)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("old", oldDescription)
            .AddParameter("new", newDescription)
            .Build();

        var url = BuildUrl("setdescr", parameters);
        var answer = await GetAsync<CountResponse>(url);

        return (answer.User, answer.Count);
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
    public async Task<(User user, BuyInfo buyInfo, Dictionary<string, ProxyInfo> proxyList)> BuyProxy(
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

        var url = BuildUrl("buy", parameters);
        var answer = await GetAsync<BuyResponse>(url);

        return (answer.User, answer.BuyInfo, answer.ProxyList);
    }

    /// <summary>
    /// Используется для продления текущих прокси
    /// </summary>
    /// <param name="period">Период продления - кол-во дней</param>
    /// <param name="proxyIds">Перечень внутренних номеров прокси в нашей системе, через запятую</param>
    /// <param name="noKey"></param>
    /// <returns></returns>
    public async Task<(User user, Dictionary<string, ProxyProlong> proxiesProlongs)> ProlongProxyAsync
        (int period, List<int> proxyIds, bool noKey = false)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("period", period)
            .AddParameter("ids", proxyIds)
            .AddParameter("noKey", noKey, false)
            .Build();

        var url = BuildUrl("prolong", parameters);
        var answer = await GetAsync<ProlongResponse>(url);

        return (answer.User, answer.ProxyList);
    }

    /// <summary>
    /// Используется для удаления прокси
    /// </summary>
    /// <param name="proxyIds">Перечень внутренних номеров прокси в нашей системе, через запятую</param>
    /// <returns></returns>
    public async Task<(User user, int deletedCount)> DeleteProxyAsync(List<int> proxyIds)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("ids", proxyIds)
            .Build();

        var url = BuildUrl("delete", parameters);
        var answer = await GetAsync<CountResponse>(url);

        return (answer.User, answer.Count);
    }

    /// <summary>
    /// Используется для удаления прокси
    /// </summary>
    /// <param name="description">Технический комментарий, который вы указывали при покупке прокси, либо через метод setdescr</param>
    /// <returns></returns>
    public async Task<(User user, int deletedCount)> DeleteProxyAsync(string description)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("descr", description)
            .Build();

        var url = BuildUrl("delete", parameters);
        var answer = await GetAsync<CountResponse>(url);

        return (answer.User, answer.Count);
    }

    /// <summary>
    /// Используется для проверки валидности (работоспособности) прокси
    /// </summary>
    /// <param name="proxyId">Внутренний номер прокси</param>
    /// <returns></returns>
    public async Task<(User user, ProxyStatus proxyStatus)> CheckProxyAsync(int proxyId)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("ids", proxyId)
            .Build();

        var url = BuildUrl("check", parameters);
        var answer = await GetAsync<CheckResponse>(url);

        var proxyStatus = new ProxyStatus { ProxyId = answer.ProxyId, Status = answer.ProxyStatus };
        return (answer.User, proxyStatus);
    }

    /// <summary>
    /// Используется для проверки валидности (работоспособности) прокси
    /// </summary>
    /// <param name="proxyIpPortUserPass">Строка прокси в формате: ip:port:user:pass</param>
    /// <returns></returns>
    public async Task<(User user, ProxyStatus proxyStatus)> CheckProxyAsync(string proxyIpPortUserPass)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("proxy", proxyIpPortUserPass)
            .Build();

        var url = BuildUrl("check", parameters);
        var answer = await GetAsync<CheckResponse>(url);

        var proxyStatus = new ProxyStatus { ProxyId = answer.ProxyId, Status = answer.ProxyStatus };
        return (answer.User, proxyStatus);
    }

    /// <summary>
    /// Используется для привязки авторизации прокси по ip
    /// </summary>
    /// <param name="ips">Список привязываемых ip-адресов</param>
    /// <returns></returns>
    public async Task<User> IpAuthorizationAsync(List<string> ips)
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("ip", ips)
            .Build();

        var url = BuildUrl("ipauth", parameters);
        var answer = await GetAsync<CheckResponse>(url);

        return answer.User;
    }

    /// <summary>
    /// Используется для удаления привязки авторизации прокси по ip
    /// </summary>
    /// <returns></returns>
    public async Task<User> DeleteIpAuthorizationAsync()
    {
        var parameters = QueryParametersBuilder.Create()
            .AddParameter("ip", "delete")
            .Build();

        var url = BuildUrl("ipauth", parameters);
        var answer = await GetAsync<CheckResponse>(url);

        return answer.User;
    }

    #region Формирование запроса

    private string BuildUrl(string endpoint, string parameters)
    {
        return $"{BaseUrl}/{apiKey}/{endpoint}{parameters}";
    }

    private async Task<T> GetAsync<T>(string url) where T : ApiResponse
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(json, _jsonOptions);

            return !result!.IsSuccess ? throw new Px6ApiException(result.ErrorId!.Value, result.Error) : result;
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
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}
