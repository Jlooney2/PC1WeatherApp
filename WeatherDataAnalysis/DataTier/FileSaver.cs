using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;
using Windows.Storage.Pickers;
using WeatherDataAnalysis.Model;

namespace WeatherDataAnalysis.DataTier
{
    /// <summary>
    /// saves the collection of data to a csv file
    /// </summary>
    public class FileSaver
    {
        #region Methods

        /// <summary>
        /// Saves to CSV.
        /// </summary>
        /// <param name="weatherDataCollection">The weather data collection.</param>
        public static async void SaveToCsv(WeatherDataCollection weatherDataCollection)
        {
            var fileSaver = new FileSavePicker {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            fileSaver.FileTypeChoices.Add("CSV", new List<string> {".csv"});

            var saveFile = await fileSaver.PickSaveFileAsync();

            if (saveFile != null)
            {
                CachedFileManager.DeferUpdates(saveFile);

                var dataForFile = new StringBuilder();
                foreach (var day in weatherDataCollection)
                {
                    dataForFile.Append($"{day.Date.ToShortDateString()},{day.High},{day.Low}{Environment.NewLine}");
                }

                await FileIO.WriteTextAsync(saveFile, dataForFile.ToString());
            }
        }

        #endregion
    }
}