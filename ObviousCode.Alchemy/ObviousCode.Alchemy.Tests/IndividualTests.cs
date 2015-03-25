using NUnit.Framework;
using System;
using ObviousCode.Alchemy.Library.Populous;
using ObviousCode.Alchemy.Library.Exceptions;

namespace ObviousCode.Alchemy.Tests
{
	[TestFixture ()]
	public class IndividualTests
	{
		[Test ()]
		public void WhenIndividualIsInitialised_ShouldBeCorrectLength()
		{
			Individual<byte> individual = Get<byte> ();

			Assert.AreEqual (40, individual.Code.Length);
		}

		[Test]
		public void WhenIndividualIsNotInitialised_IsInitialisedPropertyIsSetToFalse()
		{
			Individual<int> individual = new Individual<int> ();

			Assert.IsFalse (individual.IsInitialised);
		}

		[Test]
		[ExpectedException(typeof(GenomeNotInitialisedException))]
		public void WhenIndividualIsNotInitialised_GenomeCallThrowsNotInitialisedException()
		{
			Individual<int> individual = new Individual<int> ();

			Type genomeType = individual.GenomeType;
		}

		[Test]
		public void WhenIndividualIsInitialised_IsInitialisedPropertyIsSetToTrue()
		{
			Individual<int> individual = new Individual<int> ();

			individual.Initialise (40);

			Assert.IsTrue (individual.IsInitialised);
		}

		[Test]
		public void WhenIndividualIsInitialised_GenomeCallDoesNotThrowNotInitialisedException()
		{
			Individual<int> individual = new Individual<int> ();

			individual.Initialise (40);

			Type genome = individual.GenomeType;
		}

		[Test]
		public void WhenByteBasedGenomeIsCreated_GenomeShouldBeCorrectlySetOnIndividual()
		{
			var i = Get<byte> ();

			Assert.AreEqual (i.GenomeType, typeof(byte));
		}

		[Test]
		public void WhenIntBasedGenomeIsCreated_GenomeShouldBeCorrectlySetOnIndividual()
		{
			var i = Get<int> ();

			Assert.AreEqual (i.GenomeType, typeof(Int32));
		}

		[Test]
		public void WhenDoubleBasedGenomeIsCreated_GenomeShouldBeCorrectlySetOnIndividual()
		{
			var i = Get<double> ();

			Assert.AreEqual (i.GenomeType, typeof(Double));
		}

		private Individual<T> Get<T> ()
		{
			Individual<T> individual = new Individual<T> ();

			individual.Initialise (40);

			return individual;
		}
	}
}

