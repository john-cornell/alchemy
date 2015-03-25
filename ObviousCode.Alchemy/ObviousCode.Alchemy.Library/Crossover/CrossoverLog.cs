using System;
using System.Collections.Generic;
using System.Linq;

namespace ObviousCode.Alchemy.Library
{
	public class CrossoverLog
	{
		public List<CrossoverLogEntry> Entries { get; private set; }

		public CrossoverLog ()
		{
			Entries = new List<CrossoverLogEntry> ();
		}

		public CrossoverLogEntry GetNextEntry(Crossover.CrossoverType method)
		{
			switch (method) {

			case Crossover.CrossoverType.OnePoint:
				Entries.Add (new OnePointCrossoverLogEntry ());
				break;
			
			case Crossover.CrossoverType.TwoPoint:
				Entries.Add (new TwoPointCrossoverLogEntry ());
				break;

			case Crossover.CrossoverType.Random:
				Entries.Add (new RandomCrossoverLogEntry ());
				break;

			default:
				throw new NotImplementedException ();
			}

			return Entries.Last ();
		}

		public void Clear()
		{
			Entries.Clear ();
		}
	}
}

