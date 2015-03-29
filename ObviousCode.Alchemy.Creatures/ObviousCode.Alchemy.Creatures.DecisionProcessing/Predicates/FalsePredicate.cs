using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class FalsePredicate : Predicate
	{
		public override bool GetValue ()
		{
			return false;
		}
	}
}

