using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class GreaterThanPredicate : Predicate
	{
		public override bool GetValue ()
		{
			// Analysis disable once EqualExpressionComparison
			return Stack.Pop ().GetValue () > Stack.Pop ().GetValue ();
		}



		public override PredicateType Type {
			get {
				return Predicate.PredicateType.GT;
				}
		}
	}
}

