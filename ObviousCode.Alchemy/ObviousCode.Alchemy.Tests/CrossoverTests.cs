using System;
using System.Linq;
using NUnit.Framework;
using ObviousCode.Alchemy.Library;
using ObviousCode.Alchemy.Library.Populous;

namespace ObviousCode.Alchemy.Tests
{
	[TestFixture]
	public class CrossoverTests
	{
		public CrossoverTests ()
		{
		}

		[SetUp]
		public void TestSetup()
		{
			ConfigurationProvider.Reset ();

			CrossoverProvider.Reset ();
		}

		[Test]
		public void WhenOnePointCrossoverProviderRequested_ShouldNotThrowException()
		{
			CrossoverProvider.GetCrossover (Crossover.CrossoverType.OnePoint);

			Assert.Pass ();
		}

		[Test]
		public void WhenTwoPointCrossoverProviderRequested_ShouldNotThrowException()
		{
			CrossoverProvider.GetCrossover (Crossover.CrossoverType.TwoPoint);

			Assert.Pass ();
		}

		[Test]
		public void WhenRandomCrossoverProviderRequested_ShouldNotThrowException()
		{
			CrossoverProvider.GetCrossover (Crossover.CrossoverType.Random);

			Assert.Pass ();
		}

		[Test]
		public void WhenCustomCrossoverProviderRequested_ShouldNotThrowException()
		{
			CrossoverProvider.GetCrossover (Crossover.CrossoverType.Custom);

			Assert.Pass ();
		}

		[Test]
		public void WhenOnePointCrossoverProviderRequested_ShouldReturnCorrectCrossover()
		{
			Assert.AreEqual(Crossover.CrossoverType.OnePoint, 
				CrossoverProvider.GetCrossover(Crossover.CrossoverType.OnePoint).CrossoverMethod);
		}

		[Test]
		public void WhenTwoPointCrossoverProviderRequested_ShouldReturnCorrectCrossover()
		{
			Crossover crossover = CrossoverProvider.GetCrossover (Crossover.CrossoverType.TwoPoint);

			Assert.AreEqual (Crossover.CrossoverType.TwoPoint, crossover.CrossoverMethod);
		}

		[Test]
		public void WhenRandomCrossoverProviderRequested_ShouldReturnCorrectCrossover()
		{
			Crossover crossover = CrossoverProvider.GetCrossover (Crossover.CrossoverType.Random);

			Assert.AreEqual (Crossover.CrossoverType.Random, crossover.CrossoverMethod);
		}

		[Test]
		public void WhenCustomCrossoverProviderRequested_ShouldReturnCorrectCrossover()
		{
			CrossoverProvider.AddCrossover (new CustomCrossover ());

			Crossover crossover = CrossoverProvider.GetCrossover (Crossover.CrossoverType.Custom);

			Assert.AreEqual (Crossover.CrossoverType.Custom, crossover.CrossoverMethod);
		}

		[Test]
		public void WhenCrossoverIsPerformed_ChildIsSetAsInitialised()
		{
			Population<int> population = new Population<int> ();

			population.Initialise (2, 1000);

			var crossover = CrossoverProvider.GetCrossover(Crossover.CrossoverType.OnePoint);

			Assert.IsTrue (crossover.PerformCrossover (population [0], population [1]).IsInitialised);
		}

		[Test]
		public void WhenCrossoverPerformed_OnePointCrossover_LogReturnsCorrectMethod()
		{
			Population<int> population = new Population<int> ();

			population.Initialise (2, 1000);

			var crossover = CrossoverProvider.GetCrossover(Crossover.CrossoverType.OnePoint);

			crossover.PerformCrossover(population[0], population[1]);

			Assert.AreEqual(Crossover.CrossoverType.OnePoint, CrossoverProvider.LastLogEntry.Method);
		}

		[Test]
		public void WhenCrossoverPerformed_TwoPointCrossover_LogReturnsCorrectMethod()
		{
			Population<byte> population = new Population<byte> ();

			population.Initialise (2, 10000);

			var crossover = CrossoverProvider.GetCrossover (Crossover.CrossoverType.TwoPoint);

			crossover.PerformCrossover (population [0], population [1]);

			Assert.AreEqual (Crossover.CrossoverType.TwoPoint, CrossoverProvider.LastLogEntry.Method);
		}

		[Test]
		public void WhenCrossoverPerformed_CustomCrossover_LogReturnsCorrectMethod()
		{
			Population<double> population = new Population<double> ();

			population.Initialise (2, 10020);

			var crossover = CrossoverProvider.GetCrossover (Crossover.CrossoverType.Random);

			crossover.PerformCrossover (population [0], population [1]);

			Assert.AreEqual (Crossover.CrossoverType.Random, CrossoverProvider.LastLogEntry.Method);
		}
			
