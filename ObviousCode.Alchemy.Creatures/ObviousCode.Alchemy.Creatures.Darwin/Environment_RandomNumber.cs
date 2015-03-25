using System;
using ObviousCode.Alchemy.Library.Populous;
using ObviousCode.Alchemy.Library;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.Darwin
{
	public class Environment_RandomNumber : Environment
	{
		Engine<byte> _engine;

		public Engine<byte> Engine {
			get {
				return _engine;
			}
		}

		public Environment_RandomNumber () : base("Random")
		{
			_engine = new Engine<byte> (
				this
			);

			_engine.Setup (10000, 32);	

			ResetForNextGeneration ();
		}			

		#region implemented abstract members of Evaluator

		public Dictionary<string, Creature> LastPopulation { get; private set; }

		protected override double EvaluateItem (Individual<byte> item)
		{
			Random random = new Random ();

			Creature creature = Incubator.Incubate (item.Code);

			int i = 0;

			while(creature.IsAlive && i < LifetimeIterations){
				creature.Digest (random.Next (1000));	
				i++;
			}

			double fitness;

			if (creature.IsAlive) {
				_alive +=1;
				fitness = Math.Min (.9999999999d, .5d + (((double) creature.Energy / 1000d) / 2d));//.5->.99995 (or .99999999)
			} else {
				_dead +=1;

				fitness = (Math.Max(1d, (double)i) / (double) _foodIteration) / 2d;//.005 -> .5
			}

			LastPopulation [item.Uid] = creature;

			return fitness;
		}

		#endregion

		public void ResetForNextGeneration()
		{
			_alive = 0;
			_dead = 0;
			LastPopulation = new Dictionary<string, Creature> ();
		}

		public bool Success { get { return _alive > 0;}}

		int _alive = 0;
		int _dead = 0;
	}
}