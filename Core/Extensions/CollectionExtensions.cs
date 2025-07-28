using System.Collections.ObjectModel;
using System.Text;

namespace Azm.Core.Extensions
{
    public static class CollectionExtensions
	{
		public static Collection<T> AddRange<T>(this Collection<T> collection, IEnumerable<T> items)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));
			if (items == null)
				throw new ArgumentNullException(nameof(items));

			foreach (var each in items)
			{
				collection.Add(each);
			}

			return collection;
		}

		public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			return new ObservableCollection<T>(source);
		}

		public static T FirstOrDefaultFromMany<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector, Predicate<T> condition)
		{
			// return default if no items
			if (source == null || !source.Any()) return default(T);

			// return result if found and stop traversing hierarchy
			var attempt = source.FirstOrDefault(t => condition(t));
			if (!Equals(attempt, default(T))) return attempt;

			// recursively call this function on lower levels of the
			// hierarchy until a match is found or the hierarchy is exhausted
			return source.SelectMany(childrenSelector)
				.FirstOrDefaultFromMany(childrenSelector, condition);
		}

		public static List<T> AsList<T>(this T source)
		{
			return new List<T> { source };
		}

		public static string ListToCsv<T>(this IEnumerable<T> sourceList)
		{
			StringBuilder csvContent = new StringBuilder(string.Empty);

			var properties = typeof(T).GetProperties();
			bool[] propIsClass = new bool[properties.Length];
			string[] propSystemFullName = new string[properties.Length];

			for (int i = 0; i < properties.Length; i++)
			{
				csvContent .Append("\"" + properties[i].Name + "\"" + (i < properties.Length - 1 ? "," : ""));
				propIsClass[i] = properties[i].PropertyType.IsClass;
				propSystemFullName[i] = properties[i].PropertyType.FullName;
			}

			csvContent.Append(Environment.NewLine.ToString());

			foreach (var sourceObject in sourceList)
			{
				for (int i = 0; i < properties.Length; i++)
				{
					var propValue = properties[i].GetValue(sourceObject, null);
					if (propIsClass[i] == true && propSystemFullName[i] != "System.String")
						csvContent.Append("\"\"" + (i < properties.Length - 1 ? "," : ""));
					else
						csvContent.Append("\"" + propValue?.ToString() + "\"" + (i < properties.Length - 1 ? "," : ""));
				}
				csvContent.Append(Environment.NewLine.ToString());
			}

			return csvContent.ToString();
		}
	}
}