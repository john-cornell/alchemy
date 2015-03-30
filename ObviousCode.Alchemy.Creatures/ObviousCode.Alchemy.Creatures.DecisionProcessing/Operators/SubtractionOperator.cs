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
	}
}

