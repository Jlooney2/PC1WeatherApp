using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using WeatherDataAnalysis.Model;
using WeatherDataAnalysis.View;

namespace WeatherDataAnalysis.DataTier
{
    /// <summary>
    ///     Handles the merging of files
    /// </summary>
    public class FileMerger
    {
        #region Data members

        private WeatherDataCollection oldWeatherDataCollection;
        private WeatherDataCollection newWeatherDataCollection;
        private bool isDoForAllChecked;
        private ContentDialogResult chosenResult;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileMerger" /> class.
        /// </summary>
        public FileMerger()
        {
            this.isDoForAllChecked = false;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Merges the weather data collections.
        /// </summary>
        /// <param name="oldWeatherDataCollection">The old weather data collection.</param>
        /// <param name="newWeatherDataCollection">The new weather data collection.</param>
        /// <returns></returns>
        public async Task<WeatherDataCollection> MergeWeatherDataCollections(
            WeatherDataCollection oldWeatherDataCollection, WeatherDataCollection newWeatherDataCollection)
        {
            this.oldWeatherDataCollection = oldWeatherDataCollection ??
                                            throw new ArgumentNullException(nameof(oldWeatherDataCollection));
            this.newWeatherDataCollection = newWeatherDataCollection ??
                                            throw new ArgumentNullException(nameof(newWeatherDataCollection));

            var updatedWeatherDataCollection = new WeatherDataCollection();

            addNonConflictingData(oldWeatherDataCollection, newWeatherDataCollection, updatedWeatherDataCollection);

            foreach (var currDay in this.newWeatherDataCollection)
            {
                if (this.oldWeatherDataCollection.Any(oldDay => oldDay.Date.Equals(currDay.Date)) &&
                    !this.isDoForAllChecked)
                {
                    var keepOrReplace = new ReplaceOrKeepDialog(currDay);
                    this.chosenResult = await keepOrReplace.ShowAsync();
                    this.isDoForAllChecked = keepOrReplace.isDoForAllChecked;
                }

                this.addConflictingDayToUpdatedCollection(currDay, updatedWeatherDataCollection);
            }

            return updatedWeatherDataCollection;
        }

        private static void addNonConflictingData(WeatherDataCollection oldWeatherDataCollection,
            WeatherDataCollection newWeatherDataCollection, WeatherDataCollection updatedWeatherDataCollection)
        {
            var oldNonConflictingData = oldWeatherDataCollection
                                        .Where(oldDay =>
                                            !newWeatherDataCollection.Any(newDay => newDay.Date.Equals(oldDay.Date)))
                                        .ToList();
            foreach (var day in oldNonConflictingData)
            {
                updatedWeatherDataCollection.Add(day);
                newWeatherDataCollection.Remove(day);
            }
        }

        private void addConflictingDayToUpdatedCollection(WeatherData currDay,
            WeatherDataCollection updatedWeatherDataCollection)
        {
            switch (this.chosenResult)
            {
                case ContentDialogResult.Primary:
                    try
                    {
                        var oldDay = this.oldWeatherDataCollection.Single(x => x.Date.Equals(currDay.Date));
                        updatedWeatherDataCollection.Add(oldDay);
                    }
                    catch (Exception)
                    {
                    }

                    break;
                case ContentDialogResult.Secondary:
                    updatedWeatherDataCollection.Add(currDay);
                    break;
            }
        }

        #endregion
    }
}