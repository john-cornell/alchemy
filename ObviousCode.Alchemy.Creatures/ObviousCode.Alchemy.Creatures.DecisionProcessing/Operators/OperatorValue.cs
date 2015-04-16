using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public abstract class OperatorValue : PredicateValue
	{
		public StackArray<PredicateValue> Stack { get; private set; }

		public OperatorValue ()
		{			
			Stack = new StackArray<PredicateValue> (2);
		}

		public void Push (PredicateValue value)
		{			
			Stack.Push (value);
		}

		public abstract OperatorValue CreateNew ();
	}
}

