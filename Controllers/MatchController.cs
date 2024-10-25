using Microsoft.AspNetCore.Mvc;
using StratzAPI.Repositories;


namespace StratzAPI.Controllers
{
    [ApiController]
    [Route("/matches")]
    public class MatchController : ControllerBase
    {
        private readonly ILogger<MatchController> _logger;
        private readonly MatchRepository _matchRepository;

        public MatchController(ILogger<MatchController> logger, MatchRepository matchRepository)
        {
            _logger = logger;
            _matchRepository = matchRepository;
        }

        [HttpPost]
        public async Task<IActionResult> PostMatch(long matchId)
        {
            _logger.LogInformation("Ingresando partida con id: {matchId}", matchId);
            await _matchRepository.GetOrFetchMatch(matchId);

            try
            {
                return Ok("Datos guardados correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
