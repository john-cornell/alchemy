using System;
using System.Linq;
using NUnit.Framework;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	[TestFixture]
	public class CreatureIncubationTests
	{
		[Test]
		public void WhenCreatureContextCreated_EnergyShouldBeCorrect()
		{
			byte[] genes = GenerateRandomGenome (256);

			byte energy = genes [genes [Incubator.StartEnergyPosition]];

			CreatureCreationContext context = Incubator.GenerateContext (genes);

			Assert.AreEqual (energy, context.Energy);
		}

		[Test]
		public void WhenCreatureContextCreated_SmallGenome_EnergyShouldBeCorrect()
		{
			int genomeLength = 8;

			byte[] genes = GenerateRandomGenome (genomeLength);

			genes [Incubator.StartEnergyPosition] = 30;

			int position = 30 % genomeLength;

			byte energy = genes [position];

			CreatureCreationContext context = Incubator.GenerateContext (genes);

			Assert.AreEqual (energy, context.Energy);
		}

		[Test]
		public void WhenCreatureContextCreated_CostOfDigestionShouldBeCorrect()
		{
			byte[] genes = GenerateRandomGenome (256);

			byte costOfDigestion = genes [genes [Incubator.CostOfDigestionRatioPosition]];

			var context = Incubator.GenerateContext (genes);

			Assert.AreEqual (costOfDigestion, context.CostOfDigestion);
		}

		[Test]
		public void WhenCreatureContextCreated_SmallGenome_CostOfDigestionShouldBeCorrect()
		{
			int genomeLength = 8;

			byte[] genes = GenerateRandomGenome (genomeLength);

			genes [Incubator.CostOfDigestionRatioPosition] = 183;

			int position = 183 % genomeLength;

			byte cost = genes [position];

			CreatureCreationContext context = Incubator.GenerateContext (genes);

			Assert.AreEqual (cost, context.CostOfDigestion);
		}
			
		[Test]
		public void WhenCreatureContextCreated_CostOfEnzymeProcessingShouldBeCorrect()
		{
			byte[] genes = GenerateRandomGenome (256);

			byte costOfEnzymeProcessing = genes [genes [Incubator.CostOfEnzymeProcessingPosition]];

			var context = Incubator.GenerateContext (genes);

			Assert.AreEqual (costOfEnzymeProcessing, context.CostOfEnzymeProcessing);
		}

		[Test]
		public void WhenCreatureContextCreated_SmallGenome_CostOfEnzymeProcessingShouldBeCorrect()
		{
			int genomeLength = 8;

			byte[] genes = GenerateRandomGenome (genomeLength);				

			int position = genes[Incubator.CostOfEnzymeProcessingPosition] % genomeLength;

			byte cost = genes [position];

			CreatureCreationContext context = Incubator.GenerateContext (genes);

			Assert.AreEqual (cost, context.CostOfEnzymeProcessing);
		}

		[Test]
		public void WhenCreatureContextCreated_MaxEnergyShouldBeCorrect()
		{
			byte[] genes = GenerateRandomGenome (256);

			byte maxEnergyPosition = genes [genes [Incubator.MaxEnergyPosition]];

			var context = Incubator.GenerateContext (genes);

			Assert.AreEqual (maxEnergyPosition, context.EnergyMaximum);
		}

		[Test]
		public void WhenCreatureContextCreated_SmallGenome_MaxEnergyShouldBeCorrect()
		{
			int genomeLength = 8;

			byte[] genes = GenerateRandomGenome (genomeLength);				

			int position = genes[Incubator.MaxEnergyPosition] % genomeLength;

			byte max = genes [position];

			CreatureCreationContext context = Incubator.GenerateContext (genes);

			Assert.AreEqual (max, context.EnergyMaximum);
		}


		[Test]
		public void WhenCreatureContextCreated_EnzymesShouldBeCorrectLength()
		{
			byte[] genes = GenerateRandomGenome (256);

			int position = Math.Max ((int) 1, (int) genes [Incubator.LengthOfEnzymeChainPosition]);

			int length = genes [position];

			var context = Incubator.GenerateContext (genes);

			Assert.AreEqual (length, context.Enzymes.Count);
		}

		[Test]
		public void WhenCreatureContextCreated_EnzymesShouldBeCorrect()
		{
			byte[] genes = GenerateRandomGenome (256);

			byte positionOfLength = 65;

			genes [Incubator.LengthOfEnzymeChainPosition] = positionOfLength;
			genes [positionOfLength] = 2;//2 enzymes

			int startOfEnzymeChain = genes [genes [Incubator.StartOfEnzymeChainPosition]];

			byte enzyme0 = genes [genes [startOfEnzymeChain % 256]];
			byte enzyme1 = genes [genes [(startOfEnzymeChain + 1) % 256]];

			var context = Incubator.GenerateContext (genes);

			Assert.AreEqual (enzyme0, context.Enzymes [0]);
			Assert.AreEqual (enzyme1, context.Enzymes [1]);
		}

		[Test]
		public void WhenCreatureContextCreated_ShortGenome_EnzymesShouldBeCorrect()
		{
			int genomeLength = 10;

			byte[] genes = GenerateRandomGenome (genomeLength);

			byte positionOfLength = 65;

			genes [Incubator.LengthOfEnzymeChainPosition] = positionOfLength;
			genes [positionOfLength % genomeLength] = 2;//2 enzymes

			int startOfEnzymeChain = genes [genes [Incubator.StartOfEnzymeChainPosition] % genomeLength];

			byte enzyme0 = genes [genes [startOfEnzymeChain % genomeLength] % genomeLength];
			byte enzyme1 = genes [genes [(startOfEnzymeChain + 1) % genomeLength] % genomeLength];

			var context = Incubator.GenerateContext (genes);

			Assert.AreEqual (enzyme0, context.Enzymes [0]);
			Assert.AreEqual (enzyme1, context.Enzymes [1]);
		}

		byte[] GenerateRandomGenome (int length)
		{
			byte[] genes = new byte[length];

			new Random().NextBytes(genes);

			return genes;
		}
	}
}

