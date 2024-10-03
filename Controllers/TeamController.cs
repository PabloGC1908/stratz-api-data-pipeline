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

        [HttpPost]
        public async Task<IActionResult> PostTeam(int matchId)
        {
            _logger.LogInformation($"Ingresando equipo con id: {matchId}");
            try
            {
                await _teamRepository.GetTeamData(matchId);
                return Ok("Datos guardados correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
