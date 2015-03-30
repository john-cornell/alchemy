using System;
using NUnit.Framework;
using ObviousCode.Alchemy.Creatures.DecisionProcessing;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	[TestFixture]
	public class DecisionTests
	{
		[Test]
		public void DecisionsRunWithoutException ()
		{
			Random random = new Random ();

			byte[] code = Array.CreateInstance (typeof(byte), 1000) as byte[];

			random.NextBytes (code);

			Decisions decisions = new Decisions (code, (byte)random.Next (255), random.Next (code.Length));

			for (int i = 0; i < 1000; i++) {
				decisions.GetDecision (random.Next ());
			}
		}
	}
}

