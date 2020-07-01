using System.Threading;
using System.Threading.Tasks;
using Plastiquewind.AirportsDistanceCalculator.Application.Contracts.Airports;
using Plastiquewind.AirportsDistanceCalculator.Shared;

using Microsoft.AspNetCore.Mvc;

namespace Plastiquewind.AirportsDistanceCalculator.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AirportsController : ControllerBase
    {
        private readonly IAirportsApplicationService airportsApplicationService;

        public AirportsController(IAirportsApplicationService airportsApplicationService)
        {
            this.airportsApplicationService = airportsApplicationService;
        }

        [HttpGet("{firstIataCode}/distance/{secondIataCode}")]
        public async Task<double> CalculateDistanceAsync(string firstIataCode, string secondIataCode, 
            CancellationToken cancellationToken = default) 
            => await this.airportsApplicationService.CalculateDistanceAsync(firstIataCode, secondIataCode, DistanceUnit.Mile, cancellationToken);
    }
}