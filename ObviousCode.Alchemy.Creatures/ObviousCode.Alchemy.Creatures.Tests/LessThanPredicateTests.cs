using System;
using NUnit.Framework;
using ObviousCode.Alchemy.Creatures.DecisionProcessing;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	[TestFixture]
	public class LessThanPredicateTest
	{
		public LessThanPredicateTest ()
		{
		}

		[Test]
		public void WhenLessThanPredicateLoadedWithGreaterValue_AnswerShouldBeFalse()
		{
			LessThanPredicate lessThan = new LessThanPredicate ();

			lessThan.Push(new Value(3.5d));
			lessThan.Push(new Value(6.5d));

			Assert.IsFalse (lessThan.GetValue ());
		}

		[Test]
		public void WhenLessThanPredicateLoadedWithLessThanValue_AnswerShouldBeTrue()
		{
			LessThanPredicate lessThan = new LessThanPredicate ();

			lessThan.Push(new Value(6.5d));
			lessThan.Push(new Value(3.5d));

			Assert.IsTrue (lessThan.GetValue ());
		}
	}
}

