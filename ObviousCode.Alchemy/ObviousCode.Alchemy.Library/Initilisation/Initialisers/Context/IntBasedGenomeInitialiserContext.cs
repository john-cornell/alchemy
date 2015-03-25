using System;

namespace ObviousCode.Alchemy.Library
{
	public class IntBasedGenomeInitialiserContext : InitialiserContext<int>
	{
		public int Min { get; set; }
		public int Max { get; set; }

		public IntBasedGenomeInitialiserContext ()
		{
			Min = 0;
			Max = 256;
		}	

		protected override void BeforeSerialisation ()
		{
			int min = Math.Min (Min, Max);

			Max = Math.Max (Min, Max);
			Min = min;
		}
	}
}

