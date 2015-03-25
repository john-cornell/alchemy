using System;
using ObviousCode.Alchemy.Library.Populous;
using System.Collections.Generic;
using System.Linq;

namespace ObviousCode.Alchemy.Library
{
	public class RouletteSelector : Selector
	{
		double? _threshold;

		public double LastMaxThreshold {
			get;
			private set;
		}
		public RouletteSelector () : base(SelectionType.Roulette)
		{
		}

		protected override void OnBeforeSelection (List<IndexedFitness> indexed)
		{
			LastMaxThreshold = indexed.Sum (f => f.Fitness);

			_threshold = ConfigurationProvider.Selection.ForcedRouletteThreshold.HasValue ?
							ConfigurationProvider.Selection.ForcedRouletteThreshold :
							ConfigurationProvider.Rnd.NextDouble () * LastMaxThreshold;

		}

		#region implemented abstract members of Selector

		protected override IEnumerable<Individual<T>> PerformSelection<T> (Population<T> population, List<IndexedFitness> fitness)
		{
			double current = 0d;
			int returnedCount = 0;

			if (ConfigurationProvider.Selection.RouletteFitnessSortDirection == SelectionConfiguration.RouletteFitnessDirection.Ascending) {
				fitness.Reverse ();
			}

			for (int i=0;i<population.Individuals.Count;i++)
			{
				current += fitness [i].Fitness;

				if (current >= _threshold) {
					returnedCount += 1;
					yield return population.Individuals [fitness [i].Index];
				}

				if (returnedCount == ConfigurationProvider.Selection.NumberOfFittestSelected)
					yield break;
			}
		}

		#endregion
	}
}

