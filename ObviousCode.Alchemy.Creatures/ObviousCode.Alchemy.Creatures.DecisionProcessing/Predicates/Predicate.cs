using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public abstract class Predicate
	{
		public Stack<PredicateValue> Stack { get; private set; }

		protected Predicate () 
		{
			Stack = new Stack<PredicateValue> (2);
		}

		public void Push(PredicateValue value)
		{
			if (Stack.Count == 2)
				throw new InvalidOperationException ("Stack Overflow");

			Stack.Push (value);
		}

		public abstract bool GetValue();
	}
}

