using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class Decisions
	{
		public enum Outcome
		{
			True,
			False,
			Die
		}

		Random _random;
		Values _values;
		int _cursor;

		public List<Predicate> DecisionProviders { get; private set; }

		byte[] _code;

		public Decisions (byte[] code, byte seed, int decisionStartPosition)
		{
			_random = new Random ((int)seed);
			_values = new Values ();
			_cursor = decisionStartPosition % code.Length;
			_code = code;

			DecisionProviders = new List<Predicate> ();

			LoadPredicates ();
		}

		void LoadPredicates ()
		{
			int positionOfPredicateCount = _code [_cursor] % _code.Length;

			IncrementCursor ();

			for (int i = 0; i < _code [positionOfPredicateCount]; i++) {
				
				Predicate predicate = PredicateProvider.GetPredicate (_random.Next ());
				predicate.Seed = _random.Next ();

				DecisionProviders.Add (predicate);
			}
		}

		public Outcome GetDecision (int predicateIndex)
		{
			try {
						
				Predicate predicate = DecisionProviders [predicateIndex % DecisionProviders.Count];

				Random random = new Random (predicate.Seed);

				predicate.Push (_values.GetValue (random.Next ()));
				predicate.Push (_values.GetValue (random.Next ()));

				return predicate.GetValue () ? Outcome.True : Outcome.False;

			} catch {
				return Outcome.Die;
			}
		}

		public void LoadConstantValue (PredicateValue value)
		{
			_values.AddConstant (value);
		}

		public void LoadTransientValue (PredicateValue value)
		{
			_values.AddTransient (value);
		}

		public void ClearTransientValues ()
		{
			_values.ClearTransientValues ();
		}

		void IncrementCursor ()
		{
			_cursor = (_cursor + 1) % _code.Length;
		}

	}
}

