using System;

namespace ObviousCode.Alchemy.Library
{
	/// <summary>
	/// Crossover configuration options
	/// </summary>
	public class CrossoverConfiguration
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ObviousCode.Alchemy.Library.CrossoverConfiguration"/> class.
		/// 
		/// Default Values:
		/// 
		/// All Crossover probability ratios = 1
		/// Chance of Random Gene crossover (when random crossover strategy is used) = .5d 
		/// </summary>
		public CrossoverConfiguration ()
		{
			_onePointRatio = 1;
			_twoPointRatio = 1;
			_randomRatio = 1;

			ChanceOfRandomGeneCrossover = .5d;
		}	
					
		double _chanceOfRandomGeneCrossover;

		/// <summary>
		/// Gets or sets the chance of random gene crossover.
		/// 
		/// When Random Crossover strategy used, the chance that a crossover of genes will occur.
		/// 
		/// Child is automatically loaded with parent1 genes, then this will be the chance (0..1) of a crossover with a given gene of parent 2
		/// 
		/// Default .5d (50 / 50 for each gene to be of either parent)
		/// </summary>
		/// <value>The chance of random gene crossover.</value>
		public double ChanceOfRandomGeneCrossover {
			get { return _chanceOfRandomGeneCrossover; }
			set {
				if (value < 0 || value > 1)
					throw new InvalidOperationException ("ChanceOfRandomGeneCrossover must be between 0 and 1");

				_chanceOfRandomGeneCrossover = value;
			}
		}			
	
		public int _onePointRatio;
		/// <summary>
		/// Gets or Sets the onepoint-crossover ratio
		/// 
		/// Crossover ratios are relative likeliness that a given Crossover strategy 
		/// will be used when an Individual is mating with another
		/// 
		/// If TwoPointCrossoverRatio and RandomCrossoverRatio are both 1 
		/// and OnePointCrossoverRatio is 2, then, on average, OnePointCrossover
		/// will be twice as likely as either of the other (or just as likely as one of the other)
		/// 
		/// If all ratios are set to 0, they will be treated as just as likely
		/// 
		/// If one or two ratios are set to 0, they will not be used
		/// 
		/// All Crossover Ratios default to 1
		/// </summary>
		/// <value>The one point crossover ratio.</value>
		public int OnePointCrossoverRatio {
			get { return _onePointRatio + _twoPointRatio + _randomRatio == 0 ? 1 : _onePointRatio; }
			set { _onePointRatio = value; 
				CrossoverProvider.RebuildSelectionArray ();
			}
		}

		int _twoPointRatio;
		/// <summary>
		/// Gets or sets the twopoint-crossover ratio.
		/// 
		/// Crossover ratios are relative likeliness that a given Crossover strategy 
		/// will be used when an Individual is mating with another
		/// 
		/// If OnePointCrossoverRatio and RandomCrossoverRatio are both 1 
		/// and TwoPointCrossoverRatio is 2, then, on average, TwoPointCrossover
		/// will be twice as likely as either of the other (or just as likely as one of the other)
		/// 
		/// If all ratios are set to 0, they will be treated as just as likely
		/// 
		/// If one or two ratios are set to 0, they will not be used
		/// All Crossover Ratios default to 1
		/// </summary>
		/// <value>The two point crossover ratio.</value>
		public int TwoPointCrossoverRatio {
			get { return _onePointRatio + _twoPointRatio + _randomRatio == 0 ? 1 : _twoPointRatio; }
			set { _twoPointRatio = value; 
				CrossoverProvider.RebuildSelectionArray ();
			}
		}

		int _randomRatio;
		/// <summary>
		/// Gets or sets the random crossover ratio.
		///
		/// Crossover ratios are relative likeliness that a given Crossover strategy 
		/// will be used when an Individual is mating with another
		/// 
		/// If OnePointCrossoverRatio and TwoPointCrossoverRatio are both 1 
		/// and RandomCrossoverRatio is 2, then, on average, RandomCrossover
		/// will be twice as likely as either of the other (or just as likely as one of the other)
		/// 
		/// If all ratios are set to 0, they will be treated as just as likely
		/// 
		/// If one or two ratios are set to 0, they will not be used
		/// All Crossover Ratios default to 1
		/// </summary>
		/// <value>The random crossover ratio.</value>
		public int RandomCrossoverRatio {
			get { return _onePointRatio + _twoPointRatio + _randomRatio == 0 ? 1 : _randomRatio; }
			set { _randomRatio = value; 
				CrossoverProvider.RebuildSelectionArray ();
			}
		}
	}
}

