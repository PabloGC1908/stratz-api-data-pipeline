using Microsoft.AspNetCore.Mvc;
using StratzAPI.Repositories;
using StratzAPI.Services;

namespace StratzAPI.Controllers
{
    [ApiController]
    [Route("/teams")]
    public class TeamController : ControllerBase
    {
        private readonly ILogger<MatchController> _logger;
        private readonly TeamRepository _teamRepository;

        public TeamController(TeamRepository teamRepository, ILogger<MatchController> logger)
        {
            _logger = logger;
            _teamRepository = teamRepository;
        }


        // Usar este metodo solo cuando la consulta de PostLeague no te genere el equipo en la tabla LeagueTeamPlayer
        [HttpPost]
        public async Task<IActionResult> PostTeamInLeague(int teamId, int leagueId)
        {
            _logger.LogInformation("Ingresando liga con id: {leagueId}", leagueId);
            try
            {
                await _teamRepository.SaveTeamInLeague(teamId, leagueId);
                return Ok("Datos guardados correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
