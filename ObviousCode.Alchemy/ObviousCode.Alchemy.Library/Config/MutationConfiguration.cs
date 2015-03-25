using System;

namespace ObviousCode.Alchemy.Library
{
	/// <summary>
	/// Mutation configuration options.
	/// </summary>
	public class MutationConfiguration
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ObviousCode.Alchemy.Library.MutationConfiguration"/> class.
		/// 
		/// Default Values:
		/// 
		/// ChanceOfMutation = 0.025d;
		/// MutationMethod = MutationStyle.Full;
		/// ContextMutationVariance = .1d;
		/// AllowUncheckedVariance = false;
		/// </summary>
		public MutationConfiguration ()
		{
			Reset ();
		}

		void Reset ()
		{
			ChanceOfMutation = 0.025d;
			MutationMethod = MutationStyle.Full;
			ContextMutationVariance = .1d;
			AllowUncheckedVariance = false;
		}
			
		/// <summary>
		/// Between 0 and 1, chance that any individual 'gene' is mutated. 
		/// Default = 0.025 (2.5%)
		/// </summary>
		/// <value>The chance of mutation.</value>
		public double ChanceOfMutation { get; set; }

		/// <summary>
		/// Mutation style.
		/// 
		/// Full mutation selects a random value for mutated gene
		/// Variance mutation bases mutation on current value of gene
		/// 
		/// </summary>
		public enum MutationStyle { Full, Variance }

		/// <summary>
		/// Gets or sets the mutation method.
		/// 
		/// Full mutation selects a random value for mutated gene
		/// Variance mutation bases mutation on current value of gene
		/// 
		/// Default = Full
		/// </summary>
		/// <value>The mutation method.</value>
		public MutationStyle MutationMethod { get; set; }

		double _contextMutationVariance;

		/// <summary>
		/// Upper bounds of range variance based mutation will shift from original value
		/// 
		/// This may be implemented differently for different Genome data types
		/// 
		/// Default = .1 (10%)
		/// </summary>
		/// <value>The context mutation weighting.</value>
		public double ContextMutationVariance 
		{ 
			get { return _contextMutationVariance; }
			set { 
				if (value <= 0d)
					throw new InvalidOperationException ("ContextMutationVariance must be greater than 0");
				if (value >= 1)
					throw new InvalidOperationException ("ContextMutationVariance must be less than 1.");

				_contextMutationVariance = value;
			}
		}

		/// <summary>
		/// Applied to  Mutation Method = Variance
		/// If true, when mutation sets value less than min value, allow value to 'overflow' to max value (or max when less than min)
		/// If false, floor at 0 and ceiling at max 
		/// Default = false - by default all values will floor or ceiling during variance if they hit a min or max
		/// </summary>
		/// <value><c>true</c> if allow variance overflow; otherwise, <c>false</c>.</value>
		public bool AllowUncheckedVariance {
			get;
			set;
		}
	}
}

