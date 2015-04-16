using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class EqualsPredicate : Predicate
	{
		public override bool GetValue ()
		{
			// Analysis disable once EqualExpressionComparison
			return Stack.Pop ().GetValue ().Equals (Stack.Pop ().GetValue ());
		}

		public override Predicate CreateNew ()
		{
			return new EqualsPredicate ();
		}

		public override PredicateType Type {
			get {
				return Predicate.PredicateType.Equals;
			}
		}

		public override string Describe ()
		{
			return string.Format ("( {0} == {1} )", Stack [0].Describe (), Stack [1].Describe ());
		}
	}
}

