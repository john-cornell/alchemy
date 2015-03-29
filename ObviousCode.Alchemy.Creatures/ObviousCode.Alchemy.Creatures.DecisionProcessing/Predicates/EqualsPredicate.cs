﻿using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class EqualsPredicate : Predicate
	{
		public override bool GetValue ()
		{
			// Analysis disable once EqualExpressionComparison
			return Stack.Pop ().GetValue ().Equals (Stack.Pop ().GetValue ());
		}
	}
}
