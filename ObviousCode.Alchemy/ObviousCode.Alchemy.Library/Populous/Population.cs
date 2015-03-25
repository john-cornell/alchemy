using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ObviousCode.Alchemy.Library.Populous
{
	public class Population
	{
		public bool IsInitialised {
			get;
			protected set;
		}

		public Population ()
		{
			IsInitialised = false;
		}
	}

	public class Population<T> : Population
	{
		List<Individual<T>> _individuals;

		public Population ()
		{
			IsInitialPopulation = true;
		}

		public bool IsInitialPopulation {
			get;
			private set;
		}

		public List<Individual<T>> Individuals {
			get {
				AssertIsInitialised ();

				return _individuals;
			}
			private set { _individuals = value; }
		}

		public Individual<T> this[int idx]
		{
			get { return _individuals [idx]; }
		}

		public void Initialise()
		{
			Initialise (null, ConfigurationProvider.Populous.PopulationSize, ConfigurationProvider.Individual.DefaultGenomeSize);
		}

		public void Initialise(InitialiserContext<T> context)
		{

		}

		public int PopulationSize { get { return _individuals.Count; } }

		public void Initialise(int populationSize, int genomeLength)
		{
			Initialise (null, populationSize, genomeLength);
		}

		public void Initialise(InitialiserContext<T> context, int populationSize, int genomeLength)
		{
			Individuals = new List<Individual<T>> (populationSize);	

			for (int i = 0; i < populationSize; i++) {

				Individual<T> individual = new Individual<T> ();

				individual.Initialise(context, genomeLength);

				_individuals.Add (individual);
			}
				
			IsInitialised = true;
		}

		public Type GenomeType 
		{ 
			get 
			{ 
				return Individuals [0].GenomeType; 
			} 
		}

		void AssertIsInitialised ()
		{
			if (!IsInitialised)
				throw new Exceptions.PopulationNotInitialisedException ();
		}

		public static Population<T> CreateEmptyPopulation ()
		{
			Population<T> population = new Population<T> ();

			population.IsInitialised = true;
			population.IsInitialPopulation = false;

			population.Individuals = new List<Individual<T>> ();

			return population;
		}

		public void Repopulate (Population<T> nextGen)
		{
			IsInitialPopulation = false;

			Individuals = nextGen.Individuals;
		}

		public void Add (Individual<T> f)
		{
			if (IsInitialPopulation)
				throw new InvalidOperationException ("Adding of individuals to initial population is not permitted.");

			Individuals.Add (f);
		}
	}
}

