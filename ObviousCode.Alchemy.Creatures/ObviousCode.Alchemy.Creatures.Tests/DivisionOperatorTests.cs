using System;
using ObviousCode.Alchemy.Creatures.DecisionProcessing;
using NUnit.Framework;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	[TestFixture]
	public class DivisionOperatorTests
	{
		public DivisionOperatorTests ()
		{
		}

		[Test]
		public void WhenDivisionOperatorSetValues_OutputShouldBeCorrect ()
		{
			DivisionOperator addition = new DivisionOperator ();

			addition.Push (new Value (10m));
			addition.Push (new Value (5.5m));

			Assert.AreEqual (10m / 5.5m, addition.GetValue ());
		}

		[Test]
		//I dunno how to name this one, deal with it
		public void WhenDivisionOperatorSetWithOperatorSourcedOperators_OutputShouldBeCorrect ()
		{
			DivisionOperator level1a_1 = new DivisionOperator ();
			DivisionOperator level1a_2 = new DivisionOperator ();

			//-8
			level1a_1.Push (new Value (16m));
			level1a_1.Push (new Value (-2m));

			//2
			level1a_2.Push (new Value (22m));
			level1a_2.Push (new Value (11m));

			DivisionOperator level1b_1 = new DivisionOperator ();
			DivisionOperator level1b_2 = new DivisionOperator ();

			//16
			level1b_1.Push (new Value (8m));
			level1b_1.Push (new Value (-.5m));

			//-4
			level1b_2.Push (new Value (16m));
			level1b_2.Push (new Value (4m));

			DivisionOperator level2a = new DivisionOperator ();
			DivisionOperator level2b = new DivisionOperator ();

			//-4
			level2a.Push (level1a_1);
			level2a.Push (level1a_2);

			//4
			level2b.Push (level1b_1);
			level2b.Push (level1b_2);

			DivisionOperator level3 = new DivisionOperator ();

			level3.Push (level2a);
			level3.Push (level2b);

			Assert.AreEqual (1, level3.GetValue ());
		}

		[Test]
		public void WhenDivisionOperatorSetTwoOperators_OutputShouldBeCorrect ()
		{
			DivisionOperator operator1 = new DivisionOperator ();

			operator1.Push (new Value (500m));
			operator1.Push (new Value (5m));

			DivisionOperator operator2 = new DivisionOperator ();

			operator2.Push (new Value (40m));
			operator2.Push (new Value (4m));

			DivisionOperator output = new DivisionOperator ();

			output.Push (operator1);
			output.Push (operator2);

			Assert.AreEqual (10m, output.GetValue ());
		}

		[Test]
		public void WhenDivisionOperatorSetOneValueOneOperator_OutputShouldBeCorrect ()
		{
			DivisionOperator operatorValue = new DivisionOperator ();
			//2
			operatorValue.Push (new Value (10m));
			operatorValue.Push (new Value (5m));

			DivisionOperator output = new DivisionOperator ();

			output.Push (operatorValue);
			output.Push (new Value (4m));

			Assert.AreEqual (.5m, output.GetValue ());
		}
	}
}

