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
		public void WhenGreaterThanPredicateLoadedWithGreaterValue_AnswerShouldBeTrue()
		{
			GreaterThanPredicate lessThan = new GreaterThanPredicate ();

			lessThan.Push(new Value(3.5d));
			lessThan.Push(new Value(6.5d));

			Assert.IsTrue (lessThan.GetValue ());
		}

		[Test]
		public void WhenGreaterThanPredicateLoadedWithLessThanValue_AnswerShouldBeFalse()
		{
			GreaterThanPredicate lessThan = new GreaterThanPredicate ();

			lessThan.Push(new Value(6.5d));
			lessThan.Push(new Value(3.5d));

			Assert.IsFalse (lessThan.GetValue ());
		}
	}
}

