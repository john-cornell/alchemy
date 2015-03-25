using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Library
{
	public static class InitialiserContextProvider
	{
		static Dictionary<int, InitialiserContext> _contexts;

		static InitialiserContextProvider ()
		{
			Reset ();
		}

		public static InitialiserContext GetContext<T>()
		{
			int hashKey = typeof(T).GetHashCode ();

			if (!_contexts.ContainsKey (hashKey))
				throw new InitialiserContextNotAvailableException (typeof(T));

			return _contexts [hashKey];
		}

		public static void AddContext<T>(InitialiserContext<T> context)
		{
			int hashkey = typeof(T).GetHashCode ();

			_contexts [hashkey] = context;
		}

		public static void Reset ()
		{
			_contexts = new Dictionary<int, InitialiserContext> ();

			AddContext (new ByteBasedGenomeInitialiserContext ());
			AddContext (new IntBasedGenomeInitialiserContext ());
			AddContext (new DoubleBasedGenomeInitialiserContext ());
			AddContext (new BoolBasedGenomeInitialiserContext ());
		}			
	}
}

