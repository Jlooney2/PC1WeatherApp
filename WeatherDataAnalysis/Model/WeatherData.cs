using System;

namespace WeatherDataAnalysis.Model
{

    /// <summary>
    /// Defines a temperature data object
    /// </summary>
    public class WeatherData : IComparable<WeatherData>
    {
        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; }
        /// <summary>
        /// Gets the high.
        /// </summary>
        /// <value>
        /// The high.
        /// </value>
        public int High { get; }
        /// <summary>
        /// Gets the low.
        /// </summary>
        /// <value>
        /// The low.
        /// </value>
        public int Low { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherData"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="highestTemperature">The highest temperature.</param>
        /// <param name="lowestTemperature">The lowest temperature.</param>
        public WeatherData(DateTime date, int highestTemperature, int lowestTemperature)
        {
            this.Date = date;
            this.High = highestTemperature;
            this.Low = lowestTemperature;
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>an int signifying how the data compares</returns>
        public int CompareTo(WeatherData other) => this.Date.CompareTo(other.Date);
    }
}
