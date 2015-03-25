using System;

namespace ObviousCode.Alchemy.Library
{
	public class BoolBasedMutator : Mutator<bool>
	{
		public BoolBasedMutator ()
		{
		}

		#region implemented abstract members of Mutator

		protected override void BeforeMutate ()
		{

		}

		protected override bool Mutate (bool originalValue)
		{
			return !originalValue;
		}

		//Bools are a switch, so if we mutate - full or variance, variance pct, just flip that switch
		protected override bool PerformMutation (bool value)
		{
			return Mutate (value);
		}

		#endregion
	}
}

