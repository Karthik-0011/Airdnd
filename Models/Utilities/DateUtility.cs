using System.Globalization;

namespace Airdnd.Models.Utilities
{
    public static class DateUtility
    {
        public struct DateRange
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        public static DateRange ParseDateRange(string selectedDates)
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(3); 

            if (!string.IsNullOrEmpty(selectedDates) && selectedDates.ToLower() != "all")
            {
                var dates = selectedDates.Split(" - ");
                if (dates.Length == 2)
                {
                    // Use simple TryParse
                    DateTime.TryParse(dates[0], out startDate);
                    DateTime.TryParse(dates[1], out endDate);
                }
            }

            return new DateRange { StartDate = startDate, EndDate = endDate };
        }
    }
}