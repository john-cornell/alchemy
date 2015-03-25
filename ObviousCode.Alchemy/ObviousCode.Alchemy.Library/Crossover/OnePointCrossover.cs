using System;
using ObviousCode.Alchemy.Library.Populous;

namespace ObviousCode.Alchemy.Library
{
	public class OnePointCrossover : Crossover
	{
		public OnePointCrossover () : base(CrossoverType.OnePoint)
		{
		}

		protected override void PopulateChild<T> (Individual<T> child, Individual<T> parent1, Individual<T> parent2)
		{
			int crossoverPoint = ConfigurationProvider.Rnd.Next (0, parent1.Code.Length);

			var entry = CreateLogEntry () as OnePointCrossoverLogEntry;

			entry.CrossoverPoint = crossoverPoint;

			Array.Copy (parent1.Code, 0, child.Code, 0, crossoverPoint);
			Array.Copy (parent2.Code, crossoverPoint, child.Code, crossoverPoint, child.Code.Length - crossoverPoint);
		}
	}
}