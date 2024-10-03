using Microsoft.AspNetCore.Mvc;
using StratzAPI.Repositories;
using StratzAPI.Services;

namespace StratzAPI.Controllers
{
    [ApiController]
    [Route("/players")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly PlayerRepository _playerRepository;

        public PlayerController(ILogger<PlayerController> logger, PlayerRepository playerRepository)
        {
            _logger = logger;
            _playerRepository = playerRepository;
        }

        [HttpPost]
        public async Task<IActionResult> PostPlayer(long playerId)
        {
            _logger.LogInformation("Ingresando jugador con id {playerId}", playerId);
            try
            {
                await _playerRepository.GetPlayerData(playerId);
                return Ok("Datos del jugador guardados correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
