using System;
using ObviousCode.Alchemy.Library.Populous;
using ObviousCode.Alchemy.Library;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.Darwin
{
	public class Environment_RandomNumber : Environment
	{
		Random _random;

		public Environment_RandomNumber () : base ("Random")
		{
			_random = new Random ();
		}

		protected override Population<byte> ExecuteOneGeneration ()
		{
			return Engine.ExecuteOneGeneration ();
		}

		protected override double Evaluate (Creature creature)
		{						
			int i = 0;
			double fitness;

			while (creature.IsAlive && i < LifetimeIterations) {
				creature.Digest (_random.Next (1000));	
				i++;
			}
				
			if (creature.IsAlive) {	
				fitness = Math.Min (.9999999999d, .5d + (((double)creature.Energy / (double)creature.MaximumEnergy) / 2d));//.5->.99995 (or .99999999)
			} else {
				fitness = (Math.Max (1d, (double)i) / (double)LifetimeIterations) / 2d;//.005 -> .5
			}				

			creature.Fitness = fitness;

			return fitness;
		}

		protected override void PrepareForNextGeneration ()
		{
		}
	}
}