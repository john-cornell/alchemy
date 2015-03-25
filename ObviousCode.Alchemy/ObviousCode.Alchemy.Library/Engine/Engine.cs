using System;
using System.Linq;
using ObviousCode.Alchemy.Library.Populous;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Library
{
	public class Engine<T>
	{
		Population<T> _population;
		Evaluator<T> _evaluator;	
		public event EventHandler<FitnessSelectionEventArgs<T>> FitnessSelectionAvailable;

		public Engine (Evaluator<T> evaluator)
		{		
			ConfigurationProvider.EnsureIsBuilt ();

			_population = new Population<T> ();
			_evaluator = evaluator;
		}

		public Population<T> Population {
			get {
				return _population;
			}
		}

		/// <summary>
		/// Initialise a population with default population size and genome length (as set in Population and Individual configurations
		/// </summary>
		public void Setup()
		{
			_population.Initialise ();
		}

		public void Setup(int populationSize, int genomeSize)
		{
			_population.Initialise (populationSize, genomeSize);
		}

		public Population<T> ExecuteOneGeneration()
		{
			Population<T> nextGen = Population<T>.CreateEmptyPopulation ();

			double[] fitness = _evaluator.Evaluate (_population);

			List<Individual<T>> fittestList = SelectFittest (fitness);

			if (FitnessSelectionAvailable != null)
				FitnessSelectionAvailable (this, new FitnessSelectionEventArgs<T> (fittestList));

			PopulateNextGeneration (nextGen, fittestList);

			MutationProvider.GetMutator<T> ().Mutate (nextGen);

			_population.Repopulate (nextGen);

			return _population;
		}

		static Individual<T>[] SelectParents (List<Individual<T>> fittestList)
		{
			Individual<T> first;
			Individual<T> second;

			if (fittestList.Count == 2) {
				first = fittestList [0];
				second = fittestList [1];
			}
			else {
				int firstIdx = ConfigurationProvider.Rnd.Next (fittestList.Count);
				int secondIdx = firstIdx;
				while (firstIdx == secondIdx) {
					secondIdx = ConfigurationProvider.Rnd.Next (fittestList.Count);
				};

				first = fittestList [firstIdx];
				second = fittestList [secondIdx];
			}

			return new Individual<T>[] { first, second };
		}

		void PopulateNextGeneration (Population<T> nextGen, List<Individual<T>> fittestList)
		{
			fittestList.ForEach (f => nextGen.Add (f));

			while (nextGen.PopulationSize < _population.PopulationSize) {
				Individual<T>[] parents = SelectParents (fittestList);
				Individual<T> child = CrossoverProvider.GetNextCrossover ().PerformCrossover (parents [0], parents [1]);
				nextGen.Add (child);
			};
		}

		List<Individual<T>> SelectFittest (double[] fitness)
		{
			Selector selector = SelectionProvider.GetSelector ();
		
			return selector.SelectFittest (_population, fitness).ToList ();
		}
	}
}

