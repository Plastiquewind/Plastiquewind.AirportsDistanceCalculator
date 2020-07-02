using System;
using System.Threading;
using System.Threading.Tasks;
using Plastiquewind.AirportsDistanceCalculator.Application.Contracts;
using Plastiquewind.AirportsDistanceCalculator.Application.Contracts.Airports;
using Plastiquewind.AirportsDistanceCalculator.Application.Contracts.Airports.Dto;
using Plastiquewind.AirportsDistanceCalculator.Application.Contracts.Converters;
using Plastiquewind.AirportsDistanceCalculator.Shared;

using Autofac.Features.Indexed;
using Flurl.Http;
using Flurl.Http.Configuration;
using GeoCoordinatePortable;

namespace Plastiquewind.AirportsDistanceCalculator.Application.Airports
{
    internal sealed class AirportsApplicationService : IAirportsApplicationService
    {
        #region Fields

        private readonly Uri airportDetailsApiUrl;
        private readonly IFlurlClient flurlClient;
        private readonly IIndex<DistanceUnit, IMetersConverterApplicationService> converterServices;

        #endregion

        #region Constructors

        public AirportsApplicationService(IApplicationModuleSettings applicationModuleSettings,
            IFlurlClientFactory flurlClientFactory,
            IIndex<DistanceUnit, IMetersConverterApplicationService> converterServices)
        {
            this.airportDetailsApiUrl = (applicationModuleSettings ?? throw new ArgumentNullException(nameof(applicationModuleSettings)))
                .AirportDetailsApiUrl ?? throw new ArgumentException($"{nameof(applicationModuleSettings.AirportDetailsApiUrl)} is required");
            this.flurlClient = (flurlClientFactory ?? throw new ArgumentNullException(nameof(flurlClient))).Get(this.airportDetailsApiUrl);
            this.converterServices = converterServices;
        }

        #endregion

        #region Public methods

        public async Task<double> CalculateDistanceAsync(string firstIataCode, string secondIataCode,
            DistanceUnit distanceUnit = default,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(firstIataCode))
            {
                throw new ArgumentNullException(nameof(firstIataCode));
            }

            if (string.IsNullOrEmpty(secondIataCode))
            {
                throw new ArgumentNullException(nameof(secondIataCode));
            }

            var (firstAirport, secondAirport) = await GetAirportsAsync(firstIataCode, secondIataCode, cancellationToken);

            return CalculateDistance(firstAirport.Location, secondAirport.Location, distanceUnit);
        }

        #endregion

        #region Private methods

        private double CalculateDistance(LocationDto firstAirportLocation, LocationDto secondAirportLocation, DistanceUnit distanceUnit)
        {
            var firstAirportGeoCoordinate = new GeoCoordinate(firstAirportLocation.Lat, firstAirportLocation.Lon);
            var secondAirportGeoCoordinate = new GeoCoordinate(secondAirportLocation.Lat, secondAirportLocation.Lon);
            var distanceInMeters = firstAirportGeoCoordinate.GetDistanceTo(secondAirportGeoCoordinate);

            return distanceUnit == DistanceUnit.Meter ? distanceInMeters : this.converterServices[distanceUnit].Convert(distanceInMeters);
        }

        private async Task<(AirportDetailsDto firstAirport, AirportDetailsDto secondAirport)> GetAirportsAsync(string firstIataCode, 
            string secondIataCode,
            CancellationToken cancellationToken)
        {
            var getFirstAirportTask = this.flurlClient
                .Request(this.airportDetailsApiUrl, firstIataCode)
                .GetJsonAsync<AirportDetailsDto>(cancellationToken);
            var getSecondAirportTask = this.flurlClient
                .Request(this.airportDetailsApiUrl, secondIataCode)
                .GetJsonAsync<AirportDetailsDto>(cancellationToken);

            var airports = await Task.WhenAll(getFirstAirportTask, getSecondAirportTask);

            return (airports[0], airports[1]);
        }

        #endregion
    }
}
