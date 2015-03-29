using System;
using ObviousCode.Alchemy.Creatures.DecisionProcessing;
using NUnit.Framework;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	[TestFixture]
	public class AdditionOperatorTests
	{
		public AdditionOperatorTests ()
		{
		}

		[Test]
		public void WhenAdditionOperatorSetValues_OutputShouldBeCorrect()
		{
			AdditionOperator addition = new AdditionOperator ();

			addition.Push (new Value (10d));
			addition.Push (new Value (5.5d));

			Assert.AreEqual(15.5d, addition.GetValue());
		}

		[Test]
		public void WhenAdditionOperatorSetOneValueOneOperator_OutputShouldBeCorrect()
		{
			AdditionOperator addition = new AdditionOperator ();

			addition.Push (new Value (10d));
			addition.Push (new Value (5.5d));

			AdditionOperator addition2 = new AdditionOperator ();

			addition2.Push (addition);
			addition2.Push (new Value (4.5d));

			Assert.AreEqual(20d, addition2.GetValue());
		}
	}
}

