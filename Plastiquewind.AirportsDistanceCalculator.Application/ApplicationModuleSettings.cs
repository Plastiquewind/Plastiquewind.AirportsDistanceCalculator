using System;

using Plastiquewind.AirportsDistanceCalculator.Application.Contracts;

namespace Plastiquewind.AirportsDistanceCalculator.Application
{
    public sealed class ApplicationModuleSettings : IApplicationModuleSettings
    {
        #region Properties

        public Uri AirportDetailsApiUrl { get; set; }

        #endregion
    }
}
