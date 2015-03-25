using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ObviousCode.Alchemy.Library
{
	public abstract class InitialiserContext
	{
		public InitialiserContext ()
		{
		}			

		public virtual JObject Serialise()
		{
			BeforeSerialisation();

			JObject properties = JsonConvert.DeserializeObject<JObject> (
				JsonConvert.SerializeObject(this)
			);

			AfterSerialisation();

			return properties;
		}

		protected virtual void BeforeSerialisation (){
		}

		protected virtual void AfterSerialisation () {
		}

		public abstract string Name {get;}

		public abstract Type GenomeType {get;}
	}

	public class InitialiserContext<T> : InitialiserContext
	{
		public override string Name {
			get {
				return string.Format ("Base Initialser Context<{0}>", typeof(T).Name);
			}
		}

		public sealed override Type GenomeType
		{
			get { return typeof(T); }
		}
	}

}

