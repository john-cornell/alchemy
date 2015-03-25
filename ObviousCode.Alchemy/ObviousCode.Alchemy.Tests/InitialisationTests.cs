using System;
using System.Linq;
using NUnit.Framework;
using ObviousCode.Alchemy.Library;
using ObviousCode.Alchemy.Library.Populous;

namespace ObviousCode.Alchemy.Tests
{
	[TestFixture]
	public class InitialisationTests
	{
		[SetUp]
		public void TestSetup()
		{
			InitialiserProvider.Reset ();
			InitialiserContextProvider.Reset ();
		}

		[Test]
		public void WhenInitialiserRequested_TypeExists_InitialiserShouldReturn()
		{
			var initialiser = InitialiserProvider.GetInitialiser<byte> ();

			Assert.IsNotNull (initialiser);
		}

		[Test]
		[ExpectedException(typeof(InitialiserNotAvailableException))]
		public void WhenInitialiserRequested_TypeDoesNotExist_InitialiserNotAvailableShouldBeThrown()
		{
			var initialiser = InitialiserProvider.GetInitialiser<UIntPtr> ();

			Assert.IsNotNull (initialiser);
		}

		[Test]
		public void WhenInitialiserRequested_TypeDoesNotExist_ResultingInitialiserNotAvailableExceptionShouldGiveCorrectType()
		{
			try
			{
				InitialiserProvider.GetInitialiser<UIntPtr>();
			}
			catch(InitialiserNotAvailableException e) {
				Assert.AreEqual (typeof(UIntPtr), e.RequestedInitialiserType);
			}
		}

		[Test]
		public void WhenInitialiserIsOverloaded_RequestReturnsOverloadedInitialiser()
		{
			NewByteBasedGenomeInitialiser newByteInitialiser = new NewByteBasedGenomeInitialiser ();

			InitialiserProvider.AddInitialiser (newByteInitialiser);

			var initialiser = InitialiserProvider.GetInitialiser<byte> ();

			Assert.AreEqual (newByteInitialiser.Name, initialiser.Name);
		}

		[Test]
		public void WhenIndividualIsInitialised_InitialiserIsRequestedFromIndividual_ShouldNotBeNull()
		{
			Individual<byte> individual = new Individual<byte> ();

			individual.Initialise (50);

			Assert.IsNotNull (individual.Initialiser);
		}

		[Test]
		public void WhenPopulationIsInitialised_InitialiserIsRequestedFromIndividual_ShouldNotBeNull()
		{
			Population<byte> population = new Population<byte> ();

			population.Initialise (1, 50);

			Assert.IsNotNull (population.Individuals [0].Initialiser);
		}

		[Test]
		public void WhenIndividualIsInitialised_InitialiserIsRequestedFromIndividual_ShouldHaveCorrectType()
		{
			Individual<byte> individual = new Individual<byte> ();

			individual.Initialise (2);

			Assert.AreEqual (typeof(byte), individual.Initialiser.GenomeType);
		}

		[Test]
		public void WhenPopulationIsInitialised_InitialiserIsRequestedFromIndividual_ShouldHaveCorrectType()
		{
			Population<byte> population = new Population<byte> ();

			population.Initialise (1, 1);

			Assert.AreEqual (typeof(byte), population.Individuals[0].Initialiser.GenomeType);
		}

		[Test]
		public void WhenIndividualIsInitialised_InitialiserIsRequestedFromIndividual_ShouldHaveCorrectName()
		{
			Individual<byte> individual = new Individual<byte> ();

			individual.Initialise (2);

			Assert.AreEqual (InitialiserProvider.GetInitialiser<byte>().Name, individual.Initialiser.Name);
		}

		[Test]
		public void WhenPopulationIsInitialised_InitialiserIsRequestedFromIndividual_ShouldHaveCorrectName()
		{
			Population<byte> population = new Population<byte> ();

			population.Initialise (5, 1);

			Assert.AreEqual (InitialiserProvider.GetInitialiser<byte>().Name, population.Individuals [0].Initialiser.Name);
		}