		[Test]
		public void WhenCrossoverIsPerformed_OnePointCrossover_ResultingIndividualShouldHaveCorrectCode()
		{
			var crossover = CrossoverProvider.GetCrossover (Crossover.CrossoverType.OnePoint);

			Population<int> population = new Population<int> ();

			population.Initialise (2, 1000);

			population [0].Code.For ((item, i) => population [0].Code [i] = 10);
			population [1].Code.For ((item, i) => population [1].Code [i] = 20);

			Individual<int> child = crossover.PerformCrossover (population [0], population [1]);

			for (int i = 0; i < CrossoverProvider.LastLogEntry.As<OnePointCrossoverLogEntry> ().CrossoverPoint; i++) {
				Assert.AreEqual (population [0] [i], child[i]);
			}

			for (int i = CrossoverProvider.LastLogEntry.As<OnePointCrossoverLogEntry> ().CrossoverPoint; i < child.GenomeLength; i++) {
				Assert.AreEqual (population [1] [i], child[i]);
			}
		}

		[Test]
		public void WhenCrossoverIsPerformed_TwoPointCrossover_ResultingIndividualShouldHaveCorrectCode()
		{
			Population<bool> population = new Population<bool> ();

			population.Initialise (2, 10000);

			population [0].Code.For ((item, i) => population [0].Code [i] = false);
			population [1].Code.For ((item, i) => population [1].Code [i] = true);

			var crossover = CrossoverProvider.GetCrossover (Crossover.CrossoverType.TwoPoint);

			Individual<bool> child = crossover.PerformCrossover (population [0], population [1]);

			var log = CrossoverProvider.LastLogEntry.As<TwoPointCrossoverLogEntry> ();

			int crossoverPoint1 = log.CrossoverPointStart;
			int crossoverPoint2 = log.CrossoverPointFinish;

			child.Code.For ((item, i) => {

				bool pass = (i >= crossoverPoint1 && i <= crossoverPoint2) == item;

				Assert.IsTrue (pass);
			});
		}

		[Test]
		public void WhenCrossoverIsPerformed_RandomCrossover_0pcChance_ResultingIndividualShouldBeIdenticalToParent1()
		{
			ConfigurationProvider.Crossover.ChanceOfRandomGeneCrossover = 0d;

			Population<int> population = new Population<int> ();

			population.Initialise (2, 10000);

			population [0].Code.For ((item, i) => population [0] [i] = 0);
			population [1].Code.For ((item, i) => population [1] [i] = 1);

			var crossover = CrossoverProvider.GetCrossover (Crossover.CrossoverType.Random);

			Individual<int> child = crossover.PerformCrossover (population [0], population [1]);

			child.Code.ForEach(i => Assert.AreEqual(0, i));
		}

		[Test]
		public void WhenCrossoverIsPerformed_RandomCrossover_100pcChance_ResultingIndividualShouldBeIdenticalToParent2()
		{
			ConfigurationProvider.Crossover.ChanceOfRandomGeneCrossover = 1d;

			Population<int> population = new Population<int> ();

			population.Initialise (2, 10000);

			population [0].Code.For ((item, i) => population [0] [i] = 0);
			population [1].Code.For ((item, i) => population [1] [i] = 1);

			var crossover = CrossoverProvider.GetCrossover (Crossover.CrossoverType.Random);

			Individual<int> child = crossover.PerformCrossover (population [0], population [1]);

			child.Code.ForEach(i => Assert.AreEqual(1, i));
		}

		[Test]
		public void WhenCrossoverIsPerformed_RandomCrossover_50pcChance_ShouldBeApprox50pcCrossoverBetweenParents()
		{
			ConfigurationProvider.Crossover.ChanceOfRandomGeneCrossover = .5d;

			Population<bool> population = new Population<bool> ();

			int genomeLength = 100000;

			population.Initialise (2, genomeLength);

			population [0].Code.For ((item, i) => population [0] [i] = false);
			population [1].Code.For ((item, i) => population [1] [i] = true);

			double acceptableMin = (double) genomeLength * ((double) 4.5d / 10);
			double acceptableMax = (double) genomeLength * ((double) 5.5d / 10);

			int crossoverCount = 0;

			Crossover crossover = CrossoverProvider.GetCrossover (Crossover.CrossoverType.Random);

			var child = crossover.PerformCrossover (population [0], population [1]);

			child.Code.ForEach (b => crossoverCount += b ? 1 : 0);

			Assert.LessOrEqual (crossoverCount, acceptableMax);
			Assert.GreaterOrEqual (crossoverCount, acceptableMin);
		}

