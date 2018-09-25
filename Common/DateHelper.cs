using System;

namespace Common
{
    public static class DateHelper
    {

        public static int RemaingDaysOfMonth
        {
            get
            {
                var today = DateTime.Today;
                return (today.Day - DateTime.DaysInMonth(today.Year, today.Month));
            }
        }

        public static (int NoOfDays, string MonthName) RemaingDaysToNextCycle
        {
            get
            {
                var after100day = DateTime.Today.AddDays(100);
                var days = (new DateTime(after100day.Year, after100day.Month, DateTime.DaysInMonth(after100day.Year, after100day.Month)) - after100day).Days;

                return (days, after100day.ToString("Y"));
            }
        }

        public static string DaysToMonth(string prefix, DateTime startDate, DateTime endDate)
        {

            var totalDays = (endDate - startDate).TotalDays;
            //var totalYears = Math.Truncate(totalDays / 365);
            var totalMonths = Math.Truncate((totalDays % 365) / 30);
            var remainingDays = Math.Truncate((totalDays % 365) % 30);
            return $"{prefix} {totalMonths} month(s) and {remainingDays} day(s)";
        }
    }
}
