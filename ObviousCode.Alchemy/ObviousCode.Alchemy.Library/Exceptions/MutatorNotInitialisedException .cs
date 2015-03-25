using System;

namespace ObviousCode.Alchemy.Library
{
	[Serializable]
	public class MutatorNotInitialisedException : Exception
	{
		public Type RequestedMutatorType { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MutatorNotInitialisedException"/> class
		/// </summary>
		public MutatorNotInitialisedException (Type requestedMutatorType)
		{
			RequestedMutatorType = requestedMutatorType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MutatorNotInitialisedException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		public MutatorNotInitialisedException (Type requestedMutatorType, string message) : base (message)
		{
			RequestedMutatorType = requestedMutatorType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MutatorNotInitialisedException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. </param>
		public MutatorNotInitialisedException (Type requestedMutatorType, string message, Exception inner) : base (message, inner)
		{
			RequestedMutatorType = requestedMutatorType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MutatorNotInitialisedException"/> class
		/// </summary>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <param name="info">The object that holds the serialized object data.</param>
		protected MutatorNotInitialisedException (Type requestedMutatorType, System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
		{
			RequestedMutatorType = requestedMutatorType;
		}
	}
}

