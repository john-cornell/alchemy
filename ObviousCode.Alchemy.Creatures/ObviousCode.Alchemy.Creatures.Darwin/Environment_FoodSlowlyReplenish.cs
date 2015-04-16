using System;
using ObviousCode.Alchemy.Library.Populous;
using ObviousCode.Alchemy.Library;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.Darwin
{
	public class Environment_FoodSlowlyReplenish : Environment, IRemainingFoodReporter
	{
		Random _random;

		public int RemainingFood { get; private set; }

		public Environment_FoodSlowlyReplenish (Action<Engine<byte>> setup) : base ("Food starts high, slowly replenishes after @ creature", setup)
		{
			_random = new Random ();
			RemainingFood = 100000 + _random.Next (100000);
		}

		public Environment_FoodSlowlyReplenish () : this (null)
		{
			
		}

		public Action<int> ReportRemainingFood { get; set; }

		public override double RunIterations (Creature creature, int iterations)
		{
			int i = 0;
			double fitness;

			while (creature.IsAlive && i < iterations) {
				
				//int food = _random.Next (1000);

				UpdateRemainingFood (creature.Digest (RemainingFood));
																
				i++;
			}
				
			UpdateRemainingFood (RemainingFood + _random.Next (20));

			if (creature.IsAlive) {	
				fitness = Math.Min (.9999999999d, .5d + (((double)creature.Energy / (double)creature.MaximumEnergy) / 2d));//.5->.99995 (or .99999999)
			} else {
				//Starving = Digestion + iterations, as leads to creatures starving themselves and selecting for lowest digestion cost					

				if (i == 0 || creature.CauseOfDeath == Creature.CausesOfDeath.Starved) {

					//double iterationComponent = Math.Max (1d, (double)i) / (double)iterations;
					//double digestionComponent = Math.Max (1d, creature.DigestionCost / 256);

					fitness = 0;//(digestionComponent / 2d);

				} else {
					fitness = (Math.Max (1d, (double)i) / (double)iterations) / 2d;
				}
			}				

			creature.Fitness = fitness;

			return fitness;
		}

		public void UpdateRemainingFood (int value)
		{
			RemainingFood = value;
			if (ReportRemainingFood != null)
				ReportRemainingFood (RemainingFood);
		}

		protected override double Evaluate (Creature creature)
		{						
			return RunIterations (creature, LifetimeIterations);
		}

		protected override void PrepareForNextGeneration ()
		{
			
		}

		public override void Reset ()
		{
			ReportRemainingFood = null;
		}
	}
}