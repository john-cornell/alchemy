using System;

namespace ObviousCode.Alchemy.Creatures
{
	public static class Incubator
	{
		public static int StartEnergyPosition = 0;
		public static int MaxEnergyPosition = 1;
		public static int EnergyExtractionRationPosition = 2;

		public static int CostOfDigestionRatioPosition = 3;
		public static int CostOfEnzymeProcessingPosition = 4;

		public static int LengthOfEnzymeChainPosition = 5;
		public static int StartOfEnzymeChainPosition = 6;

		public static Creature Incubate(byte[] genes)
		{
			var context = GenerateContext (genes);

			return new Creature (context);
		}

		public static CreatureCreationContext GenerateContext (byte[] genes)
		{
			if (genes.Length < 6)
				throw new InvalidOperationException ("Genome should be 6 long or greater");
			int startingEnergy = genes [genes [StartEnergyPosition] % genes.Length];
			int maxEnergy = genes [genes [MaxEnergyPosition] % genes.Length];

			double energyExtractionRatio = (double)genes [genes [EnergyExtractionRationPosition] % genes.Length] / 255;

			int costOfDigestion = Math.Max ((byte) 1, genes [genes [CostOfDigestionRatioPosition] % genes.Length]);
			int costOfEnzymeProcessing = Math.Max ((byte) 1, genes [genes [CostOfEnzymeProcessingPosition] % genes.Length]);
			int lengthOfEnzymeChain = Math.Max((byte) 1, genes [genes [LengthOfEnzymeChainPosition] % genes.Length]);
			int startOfEnzymeChainPosition = genes [genes [StartOfEnzymeChainPosition] % genes.Length];

			CreatureCreationContext context = new CreatureCreationContext ();

			context.Energy = startingEnergy;
			context.EnergyMaximum = maxEnergy;
			context.EnergyExtractionRatio = energyExtractionRatio;

			context.CostOfDigestion = costOfDigestion;
			context.CostOfEnzymeProcessing = costOfEnzymeProcessing;
			context.Enzymes = new System.Collections.Generic.List<byte> ();

			int cursor = startOfEnzymeChainPosition;

			while (context.Enzymes.Count < lengthOfEnzymeChain) {			
				context.Enzymes.Add (genes [genes [cursor % genes.Length] % genes.Length]);
				cursor++;			
			}

			return context;
		}
	}
}

