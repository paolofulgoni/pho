using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Pho.Core.Services;
using Pho.Web.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Pho.Web.Controllers
{
    /// <summary>
    /// Asteroids API
    /// </summary>
    [ApiController]
    [Route("asteroids")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class AsteroidsController : ControllerBase
    {
        private const int _maxAsteroids = 3;
        private readonly IHazardousAsteroidsService _hazardousAsteroidsService;

        public AsteroidsController(IHazardousAsteroidsService hazardousAsteroidsService)
        {
            _hazardousAsteroidsService = hazardousAsteroidsService;
        }

        /// <summary>
        /// Top 3 largest asteroids with potential risk of impact in the next specified days.
        /// </summary>
        /// <param name="days">Days range from today. It must be between 0 and 7.</param>
        /// <returns>Up to 3 asteroids, ordered by descending diameter</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<ActionResult<IEnumerable<AsteroidViewModel>>> Get([Required, Range(0, 7)] int days)
        {
            var asteroids = await _hazardousAsteroidsService.GetLargestHazardousAsteroids(days, _maxAsteroids);

            var response = asteroids
                .Select(asteroid => AsteroidViewModel.From(asteroid))
                .OrderByDescending(asteroidViewModel => asteroidViewModel.Diameter)
                .ToList();

            return response;
        }
    }
}
