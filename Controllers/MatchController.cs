using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using StratzAPI.Services;


namespace StratzAPI.Controllers
{
    [ApiController]
    [Route("/matches")]
    public class MatchController : ControllerBase
    {
        private readonly ILogger<MatchController> _logger;

        public MatchController(ILogger<MatchController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostMatch(long matchId)
        {
            _logger.LogInformation("Ingresando partida con id: {matchId}", matchId);


            try
            {
                return Ok("Datos guardados correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostLeagueMatches(int leagueId)
        {
            _logger.LogInformation("Ingresando partidas de la liga con id: {leagueId}", leagueId);


            try
            {
                return Ok("Datos guardados correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostSerie(long serieId)
        {
            _logger.LogInformation("Ingresando serie con id: {serieId}", serieId);


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
