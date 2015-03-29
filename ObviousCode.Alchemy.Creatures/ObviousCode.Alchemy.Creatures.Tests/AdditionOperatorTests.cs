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
		public void WhenAdditionOperatorSetValues_OutputShouldBeCorrect ()
		{
			AdditionOperator addition = new AdditionOperator ();

			addition.Push (new Value (10d));
			addition.Push (new Value (5.5d));

			Assert.AreEqual (15.5d, addition.GetValue ());
		}

		[Test]
		//I dunno how to name this one, deal with it
		public void WhenAdditionOperatorSetWithOperatorSourcedOperators_OutputShouldBeCorrect ()
		{
			AdditionOperator level1a_1 = new AdditionOperator ();
			AdditionOperator level1a_2 = new AdditionOperator ();

			level1a_1.Push (new Value (14.4d));
			level1a_1.Push (new Value (-4.4d));

			level1a_2.Push (new Value (15.2d));
			level1a_2.Push (new Value (4.8));

			AdditionOperator level1b_1 = new AdditionOperator ();
			AdditionOperator level1b_2 = new AdditionOperator ();

			level1b_1.Push (new Value (7.3d));
			level1b_1.Push (new Value (2.7d));

			level1b_2.Push (new Value (9d));
			level1b_2.Push (new Value (1d));

			AdditionOperator level2a = new AdditionOperator ();
			AdditionOperator level2b = new AdditionOperator ();

			level2a.Push (level1a_1);
			level2a.Push (level1a_2);

			level2b.Push (level1b_1);
			level2b.Push (level1b_2);

			AdditionOperator level3 = new AdditionOperator ();

			level3.Push (level2a);
			level3.Push (level2b);

			Assert.AreEqual (50, level3.GetValue ());
		}

		[Test]
		public void WhenAdditionOperatorSetTwoOperators_OutputShouldBeCorrect ()
		{
			AdditionOperator operator1 = new AdditionOperator ();

			operator1.Push (new Value (10d));
			operator1.Push (new Value (5.5d));

			AdditionOperator operator2 = new AdditionOperator ();

			operator2.Push (new Value (12.2d));
			operator2.Push (new Value (4.5d));

			AdditionOperator output = new AdditionOperator ();

			output.Push (operator1);
			output.Push (operator2);

			Assert.AreEqual (32.2d, output.GetValue ());
		}

		[Test]
		public void WhenAdditionOperatorSetOneValueOneOperator_OutputShouldBeCorrect ()
		{
			AdditionOperator operatorValue = new AdditionOperator ();

			operatorValue.Push (new Value (10d));
			operatorValue.Push (new Value (5.5d));

			AdditionOperator output = new AdditionOperator ();

			output.Push (operatorValue);
			output.Push (new Value (4.5d));

			Assert.AreEqual (20d, output.GetValue ());
		}
	}
}

