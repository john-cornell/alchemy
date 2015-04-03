using System;

namespace ObviousCode.Alchemy.Creatures
{
	public static class Incubator
	{
		//Consider only set position as seed for position randomiser
		public static int StartEnergyPosition = 0;
		public static int MaxEnergyPosition = 1;
		public static int EnergyExtractionRationPosition = 2;

		public static int CostOfDigestionRatioPosition = 3;
		public static int CostOfEnzymeProcessingPosition = 4;

		public static int DiningMethodPosition = 5;

		public static int DecisionSeedPosition_Eat = 6;
		public static int DecisionPredicateCountPosition_Eat = 7;
		public static int DecisionPredicateIndexPosition_Eat = 8;

		public static int LengthOfEnzymeChainPosition = 9;
		public static int StartOfEnzymeChainPosition = 10;

		public static Creature Incubate (byte[] genes)
		{
			var context = GenerateContext (genes);

			return new Creature (context);
		}

		public static CreatureCreationContext GenerateContext (byte[] genes)
		{
			if (genes.Length < 6)
				throw new InvalidOperationException ("Genome should be 6 long or greater");			

			int startingEnergy = genes [genes [StartEnergyPosition % genes.Length] % genes.Length];
			int maxEnergy = genes [genes [MaxEnergyPosition % genes.Length] % genes.Length];

			double energyExtractionRatio = (double)genes [genes [EnergyExtractionRationPosition % genes.Length] % genes.Length] / 255;

			int costOfDigestion = Math.Max ((byte)1, genes [genes [CostOfDigestionRatioPosition % genes.Length] % genes.Length]);
			int costOfEnzymeProcessing = Math.Max ((byte)1, genes [genes [CostOfEnzymeProcessingPosition % genes.Length] % genes.Length]);
			int lengthOfEnzymeChain = Math.Max ((byte)1, genes [genes [LengthOfEnzymeChainPosition % genes.Length] % genes.Length]);
			int startOfEnzymeChainPosition = genes [genes [StartOfEnzymeChainPosition % genes.Length] % genes.Length];

			int decisionSeed_Eat = genes [genes [DecisionSeedPosition_Eat % genes.Length] % genes.Length];
			int decisionPredicateIndex_Eat = genes [genes [DecisionPredicateIndexPosition_Eat % genes.Length] % genes.Length];
			int decisionPredicateCount_Eat = genes [genes [DecisionPredicateCountPosition_Eat % genes.Length] % genes.Length];

			EatStrategy diningMethod = (EatStrategy)(genes [genes [DiningMethodPosition % genes.Length] % genes.Length]
			                           % Enum.GetValues (typeof(EatStrategy)).Length);

			CreatureCreationContext context = new CreatureCreationContext ();

			context.Energy = startingEnergy;
			context.EnergyMaximum = maxEnergy;
			context.EnergyExtractionRatio = energyExtractionRatio;

			context.DecisionSeed_Eat = (byte)decisionSeed_Eat;
			context.DecisionPredicateCount_Eat = decisionPredicateCount_Eat;
			context.DecisionPredicateIndex_Eat = decisionPredicateIndex_Eat;

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
	}
}

