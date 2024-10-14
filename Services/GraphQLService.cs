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

            _client.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", $"{STRATZ_API_KEY}");
        }

        public async Task<T?> SendGraphQLQueryAsync<T>(string query, object variables)
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
                            if (error.Message.Contains("429") || error.Message.Contains("Too Many Requests"))
                            {
                                _logger.LogWarning("Se ha excedido el límite de peticiones a la API. Mensaje: {Message}", error.Message);
                            }
                            else
                            {
                                _logger.LogError("GraphQL Error: {Message}", error.Message);
                            }
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
                _logger.LogError("Excepción durante la consulta GraphQL: {Message}", ex.Message);
                return default;
            }
        }

    }
}
