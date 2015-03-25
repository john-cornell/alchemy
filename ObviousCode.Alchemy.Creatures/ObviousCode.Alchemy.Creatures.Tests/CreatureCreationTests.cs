using System;
using NUnit.Framework;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	[TestFixture]
	public class CreatureCreationTests
	{
		[Test]
		public void WhenCreatureCreatedWithoutEnzymes_EnzymeListShouldBeInstanciated()
		{
			Creature creature = new Creature (new CreatureCreationContext{

			});

			Assert.IsNotNull (creature.Enzymes);
		}

		[Test]
		public void WhenCreatureCreatedWithoutEnzymes_EnzymeListShouldBeEmpty()
		{
			Creature creature = new Creature (new CreatureCreationContext{

			});

			Assert.AreEqual (0, creature.Enzymes.Count);
		}

		[Test]
		public void WhenCreatureCreated_EnergyNotSet_EnergyIs0()
		{
			Creature creature = new Creature(new CreatureCreationContext{

			});

			Assert.AreEqual(0, creature.Energy);
		}

		[Test]
		public void WhenCreatureCreated_EnergyNotSet_CreatureIsAlive()
		{
			Creature creature = new Creature(new CreatureCreationContext{

			});

			Assert.AreEqual (Creature.CausesOfDeath.StillAlive, creature.CauseOfDeath);
		}

		[Test]
		public void WhenCreatureCreated_EnergyNotSet_CauseOfDeathNotSet()
		{
			Creature creature = new Creature(new CreatureCreationContext{

			});

			Assert.AreEqual (creature.CauseOfDeath, Creature.CausesOfDeath.StillAlive);
		}

		[Test]
		public void WhenCreatureCreated_PositiveEnergySet_EnergyIsSet()
		{
			Creature creature = new Creature(new CreatureCreationContext{
				Energy = 100
			});

			Assert.AreEqual (100, creature.Energy);
		}

		[Test]
		public void WhenCreatureCreated_PositiveEnergySet_CreatureIsAlive()
		{
			Creature creature = new Creature(new CreatureCreationContext{
				Energy = 100
			});

			Assert.AreEqual (Creature.CausesOfDeath.StillAlive, creature.CauseOfDeath);
		}

		[Test]
		public void WhenCreatureCreated_PositiveEnergySet_CauseOfDeathNotSet()
		{
			Creature creature = new Creature (new CreatureCreationContext {
				Energy = 100
			});

			Assert.AreEqual (Creature.CausesOfDeath.StillAlive, creature.CauseOfDeath);
		}

		[Test]
		public void WhenCreatureCreatedWithNegativeEnergy_DiesImmediately()
		{
			Creature creature = new Creature (new CreatureCreationContext {
				Energy = -1
			});

			Assert.IsFalse (creature.IsAlive);
		}

		[Test]
		public void WhenCreatureCreatedWithNegativeEnergy_StarvesImmediately()
		{
			Creature creature = new Creature (new CreatureCreationContext {
				Energy = -1
			});

			Assert.AreEqual (Creature.CausesOfDeath.Starved, creature.CauseOfDeath);
		}

		[Test]
		public void WhenCreatureCreated_StartingEnergySet()
		{
			Creature creature = new Creature (new CreatureCreationContext {
				Energy = 100
			});

			Assert.AreEqual(100, creature.Energy);
		}

		[Test]
		public void WhenCreatureCreatedWithEnzymes_EnzymesShouldBeSet()
		{
			Creature creature = new Creature (new CreatureCreationContext{
				Enzymes = new System.Collections.Generic.List<byte>{
					25, 50, 100, 200, 255
				}
			});

			Assert.IsTrue (
				creature.Enzymes.Contains(25)
			);

			Assert.IsTrue (
				creature.Enzymes.Contains(50)
			);
			Assert.IsTrue (
				creature.Enzymes.Contains(100)
			);
			Assert.IsTrue (
				creature.Enzymes.Contains(200)
			);
			Assert.IsTrue (
				creature.Enzymes.Contains(255)
			);
		}
	}
}

