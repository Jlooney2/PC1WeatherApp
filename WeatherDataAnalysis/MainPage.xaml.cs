using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WeatherDataAnalysis.DataTier;
using WeatherDataAnalysis.Model;
using WeatherDataAnalysis.View;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WeatherDataAnalysis
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Data members

        /// <summary>
        ///     The application height
        /// </summary>
        public const int ApplicationHeight = 500;

        /// <summary>
        ///     The application width
        /// </summary>
        public const int ApplicationWidth = 625;

        private WeatherDataCollection currentWeatherCollection;
        private StringBuilder errors;
        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));

            this.currentWeatherCollection = null;
            this.errors = null;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the ClickAsync event of the loadFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private async void loadFile_ClickAsync(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            filePicker.FileTypeFilter.Add(".csv");
            filePicker.FileTypeFilter.Add(".txt");

            var chosenFile = await filePicker.PickSingleFileAsync();
            if (chosenFile == null)
            {
                this.summaryTextBox.Text = "No file was chosen.";
            }
            else
            {
                var fileParser = new WeatherFileParser();

                int.TryParse(this.lowerBoundTextBox.Text, out var lowerbound);
                int.TryParse(this.upperBoundTextBox.Text, out var upperbound);
                var reportBuilder = new WeatherReportBuilder(lowerbound, upperbound);

                var newWeatherCollection = await fileParser.ParseTemperatureFileAsync(chosenFile);

                if (this.currentWeatherCollection == null)
                {
                    this.currentWeatherCollection = newWeatherCollection;
                }
                else
                {
                    await this.handleNewFileWithExistingFile(newWeatherCollection);
                }

                this.errors = fileParser.ErrorMessages;
                var report = reportBuilder.CreateReport(this.currentWeatherCollection, this.getBucketSize());
                this.summaryTextBox.Text = report + this.errors;
            }
        }

        private async Task handleNewFileWithExistingFile(WeatherDataCollection newWeatherCollection)
        {
            var existingFileDialog = new ContentDialog {
                Title = "File Already Loaded",
                Content = "A file has already been loaded, how would you like to handle this?",
                PrimaryButtonText = "Replace Current File",
                SecondaryButtonText = "Merge the Files"
            };

            var result = await existingFileDialog.ShowAsync();

            switch (result)
            {
                case ContentDialogResult.Primary:
                    this.currentWeatherCollection = newWeatherCollection;
                    break;
                case ContentDialogResult.Secondary:
                {
                    var fileMerger = new FileMerger();
                    newWeatherCollection =
                        await fileMerger.MergeWeatherDataCollections(this.currentWeatherCollection, newWeatherCollection);
                    this.currentWeatherCollection = newWeatherCollection;
                    break;
                }
            }
        }

        private void BoundsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.currentWeatherCollection != null)
            {
                int.TryParse(this.lowerBoundTextBox.Text, out var lowerbound);
                int.TryParse(this.upperBoundTextBox.Text, out var upperbound);
                var reportBuilder = new WeatherReportBuilder(lowerbound, upperbound);
                var report = reportBuilder.CreateReport(this.currentWeatherCollection,this.getBucketSize());
                this.summaryTextBox.Text = report + this.errors;
            }
        }

        private int getBucketSize()
        {
            if ((bool)this.rbBucketSizeFive.IsChecked)
            {
                return 5;
            }
            else if((bool)this.rbBucketSizeTen.IsChecked)
            {
                return 10;
            }
            else
            {
                return 20;
            }
        }

        #endregion

        private void bucketSize_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (this.currentWeatherCollection != null)
            {
                int.TryParse(this.lowerBoundTextBox.Text, out var lowerbound);
                int.TryParse(this.upperBoundTextBox.Text, out var upperbound);
                var reportBuilder = new WeatherReportBuilder(lowerbound, upperbound);
                var report = reportBuilder.CreateReport(this.currentWeatherCollection, this.getBucketSize());
                this.summaryTextBox.Text = report + this.errors;
            }
        }

        private void clearDataButton_Click(object sender, RoutedEventArgs e)
        {
            this.currentWeatherCollection = null;
            this.summaryTextBox.Text = string.Empty;
        }

        private void saveToFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentWeatherCollection != null)
            {
                FileSaver.SaveToCsv(this.currentWeatherCollection);
            }
        }
    }
}