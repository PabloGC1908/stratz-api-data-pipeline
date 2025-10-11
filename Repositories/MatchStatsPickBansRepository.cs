using StratzAPI.Data;
using StratzAPI.DTOs.Match;
using StratzAPI.Models;
using StratzAPI.Services;

namespace StratzAPI.Repositories;

public class MatchStatsPickBansRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<MatchStatsPickBansRepository> _logger;

    public MatchStatsPickBansRepository(AppDbContext context, ILogger<MatchStatsPickBansRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ProcessMatchPickBan(ICollection<MatchPickBansDto> pickBansDto, long matchId)
    {
        if (pickBansDto == null || pickBansDto.Count == 0)
        {
            _logger.LogWarning("No hay picks o bans para el match {matchId}", matchId);
            return;
        }

        foreach (var pickBan in pickBansDto)
        {
            MatchPickBans matchPickBans = Map(pickBan, matchId);

            await _context.MatchPickBans.AddAsync(matchPickBans);
            await _context.SaveChangesAsync();
        }
    }

    public MatchPickBans Map(MatchPickBansDto pickBansDto, long matchId)
    {
        return new MatchPickBans
        {
            MatchId = matchId,
            IsPick = pickBansDto.IsPick,
            HeroId = pickBansDto.HeroId,
            Order = pickBansDto.Order,
            IsRadiant = pickBansDto.IsRadiant,
            PlayerIndex = pickBansDto.PlayerIndex,
            IsCaptain = pickBansDto.IsCaptain ?? false
        };
    }
}
