using System;
using Plastiquewind.AirportsDistanceCalculator.Application.Airports;
using Plastiquewind.AirportsDistanceCalculator.Application.Contracts.Airports.Dto;
using Plastiquewind.AirportsDistanceCalculator.Application.Contracts;

using Xunit;
using Autofac.Extras.Moq;
using Flurl.Http.Testing;
using Shouldly;
using Autofac;
using Flurl.Http.Configuration;

namespace Plastiquewind.AirportsDistanceCalculator.Application.UnitTests
{
    public class AirportsApplicationServiceUnitTests
    {
        [Fact]
        public async void CalculateDistanceAsync_Returns_Distance_In_Meters_By_Default()
        {
            // Arrange
            using var httpTest = new HttpTest();
            using var mock = AutoMock
                .GetLoose(cfg => cfg.RegisterInstance(new PerBaseUrlFlurlClientFactory()).As<IFlurlClientFactory>().SingleInstance());
            mock.Mock<IApplicationModuleSettings>()
                .Setup(settings => settings.AirportDetailsApiUrl).Returns(new Uri("http://localhost"));
            httpTest
                .RespondWithJson(new AirportDetailsDto
                {
                    Location = new LocationDto
                    {
                        Lat = 52.309069,
                        Lon = 4.763385
                    }
                })
                .RespondWithJson(new AirportDetailsDto
                {
                    Location = new LocationDto
                    {
                        Lat = 55.966324,
                        Lon = 37.416574
                    }
                });
            
            var airportsApplicationService = mock.Create<AirportsApplicationService>();
            var expectedResult = 2146368.0867643384;

            // Act
            var actualResult = await airportsApplicationService.CalculateDistanceAsync("AMS", "SVO");

            // Assert
            actualResult.ShouldBe(expectedResult);
        }
    }
}
