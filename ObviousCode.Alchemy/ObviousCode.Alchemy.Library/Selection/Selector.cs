using System;
using System.Collections.Generic;
using System.Linq;
using ObviousCode.Alchemy.Library.Populous;

namespace ObviousCode.Alchemy.Library
{
	public abstract class Selector
	{
		public Selector (SelectionType method)
		{
			SelectionMethod = method;

			Name = string.Format ("Default {0} Selector", method.ToString ());
		}

		public SelectionType SelectionMethod {
			get;
			private set;
		}

		public IEnumerable<Individual<T>> SelectFittest<T> (Population<T> population, double[] fitness)
		{
			try {
				if (population.Individuals.Count != fitness.Length) {
					throw new InvalidOperationException ("Population size different from fitness array length");
				}					

				List<IndexedFitness> indexed = new List<IndexedFitness> (fitness.Length);

				for (int i = 0; i < fitness.Length; i++) {

					population.Individuals [i].Fitness = fitness [i];

					indexed.Add (new IndexedFitness {
						Index = i, 
						Fitness = fitness [i]
					});						
				}

				indexed = new List<IndexedFitness> (indexed.OrderByDescending (f => f.Fitness));

				OnBeforeSelection (indexed);

				return PerformSelection<T> (population, indexed);
			} finally {
				OnAfterSelection ();
			}
		}

		protected virtual void OnBeforeSelection (List<IndexedFitness> indexedFitness) { }

		protected virtual void OnAfterSelection () { }

		protected abstract IEnumerable<Individual<T>> PerformSelection<T>(Population<T> population, List<IndexedFitness> fitness);
		
		public string Name {
			get;
			protected set;
		}
	}
}

