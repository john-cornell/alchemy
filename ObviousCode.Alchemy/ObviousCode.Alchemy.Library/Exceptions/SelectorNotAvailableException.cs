using System;

namespace ObviousCode.Alchemy.Library
{
	
	[Serializable]
	public class SelectorNotAvailableException : Exception
	{
		public SelectionType RequestedSelector { get; private set;}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SelectorNotAvailableException"/> class
		/// </summary>
		public SelectorNotAvailableException (SelectionType requestedSelector)
		{
			RequestedSelector = requestedSelector;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SelectorNotAvailableException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		public SelectorNotAvailableException (SelectionType requestedSelector, string message) : base (message)
		{
			RequestedSelector = requestedSelector;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SelectorNotAvailableException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. </param>
		public SelectorNotAvailableException (SelectionType requestedSelector, string message, Exception inner) : base (message, inner)
		{
			RequestedSelector = requestedSelector;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SelectorNotAvailableException"/> class
		/// </summary>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <param name="info">The object that holds the serialized object data.</param>
		protected SelectorNotAvailableException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
		{
		}
	}
}