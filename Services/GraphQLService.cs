using Microsoft.Extensions.Logging;
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

        public GraphQLService(ILogger<GraphQLService> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.stratz.com/graphql")
            };

            // 🔑 Token STRATZ (usa variable de entorno en producción)
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

        public async Task<T?> SendGraphQLQueryAsync<T>(string query, object variables = null)
        {
            var payload = new
            {
                query,
                variables
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                _logger.LogInformation("Enviando consulta GraphQL a STRATZ...");
                var response = await _httpClient.PostAsync("", content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error HTTP: {StatusCode} - {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Contenido del error: {Error}", errorContent);
                    return default;
                }

                var responseString = await response.Content.ReadAsStringAsync();

                // STRATZ devuelve JSON con propiedad "data"
                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);

                if (jsonResponse.TryGetProperty("errors", out var errors))
                {
                    _logger.LogError("Errores GraphQL detectados: {Errors}", errors.ToString());
                    return default;
                }

                _logger.LogInformation("Consulta ejecutada correctamente.");
                var data = jsonResponse.GetProperty("data").Deserialize<T>();

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError("Excepción al realizar consulta GraphQL: {Message}", ex.Message);
                return default;
            }
        }
    }
}

