using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherDataAnalysis.Model;

namespace WeatherDataAnalysisTestProject.WeatherDataCollection
{
    /// <summary>
    /// Input        Expected Output
    /// none         Exception
    /// 90           90
    /// 90,100,70    100
    /// </summary>
    [TestClass]
    public class GetDaysWithHighestTempForAYearTest
    {
        [TestMethod]
        public void TestEmptyCollection()
        {
            var weatherData = new WeatherDataAnalysis.Model.WeatherDataCollection();
            Assert.ThrowsException<InvalidOperationException>(() => weatherData.GetDaysWithHighestTempForAYear());
        }

        [TestMethod]
        public void TestCollectionSize1()
        {
            var weatherData = new WeatherDataAnalysis.Model.WeatherDataCollection {new WeatherData(DateTime.Now, 90, 40)};
            Assert.AreEqual(90, weatherData.GetDaysWithHighestTempForAYear()[0].High);
        }

        [TestMethod]
        public void TestCollectionSize3()
        {
            var weatherData = new WeatherDataAnalysis.Model.WeatherDataCollection
            {
                new WeatherData(DateTime.Now, 90, 40),
                new WeatherData(DateTime.Now.AddDays(1), 100, 40),
                new WeatherData(DateTime.Now.AddDays(2), 70, 40)
            };
            Assert.AreEqual(100, weatherData.GetDaysWithHighestTempForAYear()[0].High);
        }
    }
}
