using System;
using NUnit.Framework;
using ObviousCode.Alchemy.Library;
using ObviousCode.Alchemy.Library.Populous;
using System.Collections.Generic;
using System.Linq;

namespace ObviousCode.Alchemy.Tests
{
	[TestFixture]
	public class SelectionTests
	{
		public SelectionTests ()
		{
		}

		[SetUp]
		public void TestSetup()
		{
			SelectionProvider.Reset ();
			ConfigurationProvider.Reset ();
		}

		[Test]
		public void WhenSelectorIsRequested_Tournament_CorrectSelectorShouldBeReturned()
		{
			Selector selector = SelectionProvider.GetSelector (SelectionType.Tournament);

			Assert.AreEqual (SelectionType.Tournament, selector.SelectionMethod);
		}

		[Test]
		public void WhenSelectorIsRequested_Truncation_CorrectSelectorShouldBeReturned()
		{
			Selector selector = SelectionProvider.GetSelector (SelectionType.Truncation);

			Assert.AreEqual (SelectionType.Truncation, selector.SelectionMethod);
		}

		[Test]
		public void WhenSelectorIsRequested_Roulette_CorrectSelectorShouldBeReturned()
		{
			Selector selector = SelectionProvider.GetSelector (SelectionType.Roulette);

			Assert.AreEqual (SelectionType.Roulette, selector.SelectionMethod);
		}

		[Test]
		[ExpectedException(typeof(SelectorNotAvailableException))]
		public void WhenSelectorIsRequested_Custom_NotSet_SelectorNotAvailableExceptionShouldBeThrown()
		{
			Selector selector = SelectionProvider.GetSelector (SelectionType.Custom);

			Assert.AreEqual (SelectionType.Custom, selector.SelectionMethod);
		}

		[Test]
		public void WhenSelectorIsRequested_Tournament_CorrectSelectionShouldBeReturned()
		{
			TournamentSelector selector = new TournamentSelector ();

			Selector requested = SelectionProvider.GetSelector (SelectionType.Tournament);

			Assert.AreEqual (selector.Name, requested.Name);
		}
			
		[Test]
		public void WhenSelectorIsRequested_Tournament_AlteredConfiguration_CorrectSelectionShouldBeReturned()
		{
			Population<int> population = new Population<int> ();

			Selector selector = SelectionProvider.GetSelector (SelectionType.Tournament);

			ConfigurationProvider.Selection.NumberOfFittestSelected = 3;

			population.Initialise (5, 5);

			for (int i = 0; i < population.Individuals.Count; i++) {
				for (int j = 0; j < population.Individuals [0].GenomeLength; j++) {
					population.Individuals [i].Code [j] = i;
				}
			}

			List<Individual<int>> fittest = selector.SelectFittest (population, new double[] { .4d, .85d, .3d, .9d, .81d }).ToList ();

			Assert.AreEqual (fittest.Select (f => f.Code), 
				new List<List<int>> {
					new List<int>{ 3, 3, 3, 3, 3 },
					new List<int>{ 1, 1, 1, 1, 1 },
					new List<int>{ 4, 4, 4, 4, 4 }
				});
		}

		[Test]
		public void WhenSelectorIsRequested_Tournament_AlteredConfiguration_CorrectSelectionFitnessShouldBeReturned()
		{
			Population<int> population = new Population<int> ();

			Selector selector = SelectionProvider.GetSelector (SelectionType.Tournament);

			ConfigurationProvider.Selection.NumberOfFittestSelected = 3;

			population.Initialise (5, 100);

			List<Individual<int>> fittest = selector.SelectFittest (population, new double[] { .4d, .85d, .3d, .9d, .81d }).ToList ();

			Assert.AreEqual (fittest.Select(f=>f.Fitness).ToList(),
				new List<double> {
					.9d, .85d, .81d
				});
		}

		[Test]
		public void WhenSelectorIsRequested_Tournament_AlteredConfiguration_CorrectSelectionCountShouldBeReturned()
		{
			Population<int> population = new Population<int> ();

			Selector selector = SelectionProvider.GetSelector (SelectionType.Tournament);

			ConfigurationProvider.Selection.NumberOfFittestSelected = 3;

			population.Initialise (5, 100);

			List<Individual<int>> fittest = selector.SelectFittest (population, new double[] { .4d, .85d, .3d, .9, .81 }).ToList ();

			Assert.AreEqual (3, fittest.Count);
		}

