using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherDataAnalysis.Model;
using WeatherDataAnalysis.View;

namespace WeatherDataAnalysis.DataTier
{
    /// <summary>
    /// Add a day
    /// </summary>
    public class DayAdder
    {
        /// <summary>
        /// Adds the new day.
        /// </summary>
        /// <param name="weatherDataCollection">The weather data collection.</param>
        public static async Task<WeatherDataCollection> AddNewDay(WeatherDataCollection weatherDataCollection)
        {
            var addNewDayDialog = new AddDayContentDialog();
            await addNewDayDialog.ShowAsync();

            var newDay = addNewDayDialog.newDay;

            if (newDay != null)
            {
                if (weatherDataCollection == null)
                {
                    weatherDataCollection = new WeatherDataCollection
                    {
                        newDay
                    };
                    
                }
                else if (weatherDataCollection.Count == 0)
                {
                    weatherDataCollection.Add(newDay);
                }
                else
                {
                    var tempWeatherCollection = new WeatherDataCollection
                    {
                        newDay
                    };
                    var fileMerger = new FileMerger();
                   weatherDataCollection = await fileMerger.MergeWeatherDataCollections(weatherDataCollection, tempWeatherCollection);
                }
            }

            return weatherDataCollection;
        }
    }
}
