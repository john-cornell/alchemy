using System;
using NUnit.Framework;
using ObviousCode.Alchemy.Creatures.DecisionProcessing;
using System.Collections.Generic;
using System.Linq;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	[TestFixture]
	public class DecisionTests
	{
		static Decisions CreateDecision (int seed, out Random random)
		{
			Random internalRandom = new Random (seed);

			byte[] code = new byte[1000] as byte[];

			internalRandom.NextBytes (code);

			Decisions decisions = new Decisions ((byte)internalRandom.Next (255), internalRandom.Next (code.Length));

			Enumerable.Range (2, internalRandom.Next (20))
				.ToList ()
				.ForEach (i => {
				//int value
				decisions.LoadConstantValue (new Value (internalRandom.Next () % 255, "TEST Random"));

			});

			random = internalRandom;

			return decisions;
		}

		[Test]
		public void WhenSameDecisionGiven_OutcomeSame ()
		{
			Random random;

			Decisions decisions = CreateDecision (new Random ().Next (), out random);

			for (int i = 0; i < 100; i++) {
				
				int predicatePosition = new Random ().Next ();
				
				Decisions.Outcome outcome = decisions.GetDecision (predicatePosition);	

				for (int j = 0; j < 100; j++) {
					Assert.AreEqual (outcome, decisions.GetDecision (predicatePosition));
				}
			}
		}

		[Test]
		public void DecisionsRunWithoutException ()
		{
			Random seedGenerator = new Random ();
			Random random;

			Decisions decisions = CreateDecision (seedGenerator.Next (), out random);

			for (int i = 0; i < 1000; i++) {
				decisions.GetDecision (random.Next ());
			}
		}

		[Test]
		public void WhenCodeIsSame_SeedSame_DecisionsMatch ()
		{
			Random seedGenerator = new Random ();

			int seed = seedGenerator.Next ();

			Random random1;
			Random random2;

			Decisions decisions1 = CreateDecision (seed, out random1);
			Decisions decisions2 = CreateDecision (seed, out random2);

			Dictionary<Decisions.Outcome, int> outcomeCount = new Dictionary<Decisions.Outcome, int> {
				{ Decisions.Outcome.True, 0 },
				{ Decisions.Outcome.False, 0 },
				{ Decisions.Outcome.Die, 0 }
			};

			for (int j = 0; j < 10000; j++) {				

				int rnd1 = random1.Next ();
				int rnd2 = random2.Next ();

				Assert.AreEqual (rnd1, rnd2);

				Decisions.Outcome outcome1 = decisions1.GetDecision (rnd1);
				Decisions.Outcome outcome2 = decisions2.GetDecision (rnd2);

				outcomeCount [outcome1]++;

				Assert.AreEqual (outcome1, outcome2);
			}
		}

		[Test]
		public void WhenCodeIsDifferent_SeedDifferent_DecisionsDontMatch ()
		{
			Random seedGenerator = new Random ();

			Random random1;
			Random random2;

			Decisions decisions1 = CreateDecision (seedGenerator.Next (), out random1);
			Decisions decisions2 = CreateDecision (seedGenerator.Next (), out random2);

			bool match = true;

			for (int j = 0; j < 10000; j++) {				

				int rnd1 = random1.Next ();
				int rnd2 = random2.Next ();

				Decisions.Outcome outcome1 = decisions1.GetDecision (rnd1);
				Decisions.Outcome outcome2 = decisions2.GetDecision (rnd2);

				match = match & outcome1 == outcome2;
			}

			Assert.IsFalse (match);
		}
	}
}

