using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WeatherDataAnalysis.Model;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherDataAnalysis.View
{

    /// <summary>
    /// Asks user to keep or replace existing data
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    public sealed partial class ReplaceOrKeepDialog
    {
        /// <summary>
        /// Gets a value indicating whether this instance is do for all checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is do for all checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsDoForAllChecked { get; private set; }
        /// <summary>
        /// Gets the chosen result.
        /// </summary>
        /// <param name="conflictingDay">The conflicting day.</param>
        /// <param name="countOfData">The count of data.</param>
        /// <value>
        /// The chosen result.
        /// </value>
        public ReplaceOrKeepDialog(WeatherData conflictingDay, int countOfData)
        {
            this.InitializeComponent();
            if (countOfData <= 1)
            {
                this.doForAllCheckBox.Visibility = Visibility.Collapsed;
            }
            this.descriptionTextBlock.Text = $"{conflictingDay.Date.ToShortDateString()} appears twice, How would you like to handle this?";
            this.IsDoForAllChecked = false;
            
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if ((bool) this.doForAllCheckBox.IsChecked)
            {
                this.IsDoForAllChecked = true;
            }

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if ((bool )this.doForAllCheckBox.IsChecked)
            {
                this.IsDoForAllChecked = true;
            }

        }
    }
}
