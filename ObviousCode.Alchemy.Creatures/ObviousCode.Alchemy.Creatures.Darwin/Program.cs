using System;
using ObviousCode.Alchemy.Library.Populous;

namespace ObviousCode.Alchemy.Creatures.Darwin
{
	class MainClass
	{
		static Environment_RandomNumber _randoms;
		static int _generation;

		public static void Main (string[] args)
		{
			_randoms = new Environment_RandomNumber ();

			for (_generation = 0; _generation < 10000; _generation++) {

				_randoms.Engine.ExecuteOneGeneration ();			

				_randoms.BeforeGeneration ();
			}
		}
	}
}