		[Test]
		public void WhenSelectorIsRequested_Truncation_AlteredConfiguration_CorrectSelectionShouldBeReturned()
		{
			Population<int> population = new Population<int> ();

			Selector selector = SelectionProvider.GetSelector (SelectionType.Truncation);

			ConfigurationProvider.Selection.RatioOfFittestInTruncationSelection = .2m;

			population.Initialise (10, 3);

			for (int i = 0; i < population.Individuals.Count; i++) {
				for (int j = 0; j < population.Individuals [0].GenomeLength; j++) {
					population.Individuals [i].Code [j] = i - j;
				}
			}

			List<Individual<int>> fittest = selector.SelectFittest (population, 
				                                new double[] { .1d, .15d, .2d, .5d, .25d, .3d, .35d, .4d, .45d,  .55d }).ToList ();

			Assert.AreEqual (
				fittest.Select (f => f.Code).ToList (),
				new List<List<int>> {
					new List<int>{ 9, 8, 7 },
					new List<int>{ 3, 2, 1 }
				});
		}			

		[Test]
		public void WhenSelectorIsRequested_Truncation_AlteredConfiguration_CorrectSelectionFitnessShouldBeReturned()
		{
			Population<int> population = new Population<int> ();

			Selector selector = SelectionProvider.GetSelector (SelectionType.Truncation);

			ConfigurationProvider.Selection.RatioOfFittestInTruncationSelection = .2m;

			population.Initialise (10, 3);

			List<Individual<int>> fittest = selector.SelectFittest (population, 
				                                new double[] { .1d, .15d, .2d, .5d, .25d, .3d, .35d, .4d, .45d,  .55d }).ToList ();

			Assert.AreEqual (
				fittest.Select (f => f.Fitness).ToList (),
				new List<double>{ .55d, .5d });
		}

		[Test]
		public void WhenSelectorIsRequested_Truncation_AlteredConfiguration_CorrectSelectionCountShouldBeReturned()
		{
			Population<int> population = new Population<int> ();

			Selector selector = SelectionProvider.GetSelector (SelectionType.Truncation);

			ConfigurationProvider.Selection.RatioOfFittestInTruncationSelection = .2m;

			population.Initialise (10, 3);

			List<Individual<int>> fittest = selector.SelectFittest (population, 
				new double[] { .1d, .15d, .2d, .25d, .3d, .35d, .4d, .45d, .5d, .55d}).ToList ();

			Assert.AreEqual (2, fittest.Count);
		}

		[Test]
		public void WhenSelectorIsRequested_Roulette_MaxThresholdCalculatedShouldBeCorrect()
		{
			Population<int> population = new Population<int> ();

			Selector selector = SelectionProvider.GetSelector (SelectionType.Roulette);

			population.Initialise (9, 10);

			selector.SelectFittest (population, 
				                                new double[] { .11d, .22d, .33d, .44d, .55d, .66d, .77d, .88d, .99d }).ToList ();

			Assert.AreEqual (Math.Round(4.95d, 2), Math.Round((selector as RouletteSelector).LastMaxThreshold, 2));
		}

		[Test]
		public void WhenSelectorIsRequested_Roulette_AlteredConfiguration_CorrectSelectionShouldBeReturned()
		{
			Population<int> population = new Population<int> ();

			Selector selector = SelectionProvider.GetSelector (SelectionType.Roulette);

			population.Initialise (9, 10);

			for (int i = 0; i < population.Individuals.Count; i++) {
				for (int j = 0; j < population.Individuals [0].GenomeLength; j++) {
					population.Individuals [i].Code [j] = i;
				}
			}

			ConfigurationProvider.Selection.ForcedRouletteThreshold = 1.2d;
			ConfigurationProvider.Selection.NumberOfFittestSelected = 4;

			List<Individual<int>> fittest = selector.SelectFittest (population, 
				new double[] { .11d, .22d, .33d, .44d, .55d, .66d, .77d, .88d, .99d }).ToList ();

			Assert.AreEqual (
				new List<List<int>> {
					new List<int> {4,4,4,4,4,4,4,4,4,4}, 
					new List<int> {5,5,5,5,5,5,5,5,5,5},
					new List<int> {6,6,6,6,6,6,6,6,6,6},
					new List<int> {7,7,7,7,7,7,7,7,7,7}
				},
				fittest.Select (f => f.Code).ToList ());

		}

