using System;

namespace ObviousCode.Alchemy.Library
{
	public class TwoPointCrossover : Crossover
	{
		public TwoPointCrossover () : base(Crossover.CrossoverType.TwoPoint)
		{
		}

		#region implemented abstract members of Crossover

		protected override void PopulateChild<T> (ObviousCode.Alchemy.Library.Populous.Individual<T> child, ObviousCode.Alchemy.Library.Populous.Individual<T> parent1, ObviousCode.Alchemy.Library.Populous.Individual<T> parent2)
		{
			var logEntry = CrossoverProvider.Log.GetNextEntry (CrossoverType.TwoPoint).As<TwoPointCrossoverLogEntry>();

			int crossoverPoint1 = ConfigurationProvider.Rnd.Next (child.GenomeLength);
			int crossoverPoint2 = crossoverPoint1;

			while(crossoverPoint1 == crossoverPoint2)
				crossoverPoint2 = ConfigurationProvider.Rnd.Next (child.GenomeLength);

			logEntry.CrossoverPointStart = Math.Min (crossoverPoint1, crossoverPoint2);
			logEntry.CrossoverPointFinish = Math.Max(crossoverPoint1, crossoverPoint2);

			Array.Copy (parent1.Code, child.Code, parent1.Code.Length);
			Array.Copy (parent2.Code, logEntry.CrossoverPointStart, child.Code, logEntry.CrossoverPointStart, logEntry.CrossoverPointFinish - logEntry.CrossoverPointStart + 1);
		}

		#endregion
	}
}

