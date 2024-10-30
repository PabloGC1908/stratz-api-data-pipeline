using GraphQL.Validation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StratzAPI.Data;
using StratzAPI.DTOs.League.Serie;
using StratzAPI.Models;
using StratzAPI.Services;

namespace StratzAPI.Repositories;

public class SerieRepository
{
    private readonly GraphQLService _graphQLService;
    private readonly MatchRepository _matchRepository;
    private readonly ILogger<SerieRepository> _logger;
    private readonly AppDbContext _context;

    public SerieRepository(GraphQLService graphQLService, MatchRepository matchRepository, 
                            ILogger<SerieRepository> logger, AppDbContext context)
    {
        _graphQLService = graphQLService;
        _matchRepository = matchRepository;
        _logger = logger;
        _context = context;
    }

    public async Task GetOrFetchLeagueSeries(int leagueId)
    {
        League? league = await _context.League.FindAsync(leagueId);

        if (league == null)
        {
            _logger.LogWarning("No se encontro el id de la liga en la base de datos, agregandolo a la base de datos con los jugadores");
        }
        else
        {
            _logger.LogInformation("Se encontro la liga, extrayendo las series de esta");

            var leagueSeriesDto = await GetLeagueSeries(leagueId);

            if (leagueSeriesDto == null)
            {
                _logger.LogError("No se obtuvo ninguna serie para la liga con ID {leagueId}", leagueId);
                throw new Exception("Problema al momento de extraer la data");
            }

            if (leagueSeriesDto.Series == null)
            {
                throw new Exception("Problema al mapeo de las series");
            }

            if (leagueSeriesDto.Series == null || !leagueSeriesDto.Series.Any())
            {
                _logger.LogWarning("No se encontraron series para la liga con ID {leagueId}", leagueId);
                return;
            }


            await ProcessLeagueSeries(leagueSeriesDto, leagueId);
        }
    }

    public async Task<LeagueSerieDto?> GetLeagueSeries(int leagueId)
    {
        const string query = @"
        query ($leagueId: Int!) {
            league(id: $leagueId) {
                series(take: 1000) {
                    id
                    matches {
                        id
                    }
                    type
                }
            }
        }";

        _logger.LogInformation("Consulta enviada: {Query}", query);

        var leagueSerieResponse = await _graphQLService.SendGraphQLQueryAsync<LeagueSerieResponseType>(query, new { leagueId });
        _logger.LogInformation("Respuesta de la API: {Response}", JsonConvert.SerializeObject(leagueSerieResponse));


        if (leagueSerieResponse == null || leagueSerieResponse.League == null)
        {
            _logger.LogError("No se obtuvo ninguna respuesta de la consulta o la estructura es incorrecta.");
            return null;
        }

        var series = leagueSerieResponse.League.Series;
        if (series == null || !series.Any())
        {
            _logger.LogWarning("No se encontraron series para la liga con ID {leagueId}.", leagueId);
            return null;
        }

        _logger.LogInformation("Se extrajo las series de la liga correctamente");

        return leagueSerieResponse.League;
    }

    public async Task ProcessLeagueSeries(LeagueSerieDto leagueSerieDto, int leagueId)
    {
        foreach (var leagueSerie in leagueSerieDto.Series)
        {
            await ProcessSerie(leagueSerie.Matches, leagueId, leagueSerie.Id, leagueSerie.Type);
        }
    }

    public async Task ProcessSerie(ICollection<MatchIdDto> matchIdDtos, int leagueId, long serieId, string type)
    {
        foreach (var matchId in matchIdDtos)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _matchRepository.GetOrFetchMatch(matchId.Id);

                bool serieExists = await _context.Serie.AnyAsync(s => s.Id == serieId && s.MatchId == matchId.Id);
                if (serieExists)
                {
                    _logger.LogInformation("La serie con Id {serieId} y MatchId {matchId.Id} ya existe en la base de datos. Continuando con el siguiente valor...", serieId, matchId.Id);
                    continue;
                }

                Serie serie = Map(serieId, leagueId, matchId.Id, type);
                await _context.Serie.AddAsync(serie);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error en alguna parte del ingreso de datos: {ex.Message}", ex.Message);
                await transaction.RollbackAsync();
            }
        }
    }


    public Serie Map(long serieId, int leagueId, long matchId, string type)
    {
        return new Serie
        {
            SerieId = serieId,
            LeagueId = leagueId,
            MatchId = matchId,
            Type = type ?? string.Empty,
            Phase = "",
        };
    }
}

