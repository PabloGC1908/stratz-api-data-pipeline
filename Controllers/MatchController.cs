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
        public async Task<IActionResult> PostMatch(long id)
        {
            _logger.LogInformation($"Ingresando partida con id: {id}");
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
