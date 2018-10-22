using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherDataAnalysis.Model;

namespace WeatherDataAnalysisTestProject.WeatherDataCollection
{
    /// <summary>
    /// Input        Expected Output
    /// none         Exception
    /// 90           90
    /// 90,100,70    70
    /// </summary>
    [TestClass]
    public class GetDaysWithLowestHighTempByYearTest
    {
        [TestMethod]
        public void TestEmptyCollection()
        {
            var weatherData = new WeatherDataAnalysis.Model.WeatherDataCollection();
            Assert.ThrowsException<InvalidOperationException>(() => weatherData.GetDaysWithLowestHighTempByYear());
        }

        [TestMethod]
        public void TestCollectionSize1()
        {
            var weatherData = new WeatherDataAnalysis.Model.WeatherDataCollection { new WeatherData(DateTime.Now, 90, 40) };
            Assert.AreEqual(90, weatherData.GetDaysWithLowestHighTempByYear()[0].High);
        }

        [TestMethod]
        public void TestCollectionSize3()
        {
            var weatherData = new WeatherDataAnalysis.Model.WeatherDataCollection
            {
                new WeatherData(DateTime.Now, 90, 70),
                new WeatherData(DateTime.Now.AddDays(1), 100, 50),
                new WeatherData(DateTime.Now.AddDays(2), 70, 40)
            };
            Assert.AreEqual(70, weatherData.GetDaysWithLowestHighTempByYear()[0].High);
        }
    }
}