		[Test]
		public void WhenSelectorIsRequested_Roulette_AlteredConfiguration_CorrectSelectionCountShouldBeReturned()
		{
			Population<int> population = new Population<int> ();

			Selector selector = SelectionProvider.GetSelector (SelectionType.Roulette);

			population.Initialise (9, 10);

			for (int i = 0; i < population.Individuals.Count; i++) {
				for (int j = 0; j < population.Individuals [0].GenomeLength; j++) {
					population.Individuals [i].Code [j] = i;
				}
			}

			ConfigurationProvider.Selection.ForcedRouletteThreshold = 1.2d;
			ConfigurationProvider.Selection.NumberOfFittestSelected = 4;

			List<Individual<int>> fittest = selector.SelectFittest (population, 
				new double[] { .11d, .22d, .33d, .44d, .55d, .66d, .77d, .88d, .99d }).ToList ();

			Assert.AreEqual (4, fittest.Count);
		}

		[Test]
		public void WhenSelectorIsRequested_Roulette_AlteredConfiguration_CorrectSelectionFitnessShouldBeReturned()
		{
			Population<int> population = new Population<int> ();

			Selector selector = SelectionProvider.GetSelector (SelectionType.Roulette);

			population.Initialise (9, 10);

			for (int i = 0; i < population.Individuals.Count; i++) {
				for (int j = 0; j < population.Individuals [0].GenomeLength; j++) {
					population.Individuals [i].Code [j] = i + j;
				}
			}

			ConfigurationProvider.Selection.ForcedRouletteThreshold = 1.2d;
			ConfigurationProvider.Selection.NumberOfFittestSelected = 4;

			List<Individual<int>> fittest = selector.SelectFittest (population, 
				                                new double[] { .11d, .22d, .33d, .44d, .55d, .66d, .77d, .88d, .99d }).ToList ();

			Assert.AreEqual (
				new List<double> {
					Math.Round (.55d, 2), 
					Math.Round (.66d, 2),
					Math.Round (.77d, 2),
					Math.Round (.88d, 2)
				},
				fittest.Select (f => Math.Round (f.Fitness.Value, 2)).ToList ());
		}

		[Test]
		public void WhenSelectorIsRequested_Roulette_AlteredConfiguration_SortDirectoryDescending_CorrectSelectionShouldBeReturned()
		{
			Population<int> population = new Population<int> ();

			Selector selector = SelectionProvider.GetSelector (SelectionType.Roulette);

			population.Initialise (9, 10);

			for (int i = 0; i < population.Individuals.Count; i++) {
				for (int j = 0; j < population.Individuals [0].GenomeLength; j++) {
					population.Individuals [i].Code [j] = i;
				}
			}

			ConfigurationProvider.Selection.ForcedRouletteThreshold = 1.2d;
			ConfigurationProvider.Selection.NumberOfFittestSelected = 4;
			ConfigurationProvider.Selection.RouletteFitnessSortDirection = SelectionConfiguration.RouletteFitnessDirection.Descending;

			List<Individual<int>> fittest = selector.SelectFittest (population, 
				                                new double[] { .11d, .22d, .33d, .44d, .55d, .66d, .77d, .88d, .99d }).ToList ();

			Assert.AreEqual (
				new List<List<int>> {
					new List<int> { 7, 7, 7, 7, 7, 7, 7, 7, 7, 7 }, 				
					new List<int> { 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 },
					new List<int> { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 },
					new List<int> { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 }
				},
				fittest.Select (f => f.Code).ToList ());
		}

