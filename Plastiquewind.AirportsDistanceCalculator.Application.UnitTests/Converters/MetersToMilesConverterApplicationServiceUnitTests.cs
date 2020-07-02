using Plastiquewind.AirportsDistanceCalculator.Application.Converters;

using Shouldly;
using Xunit;

namespace Plastiquewind.AirportsDistanceCalculator.Application.UnitTests.Converters
{
    public class MetersToMilesConverterApplicationServiceUnitTests
    {
        [Fact]
        public void Convert_Returns_Miles()
        {
            // Arrange
            var meters = 2146368.0867643384;
            var expected = 1333.6912970529224;
            var converter = new MetersToMilesConverterApplicationService();

            // Act
            var actual = converter.Convert(meters);

            // Assert
            actual.ShouldBe(expected);
        }
    }
}
