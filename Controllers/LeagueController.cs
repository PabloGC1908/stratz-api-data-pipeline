using Microsoft.AspNetCore.Mvc;
using StratzAPI.Repositories;
using StratzAPI.Services;

namespace StratzAPI.Controllers
{
    [ApiController]
    [Route("/leagues")]
    public class LeagueController : ControllerBase
    {
        private readonly ILogger<MatchController> _logger;
        private readonly LeagueRepository _leagueRepository;

        public LeagueController(LeagueRepository leagueRepository, ILogger<MatchController> logger)
        {
            _logger = logger;
            _leagueRepository = leagueRepository;
        }


        // Crea la liga que se ingresa y los equipos que participan en ella
        [HttpPost]
        public async Task<IActionResult> PostLeague(int leagueId)
        {
            _logger.LogInformation("Ingresando partida con id: {leagueId}", leagueId);
            try
            {
                await _leagueRepository.GetOrFetchLeagueAsync(leagueId);
                return Ok("Datos guardados correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
