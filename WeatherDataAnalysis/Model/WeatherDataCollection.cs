using System;
using System.Collections.Generic;
using System.Linq;

namespace WeatherDataAnalysis.Model
{
    /// <summary>
    ///     Defines a collection for Temperature data
    /// </summary>
    public class WeatherDataCollection
    {
        #region Properties

        /// <summary>
        ///     Gets the days collection.
        /// </summary>
        /// <value>
        ///     The days collection.
        /// </value>
        public List<WeatherData> DaysCollection { get; }

        /// <summary>
        ///     Gets the collection grouped by month.
        /// </summary>
        /// <value>
        ///     The collection grouped by month.
        /// </value>
        public List<List<WeatherData>> CollectionGroupedByMonth { get; private set; }

        /// <summary>
        /// Gets all years.
        /// </summary>
        /// <value>
        /// All years.
        /// </value>
        public List<int> AllYears { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeatherDataCollection" /> class.
        /// </summary>
        public WeatherDataCollection()
        {
            this.DaysCollection = new List<WeatherData>();
            this.CollectionGroupedByMonth = new List<List<WeatherData>>();
            this.AllYears = new List<int>();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the specified new day.
        /// </summary>
        /// <param name="newDay">The new day.</param>
        public void Add(WeatherData newDay)
        {
            this.DaysCollection.Add(newDay);
        }

        /// <summary>
        ///     Gets the days with the highest temperature by year.
        /// </summary>
        /// <returns>A collection of days with the highest temperature for the year</returns>
        public List<WeatherData> GetDaysWithHighestTempForAYear()
        {
            var highestTemp = this.DaysCollection.Max(day => day.High);
            var daysWithHighestTemp = this.DaysCollection.FindAll(day => day.High == highestTemp);
            return daysWithHighestTemp;
        }

        
        /// <summary>
        ///     Gets the days with the lowest high temperature by year.
        /// </summary>
        /// <returns>A collection of days with the lowest high temperature for the year</returns>
        public List<WeatherData> GetDaysWithLowestHighTempByYear()
        {
            var lowestHigh = this.DaysCollection.Min(x => x.High);
            var daysWithLowestHighTemp = this.DaysCollection.FindAll(day => day.High == lowestHigh);
            return daysWithLowestHighTemp;
        }

        /// <summary>
        ///     Gets the days with the lowest temperature by year.
        /// </summary>
        /// <returns>A collection of days with the lowest temperature for the year</returns>
        public List<WeatherData> GetDaysWithLowestTempByYear()
        {
            var lowestTemp = this.DaysCollection.Min(day => day.Low);
            var daysWithLowestTemp = this.DaysCollection.FindAll(day => day.Low == lowestTemp);
            return daysWithLowestTemp;
        }

        /// <summary>
        ///     Gets the days with the highest low temperature by year.
        /// </summary>
        /// <returns>A collection of days with the highest low temperature for the year</returns>
        public List<WeatherData> GetDaysWithHighestLowTempByYear()
        {
            var highestLow = this.DaysCollection.Max(day => day.Low);
            var daysWithHighestLowTemp = this.DaysCollection.FindAll(day => day.Low == highestLow);
            return daysWithHighestLowTemp;
        }

        /// <summary>
        ///     Gets the average high temperature for year.
        /// </summary>
        /// <returns></returns>
        public double GetAverageHighTempForYear()
        {
            var averageHighTemp = this.DaysCollection.Average(day => day.High);
            return averageHighTemp;
        }

        /// <summary>
        ///     Gets the average low temperature for year.
        /// </summary>
        /// <returns></returns>
        public double GetAverageLowTempForYear()
        {
            var averageLowTemp = this.DaysCollection.Average(day => day.Low);
            return averageLowTemp;
        }

        /// <summary>
        ///     Gets the days with the highest temperature for each month.
        /// </summary>
        /// <returns>A collection of days with the highest temperature for each month</returns>
        public List<List<WeatherData>> GetDaysWithHighestTempForEachMonth()
        {
            var daysWithHighestTemps = new List<List<WeatherData>>();

            foreach (var current in this.CollectionGroupedByMonth)
            {
                this.getHighestTempInAMonth(current, daysWithHighestTemps);
            }

            return daysWithHighestTemps;
        }

        private void getHighestTempInAMonth(List<WeatherData> current,
            List<List<WeatherData>> daysWithHighestTemps)
        {
            var high = current.Max(day => day.High);
            var allWithHigh = current.FindAll(day => day.High == high);
            daysWithHighestTemps.Add(allWithHigh);
        }

        /// <summary>
        ///     Gets the days with the lowest temperature for each month.
        /// </summary>
        /// <returns>A collection of days with the lowest temperature for each month</returns>
        public List<List<WeatherData>> GetDaysWithLowestTempForEachMonth()
        {
            var daysWithLowestTemps = new List<List<WeatherData>>();

            foreach (var current in this.CollectionGroupedByMonth)
            {
                getLowestTempInAMonth(current, daysWithLowestTemps);
            }

            return daysWithLowestTemps;
        }

        private static void getLowestTempInAMonth(List<WeatherData> current,
            List<List<WeatherData>> daysWithLowestTemps)
        {
            var low = current.Min(day => day.Low);
            var allDaysWithHigh = current.FindAll(day => day.Low == low);
            daysWithLowestTemps.Add(allDaysWithHigh);
        }

        /// <summary>
        ///     Gets the average high temperature for each month.
        /// </summary>
        /// <returns>The average high temperature for the month</returns>
        public List<double> GetAverageHighTempForEachMonth()
        {
            var averageHighs = new List<double>();
            foreach (var current in this.CollectionGroupedByMonth)
            {
                var averageHigh = current.Average(day => day.High);
                averageHighs.Add(averageHigh);
            }

            return averageHighs;
        }

        /// <summary>
        ///     Gets the average low temperature for each month.
        /// </summary>
        /// <returns>The average low temperature for the month</returns>
        public List<double> GetAverageLowTempForEachMonth()
        {
            var averageLows = new List<double>();
            foreach (var current in this.CollectionGroupedByMonth)
            {
                var averageLow = current.Average(day => day.Low);
                averageLows.Add(averageLow);
            }

            return averageLows;
        }

        /// <summary>
        ///     Gets the days with temperature greater than equal to.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <returns>The number of days with a temperature greater than the specified value</returns>
        public int GetDaysWithTempGreaterThanEqualTo(int lowerBound)
        {
            var highDays = this.DaysCollection.FindAll(day => day.High >= lowerBound || day.Low >= lowerBound).ToList();
            return highDays.Count;
        }

        /// <summary>
        ///     Gets the days with temperature less than equal to.
        /// </summary>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>The number of days with a temperature less than the specified value</returns>
        public int GetDaysWithTempLessThanEqualTo(int upperBound)
        {
            var days = this.DaysCollection.FindAll(day => day.High <= upperBound || day.Low <= upperBound).ToList();
            return days.Count;
        }

        /// <summary>
        ///     Groups the temperature data by month.
        /// </summary>
        public void GroupByMonth()
        {
            this.CollectionGroupedByMonth = this.DaysCollection.GroupBy(day => day.Date.Month).Select(group => group.ToList()).ToList();
        }



        /// <summary>
        /// Groups the by year.
        /// </summary>
        /// <returns> A collection grouped by years</returns>
        public List<List<WeatherData>> GroupByYear()
        {
            return this.DaysCollection.GroupBy(x => x.Date.Year).Select(group => group.ToList()).ToList(); 
        }

        #endregion
    }
}