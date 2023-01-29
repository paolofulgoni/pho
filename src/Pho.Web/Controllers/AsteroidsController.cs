using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Pho.Core.Services;
using Pho.Web.ViewModels;
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

        /// <summary>
        /// Top 3 largest asteroids with potential risk of impact in the next specified days.
        /// </summary>
        /// <param name="days">Days range from today. It must be between 0 and 1000.</param>
        /// <returns>Up to 3 asteroids, ordered by descending diameter</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AsteroidViewModel>>> Get([Required, Range(0, 1000)] int days)
        {
            var asteroids = await _asteroidService.GetLargestPotentiallyHazardousAsteroids(days);

            var response = asteroids
                .Select(asteroid => AsteroidViewModel.From(asteroid))
                .OrderByDescending(asteroidViewModel => asteroidViewModel.Diameter)
                .ToList();

            return response;
        }
    }
}
