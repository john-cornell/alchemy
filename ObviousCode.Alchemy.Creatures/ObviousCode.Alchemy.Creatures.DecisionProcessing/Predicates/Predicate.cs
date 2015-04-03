using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public abstract class Predicate
	{
		public enum PredicateType { Equals, False, GT, LT, True }

		public Stack<PredicateValue> Stack { get; private set; }

		public int Seed { get; set; }

		protected Predicate ()
		{
			Stack = new Stack<PredicateValue> (2);
		}

		public void Push (PredicateValue value)
		{
			if (Stack.Count == 2)
				throw new StackOverflowException (
					string.Format ("Stackoverflow in Predicate: '{0}'", GetType ()));

			Stack.Push (value);
		}

		public abstract PredicateType Type {get;}
		public abstract bool GetValue ();
	}
}

