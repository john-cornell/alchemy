using System;
using Newtonsoft.Json.Linq;

namespace ObviousCode.Alchemy.Library
{
	public class DoubleBasedMutator : Mutator<double>
	{
		public DoubleBasedMutator ()
		{
		}

		protected override void BeforeMutate ()
		{
			LoadProperties ();
		}

		public void LoadProperties ()
		{
			JObject properties = InitialiserContextProvider.GetContext<double> ().Serialise();

			Min = properties.Value<double> ("Min");
			Max = properties.Value<double> ("Max");
		}

		protected override double Mutate (double originalValue)
		{		
			double variancePct = ConfigurationProvider.Mutation.ContextMutationVariance;

			double maxVariance = Math.Floor ((Max - Min) * variancePct);

			double negator = ConfigurationProvider.Rnd.NextDouble () < .5d ? -1d : 1d;		

			if (maxVariance == 0)
				throw new InvalidOperationException ("Maximum Variance equated to 0.");

			double variance = ConfigurationProvider.Rnd.NextDouble() * maxVariance;

			return ApplyVarianceControls (originalValue + (variance * negator));
		}
	}
}

