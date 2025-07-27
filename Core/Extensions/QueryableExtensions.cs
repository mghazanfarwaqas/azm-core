using System.Collections.ObjectModel;
using System.Reflection;
using Core.Utils;

namespace Core.Extensions
{
	public static class QueryableExtensions
	{
		public static void Merge<T>(this ObservableCollection<T> source, IEnumerable<T> collection)
		{
			Merge<T>(source, collection, false);
		}

		public static void Merge<T>(this ObservableCollection<T> source, IEnumerable<T> collection, bool ignoreDuplicates)
		{
			if (collection != null)
			{
				foreach (T item in collection)
				{
					bool addItem = true;

					if (ignoreDuplicates)
						addItem = !source.Contains(item);

					if (addItem)
						source.Add(item);
				}
			}
		}


		static Dictionary<string, bool> BrowsableProperties = new Dictionary<string, bool>();
		static Dictionary<string, PropertyInfo[]> BrowsablePropertyInfos = new Dictionary<string, PropertyInfo[]>();

		public async static Task<PagedList<T>> ToPagedList<T>(this IQueryable<T> source, int page, int pageSize)
		{
			var pagedList = new PagedList<T>(source, page, pageSize);
			await pagedList.GetPagedResults();
			return pagedList;
		}
	}
}