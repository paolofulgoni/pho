using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Pho.Core.Aggregates;
using Pho.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace Pho.Web.Controllers
{
    [ApiController]
    [Route("asteroids")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class AsteroidsController : ControllerBase
    {
        private readonly IAsteroidService _asteroidService;

        public AsteroidsController(IAsteroidService asteroidService)
        {
            _asteroidService = asteroidService;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Asteroid>>> Get([Required] int days)
        {
            var asteroids = await _asteroidService.GetPotentiallyHazardousAsteroids(days);

            return Ok(asteroids);
        }
    }
}