		[Test]
		public void WhenSelectorIsRequested_Roulette_AlteredConfiguration_SortDirectoryDescending_CorrectSelectionFitnessShouldBeReturned()
		{
			Population<int> population = new Population<int> ();

			Selector selector = SelectionProvider.GetSelector (SelectionType.Roulette);

			population.Initialise (9, 10);

			for (int i = 0; i < population.Individuals.Count; i++) {
				for (int j = 0; j < population.Individuals [0].GenomeLength; j++) {
					population.Individuals [i].Code [j] = i + j;
				}
			}

			ConfigurationProvider.Selection.ForcedRouletteThreshold = 1.2d;
			ConfigurationProvider.Selection.NumberOfFittestSelected = 4;
			ConfigurationProvider.Selection.RouletteFitnessSortDirection = SelectionConfiguration.RouletteFitnessDirection.Descending;

			List<Individual<int>> fittest = selector.SelectFittest (population, 
				new double[] { .11d, .22d, .33d, .44d, .55d, .66d, .77d, .88d, .99d }).ToList ();

			Assert.AreEqual (
				new List<double> {
					Math.Round (.88d, 2), 
					Math.Round (.77d, 2),
					Math.Round (.66d, 2),
					Math.Round (.55d, 2)
				},
				fittest.Select (f => Math.Round (f.Fitness.Value, 2)).ToList ());
		}

		[Test]
		public void WhenSelectorIsOverloadedAndRequested_Tournament_CorrectSelectorShouldBeReturned()
		{
			CustomTournamentSelector selector = new CustomTournamentSelector ();

			SelectionProvider.Add (selector);

			Selector requested = SelectionProvider.GetSelector(SelectionType.Tournament);

			Assert.AreSame (selector, requested);
		}

		[Test]
		public void WhenSelectorIsOverloadedAndRequested_Truncation_CorrectSelectorShouldBeReturned()
		{
			CustomTruncationSelector selector = new CustomTruncationSelector ();

			SelectionProvider.Add (selector);

			Selector requested = SelectionProvider.GetSelector (SelectionType.Truncation);

			Assert.AreSame (selector, requested);
		}

		[Test]
		public void WhenSelectorIsOverloadedAndRequested_Roulette_CorrectSelectorShouldBeReturned()
		{
			CustomRouletteSelector selector = new CustomRouletteSelector ();

			SelectionProvider.Add (selector);

			Selector requested = SelectionProvider.GetSelector (SelectionType.Roulette);

			Assert.AreSame (selector, requested);
		}

		[Test]
		public void WhenSelectorIsOverloadedAndRequested_Custom_CorrectSelectorShouldBeReturned()
		{
			CustomSelector selector = new CustomSelector ();

			SelectionProvider.Add (selector);

			Selector requested = SelectionProvider.GetSelector (SelectionType.Custom);

			Assert.AreSame (selector, requested);
		}			
	}

	public class CustomTournamentSelector : Selector
	{
		public CustomTournamentSelector () : base(SelectionType.Tournament)
		{
			Name = "Custom Tournament";
		}

		#region implemented abstract members of Selector

		protected override IEnumerable<Individual<T>> PerformSelection<T> (Population<T> population, List<IndexedFitness> fitness)
		{
			yield break;
		}			

		#endregion
	}

	public class CustomTruncationSelector : Selector
	{
		public CustomTruncationSelector () : base(SelectionType.Truncation)
		{
			Name = "Custom Truncation";
		}

		#region implemented abstract members of Selector

		protected override IEnumerable<Individual<T>> PerformSelection<T> (Population<T> population, List<IndexedFitness> fitness)
		{
			yield break;
		}

		#endregion
	}

	public class CustomRouletteSelector : Selector
	{
		public CustomRouletteSelector () : base(SelectionType.Roulette)
		{
			Name = "Custom Roulette";
		}

		#region implemented abstract members of Selector

		protected override IEnumerable<Individual<T>> PerformSelection<T> (Population<T> population, List<IndexedFitness> fitness)
		{
			yield break;
		}

		#endregion
	}

	class CustomSelector : Selector
	{
		public CustomSelector () : base(SelectionType.Custom)
		{
			Name = "Custom Selector";
		}

		#region implemented abstract members of Selector

		protected override IEnumerable<Individual<T>> PerformSelection<T> (Population<T> population, List<IndexedFitness> fitness)
		{
			yield break;
		}

		#endregion
	}
}

