using System;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
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

            this.addNonConflictingData(updatedWeatherDataCollection);

            foreach (var conflictingDay in this.newWeatherDataCollection)
            {
                if (this.oldWeatherDataCollection.Any(oldDay => oldDay.Date.Equals(conflictingDay.Date)) &&
                    !this.isDoForAllChecked)
                {
                    var keepOrReplace = new ReplaceOrKeepDialog(conflictingDay);
                    this.chosenResult = await keepOrReplace.ShowAsync();
                    this.isDoForAllChecked = keepOrReplace.IsDoForAllChecked;
                }

                this.addConflictingDayToUpdatedCollection(conflictingDay, updatedWeatherDataCollection);
            }

            var tempWeatherDataCollection = updatedWeatherDataCollection.ToList();
            tempWeatherDataCollection.Sort();
            updatedWeatherDataCollection.Clear();

            foreach (var day in tempWeatherDataCollection)
            {
                updatedWeatherDataCollection.Add(day);
            }
            return updatedWeatherDataCollection;
        }

        private void addNonConflictingData( WeatherDataCollection updatedWeatherDataCollection)
        {
            var oldNonConflictingData = this.oldWeatherDataCollection.Where(oldDay => !this.newWeatherDataCollection.Any(newDay => newDay.Date.Equals(oldDay.Date))).ToList();
            var newNonConflictingData = this.newWeatherDataCollection.Where(newDay => !this.oldWeatherDataCollection.Any(oldDay => oldDay.Date.Equals(newDay.Date))).ToList();

            foreach (var day in oldNonConflictingData)
            {
                updatedWeatherDataCollection.Add(day);
            }

            foreach (var day in newNonConflictingData)
            {
                updatedWeatherDataCollection.Add(day);
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