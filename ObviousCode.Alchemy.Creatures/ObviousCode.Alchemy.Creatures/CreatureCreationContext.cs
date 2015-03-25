using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures
{
	public class CreatureCreationContext
	{
		public List<byte> Enzymes { get; set; }
		public int Energy { get; set; }

		public int CostOfDigestion {get;set;}
		public int CostOfEnzymeProcessing {get;set;}

		public int EnergyMaximum { get; set;}
		public double EnergyExtractionRatio { get; set;}

		public CreatureCreationContext ()
		{
			Enzymes = new List<byte> ();
			Energy = 0;

			CostOfDigestion = 1;
			CostOfEnzymeProcessing = 1;
			EnergyMaximum = 1000;

			EnergyExtractionRatio = .1d;
		}
	}
}