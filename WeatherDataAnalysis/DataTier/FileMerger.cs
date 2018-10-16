using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using WeatherDataAnalysis.Model;
using WeatherDataAnalysis.View;

namespace WeatherDataAnalysis.DataTier
{
    /// <summary>
    /// Handles the merging of files
    /// </summary>
    public class FileMerger
    {
        private WeatherDataCollection oldWeatherDataCollection;
        private WeatherDataCollection newWeatherDataCollection;
        private bool isDoForAllChecked;
        private ContentDialogResult chosenResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileMerger"/> class.
        /// </summary>
        public FileMerger()
        {
            this.isDoForAllChecked = false;
        }
        /// <summary>
        /// Merges the weather data collections.
        /// </summary>
        /// <param name="oldWeatherDataCollection">The old weather data collection.</param>
        /// <param name="newWeatherDataCollection">The new weather data collection.</param>
        /// <returns></returns>
        public async Task<WeatherDataCollection> MergeWeatherDataCollections(WeatherDataCollection oldWeatherDataCollection, WeatherDataCollection newWeatherDataCollection)
        {
            this.oldWeatherDataCollection = oldWeatherDataCollection ?? throw new ArgumentNullException(nameof(oldWeatherDataCollection));
            this.newWeatherDataCollection = newWeatherDataCollection ?? throw new ArgumentNullException(nameof(newWeatherDataCollection));

            var updatedWeatherDataCollection = new WeatherDataCollection();

            foreach (var currDay in this.newWeatherDataCollection.DaysCollection)
            {
                if (this.oldWeatherDataCollection.DaysCollection.Exists(oldDay => oldDay.Date.Equals(currDay.Date)) && !this.isDoForAllChecked)
                {
                    var keepOrReplace = new ReplaceOrKeepDialog(currDay);
                    this.chosenResult = await keepOrReplace.ShowAsync();
                    this.isDoForAllChecked = keepOrReplace.isDoForAllChecked;
                }

                this.addDayToTheUpdatedCollection(currDay, updatedWeatherDataCollection);
            }

            return updatedWeatherDataCollection;
        }

        private void addDayToTheUpdatedCollection(WeatherData currDay, WeatherDataCollection updatedWeatherDataCollection)
        {
            switch (this.chosenResult)
            {
                case ContentDialogResult.Primary:
                    var oldDay = this.oldWeatherDataCollection.DaysCollection.Find(x => x.Date.Equals(currDay.Date));
                    updatedWeatherDataCollection.Add(oldDay);
                    break;
                case ContentDialogResult.Secondary:
                    updatedWeatherDataCollection.Add(currDay);
                    break;
            }
        }
    }
}
