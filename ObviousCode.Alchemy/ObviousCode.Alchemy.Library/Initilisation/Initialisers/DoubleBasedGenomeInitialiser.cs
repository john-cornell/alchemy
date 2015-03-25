using System;

namespace ObviousCode.Alchemy.Library
{
	public class DoubleBasedGenomeInitialiser : IndividualInitialiser<double>
	{
		double _min;
		double _max;

		public DoubleBasedGenomeInitialiser ()
		{

		}

		protected override void BeforeInitialisation (Newtonsoft.Json.Linq.JObject properties)
		{
			_min = properties.Value<double> ("Min");
			_max = properties.Value<double> ("Max");
		}

		protected override double GetNextValue (int idx)
		{
			return GetRandomValue ();
		}
			
		public override double GetRandomValue ()
		{		
			if (_min == 0)
				return ConfigurationProvider.Rnd.NextDouble () * _max;

			double difference = _max - _min;		

			return (ConfigurationProvider.Rnd.NextDouble () * difference) + _min;
		}
	}
}

