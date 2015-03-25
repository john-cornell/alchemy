using System;

namespace ObviousCode.Alchemy.Library
{
	public class BoolBasedGenomeInitialiser : IndividualInitialiser<bool>
	{
		public BoolBasedGenomeInitialiser ()
		{
		}

		protected override void BeforeInitialisation (Newtonsoft.Json.Linq.JObject properties){

		}		

		protected override bool GetNextValue (int idx)
		{
			return GetRandomValue ();
		}

		public override bool GetRandomValue ()
		{
			return ConfigurationProvider.Rnd.NextDouble () < .5d;
		}
	}
}

