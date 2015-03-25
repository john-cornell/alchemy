using System;
using ObviousCode.Alchemy.Library;

namespace ObviousCode.Alchemy.Creatures.Darwin
{
	public abstract class Environment: Evaluator<byte>
	{
		public string Label {
			get;
			private set;
		}

		public int LifetimeIterations {
			get;
			set;
		}

		public Environment (string label) 
		{
			Label = label;
			LifetimeIterations = 1000;
		}

		#region implemented abstract members of Evaluator

		protected override double EvaluateItem (ObviousCode.Alchemy.Library.Populous.Individual<byte> item)

		#endregion
	}
}

