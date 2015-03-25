using System;

namespace ObviousCode.Alchemy.Library
{
	public static class ConfigurationProvider
	{
		/// <summary>
		/// Randomiser Instance for Application
		/// </summary>
		/// <value>The instance of the Random class for all randome values in the application.</value>
		public static Random Rnd {get; private set;}

		/// <summary>
		/// Initializes the <see cref="ObviousCode.Alchemy.Library.ConfigurationProvider"/> class.
		/// </summary>
		static ConfigurationProvider ()
		{
			Reset ();
		}

		/// <summary>
		/// Called by Engine to ensure Configuration have all initialised before itis called
		/// </summary>
		public static void EnsureIsBuilt () { 
			//ensures static ctor is run
		}

		/// <summary>
		/// Configuration settings for Individuals in population
		/// </summary>
		/// <value>The individual configuration settings.</value>
		public static IndividualConfiguration Individual {
			get;
			set;
		}

		/// <summary>
		/// Configuration settings for the Population
		/// </summary>
		/// <value>The population configuration settings.</value>
		public static PopulousConfiguration Populous {
			get;
			set;
		}

		/// <summary>
		/// Configuration settings for gene fitness selection
		/// </summary>
		/// <value>The selection configuration settings.</value>
		public static SelectionConfiguration Selection {
			get;
			set;
		}

		/// <summary>
		/// Configuration settings for gene mutation
		/// </summary>
		/// <value>The mutation configuration settings.</value>
		public static MutationConfiguration Mutation {
			get;
			set;
		}

		/// <summary>
		/// Configuration Settings for gene crossover
		/// </summary>
		/// <value>The crossover configuration settings.</value>
		public static CrossoverConfiguration Crossover {
			get;
			set;
		}

		/// <summary>
		/// Resets all configuration settings for this instance.
		/// </summary>
		public static void Reset ()
		{
			Rnd = new Random ((int)DateTime.Now.Ticks);
			Individual = new IndividualConfiguration ();
			Populous = new PopulousConfiguration ();
			Selection = new SelectionConfiguration ();
			Mutation = new MutationConfiguration ();
			Crossover = new CrossoverConfiguration ();

			CrossoverProvider.RebuildSelectionArray ();
		}
	}
}

