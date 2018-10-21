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
        private readonly ICollection<WeatherData> daysCollection;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        public int Count => this.daysCollection.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        public bool IsReadOnly => this.daysCollection.IsReadOnly;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeatherDataCollection" /> class.
        /// </summary>
        public WeatherDataCollection()
        {
            this.daysCollection = new List<WeatherData>();
        }

        #endregion

        #region Methods


        /// <summary>
        /// Adds the specified new day.
        /// </summary>
        /// <param name="newDay">The new day.</param>
        /// <exception cref="NullReferenceException">newDay</exception>
        public void Add(WeatherData newDay)
        {
            if (newDay == null)
            {
                throw new NullReferenceException(nameof(newDay));
            }

            this.daysCollection.Add(newDay);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        public void Clear()
        {
            this.daysCollection.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        public bool Contains(WeatherData item)
        {
            return this.daysCollection.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(WeatherData[] array, int arrayIndex)
        {
            this.daysCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        public bool Remove(WeatherData item)
        {
            return this.daysCollection.Remove(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<WeatherData> GetEnumerator()
        {
            return this.daysCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.daysCollection.GetEnumerator();
        }

        /// <summary>
        ///     Gets the days with the highest temperature by year.
        /// </summary>
        /// <returns>A collection of days with the highest temperature for the year</returns>
        public List<WeatherData> GetDaysWithHighestTempForAYear()
        {
            var highestTemp = this.daysCollection.Max(day => day.High);
            var daysWithHighestTemp = this.daysCollection.Where(day => day.High == highestTemp).ToList();
            return daysWithHighestTemp;
        }

        /// <summary>
        ///     Gets the days with the lowest high temperature by year.
        /// </summary>
        /// <returns>A collection of days with the lowest high temperature for the year</returns>
        public List<WeatherData> GetDaysWithLowestHighTempByYear()
        {
            var lowestHigh = this.daysCollection.Min(x => x.High);
            var daysWithLowestHighTemp = this.daysCollection.Where(day => day.High == lowestHigh).ToList();
            return daysWithLowestHighTemp;
        }

        /// <summary>
        ///     Gets the days with the lowest temperature by year.
        /// </summary>
        /// <returns>A collection of days with the lowest temperature for the year</returns>
        public List<WeatherData> GetDaysWithLowestTempByYear()
        {
            var lowestTemp = this.daysCollection.Min(day => day.Low);
            var daysWithLowestTemp = this.daysCollection.Where(day => day.Low == lowestTemp).ToList();
            return daysWithLowestTemp;
        }

        /// <summary>
        ///     Gets the days with the highest low temperature by year.
        /// </summary>
        /// <returns>A collection of days with the highest low temperature for the year</returns>
        public List<WeatherData> GetDaysWithHighestLowTempByYear()
        {
            var highestLow = this.daysCollection.Max(day => day.Low);
            var daysWithHighestLowTemp = this.daysCollection.Where(day => day.Low == highestLow).ToList();
            return daysWithHighestLowTemp;
        }

        /// <summary>
        ///     Gets the average high temperature for year.
        /// </summary>
        /// <returns></returns>
        public double GetAverageHighTempForYear()
        {
            var averageHighTemp = this.daysCollection.Average(day => day.High);
            return averageHighTemp;
        }

        /// <summary>
        ///     Gets the average low temperature for year.
        /// </summary>
        /// <returns></returns>
        public double GetAverageLowTempForYear()
        {
            var averageLowTemp = this.daysCollection.Average(day => day.Low);
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
                this.findHighestTempInAMonth(current, daysWithHighestTemps);
            }

            return daysWithHighestTemps;
        }

        private void findHighestTempInAMonth(List<WeatherData> current,
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
                this.findLowestTempInAMonth(current, daysWithLowestTemps);
            }

            return daysWithLowestTemps;
        }

        private void findLowestTempInAMonth(List<WeatherData> current,
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
            var highDays = this.daysCollection.Where(day => day.High >= lowerBound || day.Low >= lowerBound).ToList();
            return highDays.Count;
        }

        /// <summary>
        ///     Gets the days with temperature less than equal to.
        /// </summary>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>The number of days with a temperature less than the specified value</returns>
        public int GetDaysWithTempLessThanEqualTo(int upperBound)
        {
            var days = this.daysCollection.Where(day => day.High <= upperBound || day.Low <= upperBound).ToList();
            return days.Count;
        }

        /// <summary>
        ///     Groups the temperature data by month.
        /// </summary>
        public List<List<WeatherData>> GroupByMonth()
        {
            return this.daysCollection.GroupBy(day => day.Date.Month).Select(group => group.ToList()).ToList();
        }

        /// <summary>
        ///     Groups the by year.
        /// </summary>
        /// <returns> A collection grouped by years</returns>
        public List<List<WeatherData>> GroupByYear()
        {
            return this.daysCollection.GroupBy(x => x.Date.Year).Select(group => group.ToList()).ToList();
        }

        /// <summary>
        /// Counts the days with high between.
        /// </summary>
        /// <param name="lowerbound">The lowerbound.</param>
        /// <param name="upperbound">The upperbound.</param>
        /// <returns>number of days between the bounds</returns>
        public int CountDaysWithHighBetween(int lowerbound, int upperbound)
        {
           var days = this.daysCollection.Where(day => day.High >= lowerbound && day.High <= upperbound).ToList();
            return days.Count;
        }

        /// <summary>
        /// Counts the days with low between.
        /// </summary>
        /// <param name="lowerbound">The lowerbound.</param>
        /// <param name="upperbound">The upperbound.</param>
        /// <returns>number of days between the bounds</returns>
        public int CountDaysWithLowBetween(int lowerbound, int upperbound)
        {
            var days = this.daysCollection.Where(day => day.Low >= lowerbound && day.Low <= upperbound).ToList();
            return days.Count;
        }

        #endregion
    }
}