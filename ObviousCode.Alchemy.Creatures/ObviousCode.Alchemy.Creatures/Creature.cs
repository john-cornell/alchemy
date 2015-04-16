using System;
using System.Collections.Generic;
using ObviousCode.Alchemy.Creatures.DecisionProcessing;
using System.Linq;

namespace ObviousCode.Alchemy.Creatures
{
	public class Creature
	{
		public Dictionary<DecisionTypes, Decisions> DecisionsLookup { get; private set; }

		public event EventHandler CreatureDied;

		public enum DecisionTypes
		{
			Eat
		}

		public enum CausesOfDeath
		{
			StillAlive = 0,
			Forced,
			Starved,
			Popped,
			ExceptionInDecision
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
			DecisionPredicateCount_Eat = context.DecisionPredicateCount_Eat;
			DecisionPredicateIndex_Eat = context.DecisionPredicateIndex_Eat;
			DecisionPredicate_RandomGeneValueCount = context.DecisionPredicate_RandomGeneValueCount_Eat;
			DecisionRandomSeed_Eat = context.DecisionRandomSeed_Eat;

			LoadDecisions ();
		}

		void LoadDecisions ()
		{
			DecisionsLookup = new Dictionary<DecisionTypes, Decisions> ();

			DecisionsLookup [DecisionTypes.Eat] = new Decisions (DecisionRandomSeed_Eat, DecisionPredicateCount_Eat);

			DecisionsLookup [DecisionTypes.Eat].LoadConstantValue (new Value (MaximumEnergy, "EnergyMaximum"));
			DecisionsLookup [DecisionTypes.Eat].LoadConstantValue (new Value ((decimal)EnergyExtractionRatio, "EnergyExtractionRatio"));
			DecisionsLookup [DecisionTypes.Eat].LoadConstantValue (new Value (Energy, "Energy"));
			DecisionsLookup [DecisionTypes.Eat].LoadConstantValue (new Value (DigestionCost, "CostOfDigestion"));
			DecisionsLookup [DecisionTypes.Eat].LoadConstantValue (new Value (EnzymeProcessCost, "CostOfEnzymeProcessing"));
			DecisionsLookup [DecisionTypes.Eat].LoadConstantValue (new Value ((int)DiningMethod, "DiningMethod"));
			DecisionsLookup [DecisionTypes.Eat].LoadConstantValue (new Value (DecisionPredicateCount_Eat, "DecisionPredicateCount_Eat"));
			DecisionsLookup [DecisionTypes.Eat].LoadConstantValue (new Value (DecisionPredicateIndex_Eat, "DecisionPredicateIndex_Eat"));
			DecisionsLookup [DecisionTypes.Eat].LoadConstantValue (new Value (DecisionRandomSeed_Eat, "DecisionRandomSeed_Eat"));
			DecisionsLookup [DecisionTypes.Eat].LoadConstantValue (new Value (DecisionPredicate_RandomGeneValueCount, "DecisionPredicate_RandomGeneValueCount"));

			Random random = new Random (DecisionRandomSeed_Eat);

			Enumerable.Range (0, DecisionPredicate_RandomGeneValueCount)
						.ToList ()
						.ForEach (i => {
				int position = random.Next () % Code.Length;

				DecisionsLookup [DecisionTypes.Eat].LoadConstantValue (
					new Value (Code [position], 
						string.Format ("(@{1})", Code [position], position)
					));
			});

		}

		public event EventHandler<EventArgs<int>> CreatureAboutToDigest;
		public event EventHandler<EventArgs<int>> CreatureDigestionCostRemoved;
		public event EventHandler<EventArgs<int>> CreatureEnzymeCostRemoved;
		public event EventHandler<EventArgs<Predicate>> EatDecisionPredicatePrepared;
		public event EventHandler<EventArgs<Decisions.Outcome>> EatDecisionResolved;
		public event EventHandler<EventArgs<int>> PotentialExtractionCalcaulated;
		public event EventHandler<EventArgs<int>> ActualEnergyExtracted;
		public event EventHandler<EventArgs<int, int, bool>> EnzymeEvaluated;
		public event EventHandler<EventArgs<int>> EnzymeProcessCompleted;

		public event EventHandler EatingComplete;

