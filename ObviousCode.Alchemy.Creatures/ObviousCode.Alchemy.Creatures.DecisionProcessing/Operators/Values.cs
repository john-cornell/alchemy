using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class Values
	{
		List<PredicateValue> _constantValues;
		List<PredicateValue> _transientValues;

		public Values ()
		{
			_constantValues = new List<PredicateValue> ();
			_transientValues = new List<PredicateValue> ();
		}

		public PredicateValue GetValue (int index)
		{
			List<PredicateValue> available = new List<PredicateValue> (
				                                 _constantValues);

			available.AddRange (_transientValues);

			index = Math.Abs (index) % available.Count;

			return available [index];
		}

		public void ClearTransientValues ()
		{
			_transientValues.Clear ();
		}

		public void AddConstant (PredicateValue value)
		{
			_constantValues.AddRange (_transientValues);
		}

		public void AddTransient (PredicateValue value)
		{			
			_transientValues.Add (value);
		}
	}
}

