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
		readonly Values _values;

		public List<Predicate> DecisionProviders { get; private set; }

		public Decisions (byte seed, int predicateCount)
		{
			_random = new Random ((int)seed);
			_values = new Values ();


			DecisionProviders = new List<Predicate> ();

			LoadPredicates (predicateCount);
		}

		void LoadPredicates (int predicateCount)
		{
			for (int i = 0; i < predicateCount; i++) {
				
				Predicate predicate = PredicateProvider.GetPredicate (_random.Next ());
				predicate.Seed = _random.Next ();

				DecisionProviders.Add (predicate);
			}
		}

		public Outcome GetDecision (int predicateIndex)
		{
			Predicate predicate = null;

			try {
						
				predicate = DecisionProviders [predicateIndex % DecisionProviders.Count];						

				Random random = new Random (predicate.Seed);
				
				predicate.Push (_values.GetValue (random));
				predicate.Push (_values.GetValue (random));

				return predicate.GetValue () ? Outcome.True : Outcome.False;

			} catch (Exception e) {
				return Outcome.Die;
			} finally {		
				if (predicate != null)
					predicate.Stack.Clear ();
			}
		}

		public void LoadConstantValue (Value value)
		{
			_values.AddConstant (value);
		}

		public void LoadTransientValue (Value value)
		{
			_values.AddTransient (value);
		}

		public void ClearTransientValues ()
		{
			_values.ClearTransientValues ();
		}
	}
}

