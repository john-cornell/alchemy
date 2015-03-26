using System;
using ObviousCode.Alchemy.Library.Populous;

namespace ObviousCode.Alchemy.Creatures.Darwin
{
	public class PopulationEventArgs : EventArgs
	{
		public PopulationEventArgs (Population<byte> population, int generation)
		{
			Population = population;
			Generation = generation;
		}

		public Population<byte> Population {
			get;
			private set;
		}

		public int Generation {
			get;
			private set;
		}
	}
}

