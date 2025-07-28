namespace Azm.Core.Extensions
{
    public static class GeneralExtensions
	{
		public static IEnumerable<DateTime> GetMonthDates(int month)
		{
			return Enumerable.Range(1, DateTime.DaysInMonth(DateTime.Now.Year, month))
							 .Select(day => new DateTime(DateTime.Now.Year, month, day))
							 .ToList();
		}

		public static IEnumerable<DateTime> GetMonthDates(int year, int month)
		{
			return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
							 .Select(day => new DateTime(year, month, day))
							 .ToList();
		}
	}
}