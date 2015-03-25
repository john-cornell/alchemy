using System;

namespace ObviousCode.Alchemy.Library
{
	public class ByteBasedGenomeInitialiserContext : InitialiserContext<byte>
	{
		public byte Min { get; set; }
		public byte Max { get; set; }

		/// <summary>
		/// If AllowUnchecked, value can 'overflow' or 'underflow' from 0 to 255 or 255 to 0 when using Variance Mutation
		/// If AllowUnchecked = false, value will floor at 0 
		/// </summary>
		/// <value><c>true</c> if allow unchecked; otherwise, <c>false</c>.</value>
		public bool AllowUnchecked { get; set; }

		public ByteBasedGenomeInitialiserContext ()
		{
			Min = (byte) 0;
			Max = (byte) 255;

			AllowUnchecked = false;
		}	

		protected override void BeforeSerialisation ()
		{
			byte min = Math.Min (Min, Max);

			Max = Math.Max (Min, Max);
			Min = min;
		}
	}
}

