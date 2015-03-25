using System;

namespace ObviousCode
{
	public static class ObjectExtensions
	{
		public static T As<T>(this object me) where T : class
		{
			return me as T;
		}

		public static T To<T>(this object me)
		{
			return (T)me;
		}
	}
}

