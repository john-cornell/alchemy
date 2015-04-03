using System;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var tests = new CreatureIncubationTests ();
			tests.WhenCreatureContextCreated_ShortGenome_EnzymesShouldBeCorrect ();
		}
	}
}
