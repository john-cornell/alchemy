using System;

namespace ObviousCode.Alchemy.Library
{
	[Serializable]
	public class InitialiserNotAvailableException : Exception
	{
		public Type RequestedInitialiserType { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:InitialiserNotAvailableException"/> class
		/// </summary>
		public InitialiserNotAvailableException (Type requestedInitialiserType)
		{
			RequestedInitialiserType = requestedInitialiserType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:InitialiserNotAvailableException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		public InitialiserNotAvailableException (Type requestedInitialiserType, string message) : base (message)
		{
			RequestedInitialiserType = requestedInitialiserType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:InitialiserNotAvailableException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. </param>
		public InitialiserNotAvailableException (Type requestedInitialiserType, string message, Exception inner) : base (message, inner)
		{
			RequestedInitialiserType = requestedInitialiserType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:InitialiserNotAvailableException"/> class
		/// </summary>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <param name="info">The object that holds the serialized object data.</param>
		protected InitialiserNotAvailableException (Type requestedInitialiserType, System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
		{
			RequestedInitialiserType = requestedInitialiserType;
		}
	}
}

