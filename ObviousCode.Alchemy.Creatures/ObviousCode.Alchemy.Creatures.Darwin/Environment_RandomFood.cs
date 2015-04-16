using System;
using ObviousCode.Alchemy.Library.Populous;
using ObviousCode.Alchemy.Library;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.Darwin
{
	public class Environment_RandomFood : Environment
	{
		Random _random;

		public Environment_RandomFood (Action<Engine<byte>> setup) : base ("Random Food Offered Each Turn", setup)
		{
			_random = new Random ();
		}

		public Environment_RandomFood () : this (null)
		{
			
		}

		public override double RunIterations (Creature creature, int iterations)
		{
			int i = 0;
			double fitness;

			while (creature.IsAlive && i < iterations) {
				
				int food = _random.Next (1000);

				creature.Digest (food);	
			
				i++;
			}
				
			if (creature.IsAlive) {	
				fitness = Math.Min (.9999999999d, .5d + (((double)creature.Energy / (double)creature.MaximumEnergy) / 2d));//.5->.99995 (or .99999999)
			} else {
				//Starving = no fitness, as leads to creatures starving themselves and selecting for lowest digestion cost
				fitness = 
					creature.CauseOfDeath == Creature.CausesOfDeath.Starved ? 0 :
					(Math.Max (1d, (double)i) / (double)iterations) / 2d;//.005 -> .5
			}				

			creature.Fitness = fitness;

			return fitness;
		}

		protected override double Evaluate (Creature creature)
		{						
			return RunIterations (creature, LifetimeIterations);
		}

		protected override void PrepareForNextGeneration ()
		{
		}
	}
}