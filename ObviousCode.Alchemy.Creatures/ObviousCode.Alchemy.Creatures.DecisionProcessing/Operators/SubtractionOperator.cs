using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class SubtractionOperator : OperatorValue
	{
		public override decimal GetValue ()
		{
			decimal rhs = Stack.Pop ().GetValue ();
			decimal lhs = Stack.Pop ().GetValue ();

			return lhs - rhs;
		}

		public override OperatorValue CreateNew ()
		{
			return new SubtractionOperator ();
		}

		public override string Describe ()
		{
			return string.Format (" ( {0} - {1} ) "
								, Stack [0].Describe ()
								, Stack [1].Describe ());
		}
	}
}

