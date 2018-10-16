using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WeatherDataAnalysis.Model;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherDataAnalysis.View
{

    /// <summary>
    /// Asks user to keep or replace existing data
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    public sealed partial class ReplaceOrKeepDialog : ContentDialog
    {
        /// <summary>
        /// Gets a value indicating whether this instance is do for all checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is do for all checked; otherwise, <c>false</c>.
        /// </value>
        public bool isDoForAllChecked { get; private set; }
        /// <summary>
        /// Gets the chosen result.
        /// </summary>
        /// <value>
        /// The chosen result.
        /// </value>
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceOrKeepDialog"/> class.
        /// </summary>
        public ReplaceOrKeepDialog(WeatherData conflictingDay)
        {
            this.InitializeComponent();
            this.DescriptionTextBlock.Text = $"{conflictingDay.Date.ToShortDateString()} appears twice, How would you like to handle this?";
            this.isDoForAllChecked = false;
            
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if ((bool) this.DoForAllCheckBox.IsChecked)
            {
                this.isDoForAllChecked = true;
            }

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if ((bool )this.DoForAllCheckBox.IsChecked)
            {
                this.isDoForAllChecked = true;
            }

        }
    }
}
