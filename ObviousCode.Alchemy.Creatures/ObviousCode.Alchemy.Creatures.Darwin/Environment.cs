using System;
using ObviousCode.Alchemy.Library;
using ObviousCode.Alchemy.Library.Populous;
using System.Collections.Generic;
using System.Linq;

namespace ObviousCode.Alchemy.Creatures.Darwin
{
	public abstract class Environment: Evaluator<byte>
	{
		public Engine<byte> Engine  { get; private set; }

		public event EventHandler<AfterSelectionStateEventArgs> AfterSelectionStateAvailable;
		public event EventHandler<PopulationEventArgs> NextGenerationAvailable;

		public Environment (string label) : this (label, null)
		{

		}

		public Environment (string label, Action<Engine<byte>> setup)
		{
			Label = label;
			LifetimeIterations = 1000;
			Generations = 0;

			Engine = new Engine<byte> (this);

			if (setup != null) {
				setup (Engine);
			} else {
				Engine.Setup (10000, 32);
			}

			LastPopulation = new Dictionary<string, Creature> ();

			BeforeGeneration ();

			Engine.FitnessSelectionAvailable += (sender, e) => {
				if (AfterSelectionStateAvailable != null) {
					var args = new AfterSelectionStateEventArgs ();
					
					e.Selection.ForEach (s => args.Selection.Add (s));

					LastPopulation						
						.ForEach (kvp => args.LastPopulation [kvp.Key] = kvp.Value);

					AfterSelectionStateAvailable (sender, args);
				}
			};
		}



		public void ExecuteGeneration ()
		{
			Population<byte> population = ExecuteOneGeneration ();

			//TODO: Consider moving Generation Count down to Engine
			Generations += 1;

			if (NextGenerationAvailable != null)
				NextGenerationAvailable (this, new PopulationEventArgs (population, Generations));
		}

		protected abstract Population<byte> ExecuteOneGeneration ();

		public void BeforeGeneration ()
		{
			LastPopulation.Clear ();
		}

		protected abstract void PrepareForNextGeneration ();

		protected override double EvaluateItem (ObviousCode.Alchemy.Library.Populous.Individual<byte> item)
		{
			Creature creature = Incubator.Incubate (item.Code);
		
			LastPopulation [item.Uid] = creature;

			return Evaluate (creature);
		}

		protected abstract double Evaluate (Creature creature);

		public string Label { get; private set; }

		public int Generations { get; private set; }

		public int LifetimeIterations { get; set; }

		public Dictionary<string, Creature> LastPopulation { get; private set; }
	}
}