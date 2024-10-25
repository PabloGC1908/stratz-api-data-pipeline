using Microsoft.AspNetCore.Mvc;
using StratzAPI.Repositories;

namespace StratzAPI.Controllers
{
    [ApiController]
    [Route("/series")]
    public class SerieController : ControllerBase
    {
        private readonly ILogger<MatchController> _logger;
        private readonly SerieRepository _serieRepository;

        public SerieController(ILogger<MatchController> logger, SerieRepository serieRepository)
        {
            _logger = logger;
            _serieRepository = serieRepository;
        }

        [HttpPost]
        public async Task<IActionResult> PostLeagueSeries(int leagueId)
        {
            _logger.LogInformation("Ingresando series de la liga: {leagueId}", leagueId);
            await _serieRepository.GetOrFetchLeagueSeries(leagueId);

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
