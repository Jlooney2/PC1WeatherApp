using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WeatherDataAnalysis.Model;

namespace WeatherDataAnalysis.View
{
    /// <summary>
    ///     Defines a report builder for Temperature data
    /// </summary>
    public class WeatherReportBuilder
    {
        #region Data members

        private readonly int lowerbound;
        private readonly int upperbound;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherReportBuilder"/> class.
        /// </summary>
        /// <param name="lowerbound">The lowerbound.</param>
        /// <param name="upperbound">The upperbound.</param>
        public WeatherReportBuilder(int lowerbound, int upperbound)
        {
            this.lowerbound = lowerbound;
            this.upperbound = upperbound;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates the report.
        /// </summary>
        /// <param name="daysForReport">The days for report.</param>
        /// <returns>A full report outlining specific information based the supplied weather data</returns>
        public string CreateReport(WeatherDataCollection daysForReport)
        {
            var report = new StringBuilder();

            var years = daysForReport.GroupByYear();
            foreach (var year in years)
            {
                var tempWeatherCollection = new WeatherDataCollection();
                foreach (var day in year)
                {
                    tempWeatherCollection.Add(day);
                }

               
            }

            report.Append(Environment.NewLine);

            this.createYearlyBreakdown(daysForReport, report);
            return report.ToString();
        }

        private void createYearlyBreakdown(WeatherDataCollection daysForReport, StringBuilder report)
        {
            var years = daysForReport.GroupByYear();

            foreach (var year in years)
            {
                var tempWeatherCollection = new WeatherDataCollection();
                foreach (var day in year)
                {
                    tempWeatherCollection.Add(day);
                }
                this.createOverview(tempWeatherCollection, report);
                report.Append(Environment.NewLine);
                this.createMonthlyBreakdown(tempWeatherCollection, report);
            }
        }

        private void createMonthlyBreakdown(WeatherDataCollection daysForReport, StringBuilder report)
        {
            
            var highestTemp = daysForReport.GetDaysWithHighestTempForEachMonth();
            var lowestTemp = daysForReport.GetDaysWithLowestTempForEachMonth();
            var averageHigh = daysForReport.GetAverageHighTempForEachMonth();
            var averageLow = daysForReport.GetAverageLowTempForEachMonth();
            var monthCount = 1;

            for (var i = 0; i < daysForReport.GroupByMonth().Count; i++)
            {
                while (highestTemp[i][0].Date.Month != monthCount && monthCount < 12)
                {
                    var prevMonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(monthCount);
                    var prevMonthYear = highestTemp[i][0].Date.Year;
                    var prevMonthsDaysOfData = $" (0 days of data)";
                    report.Append(prevMonthName + $" {prevMonthYear}" + prevMonthsDaysOfData + Environment.NewLine);
                    report.Append(Environment.NewLine);
                    monthCount++;
                }

                var monthName = highestTemp[i][0].Date.ToString("MMMM", CultureInfo.InvariantCulture);
                var year = highestTemp[i][0].Date.Year;
                var daysOfData = $" ({daysForReport.GroupByMonth()[i].Count} days of data)";
                report.Append(monthName + $" {year}" + daysOfData + Environment.NewLine);

                this.formatHighestTempForMonths(highestTemp, i, report);
                this.formatLowestTempForMonths(lowestTemp, i, report);
                report.Append($"Average High: {averageHigh[i]:F}" + Environment.NewLine);
                report.Append($"Average Low: {averageLow[i]:F}" + Environment.NewLine);
                report.Append(Environment.NewLine);
                monthCount++;
            }
        }

        private void formatLowestTempForMonths(List<List<WeatherData>> lowestTemp, int i, StringBuilder report)
        {
            var lowestTempValue = lowestTemp[i][0].Low;

            report.Append($"Lowest Temp: {lowestTempValue} on the ");

            this.createDayWithSuffix(lowestTemp, i, report);

            report.Append(Environment.NewLine);
        }

        private void formatHighestTempForMonths(List<List<WeatherData>> highestTemp, int i, StringBuilder report)
        {
            var highestTempValue = highestTemp[i][0].High;

            report.Append($"Highest Temp: {highestTempValue} on the ");

            this.createDayWithSuffix(highestTemp, i, report);

            report.Append(Environment.NewLine);
        }

        private void createDayWithSuffix(List<List<WeatherData>> temps, int i, StringBuilder report)
        {
            foreach (var currLow in temps[i])
            {
                var day = string.Format(currLow.Date.Day + "{0}", this.getDaySuffix(currLow.Date));
                report.Append(day + " ");
            }
        }

        private void createOverview(WeatherDataCollection daysForReport, StringBuilder report)
        {
            var dates = new List<string>();
            foreach (var current in daysForReport.GetDaysWithHighestLowTempByYear())
            {
                dates.Add(current.Date.ToShortDateString());
            }

            var daysWithHighestLow = string.Join(", ", dates);

            report.Append(
                $"Highest temp occurred on {this.formatDaysWithHighestTempForYear(daysForReport)}: {daysForReport.GetDaysWithHighestTempForAYear()[0].High}" +
                Environment.NewLine);
            report.Append(
                $"Lowest temp occurred on {this.formatDaysWithLowestTempForYear(daysForReport)}: {daysForReport.GetDaysWithLowestTempByYear()[0].Low}" +
                Environment.NewLine);
            report.Append(
                $"Lowest high temp occurred on {this.formatDaysWithLowestHighTempForYear(daysForReport)}: {daysForReport.GetDaysWithLowestHighTempByYear()[0].High}" +
                Environment.NewLine);
            report.Append(
                $"Highest low temp occurred on {daysWithHighestLow}: {daysForReport.GetDaysWithHighestLowTempByYear()[0].Low}" +
                Environment.NewLine);
            report.Append($"The average high: {daysForReport.GetAverageHighTempForYear():F}" +
                          Environment.NewLine);
            report.Append($"The average low: {daysForReport.GetAverageLowTempForYear():F}" +
                          Environment.NewLine);
            report.Append(
                $"Number of days with temp {this.upperbound} or greater: {daysForReport.GetDaysWithTempGreaterThanEqualTo(this.upperbound)}" +
                Environment.NewLine);
            report.Append(
                $"Number of days with temp {this.lowerbound} or less: {daysForReport.GetDaysWithTempLessThanEqualTo(this.lowerbound)}" +
                Environment.NewLine);
        }

        private string formatDaysWithLowestHighTempForYear(WeatherDataCollection daysForReport)
        {
            var dates = new List<string>();
            foreach (var current in daysForReport.GetDaysWithLowestHighTempByYear())
            {
                dates.Add(current.Date.ToShortDateString());
            }

            var daysWithLowestHigh = string.Join(", ", dates);
            return daysWithLowestHigh;
        }

        private string formatDaysWithLowestTempForYear(WeatherDataCollection daysForReport)
        {
            var dates = new List<string>();
            foreach (var current in daysForReport.GetDaysWithLowestTempByYear())
            {
                dates.Add(current.Date.ToShortDateString());
            }

            var daysWithLowest = string.Join(",", dates);
            return daysWithLowest;
        }

        private string formatDaysWithHighestTempForYear(WeatherDataCollection daysForReport)
        {
            var dates = new List<string>();
            foreach (var current in daysForReport.GetDaysWithHighestTempForAYear())
            {
                dates.Add(current.Date.ToShortDateString());
            }

            var daysWithHighest = string.Join(",", dates);
            return daysWithHighest;
        }

        private string getDaySuffix(DateTime day)
        {
            string suffix;
            var dayOfMonth = day.Date.Day;

            switch (dayOfMonth % 10)
            {
                case 1 when dayOfMonth != 11:
                    suffix = "st";
                    break;
                case 2 when dayOfMonth != 12:
                    suffix = "nd";
                    break;
                case 3 when dayOfMonth != 13:
                    suffix = "rd";
                    break;
                default:
                    suffix = "th";
                    break;
            }

            return suffix;
        }

        #endregion
    }
}