using System;
using NUnit.Framework;
using ObviousCode.Alchemy.Library;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ObviousCode.Alchemy.Tests
{
	[TestFixture]
	public class InitialiserContextTests
	{
		[Test]
		public void WhenInitialiserContextProviderCalled_ContextExists_ShouldNotThrowException()
		{
			InitialiserContextProvider.GetContext<byte> ();

			Assert.Pass ();
		}

		[Test]
		public void WhenInitialiserContextProviderCalled_ContextExists_ShouldNotReturnNull()
		{
			Assert.IsNotNull (InitialiserContextProvider.GetContext<int> ());
		}

		[Test]
		public void WhenInitialiserContextProviderCalled_ContextExists_ContextShouldBeOfCorrectType()
		{
			var context = InitialiserContextProvider.GetContext<int> ();

			Assert.AreEqual (typeof(int), context.GenomeType);
		}

		[Test]
		[ExpectedException(typeof(InitialiserContextNotAvailableException))]
		public void WhenInitialiserContextProviderCalled_ContextDoesNotExist_ShouldThrowException()
		{
			InitialiserContextProvider.GetContext<UIntPtr> ();

			Assert.Fail ();
		}			

		[Test]
		public void WhenByteBasedContextRequested_ShouldReturn()
		{
			var context = InitialiserContextProvider.GetContext<byte> ();

			Assert.IsNotNull (context);
		}

		[Test]
		public void WhenIntBasedContextRequested_ShouldReturn()
		{
			var context = InitialiserContextProvider.GetContext<int> ();

			Assert.IsNotNull (context);
		}

		[Test]
		public void WhenIntBasedContextRequested_DefaultValues_MinValueShouldBe_0()
		{
			var context = InitialiserContextProvider.GetContext<int> ();

			JObject properties = context.Serialise ();

			Assert.AreEqual (0, properties.Value<int> ("Min"));
		}

		[Test]
		public void WhenIntBasedContextRequested_DefaultValues_MaxValueShouldBe_255()
		{
			var context = InitialiserContextProvider.GetContext<int> ();

			JObject properties = context.Serialise ();

			Assert.AreEqual (256, properties.Value<int> ("Max"));
		}

		[Test]
		public void WhenIntBasedContextCreated_DefaultValues_MinValueShouldBe_0()
		{
			IntBasedGenomeInitialiserContext context = new IntBasedGenomeInitialiserContext ();

			Assert.AreEqual (0, context.Min);
		}

		[Test]
		public void WhenIntBasedContextCreated_DefaultValues_MaxValueShouldBe_255()
		{
			IntBasedGenomeInitialiserContext context = new IntBasedGenomeInitialiserContext ();

			Assert.AreEqual (256, context.Max);
		}

		[Test]
		public void WhenIntBasedContextCreated_PassedValues_MinShouldBeUpdated()
		{
			IntBasedGenomeInitialiserContext context = new IntBasedGenomeInitialiserContext {
				Min = 100,
				Max = 101
			};

			Assert.AreEqual (100, context.Min);
		}

		[Test]
		public void WhenIntBasedContextCreated_PassedValues_MaxShouldBeUpdated()
		{
			IntBasedGenomeInitialiserContext context = new IntBasedGenomeInitialiserContext {
				Min = 100,
				Max = 101
			};

			Assert.AreEqual (101, context.Max);
		}

		[Test]
		public void WhenIntBasedContextCreated_MinAndMaxWrongWayAround_FixedInSerialisedObject()
		{
			IntBasedGenomeInitialiserContext context = new IntBasedGenomeInitialiserContext {
				Min = 100,
				Max = 50
			};

			JObject properties = context.Serialise ();

			IntBasedGenomeInitialiserContext deserialised = 
				JsonConvert.DeserializeObject<IntBasedGenomeInitialiserContext> (JsonConvert.SerializeObject(properties));

			Assert.IsTrue (deserialised.Min < deserialised.Max);
		}

		[Test]
		public void WhenIntBasedContextCreated_MinAndMaxWrongWayAround_FixedInContextAfterSerialisation()
		{
			IntBasedGenomeInitialiserContext context = new IntBasedGenomeInitialiserContext {
				Min = 100,
				Max = 50
			};

			context.Serialise ();

			Assert.IsTrue (context.Min < context.Max);
		}
	}
}

