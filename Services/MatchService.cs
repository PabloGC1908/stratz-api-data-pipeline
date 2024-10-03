using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using StratzAPI.Controllers;
using StratzAPI.Data;
using StratzAPI.Models;
using System.Net.Http.Headers;

namespace StratzAPI.Services
{
    public class MatchService
    {
        private readonly ILogger<MatchService> _logger;
        private readonly GraphQLService _graphQLService;
        private readonly AppDbContext _dbContext;

        public MatchService(ILogger<MatchService> logger, GraphQLService graphQLService, AppDbContext dbContext)
        {
            _graphQLService = graphQLService;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task SaveMatchDataAsync(long matchId)
        {
            var matchData = await _graphQLService.GetMatchDataAsync(matchId);

            if (matchData == null)
            {
                _logger.LogError("No se recibió ningún dato de la API GraphQL.");
                return;
            }

            _logger.LogInformation(matchData.ToString());

            var match = new Match
            {
                Id = matchData.Id,
                DidRadiantWin = matchData.DidRadiantWin,
                DurationSeconds = matchData.DurationSeconds,
                StartDateTime = matchData.StartDateTime,
                EndDateTime = matchData.EndDateTime,
                FirstBloodTime = matchData.FirstBloodTime,
                LeagueId = matchData.LeagueId,
                RadiantTeamId = matchData.RadiantTeamId,
                DireTeamId = matchData.DireTeamId,
                GameVersionId = matchData.GameVersionId,
                MatchStatistics = new List<MatchStatistics>()
            };

            var matchStatistics = matchData.WinRates.Select((winRate, index) => new MatchStatistics
            {
                MatchId = matchData.Id,
                Match = match,
                WinRate = winRate,
                PredictedWinRate = matchData.PredictedWinRates[index],
                RadiantKills = matchData.RadiantKills[index],
                DireKills = matchData.DireKills[index],
                RadiantNetworthLead = matchData.RadiantNetworthLeads[index],
                RadiantExperienceLead = matchData.RadiantExperienceLeads[index]
            }).ToList();

            // Guardar los datos en la base de datos
            await _dbContext.Match.AddAsync(match);
            await _dbContext.MatchStatistics.AddRangeAsync(matchStatistics);
            await _dbContext.SaveChangesAsync();
        }


    }
}
