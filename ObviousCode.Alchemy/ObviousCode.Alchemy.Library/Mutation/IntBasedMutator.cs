using System;
using Newtonsoft.Json.Linq;

namespace ObviousCode.Alchemy.Library
{
	public class IntBasedMutator : Mutator<int>
	{
		IntBasedGenomeInitialiserContext _overrideContext;
	
		public IntBasedMutator ()
		{
			
		}

		public IntBasedMutator (IntBasedGenomeInitialiserContext overrideContext)
		{
			_overrideContext = overrideContext;
		}

		protected override void BeforeMutate ()
		{
			LoadProperties ();
		}

		public void LoadProperties ()
		{
			JObject properties;
			if (_overrideContext == null) {
				properties = InitialiserContextProvider.GetContext<int> ().Serialise ();
			}
			else {
				properties = _overrideContext.Serialise ();
			}
			Min = properties.Value<int> ("Min");
			Max = properties.Value<int> ("Max");
		}

		//Expose this simply for ByteBasedMutator
		internal byte Mutate(byte originalByte)
		{
			return (byte)Mutate ((int)originalByte);
		}

		protected override int Mutate (int originalValue)
		{		
			double variancePct = ConfigurationProvider.Mutation.ContextMutationVariance;

			int maxVariance = (int)Math.Floor ((double)(Max - Min) * variancePct);

			int negator = ConfigurationProvider.Rnd.NextDouble () < .5d ? -1 : 1;		

			if (maxVariance == 0)
				throw new InvalidOperationException ("Maximum Variance equated to 0.");

			int variance = ConfigurationProvider.Rnd.Next (maxVariance) + 1;
			
			return ApplyVarianceControls (originalValue + (variance * negator));
		}

	}
}

