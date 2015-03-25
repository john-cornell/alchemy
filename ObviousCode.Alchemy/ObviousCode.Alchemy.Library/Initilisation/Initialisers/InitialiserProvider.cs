using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Library
{
	public static class InitialiserProvider
	{
		static Dictionary<int, IndividualInitialiser> _initialisers;

		static InitialiserProvider ()
		{
			LoadDefaultProviders ();
		}

		public static IndividualInitialiser<T> GetInitialiser<T>()
		{
			int hashKey = typeof(T).GetHashCode ();

			if (_initialisers.ContainsKey (hashKey))
				return _initialisers [hashKey] as IndividualInitialiser<T>;

			throw new InitialiserNotAvailableException (typeof(T));
		}

		public static void AddInitialiser(IndividualInitialiser initialiser)
		{
			int hashKey = initialiser.GenomeType.GetHashCode ();

			_initialisers [hashKey] = initialiser;
		}

		static void LoadDefaultProviders ()
		{
			_initialisers = new Dictionary<int, IndividualInitialiser> ();

			AddInitialiser (new ByteBasedGenomeInitialiser ());
			AddInitialiser (new IntBasedGenomeInitialiser ());
			AddInitialiser (new DoubleBasedGenomeInitialiser ());
			AddInitialiser (new BoolBasedGenomeInitialiser ());
		}

		public static void Reset()
		{
			LoadDefaultProviders ();
		}
	}
}

