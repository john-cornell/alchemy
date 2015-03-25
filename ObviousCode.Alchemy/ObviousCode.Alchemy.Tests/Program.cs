using System;

namespace ObviousCode.Alchemy.Tests
{
	public class Program
	{
		public Program ()
		{

		}

		public static void Main(string[] args)
		{
			var tests = new EngineTests ();

			tests.WhenNextGenerationCreated_FullMutation_FittestShouldBeFullyMutated ();
		}
	}
}

