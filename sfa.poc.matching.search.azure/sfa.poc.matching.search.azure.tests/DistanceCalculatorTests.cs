using NUnit.Framework;
using sfa.poc.matching.search.azure.application.Helpers;
using sfa.poc.matching.search.azure.application.Interfaces;

namespace sfa.poc.matching.search.azure.tests
{
    public class DistanceCalculatorTests
    {
        [Test]
        public void DistanceCalculatorReturnsExpectedDistanceBetweenTwoCloseLocations()
        {
            //Start location lat/long
            var startLatitude = 52.404079d;
            var startLongitude = -1.509867d;

            //Destination lat/long
            var targetLatitude = 52.350205d;
            var targetLongitude = -1.563843d;
            
            var expectedDistance = 4.36947697529171d;
            var delta = 0.01d;

            var calculator = new DistanceCalculator();

            var distance = calculator.DistanceFromLatLong(
                startLatitude, startLongitude,
                targetLatitude, targetLongitude);

            TestContext.WriteLine(
                $"Distance returned {distance}, expected distance {expectedDistance}. Difference {expectedDistance - distance:0.00#}");

            Assert.AreEqual(expectedDistance, distance, delta);
            //Assert.AreEqual(expectedDistance, distance, delta1);
        }

        [Test]
        public void DistanceCalculatorReturnsExpectedDistanceBetweenTwoModeratelyCloseLocations()
        {
            //Start location lat/long
            var startLatitude = 52.404079d;
            var startLongitude = -1.509867d;

            //Destination lat/long
            var targetLatitude = 52.757633;
            var targetLongitude = -1.386055d;
            
            var expectedDistance = 24.9966446359068d;
            var delta = 0.05d;

            var calculator = new DistanceCalculator();

            var distance = calculator.DistanceFromLatLong(
                startLatitude, startLongitude,
                targetLatitude, targetLongitude);

            TestContext.WriteLine(
                $"Distance returned {distance}, expected distance {expectedDistance}. Difference {expectedDistance - distance:0.00#}");

            Assert.AreEqual(expectedDistance, distance, delta);
        }

        [Test]
        public void DistanceCalculatorReturnsExpectedDistanceBetweenTwoFiftyMileishCloseLocations()
        {
            //Start location lat/long
            var startLatitude = 52.404079d;
            var startLongitude = -1.509867d;

            //Destination lat/long
            var targetLatitude = 52.107216d;

            var targetLongitude = -0.437497d;

            var expectedDistance = 49.9164577001829d;
            var delta = 0.05d;

            var calculator = new DistanceCalculator();

            var distance = calculator.DistanceFromLatLong(
                startLatitude, startLongitude,
                targetLatitude, targetLongitude);

            TestContext.WriteLine(
                $"Distance returned {distance}, expected distance {expectedDistance}. Difference {expectedDistance - distance:0.00#}");

            Assert.AreEqual(expectedDistance, distance, delta);
        }
        
        [Test]
        public void DistanceCalculatorReturnsExpectedDistanceBetweenTwoHundredMileishCloseLocations()
        {
            //Start location lat/long
            var startLatitude = 52.404079d;
            var startLongitude = -1.509867d;

            //Destination lat/long
            var targetLatitude = 51.395021;

            var targetLongitude = 0.163423;

            var expectedDistance = 99.9387408594718d;
            var delta = 0.05d;

            var calculator = new DistanceCalculator();

            var distance = calculator.DistanceFromLatLong(
                startLatitude, startLongitude,
                targetLatitude, targetLongitude);

            TestContext.WriteLine(
                $"Distance returned {distance}, expected distance {expectedDistance}. Difference {expectedDistance - distance:0.00#}");

            Assert.AreEqual(expectedDistance, distance, delta);
        }
    }
}
