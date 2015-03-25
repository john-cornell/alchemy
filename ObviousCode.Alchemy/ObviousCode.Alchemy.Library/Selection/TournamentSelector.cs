using System;
using System.Collections.Generic;
using System.Linq;
using ObviousCode.Alchemy.Library.Populous;

namespace ObviousCode.Alchemy.Library
{
	public class TournamentSelector : Selector	
	{
		public TournamentSelector () : base(SelectionType.Tournament)
		{
		}

		protected override IEnumerable<Individual<T>> PerformSelection<T> (Population<T> population, List<IndexedFitness> fitness)
		{
			for (int i = 0; i < ConfigurationProvider.Selection.NumberOfFittestSelected; i++) {
				yield return population.Individuals [fitness [i].Index];
			}
		}
	}
}

