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
    }
}
