using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class Predicate
	{
		double? _value1;
		double? _value2;

		public enum PredicateType { Comparison }
		public PredicateType TypeOfPredicate { get; private set; }

		public Predicate (PredicateType predicateType) 
		{
			TypeOfPredicate = predicateType;
		}

		public int Push(int value)
		{
			Push ((double)value);
		}

		public int Push(double value)
		{
			if (!_value1.HasValue)
				_value1 = value;
			else if (!_value2.HasValue)
				_value2 = value;
			else throw new StackOverflowException(
				string.Format("Predicate {0} Stack is full");
		}
	}
}