		public int Digest (int food)
		{						
			if (CreatureAboutToDigest != null)
				CreatureAboutToDigest (this, new EventArgs<int> (food));

			int lastDigestion = -1;		

			//Temporarilly before decision, otherwise no evolutionary pressure to eat
			Energy -= DigestionCost;
			if (CreatureDigestionCostRemoved != null)
				CreatureDigestionCostRemoved (this, new EventArgs<int> (DigestionCost));
			
			Decisions.Outcome eatDecision = ThinkAboutEating ();

			if (EatDecisionResolved != null)
				EatDecisionResolved (this, new EventArgs<Decisions.Outcome> (eatDecision));

			if (eatDecision == Decisions.Outcome.False)
				return food;
			if (eatDecision == Decisions.Outcome.Die) {
				Die (CausesOfDeath.ExceptionInDecision);
				return food;
			}

			if (!IsAlive)
				return food;

			foreach (var enzyme in Enzymes) {								

				if (EnzymeProcessCost == 0) {

				}

				Energy -= EnzymeProcessCost;
			
				if (CreatureEnzymeCostRemoved != null)
					CreatureEnzymeCostRemoved (this, new EventArgs<int> (EnzymeProcessCost));

				if (!IsAlive)
					return food;

				int enzymeOutput = food & enzyme;
				bool eat = (enzymeOutput) > 0;

				if (EnzymeEvaluated != null)
					EnzymeEvaluated (this, new EventArgs<int, int, bool> (enzyme, enzymeOutput, eat));

				if (eat) {

					if (DiningMethod == EatStrategy.ExtractFirst) {

						int potentialEnergyToExtract = food & enzyme;

						FirePotentialEnergyEvent (potentialEnergyToExtract);

						lastDigestion = ExtractEnergy (potentialEnergyToExtract);
						
						FireActualEnergyEvent (lastDigestion);

						food = food - lastDigestion;

						Eat (lastDigestion);

					} else if (DiningMethod == EatStrategy.BreakDownFirst) {
						
						lastDigestion = food & enzyme;

						FirePotentialEnergyEvent (lastDigestion);

						food = food - lastDigestion;

						int actualEnergy = ExtractEnergy (lastDigestion);

						FireActualEnergyEvent (actualEnergy);

						Eat (actualEnergy);

					} else {
						food = 0;

						lastDigestion = food & enzyme;

						FirePotentialEnergyEvent (lastDigestion);

						int actualEnergy = ExtractEnergy (lastDigestion);

						FireActualEnergyEvent (actualEnergy);

						Eat (actualEnergy);
					}

					if (EnzymeProcessCompleted != null)
						EnzymeProcessCompleted (this, new EventArgs<int> (food));

					if (food == 0)
						return 0;
				}								

				if (EatingComplete != null)
					EatingComplete (this, EventArgs.Empty);
			}

			return food;
		}

		void FirePotentialEnergyEvent (int potentialEnergyToExtract)
		{
			if (PotentialExtractionCalcaulated != null)
				PotentialExtractionCalcaulated (this, new EventArgs<int> (potentialEnergyToExtract));
		}

		void FireActualEnergyEvent (int lastDigestion)
		{
			if (ActualEnergyExtracted != null)
				ActualEnergyExtracted (this, new EventArgs<int> (lastDigestion));
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

		private Decisions.Outcome ThinkAboutEating ()
		{			 			
			DecisionsLookup [DecisionTypes.Eat].ClearTransientValues ();

			DecisionsLookup [DecisionTypes.Eat].LoadTransientValue (new Value ((decimal)Energy, "Current Energy"));

			Predicate predicate = 
				DecisionsLookup [DecisionTypes.Eat].GetDecisionProvider (DecisionPredicateIndex_Eat);

			if (predicate == null)
				return 	Decisions.Outcome.Die;

			if (EatDecisionPredicatePrepared != null)
				EatDecisionPredicatePrepared (this, new EventArgs<Predicate> (predicate));

			return DecisionsLookup [DecisionTypes.Eat].GetDecision (predicate);
		}



		public Predicate EatDecision { get { return DecisionsLookup [DecisionTypes.Eat].DecisionProviders [DecisionPredicateIndex_Eat % DecisionsLookup [DecisionTypes.Eat].DecisionProviders.Count]; } }

		public int DecisionPredicateIndex_Eat { get; private set; }

		public int DecisionPredicateCount_Eat { get; private set; }

		public byte DecisionRandomSeed_Eat { get; private set; }

		public int DecisionPredicate_RandomGeneValueCount { get; private set; }

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