		[Test]
		public void WhenCrossoverSelectionDefaultIsRequested_ShouldBeCorrect()
		{
			ConfigurationProvider.EnsureIsBuilt ();

			CrossoverProvider.AssertSelectionArray(
				new Crossover.CrossoverType[]
				{
					Crossover.CrossoverType.OnePoint,
					Crossover.CrossoverType.TwoPoint,
					Crossover.CrossoverType.Random
				});
		}

		[Test]
		public void WhenCrossoverConfigurationRatiosChanged_CrossoverSelectionRequested_ShouldBeCorrect()
		{
			ConfigurationProvider.Crossover.OnePointCrossoverRatio = 4;
			ConfigurationProvider.Crossover.TwoPointCrossoverRatio = 3;
			ConfigurationProvider.Crossover.RandomCrossoverRatio = 1;

			CrossoverProvider.AssertSelectionArray (
				new Crossover.CrossoverType[]
				{
					Crossover.CrossoverType.OnePoint,
					Crossover.CrossoverType.OnePoint,
					Crossover.CrossoverType.OnePoint,
					Crossover.CrossoverType.OnePoint,
					Crossover.CrossoverType.TwoPoint,
					Crossover.CrossoverType.TwoPoint,
					Crossover.CrossoverType.TwoPoint,
					Crossover.CrossoverType.Random
				});
		}

		[Test]
		public void WhenCrossoverSelectionDefaultIsRequested_RequestedCrossoversShouldHaveApproxEqualChance()
		{
			int onePoint = 0;
			int twoPoint = 0;
			int random = 0;

			int tries = 10000;

			int min = (int)(tries * ((double)3 / 10));
			int max = (int)(tries * ((double)4 / 10));

			ConfigurationProvider.EnsureIsBuilt ();

			for (int i = 0; i < tries; i++) {
				switch (CrossoverProvider.GetNextCrossover ().CrossoverMethod) {
				case Crossover.CrossoverType.OnePoint:
					onePoint++;
					break;
				case Crossover.CrossoverType.TwoPoint:
					twoPoint++;
					break;
				case Crossover.CrossoverType.Random:
					random++;
					break;
				}
			}

			Assert.LessOrEqual (onePoint, max);
			Assert.GreaterOrEqual (onePoint, min);
					
			Assert.LessOrEqual (twoPoint, max);
			Assert.GreaterOrEqual (twoPoint, min);
		
			Assert.LessOrEqual (random, max);
			Assert.GreaterOrEqual (random, min);}

		[Test]
		public void WhenCrossoverConfigurationRatiosChanged_CrossoverSelectionRequested_RequestedCrossoversShouldHaveApproxCorrectChance()
		{
			int onePoint = 0;
			int twoPoint = 0;
			int random = 0;

			ConfigurationProvider.Crossover.OnePointCrossoverRatio = 1;
			ConfigurationProvider.Crossover.TwoPointCrossoverRatio = 1;
			ConfigurationProvider.Crossover.RandomCrossoverRatio = 1;

			int tries = 30000;

			Enumerable.Range (0, tries)
				.ForEach (i => {
					switch (CrossoverProvider.GetNextCrossover ().CrossoverMethod) {
					case Crossover.CrossoverType.OnePoint:
						onePoint++;
						break;
					case Crossover.CrossoverType.TwoPoint:
						twoPoint++;
						break;
					case Crossover.CrossoverType.Random:
						random++;
						break;
					}});

			double acceptableErrorPercent = .05;

			int average = (int) (tries / 3);

			int acceptableError = (int) (average * acceptableErrorPercent);

			int acceptableMin = average - acceptableError;
			int acceptableMax = average + acceptableError;

			Assert.GreaterOrEqual (onePoint, acceptableMin);
			Assert.LessOrEqual (onePoint, acceptableMax);

			Assert.GreaterOrEqual (twoPoint, acceptableMin);
			Assert.LessOrEqual (twoPoint, acceptableMax);

			Assert.GreaterOrEqual (random, acceptableMin);
			Assert.LessOrEqual (random, acceptableMax);
		}
	}

	public class CustomCrossover : Crossover
	{
		public CustomCrossover () : base(Crossover.CrossoverType.Custom)
		{
			
		}

		#region implemented abstract members of Crossover

		protected override void PopulateChild<T> (Individual<T> child, Individual<T> parent1, Individual<T> parent2)
		{
			child.Code.For ((item, i) => {
				if (i % 2 == 0)
				{
					child.Code[i] = parent1.Code[i];
				}
				else
				{
					child.Code[i] = parent2.Code[i];
				}
			});
		}

		#endregion
	}
}

