using Microsoft.Extensions.Logging;
using StratzAPI.DTOs.Match;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StratzAPI.Services
{
    public class GraphQLService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GraphQLService> _logger;

        private static readonly Queue<DateTime> _requestTimestamps = new();
        private static readonly object _lock = new();
        private const int MAX_REQUESTS_PER_MINUTE = 250;
        private static readonly TimeSpan WINDOW = TimeSpan.FromMinutes(1);

        private int _backoffMinutes = 2;
        private const int MAX_BACKOFF_MINUTES = 30;
        private readonly Random _random = new();

        public GraphQLService(ILogger<GraphQLService> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.stratz.com/graphql")
            };

            var STRATZ_API_KEY = Environment.GetEnvironmentVariable("STRATZ_API_KEY")
                ?? "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJTdWJqZWN0IjoiMDRhNjdhMDMtODNkZi00OGE3LWEzM2MtYjAzZjYxMWFhZDllIiwiU3RlYW1JZCI6IjIwNDYxNzQ2MiIsIkFQSVVzZXIiOiJ0cnVlIiwibmJmIjoxNzYwMTM1NjQxLCJleHAiOjE3OTE2NzE2NDEsImlhdCI6MTc2MDEzNTY0MSwiaXNzIjoiaHR0cHM6Ly9hcGkuc3RyYXR6LmNvbSJ9.BKM3-PVkahqDzuOnlzpccv_O1IozBkQ-MJWMQJNJjj8";

            _logger.LogInformation("STRATZ_API_KEY detectada: {key}", string.IsNullOrEmpty(STRATZ_API_KEY) ? "NO DETECTADA" : "[OK]");

            if (string.IsNullOrWhiteSpace(STRATZ_API_KEY))
            {
                _logger.LogError("No se encontró STRATZ_API_KEY. No se podrá autenticar contra STRATZ.");
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", STRATZ_API_KEY);
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "STRATZ_API");
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public async Task<T?> SendGraphQLQueryAsync<T>(string query, object? variables = null)
        {
            var payload = new { query, variables };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            while (true)
            {
                await WaitForRateLimitAsync();

                try
                {
                    _logger.LogInformation("Enviando consulta GraphQL a STRATZ...");
                    var response = await _httpClient.PostAsync("", content);

                    if ((int)response.StatusCode == 429)
                    {
                        await HandleBackoffAsync("STRATZ devolvió 429 (Too Many Requests)");
                        continue;
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError("Error HTTP {StatusCode} - {ReasonPhrase}\nContenido: {Error}",
                            response.StatusCode, response.ReasonPhrase, errorContent);

                        if ((int)response.StatusCode >= 500)
                        {
                            await HandleBackoffAsync("Error 5xx del servidor STRATZ");
                            continue;
                        }

                        return default;
                    }

                    var responseString = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);

                    if (jsonResponse.TryGetProperty("errors", out var errors))
                    {
                        _logger.LogError("Errores GraphQL detectados: {Errors}", errors.ToString());
                        return default;
                    }

                    _logger.LogInformation("Consulta ejecutada correctamente.");
                    _backoffMinutes = 2;

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return jsonResponse.GetProperty("data").Deserialize<T>(options);
                }
                catch (HttpRequestException ex)
                {
                    await HandleBackoffAsync($"Error de red: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Excepción inesperada: {Message}", ex.Message);
                    await HandleBackoffAsync("Error general durante la petición");
                }
            }
        }

        private static async Task WaitForRateLimitAsync()
        {
            while (true)
            {
                lock (_lock)
                {
                    var now = DateTime.UtcNow;

                    // limpia peticiones antiguas
                    while (_requestTimestamps.Count > 0 && now - _requestTimestamps.Peek() > WINDOW)
                        _requestTimestamps.Dequeue();

                    if (_requestTimestamps.Count < MAX_REQUESTS_PER_MINUTE)
                    {
                        _requestTimestamps.Enqueue(now);

                        // log cada 25 peticiones (opcional)
                        if (_requestTimestamps.Count % 25 == 0)
                        {
                            Console.WriteLine($"[INFO] Peticiones en el último minuto: {_requestTimestamps.Count}/{MAX_REQUESTS_PER_MINUTE}");
                        }

                        return;
                    }
                }

                await Task.Delay(200);
            }
        }

        private async Task HandleBackoffAsync(string reason)
        {
            int jitter = _random.Next(0, 30);
            _logger.LogWarning("{reason}. Esperando {minutes} minutos (+{jitter}s)...", reason, _backoffMinutes, jitter);

            await Task.Delay(TimeSpan.FromMinutes(_backoffMinutes).Add(TimeSpan.FromSeconds(jitter)));

            _backoffMinutes = Math.Min(_backoffMinutes * 2, MAX_BACKOFF_MINUTES);
        }
    }
}

