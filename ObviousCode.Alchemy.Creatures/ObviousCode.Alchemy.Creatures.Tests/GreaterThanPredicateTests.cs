using System;
using NUnit.Framework;
using ObviousCode.Alchemy.Creatures.DecisionProcessing;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	[TestFixture]
	public class GreaterThanPredicateTest
	{
		public GreaterThanPredicateTest ()
		{
		}

		[Test]
		public void WhenGreaterThanPredicateLoadedWithGreaterValue_AnswerShouldBeTrue ()
		{
			GreaterThanPredicate greaterThan = new GreaterThanPredicate ();

			greaterThan.Push (new Value (3.5d));
			greaterThan.Push (new Value (6.5d));

			Assert.IsTrue (greaterThan.GetValue ());
		}

		[Test]
		public void WhenGreaterThanPredicateLoadedWithLessThanValue_AnswerShouldBeFalse ()
		{
			GreaterThanPredicate greaterThan = new GreaterThanPredicate ();

			greaterThan.Push (new Value (6.5d));
			greaterThan.Push (new Value (3.5d));

			Assert.IsFalse (greaterThan.GetValue ());
		}
	}
}

