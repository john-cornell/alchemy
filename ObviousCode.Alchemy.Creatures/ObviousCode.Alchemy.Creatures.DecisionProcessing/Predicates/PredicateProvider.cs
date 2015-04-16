using System;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class PredicateProvider
	{
		static Predicate[] _predicates;

		static PredicateProvider ()
		{
			_predicates = new Predicate[] {
				new EqualsPredicate (),
				new FalsePredicate (),
				new LessThanPredicate (),
				new GreaterThanPredicate (),
				new TruePredicate ()
			};
		}

		public static Predicate GetPredicate (int index)
		{
			index = Math.Abs (index) % _predicates.Length;//avoid index out of range

			return _predicates [index].CreateNew ();
		}
	}
}

