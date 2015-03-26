using System;
using System.Collections.Generic;
using ObviousCode.Alchemy.Library.Populous;

namespace ObviousCode.Alchemy.Creatures.Darwin
{
	public class AfterSelectionStateEventArgs : EventArgs
	{
		public AfterSelectionStateEventArgs ()
		{
			Selection = new List<Individual<byte>> ();
			LastPopulation = new Dictionary<string, Creature> ();
		}

		public List<Individual<byte>> Selection {
			get;
			set;
		}

		public Dictionary<string, Creature> LastPopulation {
			get;
			set;
		}
	}
}

