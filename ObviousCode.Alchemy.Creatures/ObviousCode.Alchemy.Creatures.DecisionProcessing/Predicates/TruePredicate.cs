using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class TruePredicate : Predicate
	{
		public override bool GetValue ()
		{
			return true;
		}
	}
}

