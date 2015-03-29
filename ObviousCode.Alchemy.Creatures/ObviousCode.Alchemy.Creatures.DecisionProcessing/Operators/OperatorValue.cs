using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public abstract class OperatorValue : PredicateValue
	{
		public Stack<PredicateValue> Stack { get; private set; }

		public OperatorValue ()
		{
			Stack = new Stack<PredicateValue> (2);
		}

		public void Push (PredicateValue value)
		{
			if (Stack.Count == 2)
				throw new StackOverflowException (
					string.Format ("Stackoverflow in Operator Value: '{0}'", GetType ()));

			Stack.Push (value);
		}
	}
}

