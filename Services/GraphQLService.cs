using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using StratzAPI.DTOs.Match;
using System.Net.Http.Headers;

namespace StratzAPI.Services
{
    public class GraphQLService
    {
        private readonly GraphQLHttpClient _client;
        private readonly ILogger<GraphQLService> _logger;

        public GraphQLService(ILogger<GraphQLService> logger) 
        {
            _logger = logger;
            _client = new GraphQLHttpClient("https://api.stratz.com/graphql", new NewtonsoftJsonSerializer());

            var STRATZ_API_KEY = Environment.GetEnvironmentVariable("STRATZ_API_KEY");

            _client.HttpClient.DefaultRequestHeaders.Add("User-Agent", "STRATZ_API");
            _client.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", $"{STRATZ_API_KEY}");
        }

        public async Task<T?> SendGraphQLQueryAsync<T>(string query, object variables)
        {
            const int maxRetries = 5;
            int retryCount = 0;
            TimeSpan delay = TimeSpan.FromSeconds(2);

            while (retryCount < maxRetries)
            {
                try
                {
                    var request = new GraphQLRequest
                    {
                        Query = query,
                        Variables = variables
                    };

                    var response = await _client.SendQueryAsync<T>(request);

                    if (response == null || response.Data == null)
                    {
                        if (response?.Errors != null)
                        {
                            foreach (var error in response.Errors)
                            {
                                _logger.LogError("GraphQL Error: {Message}", error.Message);
                            }
                        }
                        else
                        {
                            _logger.LogError("Error desconocido al realizar la consulta GraphQL.");
                        }
                        return default;
                    }

                    _logger.LogInformation("Consulta GraphQL ejecutada correctamente.");
                    return response.Data;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("ServiceUnavailable") || ex.Message.Contains("503"))
                    {
                        retryCount++;
                        _logger.LogWarning("Petición fallida con status 503 (Service Unavailable). Intento {retryCount} de {maxRetries}. " +
                                "Esperando {delay.TotalSeconds} segundos antes del reintento...", retryCount, maxRetries, delay.TotalSeconds);

                        if (retryCount >= maxRetries)
                        {
                            _logger.LogError("Excedido el número máximo de reintentos. No se pudo completar la consulta GraphQL.");
                            return default;
                        }

                        await Task.Delay(delay);
                        delay = delay * 2;
                    }
                    else
                    {
                        _logger.LogError("Excepción durante la consulta GraphQL: {Message}", ex.Message);
                        return default;
                    }
                }
            }

            return default;
        }


    }
}
