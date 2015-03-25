using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Library
{
	public static class MutationProvider
	{
		static Dictionary<int, Mutator> _mutators;

		static MutationProvider ()
		{
			Reset ();
		}

		public static void Reset ()
		{
			_mutators = new Dictionary<int, Mutator> ();

			AddMutator (new IntBasedMutator ());
			AddMutator (new BoolBasedMutator ());
			AddMutator (new ByteBasedMutator ());
			AddMutator (new DoubleBasedMutator ());
		}

		public static void AddMutator(Mutator mutator)
		{
			int hashKey = mutator.GenomeType.GetHashCode();

			_mutators[hashKey] = mutator;
		}

		public static Mutator<T> GetMutator<T>()
		{
			int hashKey = typeof(T).GetHashCode ();

			if (!_mutators.ContainsKey (hashKey))
				throw new MutatorNotAvailableException (typeof(T));

			return _mutators [hashKey] as Mutator<T>;
		}
	}
}

