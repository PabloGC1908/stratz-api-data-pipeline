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

        public async Task<MatchDto> GetMatchDataAsync(long matchId)
        {
            var query = new GraphQLRequest
            {
                Query = @"
            query($id: Long!) {
                match(id: $id) {
                    id
                    didRadiantWin
                    durationSeconds
                    startDateTime
                    endDateTime
                    firstBloodTime
                    leagueId
                    radiantTeamId
                    direTeamId
                    gameVersionId
                    winRates
                    predictedWinRates
                    radiantKills
                    direKills
                    radiantNetworthLeads
                    radiantExperienceLeads
                    pickBans {
                        isPick
                        heroId
                        order
                        isRadiant
                        isCaptain
                        playerIndex
                    }
                    players {
                        steamAccountId
                        isRadiant
                        isVictory
                        heroId
                        kills
                        deaths
                        assists
                        numLastHits
                        numDenies
                        goldPerMinute
                        networth
                        experiencePerMinute
                        level
                        gold
                        goldSpent
                        heroDamage
                        towerDamage
                        heroHealing
                        lane
                        position
                        role
                        roleBasic
                        award
                        item0Id
                        item1Id
                        item2Id
                        item3Id
                        item4Id
                        item5Id
                        backpack0Id
                        backpack1Id
                        backpack2Id
                        neutral0Id
                    }
                }
            }",
                Variables = new { id = matchId }
            };

            var response = await _client.SendQueryAsync<MatchResponseType>(query);
            

            if (response == null || response.Data == null)
            {
                _logger.LogError("No se pudo mapear correctamente.");
                return null;
            }

            long matchResponseId = response.Data.match.Id;

            _logger.LogInformation("Se mapeo correctamente, id de la partida: {matchResponseId}", matchResponseId);

            return response.Data.match;
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
                _logger.LogError("Excepción durante la consulta GraphQL: {Message}", ex.Message);
                return default;
            }
        }
    }
}
