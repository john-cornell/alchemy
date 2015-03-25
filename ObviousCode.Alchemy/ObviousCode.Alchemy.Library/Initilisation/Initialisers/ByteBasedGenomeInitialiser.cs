using System;

namespace ObviousCode.Alchemy.Library
{
	public class ByteBasedGenomeInitialiser : IndividualInitialiser<byte>
	{
		byte _min;
		byte _max;

		protected override void BeforeInitialisation (Newtonsoft.Json.Linq.JObject properties)
		{
			_min = properties.Value<byte> ("Min");
			_max = properties.Value<byte> ("Max");
		}

		protected override byte GetNextValue (int idx)
		{
			return GetRandomValue ();
		}

		public override byte GetRandomValue ()
		{
			return _min == _max ? (byte) _min : (byte) ConfigurationProvider.Rnd.Next (_min, _max);
		}
	}
}

