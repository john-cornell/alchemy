using NUnit.Framework;
using System;
using ObviousCode.Alchemy.Library.Populous;
using ObviousCode.Alchemy.Library.Exceptions;

namespace ObviousCode.Alchemy.Tests
{
	[TestFixture ()]
	public class PopulationTests
	{
		[Test ()]
		public void WhenPopulationIsInitialised_PopulationSizeShouldBeCorrectLength()
		{
			Population<int> population = new Population<int> ();

			population.Initialise (100, 40);

			Assert.AreEqual (100, population.Individuals.Count);
		}

		[Test ()]
		public void WhenPopulationIsInitialised_GenomeSizeShouldBeCorrectLength()
		{
			Population<byte> population = new Population<byte> ();

			population.Initialise (100, 40);

			Assert.AreEqual (40, population.Individuals [0].Code.Length);
		}

		[Test]
		public void WhenPopulationIsNotInitialised_IsInitialisedPropertyIsSetToFalse()
		{
			Population<float> population = new Population<float> ();

			Assert.IsFalse (population.IsInitialised);
		}

		[Test]
		[ExpectedException(typeof(PopulationNotInitialisedException))]
		public void WhenPopulationIsNotInitialised_IndividualCallThrowsNotInitialisedException()
		{
			Population<double> population = new Population<double> ();

			Individual individual = population.Individuals [0];
		}

		[Test]
		public void WhenPopulationIsInitialised_IsInitialisedPropertyIsSetToTrue()
		{
			Population<int> population = new Population<int> ();

			population.Initialise (100, 64);

			Assert.IsTrue (population.IsInitialised);
		}

		[Test]
		public void WhenPopulationIsInitialised_IndividualCallDoesNotThrowNotInitialisedException()
		{
			Population<int> population = new Population<int> ();

			population.Initialise (100, 64);

			Individual individual = population.Individuals [0];
		}

		[Test]
		public void WhenByteBasedPopulationIsCreated_GenomeShouldBeCorrectlySetOnPopulation()
		{
			Population<byte> population = new Population<byte> ();

			population.Initialise (100, 64);

			Assert.AreEqual (typeof(byte), population.GenomeType);
		}

		[Test]
		public void WhenIntBasedGenomeIsCreated_GenomeShouldBeCorrectlySetOnPopulation()
		{
			Population<int> population = new Population<int> ();

			population.Initialise (100, 64);

			Assert.AreEqual (typeof(Int32), population.GenomeType);
		}

		[Test]
		public void WhenDoubleBasedGenomeIsCreated_GenomeShouldBeCorrectlySetOnPopulation()
		{
			Population<double> population = new Population<double> ();

			population.Initialise (100, 64);

			Assert.AreEqual (typeof(Double), population.GenomeType);
		}
	}
}

