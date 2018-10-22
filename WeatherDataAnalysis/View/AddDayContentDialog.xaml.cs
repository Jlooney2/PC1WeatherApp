using Windows.UI.Xaml.Controls;
using WeatherDataAnalysis.Model;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherDataAnalysis.View
{
    /// <summary>
    /// Requests data for a new WeatherData object
    /// </summary>
    /// <seealso cref="T:Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="T:Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="T:Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class AddDayContentDialog
    {
        /// <summary>
        /// Gets the new day.
        /// </summary>
        /// <value>
        /// The new day.
        /// </value>
        public WeatherData NewDay { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="AddDayContentDialog"/> class.
        /// </summary>
        public AddDayContentDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var date = this.newDayDatePicker.Date.Date;
            int.TryParse(this.highTempTextBox.Text, out var high);
            int.TryParse(this.lowTempTextBox.Text, out var low);
            this.NewDay = new WeatherData(date,high,low);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.NewDay = null;
        }
    }
}
