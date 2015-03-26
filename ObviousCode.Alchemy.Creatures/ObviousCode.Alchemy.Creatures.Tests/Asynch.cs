using System;
using System.Threading.Tasks;

namespace ObviousCode.Alchemy.Creatures.Tests
{
	public class Asynch
	{
		public static void Main (string[] args)
		{
			new JohnTest().DoSlowWork();
			Console.Write("Click a key to stop program");
			Console.ReadLine();
		}
	}

	public class JohnTest
	{
		public async void DoSlowWork()
		{
			while (true)
			{
				await Task.Delay(1000);
				Console.Write("Progress Update");
			}
		}
	}
}

