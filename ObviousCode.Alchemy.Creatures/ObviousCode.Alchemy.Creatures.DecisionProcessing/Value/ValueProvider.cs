using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class ValueProvider
	{
		List<OperatorValue> _operators;

		public ValueProvider ()
		{
			_operators = new List<OperatorValue> ();

			_operators.Add (new AdditionOperator ());
			_operators.Add (new SubtractionOperator ());
			_operators.Add (new MultiplicationOperator ());
			_operators.Add (new DivisionOperator ());
		}

		public PredicateValue GetValue (List<Value> availableValues, Random random, double operatorChance)
		{			
			bool operatorValue = random.NextDouble () <= operatorChance;

			operatorChance = operatorChance * .9;

			if (operatorValue) {
				return BuildOperatorValue (availableValues, random, operatorChance);
			} else {
				return availableValues [random.Next () % availableValues.Count];
			}
		}

		PredicateValue BuildOperatorValue (List<Value> availableValues, Random random, double operatorChance)
		{
			ValueProvider valueProvider = new ValueProvider ();

			OperatorValue operatorValue = _operators [random.Next () % _operators.Count].CreateNew ();

			operatorValue.Push (valueProvider.GetValue (availableValues, random, operatorChance));
			operatorValue.Push (valueProvider.GetValue (availableValues, random, operatorChance));

			return operatorValue;
		}
	}
}

