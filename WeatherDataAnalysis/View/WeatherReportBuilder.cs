﻿using System;
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

        private readonly int lowerBound;
        private readonly int upperBound;
        private int bucketSize;
        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeatherReportBuilder" /> class.
        /// </summary>
        /// <param name="lowerBound">The lowerBound.</param>
        /// <param name="upperBound">The upperBound.</param>
        public WeatherReportBuilder(int lowerBound, int upperBound)
        {
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the report.
        /// </summary>
        /// <param name="daysForReport">The days for report.</param>
        /// <param name="bucketSize">Size of the bucket.</param>
        /// <returns>
        /// A full report outlining specific information based the supplied weather data
        /// </returns>
        public string CreateReport(WeatherDataCollection daysForReport,int bucketSize)
        {
            var report = new StringBuilder();
            this.bucketSize = bucketSize;
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

                var upperBound = tempWeatherCollection.GetDaysWithHighestTempForAYear()[0].High;
                var lowerBound = tempWeatherCollection.GetDaysWithLowestHighTempByYear()[0].High;
                

                report.Append("Highest Temp Histogram" + Environment.NewLine);
                this.createHistogram(report, tempWeatherCollection,lowerBound,upperBound,this.bucketSize,HighLow.High);
                report.Append(Environment.NewLine);


                upperBound = tempWeatherCollection.GetDaysWithHighestLowTempByYear()[0].Low;
                lowerBound = tempWeatherCollection.GetDaysWithLowestTempByYear()[0].Low;
                

                report.Append("Lowest Temp Histogram" + Environment.NewLine);
                this.createHistogram(report,tempWeatherCollection,lowerBound,upperBound,this.bucketSize, HighLow.Low);
                report.Append(Environment.NewLine);


                this.createMonthlyBreakdown(tempWeatherCollection, report);
            }
        }

        private void createHistogram(StringBuilder report, WeatherDataCollection tempWeatherCollection, int lowerBound, int upperBound,int bucketSize, HighLow highOrLow)
        {
            var initialTierLowerBound = lowerBound - (lowerBound % bucketSize);
            int initialOffset = 1; 
            var initialTierUpperBound = initialTierLowerBound + bucketSize - initialOffset;
            var highestTierOffset = bucketSize - 1;
            var finalTierUpperBound = upperBound - upperBound % bucketSize + highestTierOffset;

            while (initialTierLowerBound <= finalTierUpperBound)
            {
                int countOfDays;
                if (highOrLow.Equals(HighLow.High))
                {
                    countOfDays = tempWeatherCollection.CountDaysWithHighBetween(initialTierLowerBound, initialTierUpperBound);
                }
                else
                {
                    countOfDays = tempWeatherCollection.CountDaysWithLowBetween(initialTierLowerBound, initialTierUpperBound);
                }
                report.Append(
                    $"{initialTierLowerBound}-{initialTierUpperBound}: {countOfDays} {Environment.NewLine}");
                initialTierLowerBound = initialTierLowerBound + bucketSize;
                initialTierUpperBound = initialTierUpperBound + bucketSize;
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
                    var prevMonthsDaysOfData = " (0 days of data)";
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
            foreach (var currentLow in temps[i])
            {
                var day = string.Format(currentLow.Date.Day + "{0}", this.getDaySuffix(currentLow.Date));
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
                $"Number of days with temp {this.upperBound} or greater: {daysForReport.GetDaysWithTempGreaterThanEqualTo(this.upperBound)}" +
                Environment.NewLine);
            report.Append(
                $"Number of days with temp {this.lowerBound} or less: {daysForReport.GetDaysWithTempLessThanEqualTo(this.lowerBound)}" +
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