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

			addition.Push (new Value (10m, "TEST"));
			addition.Push (new Value (5.5m, "TEST"));

			Assert.AreEqual (15.5m, addition.GetValue ());
		}

		[Test]
		//I dunno how to name this one, deal with it
		public void WhenAdditionOperatorSetWithOperatorSourcedOperators_OutputShouldBeCorrect ()
		{
			AdditionOperator level1a_1 = new AdditionOperator ();
			AdditionOperator level1a_2 = new AdditionOperator ();

			level1a_1.Push (new Value (14.4m, "TEST"));
			level1a_1.Push (new Value (-4.4m, "TEST"));

			level1a_2.Push (new Value (15.2m, "TEST"));
			level1a_2.Push (new Value (4.8m, "TEST"));

			AdditionOperator level1b_1 = new AdditionOperator ();
			AdditionOperator level1b_2 = new AdditionOperator ();

			level1b_1.Push (new Value (7.3m, "TEST"));
			level1b_1.Push (new Value (2.7m, "TEST"));

			level1b_2.Push (new Value (9m, "TEST"));
			level1b_2.Push (new Value (1m, "TEST"));

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

			operator1.Push (new Value (10m, "TEST"));
			operator1.Push (new Value (5.5m, "TEST"));

			AdditionOperator operator2 = new AdditionOperator ();

			operator2.Push (new Value (12.2m, "TEST"));
			operator2.Push (new Value (4.5m, "TEST"));

			AdditionOperator output = new AdditionOperator ();

			output.Push (operator1);
			output.Push (operator2);

			Assert.AreEqual (32.2m, output.GetValue ());
		}

		[Test]
		public void WhenAdditionOperatorSetOneValueOneOperator_OutputShouldBeCorrect ()
		{
			AdditionOperator operatorValue = new AdditionOperator ();

			operatorValue.Push (new Value (10m, "TEST"));
			operatorValue.Push (new Value (5.5m, "TEST"));

			AdditionOperator output = new AdditionOperator ();

			output.Push (operatorValue);
			output.Push (new Value (4.5m, "TEST"));

			Assert.AreEqual (20m, output.GetValue ());
		}
	}
}

