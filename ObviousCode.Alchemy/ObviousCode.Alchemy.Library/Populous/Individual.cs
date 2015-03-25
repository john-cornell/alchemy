using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ObviousCode.Alchemy.Library.Populous
{
	public class Individual
	{
		public string Uid {
			get;
			private set;
		}

		public Individual ()
		{
			IsInitialised = false;
			Uid = Guid.NewGuid ().ToString ();
		}

		public bool IsInitialised {
			get;
			protected set;
		}			

		Type _genomeType;
		public Type GenomeType { 
			get { 
				AssertIsInitialised ();

				return _genomeType;
			} 
			protected set { 
				_genomeType = value;
			}
		}

		protected void AssertIsInitialised ()
		{
			if (!IsInitialised)
				throw new Exceptions.GenomeNotInitialisedException ();
		}

	}

	public class Individual<T> : Individual
	{	
		public IndividualInitialiser Initialiser { get; private set; }

		public Individual ()
		{
			GenomeType = typeof(T);
		}

		public T[] _code;

		public T[] Code  {
			get {
				AssertIsInitialised ();
				return _code;
			}
			private set { _code = value; }
		}

		public T this[int idx]
		{
			get { return _code [idx]; }
			set { _code [idx] = value; }
		}

		public void Initialise()
		{
			Initialise (null, ConfigurationProvider.Individual.DefaultGenomeSize);
		}

		public void Initialise(int length)
		{
			Initialise (null, length);
		}

		public void Initialise(InitialiserContext<T> context)
		{
			Initialise (context, ConfigurationProvider.Individual.DefaultGenomeSize);
		}

		public void Initialise(InitialiserContext<T> context, int length)
		{
			Code = new T[length];

			Initialiser = InitialiserProvider.GetInitialiser<T> ();

			if (context == null) {
				(Initialiser as IndividualInitialiser<T>).Initialise (_code);
			} else {
				(Initialiser as IndividualInitialiser<T>).Initialise (context.Serialise(), _code);
			}
			IsInitialised = true;
		}	

		internal void InitialiseChild (int length)
		{
			Code = new T[length];

			IsInitialised = true;
		}

		public int GenomeLength { get { AssertIsInitialised (); return Code.Length; } }

		public double? Fitness { get; set; }
	}
}