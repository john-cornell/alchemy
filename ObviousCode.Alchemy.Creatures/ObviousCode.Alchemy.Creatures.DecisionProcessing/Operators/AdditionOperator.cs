using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class AdditionOperator : OperatorValue
	{
		public AdditionOperator ()
		{
		}

		public override double GetValue ()
		{
			return Stack.Pop ().GetValue () + Stack.Pop ().GetValue ();
		}

	}
}

