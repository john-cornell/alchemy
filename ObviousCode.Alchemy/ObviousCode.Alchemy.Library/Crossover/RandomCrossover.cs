using System;

namespace ObviousCode.Alchemy.Library
{
	public class RandomCrossover : Crossover
	{
		public RandomCrossover () : base(Crossover.CrossoverType.Random)
		{
		}

		#region implemented abstract members of Crossover

		protected override void PopulateChild<T> (ObviousCode.Alchemy.Library.Populous.Individual<T> child, ObviousCode.Alchemy.Library.Populous.Individual<T> parent1, ObviousCode.Alchemy.Library.Populous.Individual<T> parent2)
		{
			CrossoverProvider.Log.GetNextEntry (CrossoverType.Random);

			Array.Copy (parent1.Code, child.Code, parent1.GenomeLength);

			parent2.Code.For ((item, i) => {
				if (ConfigurationProvider.Rnd.NextDouble()<=ConfigurationProvider.Crossover.ChanceOfRandomGeneCrossover)
				{
					child[i] = parent2[i];
				}
			});
		}

		#endregion
	}
}

