using System;
using Newtonsoft.Json.Linq;

namespace ObviousCode.Alchemy.Library
{
	public class ByteBasedMutator : Mutator<byte>
	{
		bool AllowUnchecked { get; set; }

		IntBasedMutator _mutator;

		public ByteBasedMutator ()
		{
		}


		protected override void BeforeMutate ()
		{
			JObject properties = InitialiserContextProvider.GetContext<byte> ().Serialise ();

			Min = properties.Value<byte> ("Min");
			Max = properties.Value<byte> ("Max");

			//Let the int based stuff do our heavy lifting, rather than repeat code
			IntBasedGenomeInitialiserContext intContext = new IntBasedGenomeInitialiserContext ();

			intContext.Min = Min;
			intContext.Max = Max;

			_mutator = new IntBasedMutator (intContext);
			_mutator.LoadProperties ();

			AllowUnchecked = properties.Value<bool> ("AllowUnchecked");
		}			

		protected override byte Mutate (byte originalValue)
		{		
			return _mutator.Mutate (originalValue);
		}
	}
}

