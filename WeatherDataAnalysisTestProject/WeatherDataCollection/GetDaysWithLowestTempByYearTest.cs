﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherDataAnalysis.Model;

namespace WeatherDataAnalysisTestProject.WeatherDataCollection
{
    /// <summary>
    /// Input        Expected Output
    /// none         Exception
    /// 40           40
    /// 90,100,70    40
    /// </summary>
    [TestClass]
    public class GetDaysWithLowestTempByYearTest
    {
        [TestMethod]
        public void TestEmptyCollection()
        {
            var weatherData = new WeatherDataAnalysis.Model.WeatherDataCollection();
            Assert.ThrowsException<InvalidOperationException>(() => weatherData.GetDaysWithLowestTempByYear());
        }

        [TestMethod]
        public void TestCollectionSize1()
        {
            var weatherData = new WeatherDataAnalysis.Model.WeatherDataCollection { new WeatherData(DateTime.Now, 90, 40) };
            Assert.AreEqual(40, weatherData.GetDaysWithLowestTempByYear()[0].Low);
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
            Assert.AreEqual(40, weatherData.GetDaysWithLowestTempByYear()[0].Low);
        }
    }
}
