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

		public override string Describe ()
		{
			return string.Format (" ( {0} > {1} ) "
								, Stack [1].Describe ()
								, Stack [0].Describe ());
		}

		public override Predicate CreateNew ()
		{
			return new GreaterThanPredicate ();
		}

		public override PredicateType Type {
			get {
				return Predicate.PredicateType.GT;
			}
		}
	}
}

