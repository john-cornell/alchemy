using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class AdditionOperator : OperatorValue
	{
		public override decimal GetValue ()
		{
			return Stack.Pop ().GetValue () + Stack.Pop ().GetValue ();
		}
	}
}

