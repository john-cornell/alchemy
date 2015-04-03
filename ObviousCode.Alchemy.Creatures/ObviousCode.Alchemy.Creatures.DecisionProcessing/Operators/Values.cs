using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class Values
	{
		const double OperatorChance = .5d;

		List<Value> _constantValues;
		List<Value> _transientValues;

		public Values ()
		{
			_constantValues = new List<Value> ();
			_transientValues = new List<Value> ();
		}

		public PredicateValue GetValue (Random random)
		{			
			List<Value> available = new List<Value> (_constantValues);

			available.AddRange (_transientValues);
		
			if (available.Count == 0)
				throw new InvalidOperationException ("No values are available");

			ValueProvider provider = new ValueProvider ();

			return provider.GetValue (available, random, OperatorChance);
		}

		public void ClearTransientValues ()
		{
			_transientValues.Clear ();
		}

		public void AddConstant (Value value)
		{
			_constantValues.Add (value);
		}

		public void AddTransient (Value value)
		{			
			_transientValues.Add (value);
		}
	}
}

