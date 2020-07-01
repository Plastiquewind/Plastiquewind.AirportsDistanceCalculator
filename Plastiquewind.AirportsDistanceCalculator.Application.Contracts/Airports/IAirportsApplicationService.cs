using System.Threading;
using System.Threading.Tasks;
using Plastiquewind.AirportsDistanceCalculator.Shared;

namespace Plastiquewind.AirportsDistanceCalculator.Application.Contracts.Airports
{
    public interface IAirportsApplicationService
    {
        Task<double> CalculateDistanceAsync(string firstIataCode, string secondIataCode,
            DistanceUnit distanceUnit = default,
            CancellationToken cancellationToken = default);
    }
}
