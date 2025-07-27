using System.ComponentModel;
using System.Globalization;

namespace Core.Extensions
{
    public static class EnumExtensions
	{
		public static int ToInt(this Enum enumValue)
		{
			return Convert.ToInt32(enumValue, CultureInfo.InvariantCulture);
		}

		public static string GetDescriptionFromEnumValue(this Enum value)
		{
			var fi = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] attributes =
				(DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes != null &&
				attributes.Length > 0)
				return attributes[0].Description;
			else
				return value.ToString();
		}

		/// <summary>
		/// Enumerates all enum values
		/// </summary>
		/// <typeparam name="T">Enum type</typeparam>
		/// <returns>IEnumerable containing all enum values</returns>
		public static IEnumerable<T> GetValues<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}
	}
}