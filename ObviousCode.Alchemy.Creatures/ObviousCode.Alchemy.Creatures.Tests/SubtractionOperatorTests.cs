using System;
using ObviousCode.Alchemy.Creatures.DecisionProcessing;
using NUnit.Framework;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	[TestFixture]
	public class SubtractionOperatorTests
	{
		public SubtractionOperatorTests ()
		{
		}

		[Test]
		public void WhenSubtractionOperatorSetValues_OutputShouldBeCorrect ()
		{
			SubtractionOperator subtraction = new SubtractionOperator ();

			subtraction.Push (new Value (10m));
			subtraction.Push (new Value (5.5m));

			Assert.AreEqual (4.5m, subtraction.GetValue ());
		}

		[Test]
		//I dunno how to name this one, deal with it
		public void WhenSubtractionOperatorSetWithOperatorSourcedOperators_OutputShouldBeCorrect ()
		{
			SubtractionOperator level1a_1 = new SubtractionOperator ();
			SubtractionOperator level1a_2 = new SubtractionOperator ();

			//40
			level1a_1.Push (new Value (34.4m));
			level1a_1.Push (new Value (-5.6m));

			//10.5
			level1a_2.Push (new Value (15.2m));
			level1a_2.Push (new Value (4.7m));

			SubtractionOperator level1b_1 = new SubtractionOperator ();
			SubtractionOperator level1b_2 = new SubtractionOperator ();

			//5.5
			level1b_1.Push (new Value (7.3m));
			level1b_1.Push (new Value (1.8m));

			//8
			level1b_2.Push (new Value (9m));
			level1b_2.Push (new Value (1m));

			SubtractionOperator level2a = new SubtractionOperator ();
			SubtractionOperator level2b = new SubtractionOperator ();

			//40 - 10.5 = 29.5
			level2a.Push (level1a_1);
			level2a.Push (level1a_2);

			//5.5 - 8 = -2.5
			level2b.Push (level1b_1);
			level2b.Push (level1b_2);

			SubtractionOperator level3 = new SubtractionOperator ();

			//29.5 - -2.5 = 32
			level3.Push (level2a);
			level3.Push (level2b);

			Assert.AreEqual (32m, level3.GetValue ());
		}

		[Test]
		public void WhenSubtractionOperatorSetTwoOperators_OutputShouldBeCorrect ()
		{
			SubtractionOperator operator1 = new SubtractionOperator ();

			//4.5
			operator1.Push (new Value (10m));
			operator1.Push (new Value (5.5m));

			SubtractionOperator operator2 = new SubtractionOperator ();

			//7.5
			operator2.Push (new Value (12.2m));
			operator2.Push (new Value (4.7m));

			SubtractionOperator output = new SubtractionOperator ();

			output.Push (operator1);
			output.Push (operator2);

			decimal value = output.GetValue ();

			Assert.AreEqual (-3m, value);
		}

		[Test]
		public void WhenSubtractionOperatorSetOneValueOneOperator_OutputShouldBeCorrect ()
		{
			SubtractionOperator operatorValue = new SubtractionOperator ();

			//4.5
			operatorValue.Push (new Value (10m));
			operatorValue.Push (new Value (5.5m));

			SubtractionOperator output = new SubtractionOperator ();

			output.Push (operatorValue);
			output.Push (new Value (4.5m));

			decimal value = output.GetValue ();

			Assert.AreEqual (0m, value);
		}
	}
}

