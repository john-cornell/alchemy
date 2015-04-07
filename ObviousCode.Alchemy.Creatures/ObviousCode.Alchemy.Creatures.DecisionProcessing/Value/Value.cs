using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public sealed class Value : PredicateValue
	{
		public string Description { get; private set; }

		decimal _value;

		public Value (decimal value, string description)
		{
			_value = value;
			Description = description;
		}

		public override decimal GetValue ()
		{
			return _value;
		}

		public override string Describe ()
		{
			return string.Format ("{0} ({1})", Description, _value.ToString ());
		}
	}
}

