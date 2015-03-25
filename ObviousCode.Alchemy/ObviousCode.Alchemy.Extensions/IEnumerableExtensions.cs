using System;
using System.Linq;
using System.Collections.Generic;

namespace ObviousCode
{
	public static class IEnumerableExtensions
	{
		public static void For<T>(this IEnumerable<T> me, Action<T, int> action)
		{
			int i = 0;

			foreach (T item in me) {
				action (item, i);
				i++;
			}
		}

		public static void ForEach<T>(this IEnumerable<T> me, Action<T> action)
		{
			foreach (T item in me) {
				action (item);
			}
		}
	}
}

