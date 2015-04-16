using System;
using System.Collections.Generic;
using System.Linq;

namespace ObviousCode.Alchemy.Creatures
{
	public class Incubator
	{
		public enum GenePosition
		{
			StartEnergyPosition,
			MaxEnergyPosition,
			EnergyExtractionRationPosition,

			CostOfDigestionRatioPosition,
			CostOfEnzymeProcessingPosition,

			DiningMethodPosition,

			DecisionSeedPosition_Eat,
			DecisionPredicateCountPosition_Eat,
			DecisionPredicateIndexPosition_Eat,
			DecisionPredicateRandomGeneValueCount_Eat,

			LengthOfEnzymeChainPosition,
			StartOfEnzymeChainPosition
		}

		public Dictionary<GenePosition, int> Positions { get; private set; }

		public Creature Incubate (byte[] genes)
		{			
			var context = GenerateContext (genes);

			return new Creature (context);
		}

		public CreatureCreationContext GenerateContext (byte[] genes)
		{
			LoadPositions (genes);

			if (genes.Length == 0)
				throw new InvalidOperationException ("0 length genomes not accepted");			

			int startingEnergy = genes [genes [Positions [GenePosition.StartEnergyPosition] % genes.Length] % genes.Length];
			int maxEnergy = genes [genes [Positions [GenePosition.MaxEnergyPosition] % genes.Length] % genes.Length];

			double energyExtractionRatio = (double)genes [genes [Positions [GenePosition.EnergyExtractionRationPosition] % genes.Length] % genes.Length] / 255;

			int costOfDigestion = Math.Max ((byte)1, genes [genes [Positions [GenePosition.CostOfDigestionRatioPosition] % genes.Length] % genes.Length]);
			int costOfEnzymeProcessing = Math.Max ((byte)1, genes [genes [Positions [GenePosition.CostOfEnzymeProcessingPosition] % genes.Length] % genes.Length]);
			int lengthOfEnzymeChain = Math.Max ((byte)1, genes [genes [Positions [GenePosition.LengthOfEnzymeChainPosition] % genes.Length] % genes.Length]);
			int startOfEnzymeChainPosition = genes [genes [Positions [GenePosition.StartOfEnzymeChainPosition] % genes.Length] % genes.Length];

			int decisionSeed_Eat = genes [genes [Positions [GenePosition.DecisionSeedPosition_Eat] % genes.Length] % genes.Length];
			int decisionPredicateIndex_Eat = genes [genes [Positions [GenePosition.DecisionPredicateIndexPosition_Eat] % genes.Length] % genes.Length];
			int decisionPredicateCount_Eat = genes [genes [Positions [GenePosition.DecisionPredicateCountPosition_Eat] % genes.Length] % genes.Length];
			int decisionPredicateRandomGeneValueCount_Eat = genes [genes [Positions [GenePosition.DecisionPredicateRandomGeneValueCount_Eat] % genes.Length] % genes.Length];

			EatStrategy diningMethod = (EatStrategy)(genes [genes [Positions [GenePosition.DiningMethodPosition] % genes.Length] % genes.Length]
			                           % Enum.GetValues (typeof(EatStrategy)).Length);

			CreatureCreationContext context = new CreatureCreationContext ();

			context.Energy = startingEnergy;
			context.EnergyMaximum = maxEnergy;
			context.EnergyExtractionRatio = energyExtractionRatio;

			context.DecisionRandomSeed_Eat = (byte)decisionSeed_Eat;
			context.DecisionPredicateCount_Eat = decisionPredicateCount_Eat;
			context.DecisionPredicateIndex_Eat = decisionPredicateIndex_Eat;
			context.DecisionPredicate_RandomGeneValueCount_Eat = decisionPredicateRandomGeneValueCount_Eat % 5;

			context.DiningMethod = diningMethod;

			context.CostOfDigestion = costOfDigestion;
			context.CostOfEnzymeProcessing = costOfEnzymeProcessing;
			context.Enzymes = new System.Collections.Generic.List<byte> ();

			context.Code = genes;

			int cursor = startOfEnzymeChainPosition;

			while (context.Enzymes.Count < lengthOfEnzymeChain) {			
				context.Enzymes.Add (genes [genes [cursor % genes.Length] % genes.Length]);
				cursor++;			
			}

			return context;
		}

		void LoadPositions (byte[] genes)
		{
			Positions = new Dictionary<GenePosition, int> ();

			int randomSeed = genes [0];

			Random random = new Random (randomSeed);

			foreach (GenePosition position in Enum.GetValues (typeof(GenePosition))) {
				Positions [position] = random.Next () % genes.Length;
			}
		}

	}
}

