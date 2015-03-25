using System;
using System.Collections.Generic;
using ObviousCode.Alchemy.Library.Populous;

namespace ObviousCode.Alchemy.Library
{
	public class FitnessSelectionEventArgs<T> : EventArgs
	{
		public List<Individual<T>> Selection {
			get;
			set;
		}

		public FitnessSelectionEventArgs (List<Individual<T>> selection)
		{
			Selection = selection;
		}
	}
}

