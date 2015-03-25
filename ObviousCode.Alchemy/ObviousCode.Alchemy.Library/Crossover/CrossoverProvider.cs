using System;
using System.Collections.Generic;
using System.Linq;

namespace ObviousCode.Alchemy.Library
{
	public static class CrossoverProvider
	{
		static Dictionary<int, Crossover> _crossovers;
		static Crossover.CrossoverType[] _selection;

		static CrossoverProvider ()
		{
			_crossovers = new Dictionary<int, Crossover> ();
			Log = new CrossoverLog ();

			Reset ();
		}

		public static CrossoverLog Log { get; private set; }

		public static void AddCrossover(Crossover crossover)
		{
			int hashKey = crossover.CrossoverMethod.GetHashCode ();

			_crossovers [hashKey] = crossover;
		}

		public static Crossover GetCrossover(Crossover.CrossoverType method)
		{
			int hashKey = method.GetHashCode ();

			return _crossovers.ContainsKey (hashKey) ? _crossovers [hashKey] : null;
		}	

		public static void AssertSelectionArray(IEnumerable<Crossover.CrossoverType> expected)
		{
			if (_selection == null)
				throw new InvalidOperationException ("Crossover Selection Array is null");

			if (_selection.Length != expected.Count ())
				throw new InvalidOperationException ("Crossover selection array and expected array are different lengths");

			expected.For ((item, i) => {
				if (_selection [i] != item)
					throw new InvalidOperationException(
						string.Format("Expected {0} at position {1}. Was {2}", _selection[i], i, item));
			});
		}

		public static void RebuildSelectionArray ()
		{
			int fullSelectionLength = 
				ConfigurationProvider.Crossover.OnePointCrossoverRatio +
				ConfigurationProvider.Crossover.TwoPointCrossoverRatio +
				ConfigurationProvider.Crossover.RandomCrossoverRatio;

			_selection = new Crossover.CrossoverType[fullSelectionLength];

			int i = 0;

			i = AddToSelectionArray(ConfigurationProvider.Crossover.OnePointCrossoverRatio, i, Crossover.CrossoverType.OnePoint);
			i = AddToSelectionArray(ConfigurationProvider.Crossover.TwoPointCrossoverRatio, i, Crossover.CrossoverType.TwoPoint);
			AddToSelectionArray(ConfigurationProvider.Crossover.RandomCrossoverRatio, i, Crossover.CrossoverType.Random);
		}
			
		private static int AddToSelectionArray(int amount, int idx, Crossover.CrossoverType crossover)
		{
			for (int i = 0; i < amount; i++) {

				_selection [idx] = crossover;

				idx = idx + 1;
			}

			return idx;
		}

		public static Crossover GetNextCrossover()
		{
			return GetCrossover(_selection[ConfigurationProvider.Rnd.Next(_selection.Length)]);
		}

		public static CrossoverLogEntry LastLogEntry
		{
			get { return Log.Entries.Count == 0 ? null : Log.Entries.Last(); }
		}

		public static void Reset ()
		{
			Log.Clear ();

			_crossovers.Clear ();

			AddCrossover (new OnePointCrossover ());
			AddCrossover (new TwoPointCrossover ());
			AddCrossover (new RandomCrossover ());
		}
	}
}

