using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Pho.Core.Services;
using Pho.Web.Helpers;
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
        /// <param name="days">Days range from today</param>
        /// <returns>Up to 3 asteroids, ordered by descending diameter</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AsteroidViewModel>>> Get([Required] int days)
        {
            if (days < 0)
            {
                return ValidationProblem(
                    ValidationHelper.SingleValidationError(nameof(days), "must be greater than or equal to 0"));
            }

            var asteroids = await _asteroidService.GetLargestPotentiallyHazardousAsteroids(days);

            var response = asteroids
                .Select(asteroid => AsteroidViewModel.From(asteroid))
                .OrderByDescending(asteroidViewModel => asteroidViewModel.Diameter);

            return Ok(response);
        }
    }
}
