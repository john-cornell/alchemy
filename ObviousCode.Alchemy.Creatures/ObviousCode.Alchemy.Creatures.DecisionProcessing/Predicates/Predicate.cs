using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public abstract class Predicate
	{
		public enum PredicateType
		{
			Equals,
			False,
			GT,
			LT,
			True

		}

		public StackArray<PredicateValue> Stack { get; private set; }

		public int Seed { get; set; }

		protected Predicate ()
		{
			Stack = new StackArray<PredicateValue> (2);
		}

		public void Push (PredicateValue value)
		{
			Stack.Push (value);
		}

		public abstract PredicateType Type { get; }

		public abstract bool GetValue ();

		public abstract string Describe ();
	}
}

