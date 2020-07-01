using Plastiquewind.AirportsDistanceCalculator.Application.Contracts.Converters;

namespace Plastiquewind.AirportsDistanceCalculator.Application.Converters
{
    internal sealed class MetersToMilesConverterApplicationService : IMetersConverterApplicationService
    {
        public double Convert(double meters) => meters / 1609.344;
    }
}
