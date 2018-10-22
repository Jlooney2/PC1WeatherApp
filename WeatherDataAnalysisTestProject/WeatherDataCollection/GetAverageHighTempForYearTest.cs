using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherDataAnalysis.Model;

namespace WeatherDataAnalysisTestProject.WeatherDataCollection
{

    /// <summary>
    /// Input        Expected Output
    /// none         Exception
    /// 90           90
    /// 90,100,74    88
    /// </summary>
    /// 
    [TestClass]
    public class GetAverageHighTempForYearTest
    {
        [TestMethod]
        public void TestEmptyCollection()
        {
            var weatherData = new WeatherDataAnalysis.Model.WeatherDataCollection();
            Assert.ThrowsException<InvalidOperationException>(() => weatherData.GetDaysWithHighestLowTempByYear());
        }

        [TestMethod]
        public void TestCollectionSize1()
        {
            var weatherData = new WeatherDataAnalysis.Model.WeatherDataCollection { new WeatherData(DateTime.Now, 90, 40) };
            Assert.AreEqual(90, weatherData.GetAverageHighTempForYear());
        }

        [TestMethod]
        public void TestCollectionSize3()
        {
            var weatherData = new WeatherDataAnalysis.Model.WeatherDataCollection
            {
                new WeatherData(DateTime.Now, 90, 70),
                new WeatherData(DateTime.Now.AddDays(1), 100, 50),
                new WeatherData(DateTime.Now.AddDays(2), 74, 40)
            };
            Assert.AreEqual(88, weatherData.GetAverageHighTempForYear());
        }
    }
}
