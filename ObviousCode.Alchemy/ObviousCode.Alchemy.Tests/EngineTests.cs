using System;
using System.Linq;
using NUnit.Framework;
using ObviousCode.Alchemy.Library;
using ObviousCode.Alchemy.Library.Populous;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Tests
{
	[TestFixture]
	public class EngineTests
	{
		public EngineTests ()
		{
		}

		[Test]
		public void WhenNextGenerationCreated_PopulationSizeShouldNotChange()
		{
			TestEvaluator<int> evaluator = new TestEvaluator<int> ();

			Engine<int> engine = new Engine<int> (evaluator);

			engine.Setup ();

			int population = engine.Population.PopulationSize;

			Population<int> nextGen =  engine.ExecuteOneGeneration ();

			Assert.AreEqual (population, nextGen.PopulationSize);
		}

		[Test]
		public void WhenNextGenerationCreated_GenomeLengthShouldNotChange()
		{
			TestEvaluator<int> evaluator = new TestEvaluator<int> ();

			Engine<int> engine = new Engine<int> (evaluator);

			engine.Setup ();

			int genomeLength = engine.Population.Individuals[0].GenomeLength;

			Population<int> nextGen =  engine.ExecuteOneGeneration ();

			Assert.AreEqual (genomeLength, nextGen.Individuals[0].GenomeLength);
		}

		[Test]
		public void WhenNextGenerationCreated_NoMutation_FittestShouldBeAddedToNextGen()
		{
			TestEvaluator<int> evaluator = new TestEvaluator<int> (
				                               (i) => {
					if (i.Code [0] == 0)
						return 0d;

					return .5d;
				}
			                               );

			Engine<int> engine = new Engine<int> (evaluator);

			engine.Setup ();

			engine.Population.Individuals.For ((ind, i) => {
				ind.Code [0] = i < 2 ? 1 : 0;
			});

			ConfigurationProvider.Mutation.ChanceOfMutation = 0d;
			int[] first = new int[engine.Population[0].GenomeLength];
			int[] second = new int[engine.Population [0].GenomeLength];

			engine.Population [0].Code.CopyTo (first, 0);
			engine.Population [1].Code.CopyTo (second, 0);

			Population<int> population = engine.ExecuteOneGeneration ();

			Assert.IsTrue(FoundMatch(new List<int[]>{first, second}, population[0]));
			Assert.IsTrue(FoundMatch(new List<int[]>{first, second}, population[1]));
		}

		[Test]
		public void WhenNextGenerationCreated_FullMutation_FittestShouldBeFullyMutated()
		{
			TestEvaluator<int> evaluator = new TestEvaluator<int> (
				(i) => {
					if (i.Code [0] == 0)
						return 0d;

					return .5d;
				}
			);

			Engine<int> engine = new Engine<int> (evaluator);

			engine.Setup ();

			engine.Population.Individuals.For ((ind, i) => {
				ind.Code [0] = i < 2 ? 1 : 0;
			});

			ConfigurationProvider.Mutation.ChanceOfMutation = 1d;
			int[] first = new int[engine.Population[0].GenomeLength];
			int[] second = new int[engine.Population [0].GenomeLength];

			engine.Population [0].Code.CopyTo (first, 0);
			engine.Population [1].Code.CopyTo (second, 0);

			Population<int> population = engine.ExecuteOneGeneration ();

			Assert.IsFalse(FoundMatch(new List<int[]>{first, second}, population[0]));
			Assert.IsFalse(FoundMatch(new List<int[]>{first, second}, population[1]));
		}

		bool FoundMatch (List<int[]> candidates, Individual<int> individual)
		{
			return candidates.Where (c => {

				bool match = true;

				individual.Code.For ((item, i) => {
					match &= (item == c [i]);
				});
		
				return match;

			}).FirstOrDefault () != null;
		}

		[Test]
		public void WhenNextGenerationCreated_NextGenShouldBeDifferentToInitialGeneration()
		{
			TestEvaluator<int> evaluator = new TestEvaluator<int> (
				(i) => {
					if (i.Code [0] == 0)
						return 0d;

					return .5d;
				}
			);

			Engine<int> engine = new Engine<int> (evaluator);

			engine.Setup ();

			engine.Population.Individuals.For ((ind, i) => {
				ind.Code [0] = i < 2 ? 1 : 0;
			});

			ConfigurationProvider.Mutation.ChanceOfMutation = 0d;
			int[] third = new int[engine.Population[0].GenomeLength];
			int[] fourth = new int[engine.Population [0].GenomeLength];

			engine.Population [2].Code.CopyTo (third, 0);
			engine.Population [3].Code.CopyTo (fourth, 0);

			Population<int> population = engine.ExecuteOneGeneration ();

			Assert.IsFalse(FoundMatch(new List<int[]>{third, fourth}, population[2]));
			Assert.IsFalse(FoundMatch(new List<int[]>{third, fourth}, population[3]));
		}
	}		

	public class TestEvaluator<T> : Evaluator<T>{

		Func<Individual<T>, double> _evaluator;

		public TestEvaluator () : this((t)=>{return ConfigurationProvider.Rnd.NextDouble ();})
		{
			
		}

		public TestEvaluator (Func<Individual<T>, double> evaluator)
		{
			_evaluator = evaluator;
		}

		#region implemented abstract members of Evaluator
		protected override double EvaluateItem (ObviousCode.Alchemy.Library.Populous.Individual<T> item)
		{
			return _evaluator (item);
		}
		#endregion
	}
}

