using System;
using ObviousCode.Alchemy.Library.Populous;

namespace ObviousCode.Alchemy.Library
{
	public abstract class Crossover
	{
		public enum CrossoverType { OnePoint, TwoPoint, Random, Custom }

		public Crossover (CrossoverType method)
		{
			CrossoverMethod = method;
		}

		protected CrossoverLogEntry CreateLogEntry()
		{
			return CrossoverProvider.Log.GetNextEntry (CrossoverMethod);
		}

		public CrossoverType CrossoverMethod  { get; private set; }

		public Individual<T> PerformCrossover<T>(Individual<T> parent1, Individual<T> parent2)
		{
			Individual<T> child = new Individual<T> ();

			child.InitialiseChild (parent1.GenomeLength);

			PopulateChild (child, parent1, parent2);

			return child;
		}

		protected abstract void PopulateChild<T> (Individual<T> child, Individual<T> parent1, Individual<T> parent2);
	}		
}

