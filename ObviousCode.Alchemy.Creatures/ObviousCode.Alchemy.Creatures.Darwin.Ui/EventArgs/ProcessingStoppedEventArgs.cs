using System;

namespace ObviousCode.Alchemy.Creatures.Darwin.Ui
{
	public class ProcessingStoppedEventArgs : EventArgs
	{
		public enum HaltReason
		{
			Complete,
			Requested,
			Exception

		}

		private ProcessingStoppedEventArgs ()
		{
			
		}

		public static ProcessingStoppedEventArgs GetProcessingCompletedEventArgs ()
		{
			return new ProcessingStoppedEventArgs {
				ReasonForStopping = HaltReason.Complete
			};
		}

		public static ProcessingStoppedEventArgs GetProcessingRequestedStopEventArgs ()
		{
			return new ProcessingStoppedEventArgs {
				ReasonForStopping = HaltReason.Requested
			};
		}

		public static ProcessingStoppedEventArgs GetProcessingExceptionEventArgs (Exception exception)
		{
			return new ProcessingStoppedEventArgs {
				ReasonForStopping = HaltReason.Exception,
				Exception = exception
			};
		}

		public HaltReason ReasonForStopping {
			get;
			private set;
		}

		public Exception Exception {
			get;
			private set;
		}
	}
}

