using System;

namespace ObviousCode.Alchemy.Library
{
	public class IntBasedGenomeInitialiser : IndividualInitialiser<int>
	{	
		int _min;
		int _max;

		public IntBasedGenomeInitialiser ()
		{
		}

		protected override void BeforeInitialisation (Newtonsoft.Json.Linq.JObject properties)
		{
			_min = properties.Value<int> ("Min");
			_max = properties.Value<int> ("Max");
		}

		protected override int GetNextValue (int idx)
		{
			return GetRandomValue ();
		}

		public override int GetRandomValue ()
		{
			return _min == _max ? _min : ConfigurationProvider.Rnd.Next (_min, _max);
		}
	}
}

