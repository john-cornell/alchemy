using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class Value : PredicateValue
	{
		decimal _value;

		public Value (decimal value)
		{
			_value = value;
		}

		public override decimal GetValue ()
		{
			return _value;
		}
	}
}

