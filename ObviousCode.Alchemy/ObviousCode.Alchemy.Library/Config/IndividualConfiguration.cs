using System;

namespace ObviousCode.Alchemy.Library
{
	/// <summary>
	/// Individual configuration options
	/// </summary>
	public class IndividualConfiguration
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ObviousCode.Alchemy.Library.IndividualConfiguration"/> class.
		/// 
		/// Default Values:
		/// 
		/// DefaultGenomeSize = 128
		/// </summary>
		public IndividualConfiguration ()
		{
			DefaultGenomeSize = 128;
		}

		/// <summary>
		/// Gets or sets the default size of the genome.
		/// </summary>
		/// <value>The number of genes in an individual.</value>
		public int DefaultGenomeSize {
			get;
			set;
		}
	}
}

