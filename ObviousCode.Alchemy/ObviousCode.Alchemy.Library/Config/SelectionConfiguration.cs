using System;

namespace ObviousCode.Alchemy.Library
{
	public enum SelectionType { Tournament, Truncation, Roulette, Custom }

	public class SelectionConfiguration
	{	
		public SelectionConfiguration ()
		{
			SelectionMethod = SelectionType.Tournament;

			NumberOfFittestSelected = 2;

			RatioOfFittestInTruncationSelection = .25m;

			RouletteFitnessSortDirection = RouletteFitnessDirection.Ascending;
		}
			
		public enum RouletteFitnessDirection
		{
			Ascending, Descending
		}

		/// <summary>
		/// Sort direction of list, based on fitness, during roulette selection
		/// Defaults to Ascending
		/// </summary>
		/// <value>The roulette fitness direction.</value>
		public RouletteFitnessDirection RouletteFitnessSortDirection {
			get;
			set;
		}

		/// <summary>
		/// Largely implemented for test purposes, little point in having this set otherwise
		/// </summary>
		/// <value>The forced roulette threshold.</value>
		public double? ForcedRouletteThreshold {
			get;
			set;
		}

		public SelectionType SelectionMethod {
			get;
			set;
		}

		int _numberOfFittestSelected;
		/// <summary>
		/// Number of fittest individuals to be selected in non-ratio (Tournament, Roulette and possibly Custom) selections
		/// </summary>
		/// <value>The number of fittest selected.</value>
		public int NumberOfFittestSelected {
			get{ return _numberOfFittestSelected; }
			set {
				if (value < 2)
					throw new InvalidOperationException ("At least 2 individuals must be selected as fittest from each generation");

				_numberOfFittestSelected = value;
			}
		}

		public decimal _ratioOfFittestInTruncationSelection;

		/// <summary>
		/// Ratio of fittest individuals to be selected in ratio (Truncation and possibly Custom) selections
		/// </summary>
		/// <value>The ratio of fittest in truncation selection.</value>
		public decimal RatioOfFittestInTruncationSelection {
			get { return _ratioOfFittestInTruncationSelection; }
			set {
				if (value >= 1m || value <= 0m) {
					throw new InvalidOperationException ("Truncation Ratio must be greater than 0, and less than 1");
				}

				_ratioOfFittestInTruncationSelection = value;
			}
		}			
	}
}

