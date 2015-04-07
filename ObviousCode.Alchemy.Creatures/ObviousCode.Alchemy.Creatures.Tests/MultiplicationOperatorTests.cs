using System;
using ObviousCode.Alchemy.Creatures.DecisionProcessing;
using NUnit.Framework;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	[TestFixture]
	public class MultiplicationOperatorTests
	{
		public MultiplicationOperatorTests ()
		{
		}

		[Test]
		public void WhenMultiplicationOperatorSetValues_OutputShouldBeCorrect ()
		{
			MultiplicationOperator op = new MultiplicationOperator ();

			op.Push (new Value (1.55m, "TEST"));
			op.Push (new Value (10m, "TEST"));

			decimal value = op.GetValue ();

			Assert.AreEqual (15.5m, value);
		}

		[Test]
		//I dunno how to name this one, deal with it
		public void WhenMultiplicationOperatorSetWithOperatorSourcedOperators_OutputShouldBeCorrect ()
		{
			MultiplicationOperator level1a_1 = new MultiplicationOperator ();
			MultiplicationOperator level1a_2 = new MultiplicationOperator ();

			//-10
			level1a_1.Push (new Value (2.5m, "TEST"));
			level1a_1.Push (new Value (-4m, "TEST"));

			//38
			level1a_2.Push (new Value (15.2m, "TEST"));
			level1a_2.Push (new Value (2.5m, "TEST"));

			MultiplicationOperator level1b_1 = new MultiplicationOperator ();
			MultiplicationOperator level1b_2 = new MultiplicationOperator ();

			//19.71
			level1b_1.Push (new Value (7.3m, "TEST"));
			level1b_1.Push (new Value (2.7m, "TEST"));

			//9
			level1b_2.Push (new Value (9m, "TEST"));
			level1b_2.Push (new Value (1m, "TEST"));

			MultiplicationOperator level2a = new MultiplicationOperator ();
			MultiplicationOperator level2b = new MultiplicationOperator ();

			//-380
			level2a.Push (level1a_1);
			level2a.Push (level1a_2);
			//177.39
			level2b.Push (level1b_1);
			level2b.Push (level1b_2);

			MultiplicationOperator level3 = new MultiplicationOperator ();

			level3.Push (level2a);
			level3.Push (level2b);

			Assert.AreEqual (-67408.2m, level3.GetValue ());
		}

		[Test]
		public void WhenMultiplicationOperatorSetTwoOperators_OutputShouldBeCorrect ()
		{
			MultiplicationOperator operator1 = new MultiplicationOperator ();

			//55
			operator1.Push (new Value (10m, "TEST"));
			operator1.Push (new Value (5.5m, "TEST"));

			MultiplicationOperator operator2 = new MultiplicationOperator ();

			//9
			operator2.Push (new Value (2m, "TEST"));
			operator2.Push (new Value (4.5m, "TEST"));

			MultiplicationOperator output = new MultiplicationOperator ();

			output.Push (operator1);
			output.Push (operator2);

			Assert.AreEqual (495m, output.GetValue ());
		}

		[Test]
		public void WhenMultiplicationOperatorSetOneValueOneOperator_OutputShouldBeCorrect ()
		{
			MultiplicationOperator operatorValue = new MultiplicationOperator ();

			//55m
			operatorValue.Push (new Value (10m, "TEST"));
			operatorValue.Push (new Value (5.5m, "TEST"));

			MultiplicationOperator output = new MultiplicationOperator ();

			//220
			output.Push (operatorValue);
			output.Push (new Value (4m, "TEST"));

			Assert.AreEqual (220m, output.GetValue ());
		}
	}
}

