using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class DivisionOperator : OperatorValue
	{
		//Allow divide by zero exception. This kills the creature
		public override decimal GetValue ()
		{
			decimal rhs = Stack.Pop ().GetValue ();
			decimal lhs = Stack.Pop ().GetValue ();

			return lhs / rhs;
		}
	}
}

