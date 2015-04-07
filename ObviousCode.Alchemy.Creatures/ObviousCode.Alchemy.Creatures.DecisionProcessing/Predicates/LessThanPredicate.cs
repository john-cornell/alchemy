using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class LessThanPredicate : Predicate
	{
		public override bool GetValue ()
		{
			return Stack.Pop ().GetValue () < Stack.Pop ().GetValue ();
		}

		public override string Describe ()
		{
			return string.Format (" ( {0} < {1} ) "
								, Stack [1].Describe ()
								, Stack [0].Describe ());
		}

		public override PredicateType Type {
			get {
				return Predicate.PredicateType.LT;
			}
		}
	}
}

