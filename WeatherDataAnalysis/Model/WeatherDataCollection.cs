using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WeatherDataAnalysis.Model
{
    /// <summary>
    ///     Defines a collection for Temperature data
    /// </summary>
    public class WeatherDataCollection : ICollection<WeatherData>
    {
        #region Data members

        /// <summary>
        ///     Gets the collection grouped by month.
        /// </summary>
        /// <value>
        ///     The collection grouped by month.
        /// </value>
        private readonly ICollection<WeatherData> DaysCollection;

        #endregion

        #region Properties

        public int Count => this.DaysCollection.Count;

        public bool IsReadOnly => this.DaysCollection.IsReadOnly;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeatherDataCollection" /> class.
        /// </summary>
        public WeatherDataCollection()
        {
            this.DaysCollection = new List<WeatherData>();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds the specified new day.
        /// </summary>
        /// <param name="newDay">The new day.</param>
        /// <exception cref="NullReferenceException">newDay</exception>
        public void Add(WeatherData newDay)
        {
            if (newDay == null)
            {
                throw new NullReferenceException(nameof(newDay));
            }

            this.DaysCollection.Add(newDay);
        }

        public void Clear()
        {
            this.DaysCollection.Clear();
        }

        public bool Contains(WeatherData item)
        {
            return this.DaysCollection.Contains(item);
        }

        public void CopyTo(WeatherData[] array, int arrayIndex)
        {
            this.DaysCollection.CopyTo(array, arrayIndex);
        }

        public bool Remove(WeatherData item)
        {
            return this.DaysCollection.Remove(item);
        }

        public IEnumerator<WeatherData> GetEnumerator()
        {
            return this.DaysCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.DaysCollection.GetEnumerator();
        }

        /// <summary>
        ///     Gets the days with the highest temperature by year.
        /// </summary>
        /// <returns>A collection of days with the highest temperature for the year</returns>
        public List<WeatherData> GetDaysWithHighestTempForAYear()
        {
            var highestTemp = this.DaysCollection.Max(day => day.High);
            var daysWithHighestTemp = this.DaysCollection.Where(day => day.High == highestTemp).ToList();
            return daysWithHighestTemp;
        }

        /// <summary>
        ///     Gets the days with the lowest high temperature by year.
        /// </summary>
        /// <returns>A collection of days with the lowest high temperature for the year</returns>
        public List<WeatherData> GetDaysWithLowestHighTempByYear()
        {
            var lowestHigh = this.DaysCollection.Min(x => x.High);
            var daysWithLowestHighTemp = this.DaysCollection.Where(day => day.High == lowestHigh).ToList();
            return daysWithLowestHighTemp;
        }

        /// <summary>
        ///     Gets the days with the lowest temperature by year.
        /// </summary>
        /// <returns>A collection of days with the lowest temperature for the year</returns>
        public List<WeatherData> GetDaysWithLowestTempByYear()
        {
            var lowestTemp = this.DaysCollection.Min(day => day.Low);
            var daysWithLowestTemp = this.DaysCollection.Where(day => day.Low == lowestTemp).ToList();
            return daysWithLowestTemp;
        }

        /// <summary>
        ///     Gets the days with the highest low temperature by year.
        /// </summary>
        /// <returns>A collection of days with the highest low temperature for the year</returns>
        public List<WeatherData> GetDaysWithHighestLowTempByYear()
        {
            var highestLow = this.DaysCollection.Max(day => day.Low);
            var daysWithHighestLowTemp = this.DaysCollection.Where(day => day.Low == highestLow).ToList();
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

            foreach (var current in this.GroupByMonth())
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

            foreach (var current in this.GroupByMonth())
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
            foreach (var current in this.GroupByMonth())
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
            foreach (var current in this.GroupByMonth())
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
            var highDays = this.DaysCollection.Where(day => day.High >= lowerBound || day.Low >= lowerBound).ToList();
            return highDays.Count;
        }

        /// <summary>
        ///     Gets the days with temperature less than equal to.
        /// </summary>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>The number of days with a temperature less than the specified value</returns>
        public int GetDaysWithTempLessThanEqualTo(int upperBound)
        {
            var days = this.DaysCollection.Where(day => day.High <= upperBound || day.Low <= upperBound).ToList();
            return days.Count;
        }

        /// <summary>
        ///     Groups the temperature data by month.
        /// </summary>
        public List<List<WeatherData>> GroupByMonth()
        {
            return this.DaysCollection.GroupBy(day => day.Date.Month).Select(group => group.ToList()).ToList();
        }

        /// <summary>
        ///     Groups the by year.
        /// </summary>
        /// <returns> A collection grouped by years</returns>
        public List<List<WeatherData>> GroupByYear()
        {
            return this.DaysCollection.GroupBy(x => x.Date.Year).Select(group => group.ToList()).ToList();
        }

        #endregion
    }
}