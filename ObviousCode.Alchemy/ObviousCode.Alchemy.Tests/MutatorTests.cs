using System;
using NUnit.Framework;
using ObviousCode.Alchemy.Library;
using ObviousCode.Alchemy.Library.Populous;
using System.Linq;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Tests
{
	[TestFixture]
	public class MutatorTests
	{
		public MutatorTests ()
		{
		}

		[Test]
		public void WhenMutatorIsRequested_Bool_CorrectMutatorShouldReturn()
		{
			Mutator mutator = MutationProvider.GetMutator<bool> ();

			Assert.AreEqual (typeof(bool), mutator.GenomeType);
		}

		[Test]
		public void WhenMutatorIsRequested_Byte_CorrectMutatorShouldReturn()
		{
			Mutator<int> mutator = MutationProvider.GetMutator<int> ();

			Assert.AreEqual (typeof(int), mutator.GenomeType);
		}

		[Test]
		public void WhenMutatorIsRequested_Double_CorrectMutatorShouldReturn()
		{
			Mutator<double> mutator = MutationProvider.GetMutator<double> ();

			Assert.AreEqual (typeof(double), mutator.GenomeType);
		}

		[Test]
		public void WhenMutatorIsRequested_Int_CorrectMutatorShouldReturn()
		{
			Mutator mutator = MutationProvider.GetMutator<int> ();

			Assert.AreEqual (typeof(int), mutator.GenomeType);
		}

		[Test]
		public void WhenMutationIsRequested_Bool_NoExceptionThrown()
		{
			MutationProvider.GetMutator<bool> ();

			Assert.Pass ();
		}

		[Test]
		public void WhenMutationIsRequested_Byte_NoExceptionThrown()
		{
			MutationProvider.GetMutator<byte> ();

			Assert.Pass ();
		}

		[Test]
		public void WhenMutationIsRequested_Double_NoExceptionThrown()
		{
			MutationProvider.GetMutator<double> ();

			Assert.Pass ();
		}
			
		[Test]
		public void WhenMutationIsRequested_Int_NoExceptionThrown()
		{
			MutationProvider.GetMutator<int> ();

			Assert.Pass ();
		}

		/// <summary>
		/// Following tests testing % chance of mutations
		/// </summary>
		[Test]
		public void WhenMutationIsRequested_Int_Chance100pc_AllValuesChanged()
		{
			Population<int> population = new Population<int> ();

			population.Initialise (1, 1000);

			int[] beforeMutation = population[0].Code.Select (i => i).ToArray ();

			Mutator<int> mutator = MutationProvider.GetMutator<int> ();

			ConfigurationProvider.Mutation.ChanceOfMutation = 1;

			mutator.Mutate (population);

			population[0].Code.For((item, i)=>
				{
					Assert.AreNotEqual(beforeMutation[i], item);
				});
		}

		[Test]
		public void WhenMutationIsRequested_Bool_Chance0pc_NoValuesChanged()
		{
			Population<bool> population = new Population<bool> ();

			population.Initialise (1, 1000);

			bool[] beforeMutation = population [0].Code.Select (i => i).ToArray ();

			ConfigurationProvider.Mutation.ChanceOfMutation = 0;

			Mutator<bool> mutator = MutationProvider.GetMutator<bool> ();

			mutator.Mutate (population);

			population [0].Code.For ((item, i) => {
				Assert.AreEqual(beforeMutation[i], item);
			});
		}

		[Test]
		public void WhenMutationIsRequested_Bool_Chance50pc_ApproxHalfValuesChanged()
		{
			Population<bool> population = new Population<bool> ();

			int genomeLength = 1000;

			population.Initialise (1, genomeLength);

			bool[] beforeMutation = population [0].Code.Select (i => i).ToArray ();

			ConfigurationProvider.Mutation.ChanceOfMutation = .5d;

			Mutator<bool> mutator = MutationProvider.GetMutator<bool> ();

			mutator.Mutate (population);

			int mutationCount = 0;

			population [0].Code.For ((item, i) => {
				mutationCount += beforeMutation [i] == item ? 0 : 1;
			});

			double acceptableMin = (double) genomeLength * ((double) 4.5d / 10);
			double acceptableMax = (double) genomeLength * ((double) 5.5d / 10);

			Assert.LessOrEqual (mutationCount, acceptableMax);
			Assert.GreaterOrEqual (mutationCount, acceptableMin);
		}

		[Test]
		public void WhenMutationIsRequested_Bool_Chance100pc_AllValuesChanged()
		{
			Population<bool> population = new Population<bool> ();

			population.Initialise (1, 1000);

			bool[] beforeMutation = population [0].Code.Select (i => i).ToArray ();

			ConfigurationProvider.Mutation.ChanceOfMutation = 0;

			Mutator<bool> mutator = MutationProvider.GetMutator<bool> ();

			mutator.Mutate (population);

			population [0].Code.For ((item, i) => {
				Assert.AreEqual(beforeMutation[i], item);
			});
		}

		[Test]
		public void WhenMutationIsRequested_Int_Chance0pc_NoValuesChanged()
		{
			Population<int> population = new Population<int> ();

			population.Initialise (1, 1000);

			int[] beforeMutation = population [0].Code.Select (i => i).ToArray ();

			Mutator<int> mutator = MutationProvider.GetMutator<int> ();

			ConfigurationProvider.Mutation.ChanceOfMutation = 0;

			mutator.Mutate (population);

			population[0].Code.For((item, i)=>
				{
					Assert.AreEqual(beforeMutation[i], item);
				});
		}

		[Test]
		public void WhenMutationIsRequested_Int_Chance50pc_ApproxHalfValuesChanged()
		{
			Enumerable.Range(0, 100).ForEach(loop=>{
				Population<int> population = new Population<int> ();
				int genomeCount = 1000;

				population.Initialise (1, genomeCount);

				//assume between 40 and 60%
				double acceptableMin = (double) genomeCount * ((double) 4 / 10);
				double acceptableMax = (double) genomeCount * ((double) 6 / 10);

				int[] beforeMutation = population[0].Code.Select (i => i).ToArray ();

				Mutator<int> mutator = MutationProvider.GetMutator<int> ();

				ConfigurationProvider.Mutation.ChanceOfMutation = 0.5d;

				mutator.Mutate (population);

				int changedCount = 0;

				population[0].Code.For ((item, i) => {
					changedCount += beforeMutation [i] == item ? 0 : 1;
				});

				Assert.GreaterOrEqual (changedCount, acceptableMin);
				Assert.LessOrEqual (changedCount, acceptableMax);
			});
		}

		/// <summary>
		/// Following tests testing variance amount of actual mutation, and that a mutation will always change
		/// a value, and not allow a random value to be the same as original value
		/// </summary>
		[Test]
		public void WhenMutationIsRequested_Int_ContextMethod_AllValuesChangedWithinVariance()
		{
			Population<int> population = new Population<int> ();

			(InitialiserContextProvider.GetContext<int>() as IntBasedGenomeInitialiserContext).Min = 100;
			(InitialiserContextProvider.GetContext<int>() as IntBasedGenomeInitialiserContext).Max = 200;

			population.Initialise (1, 10000);

			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;
			ConfigurationProvider.Mutation.ContextMutationVariance = .25;

			Mutator<int> mutator = MutationProvider.GetMutator<int> ();

			population[0].Code.For ((item, i) => {

				int mod5 = i % 5;

				population[0].Code [i] = 10 + ((mod5 % 5) * 10) 
					+ (InitialiserContextProvider.GetContext<int>() as IntBasedGenomeInitialiserContext).Min;
			});				



			mutator.Mutate (population);

			int variance = 25;

			population[0].Code.For ((item, i) => {

				int mod5 = i % 5;

				int original = 10 + ((mod5 % 5) * 10) 
					+ (InitialiserContextProvider.GetContext<int>() as IntBasedGenomeInitialiserContext).Min;

				int min = original - variance;
				int max = original + variance;

				Assert.GreaterOrEqual (item, min);
				Assert.LessOrEqual (item, max);
			});
		}

		[Test]
		public void WhenMutationIsRequested_Int_ContextMethod_VarianceIsApprox50pcNegativeVariance()
		{
			Population<int> population = new Population<int> ();

			int genomeCount = 1000;

			population.Initialise (1, genomeCount);

			int[] original = population [0].Code.ToArray();

			Mutator<int> mutator = MutationProvider.GetMutator<int> ();

			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.ContextMutationVariance = .33;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;

			mutator.Mutate (population);

			int negativeCount = 0;

			population[0].Code.For ((item, i) => {
				negativeCount += population[0].Code[i] < original[i] ? 1 : 0;
			});

			double acceptableMin = (double) genomeCount * ((double) 4.5 / 10);
			double acceptableMax = (double) genomeCount * ((double) 5.5 / 10);

			Assert.GreaterOrEqual (negativeCount, acceptableMin);
			Assert.LessOrEqual (negativeCount, acceptableMax);
		}

		[Test]
		public void WhenMutationIsRequested_Int_FullRandomMethod_AllValuesChanged()
		{
			Population<int> population = new Population<int> ();

			population.Initialise (1, 100000);

			int[] beforeMutation = population[0].Code.Select (i => i).ToArray ();

			Mutator<int> mutator = MutationProvider.GetMutator<int> ();

			ConfigurationProvider.Mutation.ChanceOfMutation = 1;

			mutator.Mutate (population);

			population[0].Code.For((item, i)=>
				{
					Assert.AreNotEqual(beforeMutation[i], item);
				});
		}
			
		[Test]
		public void WhenMutationIsSetToVariance_Int_UncheckedOverflow_MutationLessThanMinUnderflowToMax()
		{
			Population<int> population = new Population<int> ();

			(InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext).Min = 0;
			(InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext).Max = 100;

			IntBasedSingleInitialiser initialiser = new IntBasedSingleInitialiser (
				(InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext).Min);

			InitialiserProvider.AddInitialiser (initialiser);

			var genomeLength = 1000;
			population.Initialise (1, genomeLength);

			ConfigurationProvider.Mutation.AllowUncheckedVariance = true;
			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.ContextMutationVariance = .1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;

			Mutator<int> mutator = MutationProvider.GetMutator<int> ();

			mutator.Mutate (population);

			int overflowCount = 0;

			population [0].Code.ForEach (i => {
				overflowCount += i >= 90 ? 1 : 0;
			});

			double acceptableMin = (double) genomeLength * ((double) 4.5d / 10);
			double acceptableMax = (double) genomeLength * ((double) 5.5d / 10);

			Assert.LessOrEqual (overflowCount, acceptableMax);
			Assert.GreaterOrEqual (overflowCount, acceptableMin);
		}

		[Test]
		public void WhenMutationIsSetToVariance_Int_CheckedOverflow_MutationLessThanMinFloorAtMin()
		{
			Population<int> population = new Population<int> ();

			(InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext).Min = 10;
			(InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext).Max = 110;

			IntBasedSingleInitialiser initialiser = new IntBasedSingleInitialiser (
				(InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext).Min);

			InitialiserProvider.AddInitialiser (initialiser);

			var genomeLength = 1000;
			population.Initialise (1, genomeLength);

			ConfigurationProvider.Mutation.AllowUncheckedVariance = false;
			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.ContextMutationVariance = .1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;

			Mutator<int> mutator = MutationProvider.GetMutator<int> ();

			mutator.Mutate (population);

			int underflowCount = 0;

			population [0].Code.ForEach (i => {
				underflowCount += 
					i ==(InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext).Min ? 1 : 0;
			});

			double acceptableMin = (double) genomeLength * ((double) 4.5d / 10);
			double acceptableMax = (double) genomeLength * ((double) 5.5d / 10);

			Assert.LessOrEqual (underflowCount, acceptableMax);
			Assert.GreaterOrEqual (underflowCount, acceptableMin);
		}

		[Test]
		public void WhenMutationIsSetToVariance_Int_CheckedOverflow_MutationMoreThanMaxCeilingAtMax()
		{
			Population<int> population = new Population<int> ();

			(InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext).Min = 10;
			(InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext).Max = 110;

			IntBasedSingleInitialiser initialiser = new IntBasedSingleInitialiser (
				(InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext).Max);

			InitialiserProvider.AddInitialiser (initialiser);

			var genomeLength = 1000;
			population.Initialise (1, genomeLength);

			ConfigurationProvider.Mutation.AllowUncheckedVariance = false;
			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.ContextMutationVariance = .1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;

			Mutator<int> mutator = MutationProvider.GetMutator<int> ();

			mutator.Mutate (population);

			int underflowCount = 0;

			population [0].Code.ForEach (i => {
				underflowCount += 
					i ==(InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext).Max ? 1 : 0;
			});

			double acceptableMin = (double) genomeLength * ((double) 4.5d / 10);
			double acceptableMax = (double) genomeLength * ((double) 5.5d / 10);

			Assert.LessOrEqual (underflowCount, acceptableMax);
			Assert.GreaterOrEqual (underflowCount, acceptableMin);
		}

		[Test]
		public void WhenMutationIsSetToVariance_Int_UncheckedOverflow_MutationMoreThanMaxOverflowToMin()
		{
			Population<int> population = new Population<int> ();

			(InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext).Min = 0;
			(InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext).Max = 100;

			IntBasedSingleInitialiser initialiser = new IntBasedSingleInitialiser (
				(InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext).Max);

			InitialiserProvider.AddInitialiser (initialiser);

			var genomeLength = 1000;
			population.Initialise (1, genomeLength);

			ConfigurationProvider.Mutation.AllowUncheckedVariance = true;
			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.ContextMutationVariance = .1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;

			Mutator<int> mutator = MutationProvider.GetMutator<int> ();

			mutator.Mutate (population);

			int underflowCount = 0;

			population [0].Code.ForEach (i => {
				underflowCount += i <= 10 ? 1 : 0;
			});

			double acceptableMin = (double) genomeLength * ((double) 4.5d / 10);
			double acceptableMax = (double) genomeLength * ((double) 5.5d / 10);

			Assert.LessOrEqual (underflowCount, acceptableMax);
			Assert.GreaterOrEqual (underflowCount, acceptableMin);
		}

		[Test]
		public void WhenMutationIsRequested_Byte_Chance0pc_NoValuesChanged()
		{
			Population<byte> population = new Population<byte> ();

			population.Initialise (1, 1000);

			byte[] beforeMutation = population [0].Code.Select (i => i).ToArray ();

			Mutator<byte> mutator = MutationProvider.GetMutator<byte> ();

			ConfigurationProvider.Mutation.ChanceOfMutation = 0;

			mutator.Mutate (population);

			population[0].Code.For((item, i)=>
				{
					Assert.AreEqual(beforeMutation[i], item);
				});					
		}

		[Test]
		public void WhenMutationIsRequested_Byte_Chance50pc_ApproxHalfValuesChanged()
		{
			Enumerable.Range(0, 100).ForEach(loop=>{
				Population<byte> population = new Population<byte> ();
				int genomeCount = 10000;

				population.Initialise (1, genomeCount);

				//assume between 45% and 55%
				double acceptableMin = (double) genomeCount * ((double) 4.5 / 10);
				double acceptableMax = (double) genomeCount * ((double) 5.5 / 10);

				byte[] beforeMutation = population[0].Code.Select (i => i).ToArray ();

				Mutator<byte> mutator = MutationProvider.GetMutator<byte> ();

				ConfigurationProvider.Mutation.ChanceOfMutation = 0.5d;

				mutator.Mutate (population);

				int changedCount = 0;

				population[0].Code.For ((item, i) => {
					changedCount += beforeMutation [i] == item ? 0 : 1;
				});

				Assert.GreaterOrEqual (changedCount, acceptableMin);
				Assert.LessOrEqual (changedCount, acceptableMax);
			});
		}

		/// <summary>
		/// Following tests testing variance amount of actual mutation, and that a mutation will always change
		/// a value, and not allow a random value to be the same as original value
		/// </summary>
		[Test]
		public void WhenMutationIsRequested_Byte_ContextMethod_AllValuesChangedWithinVariance()
		{
			var population = new Population<byte> ();

			(InitialiserContextProvider.GetContext<byte>() as ByteBasedGenomeInitialiserContext).Min = 100;
			(InitialiserContextProvider.GetContext<byte>() as ByteBasedGenomeInitialiserContext).Max = 200;

			population.Initialise (1, 10000);

			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;
			ConfigurationProvider.Mutation.ContextMutationVariance = .25;

			Mutator<byte> mutator = MutationProvider.GetMutator<byte> ();
			byte initialState = 150;

			population[0].Code.For ((item, i) => {

				population[0].Code [i] = initialState;
			});	

			mutator.Mutate (population);

			byte variance = 25;

			population[0].Code.For ((item, i) => {

				Assert.GreaterOrEqual (item, initialState - variance);
				Assert.LessOrEqual (item, initialState + variance);
			});
		}

		[Test]
		public void WhenMutationIsRequested_Byte_ContextMethod_VarianceIsApprox50pcNegativeVariance()
		{
			var population = new Population<byte> ();

			(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Min = 100;
			(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Max = 200;

			var genomeCount = 10000;
			population.Initialise (1, genomeCount);

			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;
			ConfigurationProvider.Mutation.ContextMutationVariance = .25;

			Mutator<byte> mutator = MutationProvider.GetMutator<byte> ();

			population [0].Code.For ((item, i) => {

				population [0].Code [i] = 150;
			});	

			mutator.Mutate (population);

			int negativeVarianceCount = 0;

			population [0].Code.For ((item, i) => {
				if (item < 150)
					negativeVarianceCount += 1;
			});

			//assume between 45% and 55%
			double acceptableMin = (double) genomeCount * ((double) 4.5 / 10);
			double acceptableMax = (double) genomeCount * ((double) 5.5 / 10);

			Assert.GreaterOrEqual (negativeVarianceCount, acceptableMin);
			Assert.LessOrEqual (negativeVarianceCount, acceptableMax);
		}

		[Test]
		public void WhenMutationIsRequested_Byte_FullRandomMethod_AllValuesChanged()
		{
			Population<byte> population = new Population<byte> ();

			population.Initialise (1, 100000);

			byte[] beforeMutation = population[0].Code.Select (i => i).ToArray ();
				
			Mutator<byte> mutator = MutationProvider.GetMutator<byte> ();
				
			ConfigurationProvider.Mutation.ChanceOfMutation = 1;

			mutator.Mutate (population);

			population[0].Code.For((item, i)=>
				{
					Assert.AreNotEqual(beforeMutation[i], item);
				});
		}

		[Test]
		public void WhenMutationIsSetToVariance_Byte_UncheckedOverflow_MutationLessThanMinUnderflowToMax()
		{
			Population<byte> population = new Population<byte> ();

			(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Min = 0;
			(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Max = 100;

			ByteBasedSingleInitialiser initialiser = new ByteBasedSingleInitialiser (
				(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Min);

			InitialiserProvider.AddInitialiser (initialiser);

			var genomeLength = 1000;
			population.Initialise (1, genomeLength);

			ConfigurationProvider.Mutation.AllowUncheckedVariance = true;
			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.ContextMutationVariance = .1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;

			Mutator<byte> mutator = MutationProvider.GetMutator<byte> ();

			mutator.Mutate (population);

			int overflowCount = 0;

			population [0].Code.ForEach (i => {
				overflowCount += i >= 90 ? 1 : 0;
			});

			double acceptableMin = (double) genomeLength * ((double) 4.5d / 10);
			double acceptableMax = (double) genomeLength * ((double) 5.5d / 10);

			Assert.LessOrEqual (overflowCount, acceptableMax);
			Assert.GreaterOrEqual (overflowCount, acceptableMin);
		}

		[Test]
		public void WhenMutationIsSetToVariance_Byte_CheckedOverflow_MutationLessThanMinFloorAtMin()
		{
			Population<byte> population = new Population<byte> ();

			(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Min = 10;
			(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Max = 110;

			ByteBasedSingleInitialiser initialiser = new ByteBasedSingleInitialiser (
				(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Min);

			InitialiserProvider.AddInitialiser (initialiser);

			var genomeLength = 100000;
			population.Initialise (1, genomeLength);

			ConfigurationProvider.Mutation.AllowUncheckedVariance = false;
			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.ContextMutationVariance = .1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;

			Mutator<byte> mutator = MutationProvider.GetMutator<byte> ();

			mutator.Mutate (population);

			int underflowCount = 0;

			population [0].Code.ForEach (i => {
				underflowCount += 
					i ==(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Min ? 1 : 0;
			});

			double acceptableMin = (double) genomeLength * ((double) 4.5d / 10);
			double acceptableMax = (double) genomeLength * ((double) 5.5d / 10);

			Assert.LessOrEqual (underflowCount, acceptableMax);
			Assert.GreaterOrEqual (underflowCount, acceptableMin);
		}

		[Test]
		public void WhenMutationIsSetToVariance_Byte_CheckedOverflow_MutationMoreThanMaxCeilingAtMax()
		{
			var population = new Population<byte> ();

			(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Min = 10;
			(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Max = 110;

			ByteBasedSingleInitialiser initialiser = new ByteBasedSingleInitialiser (
				(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Max);

			InitialiserProvider.AddInitialiser (initialiser);

			var genomeLength = 1000;
			population.Initialise (1, genomeLength);

			ConfigurationProvider.Mutation.AllowUncheckedVariance = false;
			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.ContextMutationVariance = .1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;

			Mutator<byte> mutator = MutationProvider.GetMutator<byte> ();

			mutator.Mutate (population);

			int underflowCount = 0;

			population [0].Code.ForEach (i => {
				underflowCount += 
					i ==(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Max ? 1 : 0;
			});

			double acceptableMin = (double) genomeLength * ((double) 4.5d / 10);
			double acceptableMax = (double) genomeLength * ((double) 5.5d / 10);

			Assert.LessOrEqual (underflowCount, acceptableMax);
			Assert.GreaterOrEqual (underflowCount, acceptableMin);
		}

		[Test]
		public void WhenMutationIsSetToVariance_Byte_UncheckedOverflow_MutationMoreThanMaxOverflowToMin()
		{
			var population = new Population<byte> ();

			(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Min = 10;
			(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Max = 110;

			ByteBasedSingleInitialiser initialiser = new ByteBasedSingleInitialiser (
				(InitialiserContextProvider.GetContext<byte> () as ByteBasedGenomeInitialiserContext).Max);

			InitialiserProvider.AddInitialiser (initialiser);

			var genomeLength = 1000;
			population.Initialise (1, genomeLength);

			ConfigurationProvider.Mutation.AllowUncheckedVariance = true;
			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.ContextMutationVariance = .1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;

			Mutator<byte> mutator = MutationProvider.GetMutator<byte> ();

			mutator.Mutate (population);

			int underflowCount = 0;

			population [0].Code.ForEach (i => {
				underflowCount += i <= 20 ? 1 : 0;
			});

			double acceptableMin = (double) genomeLength * ((double) 4.5d / 10);
			double acceptableMax = (double) genomeLength * ((double) 5.5d / 10);

			Assert.LessOrEqual (underflowCount, acceptableMax);
			Assert.GreaterOrEqual (underflowCount, acceptableMin);
		}

		//**********************
		[Test]
		public void WhenMutationIsRequested_Double_Chance0pc_NoValuesChanged()
		{
			Population<double> population = new Population<double> ();

			population.Initialise (1, 1000);

			double[] beforeMutation = population [0].Code.Select (i => i).ToArray ();

			Mutator<double> mutator = MutationProvider.GetMutator<double> ();

			ConfigurationProvider.Mutation.ChanceOfMutation = 0;

			mutator.Mutate (population);

			population[0].Code.For((item, i)=>
				{
					Assert.AreEqual(beforeMutation[i], item);
				});		
		}

		[Test]
		public void WhenMutationIsRequested_Double_Chance50pc_ApproxHalfValuesChanged()
		{
			Enumerable.Range(0, 100).ForEach(loop=>{
				Population<double> population = new Population<double> ();
				int genomeCount = 10000;

				population.Initialise (1, genomeCount);

				//assume between 45% and 55%
				double acceptableMin = (double) genomeCount * ((double) 4.5 / 10);
				double acceptableMax = (double) genomeCount * ((double) 5.5 / 10);

				double[] beforeMutation = population[0].Code.Select (i => i).ToArray ();

				Mutator<double> mutator = MutationProvider.GetMutator<double> ();

				ConfigurationProvider.Mutation.ChanceOfMutation = 0.5d;

				mutator.Mutate (population);

				int changedCount = 0;

				population[0].Code.For ((item, i) => {
					changedCount += beforeMutation [i] == item ? 0 : 1;
				});

				Assert.GreaterOrEqual (changedCount, acceptableMin);
				Assert.LessOrEqual (changedCount, acceptableMax);
			});
		}

		/// <summary>
		/// Following tests testing variance amount of actual mutation, and that a mutation will always change
		/// a value, and not allow a random value to be the same as original value
		/// </summary>
		[Test]
		public void WhenMutationIsRequested_Double_ContextMethod_AllValuesChangedWithinVariance()
		{
			var population = new Population<double> ();

			(InitialiserContextProvider.GetContext<double>() as DoubleBasedGenomeInitialiserContext).Min = 100d;
			(InitialiserContextProvider.GetContext<double>() as DoubleBasedGenomeInitialiserContext).Max = 200d;

			population.Initialise (1, 10000);

			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;
			ConfigurationProvider.Mutation.ContextMutationVariance = .25;

			var mutator = MutationProvider.GetMutator<double> ();
			double initialState = 150d;

			population[0].Code.For ((item, i) => {

				population[0].Code [i] = initialState;
			});	

			mutator.Mutate (population);

			double variance = 25d;

			population[0].Code.For ((item, i) => {
				Assert.GreaterOrEqual (item, initialState - variance);
				Assert.LessOrEqual (item, initialState + variance);
			});
		}

		[Test]
		public void WhenMutationIsRequested_Double_ContextMethod_VarianceIsApprox50pcNegativeVariance()
		{
			var population = new Population<double> ();

			(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Min = 100d;
			(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Max = 200d;

			var genomeCount = 10000;

			population.Initialise (1, genomeCount);

			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;
			ConfigurationProvider.Mutation.ContextMutationVariance = .25;

			Mutator<double> mutator = MutationProvider.GetMutator<double> ();

			population [0].Code.For ((item, i) => {

				population [0].Code [i] = 150;
			});	

			mutator.Mutate (population);

			int negativeVarianceCount = 0;

			population [0].Code.For ((item, i) => {
				if (item < 150)
					negativeVarianceCount += 1;
			});

			//assume between 45% and 55%
			double acceptableMin = (double) genomeCount * ((double) 4.5 / 10);
			double acceptableMax = (double) genomeCount * ((double) 5.5 / 10);

			Assert.GreaterOrEqual (negativeVarianceCount, acceptableMin);
			Assert.LessOrEqual (negativeVarianceCount, acceptableMax);
		}

		[Test]
		public void WhenMutationIsRequested_Double_FullRandomMethod_AllValuesChanged()
		{
			Population<double> population = new Population<double> ();

			population.Initialise (1, 100000);

			double[] beforeMutation = population[0].Code.Select (i => i).ToArray ();

			Mutator<double> mutator = MutationProvider.GetMutator<double> ();

			ConfigurationProvider.Mutation.ChanceOfMutation = 1;

			mutator.Mutate (population);

			population[0].Code.For((item, i)=>
				{
					Assert.AreNotEqual(beforeMutation[i], item);
				});
		}

		[Test]
		public void WhenMutationIsSetToVariance_Double_UncheckedOverflow_MutationLessThanMinUnderflowToMax()
		{
			Population<double> population = new Population<double> ();

			(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Min = 0;
			(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Max = 100;

			DoubleBasedSingleInitialiser initialiser = new DoubleBasedSingleInitialiser (
				(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Min);

			InitialiserProvider.AddInitialiser (initialiser);

			var genomeLength = 1000;
			population.Initialise (1, genomeLength);

			ConfigurationProvider.Mutation.AllowUncheckedVariance = true;
			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.ContextMutationVariance = .1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;

			Mutator<double> mutator = MutationProvider.GetMutator<double> ();

			mutator.Mutate (population);

			int overflowCount = 0;

			population [0].Code.ForEach (i => {
				overflowCount += i >= 90 ? 1 : 0;
			});

			double acceptableMin = (double) genomeLength * ((double) 4.5d / 10);
			double acceptableMax = (double) genomeLength * ((double) 5.5d / 10);

			Assert.LessOrEqual (overflowCount, acceptableMax);
			Assert.GreaterOrEqual (overflowCount, acceptableMin);
		}

		[Test]
		public void WhenMutationIsSetToVariance_Double_CheckedOverflow_MutationLessThanMinFloorAtMin()
		{
			var population = new Population<double> ();

			(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Min = 10d;
			(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Max = 110d;

			DoubleBasedSingleInitialiser initialiser = new DoubleBasedSingleInitialiser (
				(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Min);

			InitialiserProvider.AddInitialiser (initialiser);

			var genomeLength = 100000;
			population.Initialise (1, genomeLength);

			ConfigurationProvider.Mutation.AllowUncheckedVariance = false;
			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.ContextMutationVariance = .1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;

			Mutator<double> mutator = MutationProvider.GetMutator<double> ();

			mutator.Mutate (population);

			int underflowCount = 0;

			population [0].Code.ForEach (i => {
				underflowCount += 
					i ==(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Min ? 1 : 0;
			});

			double acceptableMin = (double) genomeLength * ((double) 4.5d / 10);
			double acceptableMax = (double) genomeLength * ((double) 5.5d / 10);

			Assert.LessOrEqual (underflowCount, acceptableMax);
			Assert.GreaterOrEqual (underflowCount, acceptableMin);
		}

		[Test]
		public void WhenMutationIsSetToVariance_Double_CheckedOverflow_MutationMoreThanMaxCeilingAtMax()
		{
			var population = new Population<double> ();

			(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Min = 10d;
			(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Max = 110d;

			DoubleBasedSingleInitialiser initialiser = new DoubleBasedSingleInitialiser (
				(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Max);

			InitialiserProvider.AddInitialiser (initialiser);

			var genomeLength = 1000;
			population.Initialise (1, genomeLength);

			ConfigurationProvider.Mutation.AllowUncheckedVariance = false;
			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.ContextMutationVariance = .1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;

			Mutator<double> mutator = MutationProvider.GetMutator<double> ();

			mutator.Mutate (population);

			int underflowCount = 0;

			population [0].Code.ForEach (i => {
				underflowCount += 
					i ==(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Max ? 1 : 0;
			});

			double acceptableMin = (double) genomeLength * ((double) 4.5d / 10);
			double acceptableMax = (double) genomeLength * ((double) 5.5d / 10);

			Assert.LessOrEqual (underflowCount, acceptableMax);
			Assert.GreaterOrEqual (underflowCount, acceptableMin);
		}

		[Test]
		public void WhenMutationIsSetToVariance_Double_UncheckedOverflow_MutationMoreThanMaxOverflowToMin()
		{
			var population = new Population<double> ();

			(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Min = 10d;
			(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Max = 110d;

			DoubleBasedSingleInitialiser initialiser = new DoubleBasedSingleInitialiser (
				(InitialiserContextProvider.GetContext<double> () as DoubleBasedGenomeInitialiserContext).Max);

			InitialiserProvider.AddInitialiser (initialiser);

			var genomeLength = 1000;
			population.Initialise (1, genomeLength);

			ConfigurationProvider.Mutation.AllowUncheckedVariance = true;
			ConfigurationProvider.Mutation.ChanceOfMutation = 1;
			ConfigurationProvider.Mutation.ContextMutationVariance = .1;
			ConfigurationProvider.Mutation.MutationMethod = MutationConfiguration.MutationStyle.Variance;

			var mutator = MutationProvider.GetMutator<double> ();

			mutator.Mutate (population);

			int underflowCount = 0;

			population [0].Code.ForEach (i => {
				underflowCount += i <= 20 ? 1 : 0;
			});

			double acceptableMin = (double) genomeLength * ((double) 4.5d / 10);
			double acceptableMax = (double) genomeLength * ((double) 5.5d / 10);

			Assert.LessOrEqual (underflowCount, acceptableMax);
			Assert.GreaterOrEqual (underflowCount, acceptableMin);
		}

		[SetUp]
		public void Setup()
		{
			ConfigurationProvider.Reset ();

			InitialiserProvider.Reset ();
			InitialiserContextProvider.Reset ();
			MutationProvider.Reset ();
		}
	}

	public class DoubleBasedSingleInitialiser : IndividualInitialiser<double>
	{
		double _value;

		public DoubleBasedSingleInitialiser (double value)
		{
			_value = value;
		}

		#region implemented abstract members of IndividualInitialiser

		protected override void BeforeInitialisation (Newtonsoft.Json.Linq.JObject properties)
		{

		}

		protected override double GetNextValue (int idx)
		{
			return _value;
		}

		public override double GetRandomValue ()
		{
			return _value;
		}

		#endregion
	}

	public class ByteBasedSingleInitialiser : IndividualInitialiser<byte>
	{
		byte _value;

		public ByteBasedSingleInitialiser (byte value)
		{
			_value = value;
		}

		#region implemented abstract members of IndividualInitialiser

		protected override void BeforeInitialisation (Newtonsoft.Json.Linq.JObject properties)
		{

		}

		protected override byte GetNextValue (int idx)
		{
			return _value;
		}

		public override byte GetRandomValue ()
		{
			return _value;
		}

		#endregion
	}

	public class IntBasedSingleInitialiser : IndividualInitialiser<int>
	{
		int _value;

		public IntBasedSingleInitialiser (int value)
		{
			_value = value;
		}

		#region implemented abstract members of IndividualInitialiser

		protected override void BeforeInitialisation (Newtonsoft.Json.Linq.JObject properties)
		{

		}

		protected override int GetNextValue (int idx)
		{
			return _value;
		}

		public override int GetRandomValue ()
		{
			return _value;
		}

		#endregion


	}
}

