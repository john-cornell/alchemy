using System;

namespace ObviousCode.Alchemy.Library
{
	public class DoubleBasedGenomeInitialiserContext : InitialiserContext<double>
	{
		public DoubleBasedGenomeInitialiserContext ()
		{
			Min = 0d;
			Min = 1d;
		}

		public double Min { get; set; }
		public double Max { get; set; }
	}
}

