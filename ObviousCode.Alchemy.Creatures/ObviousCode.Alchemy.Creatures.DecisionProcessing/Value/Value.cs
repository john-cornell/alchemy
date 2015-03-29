using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class Value : PredicateValue
	{
		double _value;

		public Value (double value)
		{
			_value = value;
		}

		public override double GetValue ()
		{
			return _value;
		}
	}
}

