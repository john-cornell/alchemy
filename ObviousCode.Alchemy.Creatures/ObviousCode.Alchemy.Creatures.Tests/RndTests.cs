using System;
using NUnit.Framework;
using ObviousCode.Alchemy.Creatures.DecisionProcessing;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	[TestFixture]
	public class RandomTests
	{
		[Test]
		public void WhenRandomSeededWithSameSeed_OutputStreamMatch ()
		{			
			Random seedGenerator = new Random ();

			int seed = seedGenerator.Next (254);

			Random rand1 = new Random (seed);

			int[] rnd1 = new int[] {
				rand1.Next (),
				rand1.Next (),
				rand1.Next (),
				rand1.Next (),
				rand1.Next (),
				rand1.Next (),
				rand1.Next (),
				rand1.Next (),
				rand1.Next (),
				rand1.Next ()
			};

			Random rand2 = new Random (seed);

			int[] rnd2 = new int[] {
				rand2.Next (),
				rand2.Next (),
				rand2.Next (),
				rand2.Next (),
				rand2.Next (),
				rand2.Next (),
				rand2.Next (),
				rand2.Next (),
				rand2.Next (),
				rand2.Next ()
			};			

			for (int i = 0; i < rnd1.Length; i++) {
				Assert.AreEqual (rnd1 [i], rnd2 [i]);
			}
		}

		[Test]
		public void WhenRandomSeededWithDifferentSeed_OutputStreamsDontMatch ()
		{			
			Random seed1Generator = new Random ();
			Random seed2Generator = new Random ();

			int seed1 = seed1Generator.Next (254);
			int seed2;
			do {
				seed2 = seed2Generator.Next (254);
			} while (seed1 == seed2);

			Random rand1 = new Random (seed1);
			Random rand2 = new Random (seed2);

			int[] rnd1 = new int[] {
				rand1.Next (),
				rand1.Next (),
				rand1.Next (),
				rand1.Next (),
				rand1.Next (),
				rand1.Next (),
				rand1.Next (),
				rand1.Next (),
				rand1.Next (),
				rand1.Next ()
			};				

			int[] rnd2 = new int[] {
				rand2.Next (),
				rand2.Next (),
				rand2.Next (),
				rand2.Next (),
				rand2.Next (),
				rand2.Next (),
				rand2.Next (),
				rand2.Next (),
				rand2.Next (),
				rand2.Next ()
			};			

			bool match = true;

			for (int i = 0; i < rnd1.Length; i++) {
				match = true & rnd1 [i] == rnd2 [i];
			}

			Assert.IsFalse (match);
		}
	}
}

