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
                throw new Exception("Problema al momento de extraer la data");
            }

            await ProcessLeagueSeries(leagueSeriesDto, leagueId);
        }
    }

    public async Task<LeagueSerieDto?> GetLeagueSeries(int leagueId)
    {
        const string query = @"
            query($leagueId: Int!) {
                league(id: $leagueId) {
                    series(take: 1000) {
                        id
                        type
                        matches {
                            id
                        }
                    }
                }
            }";

        var leagueSerieResponse = await _graphQLService.SendGraphQLQueryAsync<LeagueSerieDto>(query, new { leagueId });

        if (leagueSerieResponse == null)
        {
            _logger.LogError("No se extrajo la data correctamente");
            return null;
        }

        _logger.LogInformation("Se extrajo las series de la liga correctamente");

        return leagueSerieResponse;
    }

    public async Task ProcessLeagueSeries(LeagueSerieDto leagueSerieDto, int leagueId)
    {
        foreach (var leagueSerie in leagueSerieDto.Series)
        {
            await ProcessSerie(leagueSerie.Matches, leagueId);
        }
    }

    public async Task ProcessSerie(ICollection<MatchIdDto> matchIdDtos, int leagueId)
    {
        foreach(var matchId in matchIdDtos)
        {
            await _matchRepository.GetOrFetchMatch(matchId.Id);
        }
    }

    public Serie Map(SerieDto serieDto, int leagueId)
    {
        return new Serie
        {
            Id = serieDto.Id,
            LeagueId = leagueId,
            Type = serieDto.Type,
            Phase = "",
        };
    }
}

