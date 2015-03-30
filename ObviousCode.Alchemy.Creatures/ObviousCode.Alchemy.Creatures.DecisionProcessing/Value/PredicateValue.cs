using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public abstract class PredicateValue
	{
		protected PredicateValue ()
		{
			
		}

		public abstract decimal GetValue ();
	}
}

