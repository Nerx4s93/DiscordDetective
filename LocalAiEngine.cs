using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordDetective;

internal static class LocalAiEngine
{
    private static readonly HttpClient _httpClient = new();
    private const string _ollamaBaseURL = "http://127.0.0.1:11434";
    private const string _modelName = "gemma:7b";

    static LocalAiEngine()
    {
        Task.Run(async () =>
        {
            await StartOllamaServer();
            await EnsureModelLoaded();
        });
    }

    public static async Task<string> SendMessageAsync(string message)
    {
        try
        {
            var requestData = new
            {
                model = _modelName,
                prompt = message,
                stream = false
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_ollamaBaseURL}/api/generate", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                using var document = JsonDocument.Parse(responseContent);
                if (document.RootElement.TryGetProperty("response", out JsonElement responseElement))
                {
                    return responseElement.GetString()?.Trim() ?? "Пустой ответ от модели";
                }
                else
                {
                    return "Не удалось получить ответ от модели";
                }
            }
            else
            {
                return $"Ошибка HTTP: {response.StatusCode}";
            }
        }
        catch (Exception ex)
        {
            return $"Ошибка при обращении к AI: {ex.Message}";
        }
    }

    #region Запуск

    private static async Task StartOllamaServer()
    {
        if (await IsOllamaRunning())
        {
            Console.WriteLine("Сервер ollama уже запущен");
            return;
        }

        Console.WriteLine("Запуск сервера ollama...");
        var startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = "/c ollama serve",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = false,
            RedirectStandardError = false
        };

        Process.Start(startInfo);

        while (!await IsOllamaRunning())
        {
            Thread.Sleep(1000);
        }

        Console.WriteLine("Сервер ollama запущен");
    }

    private static async Task EnsureModelLoaded()
    {
        if (await IsModelLoaded())
        {
            Console.WriteLine("Модель gemma:7b уже запущена");
            return;
        }

        Console.WriteLine("Запуск модели gemma:7b...");

        var request = new
        {
            model = "gemma:7b",
            prompt = "",
            stream = false
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await _httpClient.PostAsync("http://127.0.0.1:11434/api/generate", content);

        Console.WriteLine("Модель gemma:7b запущена");
    }

    private static async Task<bool> IsOllamaRunning()
    {
        try
        {
            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(500)
            };

            var resp = await client.GetAsync("http://127.0.0.1:11434/api/tags");
            return resp.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private static async Task<bool> IsModelLoaded()
    {
        try
        {
            var request = new
            {
                name = _modelName
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_ollamaBaseURL}/api/show", content);

            if (!response.IsSuccessStatusCode)
                return false;

            var body = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(body);

            if (doc.RootElement.TryGetProperty("status", out var status))
            {
                return status.GetString() == "loaded";
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    #endregion
}