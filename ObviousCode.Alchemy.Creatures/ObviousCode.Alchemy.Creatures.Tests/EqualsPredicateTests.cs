using System;
using NUnit.Framework;
using ObviousCode.Alchemy.Creatures.DecisionProcessing;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	[TestFixture]
	public class EqualsPredicateTest
	{
		public EqualsPredicateTest ()
		{
		}

		[Test]
		public void WhenEqualsPredicateLoadedWithNonMatchingValues_AnswerShouldBeFalse ()
		{
			EqualsPredicate equals = new EqualsPredicate ();

			equals.Push (new Value (3.5d));
			equals.Push (new Value (6.5d));

			Assert.IsFalse (equals.GetValue ());
		}

		[Test]
		public void WhenEqualsPredicateLoadedWithMatchingValues_AnswerShouldBeFalse ()
		{
			EqualsPredicate equals = new EqualsPredicate ();

			equals.Push (new Value (3.5d));
			equals.Push (new Value (3.5d));

			Assert.IsTrue (equals.GetValue ());
		}

		[Test]
		[ExpectedException (typeof(InvalidOperationException))]
		public void WhenNoValuesAddedToPredicate_Fails ()
		{
			EqualsPredicate equals = new EqualsPredicate ();

			Assert.IsTrue (equals.GetValue ());
		}

		[Test]
		[ExpectedException (typeof(InvalidOperationException))]
		public void WhenOneValueAddedToPredicate_Fails ()
		{
			EqualsPredicate equals = new EqualsPredicate ();

			equals.Push (new Value (23d));

			Assert.IsTrue (equals.GetValue ());
		}

		[Test]
		[ExpectedException (typeof(StackOverflowException))]
		public void WhenThirdValueAddedToPredicate_Fails ()
		{
			EqualsPredicate equals = new EqualsPredicate ();

			equals.Push (new Value (123.456d));
			equals.Push (new Value (123.456d));
			equals.Push (new Value (123.456d));

			Assert.IsTrue (equals.GetValue ());
		}
	}
}

