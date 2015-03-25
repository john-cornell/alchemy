using System;
using Newtonsoft.Json.Linq;

namespace ObviousCode.Alchemy.Library
{
	public abstract class IndividualInitialiser
	{
		public abstract Type GenomeType { get; }

		public abstract string Name {get;}
	}

	public abstract class IndividualInitialiser<T> : IndividualInitialiser
	{	
		public sealed override Type GenomeType
		{
			get { return typeof(T); }
		}

		public void Initialise (T[] code) { Initialise(InitialiserContextProvider.GetContext<T>().Serialise(), code); }

		public void Initialise (JObject properties, T[] code)
		{
			BeforeInitialisation (properties);

			Run (code);
		}

		protected abstract void BeforeInitialisation (JObject properties);

		public void Run(T[] code)
		{
			for (int i = 0; i < code.Length; i++) {
				code [i] = GetNextValue (i);
			}
		}

		protected abstract T GetNextValue (int idx);

		public abstract T GetRandomValue();

		protected virtual void AfterInitialisation() {
		}

		public override string Name {
			get {
				return string.Format ("Default {0} Initialiser", typeof(T).Name);
			}
		}
	}
}