		[Test]
		[ExpectedException(typeof(InitialiserNotAvailableException))]
		public void WhenIndividualIsInitialised_NoInitialiserIsAvailable_InitialiserNotAvailableExceptionShouldBeThrown()
		{
			Individual<UIntPtr> individual = new Individual<UIntPtr> ();

			individual.Initialise (1);

			Assert.Pass ();
		}

		[Test]
		[ExpectedException(typeof(InitialiserNotAvailableException))]
		public void WhenPopulationIsInitialised_NoInitialiserIsAvailable_InitialiserNotAvailableExceptionShouldBeThrown()
		{
			Population<UIntPtr> population = new Population<UIntPtr> ();

			population.Initialise (1, 1);

			Assert.Pass ();
		}

		[Test]
		public void WhenBoolBasedPopulationIsInitialised_ShouldNotFail()
		{
			Population<bool> population = new Population<bool> ();

			population.Initialise (1, 1000);

			Assert.Pass ();
		}

		[Test]
		public void WhenByteBasedPopulationIsInitialised_ShouldNotFail()
		{
			Population<byte> population = new Population<byte> ();

			population.Initialise (1, 1);

			Assert.Pass ();
		}
			
		[Test]
		public void WhenDoubleBasedPopulationIsInitialised_ShouldNotFail()
		{
			Population<double> population = new Population<double> ();

			population.Initialise (100, 5000);

			Assert.Pass ();
		}

		[Test]
		public void WhenIntBasedPopulationIsInitialised_ShouldNotFail()
		{
			Population<int> population = new Population<int> ();

			population.Initialise (1, 1);
		}

		[Test]
		public void WhenIntBasedPopulationInInitialised_ValuesShouldBeCorrect()
		{
			int min = 10, max = 47;

			Population<int> population = new Population<int> ();

			IntBasedGenomeInitialiserContext context = new IntBasedGenomeInitialiserContext {
				Min = min,
				Max = max
			};

			InitialiserContextProvider.AddContext (context);

			population.Initialise (100, 5000);

			population.Individuals.ForEach (i => {
				foreach (int x in i.Code) {
					Assert.IsTrue (x >= min && x < max);
				}
			});
		}

		[Test]
		public void WhenIntBasedPopulationInInitialised_MinAndMaxEqual_ValuesShouldBeCorrect()
		{
			int min = 186, max = min;

			Population<int> population = new Population<int> ();

			var context = InitialiserContextProvider.GetContext<int> () as IntBasedGenomeInitialiserContext;

			context.Min = min;
			context.Max = max;

			population.Initialise (context, 100, 5000);

			population.Individuals.ForEach (i => {
				foreach(int x in i.Code){
					Assert.AreEqual(186, x);
				}
			});
		}

		//Assuming inclusive
		[Test]
		public void WhenDoubleBasedPopulationInitialised_DefaultContext_ValuesShouldBeBetween0And1(){
			Population<double> population = new Population<double> ();

			population.Initialise (100, 5000);

			population.Individuals.ForEach (i => {
				foreach(double x in i.Code){
					Assert.IsTrue(x>=0d && x<=1d);
				}
			});
		}
			
		[Test]
		public void WhenDoubleBasedPopulationInitialised_ValuesShouldBeBetweenMinAndMax(){

			var context = new DoubleBasedGenomeInitialiserContext ();

			context.Min = 15d;
			context.Max = 192d;

			InitialiserContextProvider.AddContext (context);

			Population<double> population = new Population<double> ();

			population.Initialise (1000, 5000);

			population.Individuals.ForEach (i => {
				foreach(double x in i.Code){
					Assert.IsTrue(x>=context.Min && x<=context.Max);//probably inclusive, but hey
				}
			});
		}
	}

	public class NewByteBasedGenomeInitialiser : IndividualInitialiser<byte>
	{
		protected override byte GetNextValue (int idx)
		{
			return (byte)255;
		}

		public override string Name {
			get {
				return "My Test Byte Initialiser";
			}
		}
			
		protected override void BeforeInitialisation (Newtonsoft.Json.Linq.JObject properties)
		{

		}

		public override byte GetRandomValue ()
		{
			throw new NotImplementedException ();
		}
	}
}

