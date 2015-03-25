using System;
using System.Collections.Generic;
using System.Linq;
using ObviousCode.Alchemy.Library.Populous;

namespace ObviousCode.Alchemy.Library
{
	public abstract class Evaluator
	{
		public Evaluator ()
		{
		}

		public Type GenomeType { get; protected set; }
	}

	public abstract class Evaluator<T> : Evaluator
	{
		public Evaluator ()
		{
			GenomeType = typeof(T);
		}
			
		public double[] Evaluate(Population<T> population)
		{
			return Evaluate (population.Individuals);
		}

		public double[] Evaluate(List<Individual<T>> individuals)
		{
			return Evaluate (individuals.ToArray ());
		}

		public double[] Evaluate(Individual<T>[] individuals)
		{
			double[] weighting = new double[individuals.Length];

			for (int i = 0; i < individuals.Length; i++) {
				weighting [i] = EvaluateItem (individuals [i]);
			}

			return weighting;
		}

		protected abstract double EvaluateItem (Individual<T> item);
	}
}

