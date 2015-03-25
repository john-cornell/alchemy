using System;

namespace ObviousCode.Alchemy.Library.Exceptions
{
	
	[Serializable]
	public class PopulationNotInitialisedException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:PopulationNotInitialisedException"/> class
		/// </summary>
		public PopulationNotInitialisedException ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PopulationNotInitialisedException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		public PopulationNotInitialisedException (string message) : base (message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PopulationNotInitialisedException"/> class
		/// </summary>
		/// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. </param>
		public PopulationNotInitialisedException (string message, Exception inner) : base (message, inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PopulationNotInitialisedException"/> class
		/// </summary>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <param name="info">The object that holds the serialized object data.</param>
		protected PopulationNotInitialisedException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
		{
		}
	}
}

