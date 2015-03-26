using System;
using System.Threading.Tasks;

namespace ObviousCode.Alchemy.Creatures.Darwin.Ui
{
	public class GenerationRunner
	{
		public event EventHandler<ProcessingStoppedEventArgs> ProcessesStopped;

		bool _stopRequested = false;

		public GenerationRunner ()
		{
		}

		public void RequestStop ()
		{
			_stopRequested = true;
		}

		public async void IterateGenerations (Environment evaluator, int iterations)
		{
			await Task.Run (() => {

				_stopRequested = false;

				int generations = 0;

				try {
					while (generations < iterations && !_stopRequested) {

						evaluator.BeforeGeneration ();

						evaluator.ExecuteGeneration ();

						generations += 1;
					}
				} catch (Exception e) {
					if (ProcessesStopped != null) {
						
						if (ProcessesStopped != null) {
							ProcessesStopped (this, ProcessingStoppedEventArgs.GetProcessingExceptionEventArgs (e));
						}

						return;
					}
				}

				if (ProcessesStopped != null) {
					if (_stopRequested) {
						ProcessesStopped (this, ProcessingStoppedEventArgs.GetProcessingRequestedStopEventArgs ());
					} else {
						ProcessesStopped (this, ProcessingStoppedEventArgs.GetProcessingCompletedEventArgs ());
					}
				} 
			});
		}
	}
}