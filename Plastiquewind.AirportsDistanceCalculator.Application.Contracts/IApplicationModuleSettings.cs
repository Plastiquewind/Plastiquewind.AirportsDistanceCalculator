using System;

namespace Plastiquewind.AirportsDistanceCalculator.Application.Contracts
{
    public interface IApplicationModuleSettings
    {
        Uri AirportDetailsApiUrl { get; }
    }
}
