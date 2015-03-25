using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Library
{
	public static class SelectionProvider
	{
		static Dictionary<SelectionType, Selector> _selectors;

		static SelectionProvider ()
		{
			Reset ();
		}

		public static void Add(Selector selector)
		{
			_selectors [selector.SelectionMethod] = selector;
		}

		public static Selector GetSelector(SelectionType method)
		{
			if (!_selectors.ContainsKey (method))
				throw new SelectorNotAvailableException (method);

			return _selectors[method];
		}

		public static Selector GetSelector()
		{
			return GetSelector (ConfigurationProvider.Selection.SelectionMethod);
		}

		public static void Reset ()
		{
			_selectors = new Dictionary<SelectionType, Selector> ();

			Add (new TournamentSelector ());
			Add (new TruncationSelector ());
			Add (new RouletteSelector ());
		}
	}
}

