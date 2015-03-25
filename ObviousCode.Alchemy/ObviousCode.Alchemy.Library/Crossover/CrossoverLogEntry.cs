using System;

namespace ObviousCode.Alchemy.Library
{
	public abstract class CrossoverLogEntry
	{
		public CrossoverLogEntry (Crossover.CrossoverType method)
		{
			Method = method;
		}			

		public DateTime TimeOfEntry {
			get;
			set;
		}

		public Crossover.CrossoverType Method {get;private set;} 
	}

	public class OnePointCrossoverLogEntry : CrossoverLogEntry
	{
		public OnePointCrossoverLogEntry () : base(Crossover.CrossoverType.OnePoint)
		{
			
		}

		public int CrossoverPoint {
			get;
			set;
		}
	}

	public class TwoPointCrossoverLogEntry : CrossoverLogEntry
	{
		public TwoPointCrossoverLogEntry () : base(Crossover.CrossoverType.TwoPoint)
		{
			
		}

		public int CrossoverPointStart {
			get;
			set;
		}

		public int CrossoverPointFinish {
			get;
			set;
		}
	}

	public class RandomCrossoverLogEntry : CrossoverLogEntry
	{
		public RandomCrossoverLogEntry () : base(Crossover.CrossoverType.Random)
		{
			
		}
	}
}

