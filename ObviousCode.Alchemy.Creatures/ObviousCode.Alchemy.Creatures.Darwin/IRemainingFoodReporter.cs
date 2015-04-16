using System;
using ObviousCode.Alchemy.Library.Populous;
using ObviousCode.Alchemy.Library;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.Darwin
{
	public interface IRemainingFoodReporter
	{
		Action<int> ReportRemainingFood { get; set; }
	}

}