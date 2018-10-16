using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using WeatherDataAnalysis.Model;
using Windows.Storage;

namespace WeatherDataAnalysis.DataTier
{
    /// <summary>
    /// Defines a parser for Temperature data file
    /// </summary>
    public class WeatherFileParser
    {
        /// <summary>
        /// Gets the error messages.
        /// </summary>
        /// <value>
        /// The error messages.
        /// </value>
        public StringBuilder errorMessages { get; private set; }
        private const int Date = 0;
        private const int High = 1;
        private const int Low = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherFileParser"/> class.
        /// </summary>
        public WeatherFileParser()
        {
            this.errorMessages = new StringBuilder();
        }

        /// <summary>
        /// Parses the temperature file asynchronous.
        /// </summary>
        /// <param name="tempFile">The temporary file.</param>
        /// <returns>A collection of weather data</returns>
        public async Task<WeatherDataCollection> ParseTemperatureFileAsync(StorageFile tempFile)
        {
            var days = new WeatherDataCollection();
            string fileText = await FileIO.ReadTextAsync(tempFile);

            var lines = fileText.Split(Environment.NewLine);
            var lineNumber = 0;
            foreach (var day in lines)
            {
                lineNumber++;
                try
                {
                    var fields = day.Split(",");
                    var date = DateTime.Parse(fields[Date]);
                    var high = Convert.ToInt16(fields[High]);
                    var low = Convert.ToInt16(fields[Low]);

                    days.Add(new WeatherData(date, high, low));
                }
                catch (Exception)
                {
                    this.errorMessages.Append($"Corrupt Data on line {lineNumber}");
                }
                
                
            }

            return days;
        }

    }
}
