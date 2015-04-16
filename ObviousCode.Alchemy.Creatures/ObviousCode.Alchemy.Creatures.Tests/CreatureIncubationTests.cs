using System;
using System.Linq;
using NUnit.Framework;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	[TestFixture]
	public class CreatureIncubationTests
	{
		[Test]
		public void WhenCreatureContextCreated_EnergyShouldBeCorrect ()
		{
			byte[] genes = GenerateRandomGenome (256);

			Incubator incubator = new Incubator ();

			CreatureCreationContext context = incubator.GenerateContext (genes);

			byte energy = genes [genes [incubator.Positions [Incubator.GenePosition.StartEnergyPosition]]];

			Assert.AreEqual (energy, context.Energy);
		}

		[Test]
		public void WhenCreatureContextCreated_SmallGenome_EnergyShouldBeCorrect ()
		{
			int genomeLength = 8;

			byte[] genes = GenerateRandomGenome (genomeLength);

			Incubator incubator = new Incubator ();

			CreatureCreationContext context = incubator.GenerateContext (genes);

			int position = genes [incubator.Positions [Incubator.GenePosition.StartEnergyPosition]];		

			Assert.AreEqual (genes [position], context.Energy);
		}

		[Test]
		public void WhenCreatureContextCreated_CostOfDigestionShouldBeCorrect ()
		{
			byte[] genes = GenerateRandomGenome (256);

			Incubator incubator = new Incubator ();

			var context = incubator.GenerateContext (genes);

			int position = genes [incubator.Positions [Incubator.GenePosition.CostOfDigestionRatioPosition]];

			byte costOfDigestion = genes [position];

			Assert.AreEqual (Math.Max ((byte)1, costOfDigestion), context.CostOfDigestion);
		}

		[Test]
		public void WhenCreatureContextCreated_SmallGenome_CostOfDigestionShouldBeCorrect ()
		{
			int genomeLength = 8;

			byte[] genes = GenerateRandomGenome (genomeLength);

			Incubator incubator = new Incubator ();

			var context = incubator.GenerateContext (genes);

			int position = genes [incubator.Positions [Incubator.GenePosition.CostOfDigestionRatioPosition]];	

			byte cost = genes [position];

			Assert.AreEqual (cost, context.CostOfDigestion);
		}

		[Test]
		public void WhenCreatureContextCreated_CostOfEnzymeProcessingShouldBeCorrect ()
		{
			byte[] genes = GenerateRandomGenome (256);

			Incubator incubator = new Incubator ();

			var context = incubator.GenerateContext (genes);

			int position = genes [incubator.Positions [Incubator.GenePosition.CostOfEnzymeProcessingPosition]];

			byte costOfEnzymeProcessing = genes [position];

			Assert.AreEqual (costOfEnzymeProcessing, context.CostOfEnzymeProcessing);
		}

		[Test]
		public void WhenCreatureContextCreated_SmallGenome_CostOfEnzymeProcessingShouldBeCorrect ()
		{
			int genomeLength = 8;

			byte[] genes = GenerateRandomGenome (genomeLength);				

			Incubator incubator = new Incubator ();

			var context = incubator.GenerateContext (genes);

			int position = genes [incubator.Positions [Incubator.GenePosition.CostOfEnzymeProcessingPosition]];

			byte costOfEnzymeProcessing = genes [position];

			Assert.AreEqual (costOfEnzymeProcessing, context.CostOfEnzymeProcessing);
		}

		[Test]
		public void WhenCreatureContextCreated_MaxEnergyShouldBeCorrect ()
		{
			byte[] genes = GenerateRandomGenome (256);

			Incubator incubator = new Incubator ();

			var context = incubator.GenerateContext (genes);

			int position = genes [incubator.Positions [Incubator.GenePosition.MaxEnergyPosition]];

			byte maxEnergyPosition = genes [position];

			Assert.AreEqual (maxEnergyPosition, context.EnergyMaximum);
		}

		[Test]
		public void WhenCreatureContextCreated_SmallGenome_MaxEnergyShouldBeCorrect ()
		{
			int genomeLength = 8;

			byte[] genes = GenerateRandomGenome (genomeLength);

			Incubator incubator = new Incubator ();

			var context = incubator.GenerateContext (genes);

			int position = genes [incubator.Positions [Incubator.GenePosition.MaxEnergyPosition]];

			byte maxEnergyPosition = genes [position];

			Assert.AreEqual (maxEnergyPosition, context.EnergyMaximum);
		}


		[Test]
		public void WhenCreatureContextCreated_EnzymesShouldBeCorrectLength ()
		{
			byte[] genes = GenerateRandomGenome (256);

			Incubator incubator = new Incubator ();

			var context = incubator.GenerateContext (genes);

			int position = incubator.Positions [Incubator.GenePosition.LengthOfEnzymeChainPosition];

			int length = genes [position];

			Assert.AreEqual (length, context.Enzymes.Count);
		}

		byte[] GenerateRandomGenome (int length)
		{
			byte[] genes = new byte[length];

			new Random ().NextBytes (genes);

			return genes;
		}
	}
}

