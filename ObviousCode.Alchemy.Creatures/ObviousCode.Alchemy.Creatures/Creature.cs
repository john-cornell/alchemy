using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures
{
	public class Creature
	{
		public event EventHandler CreatureDied;

		public enum CausesOfDeath
		{
			StillAlive = 0,
			Forced,
			Starved,
			Popped

		}

		public Creature (CreatureCreationContext context)
		{
			IsAlive = true;
			Enzymes = new List<byte> (context.Enzymes);
			MaximumEnergy = context.EnergyMaximum;
			EnergyExtractionRatio = context.EnergyExtractionRatio;
			Energy = context.Energy;
			DigestionCost = context.CostOfDigestion;
			EnzymeProcessCost = context.CostOfEnzymeProcessing;
			Code = context.Code;
			DiningMethod = context.DiningMethod;
		}

		public int Digest (int food)
		{
			int lastDigestion = -1;

			Energy -= DigestionCost;

			if (!IsAlive)
				return food;

			foreach (var enzyme in Enzymes) {

				Energy -= EnzymeProcessCost;

				if (!IsAlive)
					return food;

				if ((food & enzyme) > 0) {

					if (DiningMethod == EatStrategy.ExtractFirst) {

						lastDigestion = ExtractEnergy (food & enzyme);
					
						food = food - lastDigestion;

						Eat (lastDigestion);
					} else if (DiningMethod == EatStrategy.BreakDownFirst) {
						lastDigestion = food & enzyme;

						food = food - lastDigestion;

						Eat (ExtractEnergy (lastDigestion));
					} else {
						food = 0;

						lastDigestion = food & enzyme;

						Eat (ExtractEnergy (lastDigestion));
					}


					if (food == 0)
						return 0;
				}								
			}

			return food;
		}

		int ExtractEnergy (int lastDigestion)
		{
			return (int)Math.Ceiling ((double)lastDigestion * EnergyExtractionRatio);
		}

		void Eat (int lastDigestion)
		{
			Energy += lastDigestion;
		}

		void Die (CausesOfDeath cause)
		{
			IsAlive = false;
			CauseOfDeath = cause;

			if (CreatureDied != null)
				CreatureDied (this, EventArgs.Empty);
		}

		public bool IsAlive { get; private set; }

		public List<byte> Enzymes { get; private set; }

		public CausesOfDeath CauseOfDeath {
			get;
			private set;
		}

		public int _energy;

		public int Energy { 
			get { 
				return _energy; 
			} 
			private set { 
				_energy = value;
				if (value < 0) {
					Die (CausesOfDeath.Starved);
				}

				if (value > MaximumEnergy) {
					Die (CausesOfDeath.Popped);
				}
			} 
		}

		public EatStrategy DiningMethod {
			get;
			set;
		}

		public int DigestionCost {
			get;
			private set;
		}

		public int EnzymeProcessCost {
			get;
			private set;
		}

		public double EnergyExtractionRatio {
			get;
			private set;
		}

		public int MaximumEnergy {
			get;
			private set;
		}

		public double Fitness {
			get;
			set;
		}

		public byte[] Code { get; set; }
	}
}

