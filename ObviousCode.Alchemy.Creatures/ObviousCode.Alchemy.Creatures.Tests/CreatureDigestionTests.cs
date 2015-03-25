using System;
using NUnit.Framework;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	[TestFixture]
	public class CreatureDigestionTests
	{
		[Test]
		public void WhenCreatureCantDigest_OutputEqualsInput()
		{
			Creature creature = new Creature (new CreatureCreationContext{

			});

			Assert.AreEqual (100, creature.Digest (100));
		}

		[Test]
		public void WhenCreatureCanDigestOnce_OutputIsCorrect()
		{
			byte enzyme = 2;

			int food = 6;
			int expectedOutput = 4;

			Creature creature = new Creature (
				                    new CreatureCreationContext {
					Enzymes = new System.Collections.Generic.List<byte> (new byte[]{ enzyme }),
					Energy = 100
				});

			Assert.AreEqual (expectedOutput, creature.Digest(food));
		}

		[Test]
		public void WhenCreatureCanDigestTwice_OutputIsCorrect()
		{
			byte[] enzymes = { 11, 33 };

			int food = 57;
			int expectedOutput = 16;

			Creature creature = new Creature (
				new CreatureCreationContext{
					Energy = 100,
					Enzymes = new System.Collections.Generic.List<byte>(enzymes)
				});

			Assert.AreEqual (expectedOutput, creature.Digest (food));
		}

		[Test]
		public void WhenCreatureCantDigest_CostOfDigestionEnergyIsDecrementedCorrectly()
		{
			Creature creature = new Creature (
				new CreatureCreationContext{
					Energy = 100,
					CostOfEnzymeProcessing = 0,
					CostOfDigestion = 1
				});

			creature.Digest (100);

			Assert.AreEqual (99, creature.Energy);
		}

		[Test]
		public void WhenCreatureCanDigestOnce_CostOfDigestionEnergyIsDecrementedCorrectly()
		{
			byte[] enzymes = new byte[]{ 128, 15, 4, 3, 2, 6, 7, 8, 9, 3, 4, 5, 6 };
			int food = 127;
			int expectedOutput = 112;

			int costOfDigestion = 10;
			int energy = 100;
			int expectedEnergy = 90 + (food & enzymes[1]);

			Creature creature = new Creature (new CreatureCreationContext {
				Enzymes = new System.Collections.Generic.List<byte>(enzymes),
				Energy = energy,
				CostOfDigestion = costOfDigestion,
				CostOfEnzymeProcessing = 0,
				EnergyExtractionRatio = 1d
			});

			Assert.AreEqual (expectedOutput, creature.Digest (food));
			Assert.AreEqual (expectedEnergy, creature.Energy);
		}

		[Test]
		public void WhenCreatureDigestsTooMuch_Dies()
		{
			byte[] enzymes = new byte[]{ 200 };

			int food = 200;

			Creature creature = new Creature (new CreatureCreationContext {
				Energy = 400,
				EnergyMaximum = 499,
				CostOfDigestion = 0,
				CostOfEnzymeProcessing = 0,
				EnergyExtractionRatio =.5d,
				Enzymes = new System.Collections.Generic.List<byte>(enzymes)
			});

			creature.Digest (food);

			Assert.IsFalse (creature.IsAlive);
		}

		[Test]
		public void WhenCreatureDigestsTooMuch_Pops()
		{
			byte[] enzymes = new byte[]{ 200 };

			int food = 200;

			Creature creature = new Creature (new CreatureCreationContext {
				Energy = 400,
				EnergyMaximum = 499,
				CostOfDigestion = 0,
				CostOfEnzymeProcessing = 0,
				EnergyExtractionRatio =.5d,
				Enzymes = new System.Collections.Generic.List<byte>(enzymes)
			});

			creature.Digest (food);

			Assert.AreEqual (ObviousCode.Alchemy.Creatures.Creature.CausesOfDeath.Popped, creature.CauseOfDeath );
		}

		[Test]
		public void WhenCreatureCanDigestTwice_CostOfDigestionEnergyIsDecrementedCorrectly()
		{
			byte[] enzymes = new byte[]{ 128, 15, 5, 3, 16, 6, 7, 8, 9, 3, 4, 5, 6 };
			int food = 127;
			int expectedOutput = 96;

			int costOfDigestion = 10;
			int energy = 100;
			int expectedEnergy = 90 + (food & enzymes[1]) + (food & enzymes[4]);

			Creature creature = new Creature (new CreatureCreationContext {
				Enzymes = new System.Collections.Generic.List<byte>(enzymes),
				Energy = energy,
				CostOfDigestion = costOfDigestion,
				CostOfEnzymeProcessing = 0,
				EnergyExtractionRatio = 1d
			});

			Assert.AreEqual (expectedOutput, creature.Digest (food));
			Assert.AreEqual (expectedEnergy, creature.Energy);
		}

		[Test]
		public void WhenCreatureCanDigestOnce_CostOfEnzymeProcessEnergyIsDecrementedCorrectly()
		{
			byte[] enzymes = new byte[]{ 128, 15, 4, 3, 2, 6, 7, 8, 9, 3, 4, 5, 6 };
			int food = 127;
			int expectedOutput = 112;

			int costOfEnzymeProcessing = 3;
			int energy = 100;
			int expectedEnergy = 100 + (food & enzymes[1]) - (enzymes.Length * costOfEnzymeProcessing);

			Creature creature = new Creature (new CreatureCreationContext {
				Enzymes = new System.Collections.Generic.List<byte>(enzymes),
				Energy = energy,
				CostOfDigestion = 0,
				CostOfEnzymeProcessing = costOfEnzymeProcessing,
				EnergyExtractionRatio = 1d
			});

			Assert.AreEqual (expectedOutput, creature.Digest (food));
			Assert.AreEqual (expectedEnergy, creature.Energy);
		}

		[Test]
		public void WhenCreatureCanDigestTwice_CostOfEnzymeProcessEnergyIsDecrementedCorrectly()
		{
			byte[] enzymes = new byte[]{ 128, 15, 4, 3, 16, 6, 7, 8, 9, 3, 4, 5, 6 };
			int food = 127;
			int expectedOutput = 96;

			int costOfEnzymeProcessing = 3;
			int energy = 100;
			int expectedEnergy = 100 + (food & enzymes[1]) + (food & enzymes[4]) - (enzymes.Length * costOfEnzymeProcessing);

			Creature creature = new Creature (new CreatureCreationContext {
				Enzymes = new System.Collections.Generic.List<byte>(enzymes),
				Energy = energy,
				CostOfDigestion = 0,
				CostOfEnzymeProcessing = costOfEnzymeProcessing,
				EnergyExtractionRatio = 1d
			});

			Assert.AreEqual (expectedOutput, creature.Digest (food));
			Assert.AreEqual (expectedEnergy, creature.Energy);
		}

		[Test]
		public void WhenCreatureCanDigestOnce_EnergyIsIncrementedCorrectly()
		{
			Creature creature = new Creature (new CreatureCreationContext {
				Energy = 100,
				CostOfDigestion = 0,
				CostOfEnzymeProcessing = 0,
				Enzymes=new System.Collections.Generic.List<byte>(new byte[]{42}),
				EnergyExtractionRatio = .25d
			});

			creature.Digest (43);

			Assert.AreEqual (111, creature.Energy);
		}

		[Test]
		public void WhenCreatureCanDigestTwice_EnergyIsIncrementedCorrectly()
		{
			Creature creature = new Creature (new CreatureCreationContext {
				Energy = 100,
				CostOfDigestion = 0,
				CostOfEnzymeProcessing = 0,
				Enzymes=new System.Collections.Generic.List<byte>(new byte[]{16, 1, 4, 32}),
				EnergyExtractionRatio = 1d
			});

			Assert.AreEqual (64, creature.Digest (112));

			Assert.AreEqual (148, creature.Energy);
		}

		[Test]
		public void WhenCreatureUsesTooMuchEnergyDuringDigestion_Dies()
		{
			int energy = 100;
			int costOfEnzymes = 10;
			int costOfDigestion = 10;

			Creature creature = new Creature(new CreatureCreationContext{
				Energy = energy,
				CostOfDigestion = costOfDigestion,
				CostOfEnzymeProcessing = costOfEnzymes,
				Enzymes = new System.Collections.Generic.List<byte>(new byte[]{
					10,10,10,10,10,10,10,10,10,10
				}),
				EnergyExtractionRatio = 1d
			});

			creature.Digest (16);

			Assert.IsFalse (creature.IsAlive);
		}

		[Test]
		public void WhenCreatureUsesTooMuchEnergyDuringDigestion_FiresDeathEvent()
		{
			int energy = 100;
			int costOfEnzymes = 10;
			int costOfDigestion = 10;

			Creature creature = new Creature (new CreatureCreationContext {
				Energy = energy,
				CostOfDigestion = costOfDigestion,
				CostOfEnzymeProcessing = costOfEnzymes,
				Enzymes = new System.Collections.Generic.List<byte> (new byte[] {
					10, 10, 10, 10, 10, 10, 10, 10, 10, 10
				}),
				EnergyExtractionRatio = 1d
			});

			bool dead = false;

			creature.CreatureDied += (sender, e) => dead = true;

			creature.Digest (16);

			Assert.IsTrue (dead);
		}
	}
}

