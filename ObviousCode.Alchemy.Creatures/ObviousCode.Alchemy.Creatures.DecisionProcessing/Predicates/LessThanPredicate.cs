using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class LessThanPredicate : Predicate
	{
		public override bool GetValue ()
		{
			return Stack.Pop ().GetValue () < Stack.Pop ().GetValue ();
		}
	}
}

