using System;
using ObviousCode.Alchemy.Creatures.DecisionProcessing;

namespace ObviousCode.Alchemy.Creatures.Darwin
{
	public class Simulator
	{
		Environment _environment;

		byte[] _code;

		public Creature SimulatedCreature { get; private set; }

		public event EventHandler<EventArgs<int>> CreatureAboutToDigest;
		public event EventHandler<EventArgs<int>> CreatureDigestionCostRemoved;
		public event EventHandler<EventArgs<int>> CreatureEnzymeCostRemoved;
		public event EventHandler<EventArgs<Predicate>> EatDecisionPredicatePrepared;
		public event EventHandler<EventArgs<Decisions.Outcome>> EatDecisionResolved;
		public event EventHandler<EventArgs<int>> PotentialExtractionCalcaulated;
		public event EventHandler<EventArgs<int>> ActualEnergyExtracted;
		public event EventHandler<EventArgs<int, int, bool>> EnzymeEvaluated;
		public event EventHandler<EventArgs<int>> EnzymeProcessCompleted;
		public event EventHandler<EventArgs> CreatureDied;

		public Simulator (Environment environment, byte[] Code)
		{
			this._code = Code;
			this._environment = environment;

			Incubator incubator = new Incubator ();

			SimulatedCreature = incubator.Incubate (_code);

			SimulatedCreature.CreatureAboutToDigest += (sender, e) => {
				if (CreatureAboutToDigest != null)
					CreatureAboutToDigest (sender, e);
			};

			SimulatedCreature.CreatureDigestionCostRemoved += (sender, e) => {
				if (CreatureDigestionCostRemoved != null)
					CreatureDigestionCostRemoved (sender, e);
			};

			SimulatedCreature.CreatureEnzymeCostRemoved += (sender, e) => {
				if (CreatureEnzymeCostRemoved != null)
					CreatureEnzymeCostRemoved (sender, e);
			};

			SimulatedCreature.EatDecisionPredicatePrepared += (sender, e) => {
				if (EatDecisionPredicatePrepared != null)
					EatDecisionPredicatePrepared (sender, e);
			};

			SimulatedCreature.EatDecisionResolved += (sender, e) => {
				if (EatDecisionResolved != null)
					EatDecisionResolved (sender, e);
			};

			SimulatedCreature.PotentialExtractionCalcaulated += (sender, e) => {
				if (PotentialExtractionCalcaulated != null)
					PotentialExtractionCalcaulated (sender, e);
			};

			SimulatedCreature.ActualEnergyExtracted += (sender, e) => {
				if (ActualEnergyExtracted != null)
					ActualEnergyExtracted (sender, e);
			};

			SimulatedCreature.EnzymeEvaluated += (sender, e) => {
				if (EnzymeEvaluated != null)
					EnzymeEvaluated (sender, e);
			};

			SimulatedCreature.EnzymeProcessCompleted += (sender, e) => {
				if (EnzymeProcessCompleted != null)
					EnzymeProcessCompleted (sender, e);
			};

			SimulatedCreature.CreatureDied += (sender, e) => {
				if (CreatureDied != null)
					CreatureDied (sender, e);
			};
		}

		public void RunTurn (int iterations)
		{
			_environment.RunIterations (SimulatedCreature, iterations);
		}
	}
}

