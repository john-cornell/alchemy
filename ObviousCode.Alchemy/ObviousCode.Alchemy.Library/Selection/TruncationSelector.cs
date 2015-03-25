using System;
using ObviousCode.Alchemy.Library.Populous;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Library
{
	public class TruncationSelector : Selector
	{
		public TruncationSelector () : base(SelectionType.Truncation)
		{
		}

		#region implemented abstract members of Selector

		protected override System.Collections.Generic.IEnumerable<Individual<T>> PerformSelection<T> (Population<T> population, List<IndexedFitness> fitness)
		{
			int selectionLength = (int)((decimal)fitness.Count * ConfigurationProvider.Selection.RatioOfFittestInTruncationSelection);

			for (int i = 0; i < selectionLength; i++) {
				yield return population.Individuals [fitness [i].Index];
			}
		}

		#endregion
	}
}
