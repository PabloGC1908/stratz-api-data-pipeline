using Microsoft.AspNetCore.Mvc;
using StratzAPI.Repositories;
using StratzAPI.Services;
using System.Text;


namespace StratzAPI.Controllers
{
    [ApiController]
    [Route("/matches")]
    public class MatchController : ControllerBase
    {
        private readonly ILogger<MatchController> _logger;
        private readonly MatchRepository _matchRepository;
        private readonly GraphQLService _graphQLService;

        public MatchController(ILogger<MatchController> logger, MatchRepository matchRepository, GraphQLService graphQLService)
        {
            _logger = logger;
            _matchRepository = matchRepository;
            _graphQLService = graphQLService;
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

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            string query = "{ match(id: 8490867982) { id didRadiantWin } }";
            var result = await _graphQLService.SendGraphQLQueryAsync<dynamic>(query);
            return Ok(result);
        }

        [HttpPost("actualizar")]
        public async Task<IActionResult> PostUpdateMatch(long matchId)
        {
            _logger.LogInformation("Actualizando partida con id: {matchId}", matchId);

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
