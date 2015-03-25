using System;
using ObviousCode.Alchemy.Library.Populous;

namespace ObviousCode.Alchemy.Library
{
	public abstract class Mutator
	{
		public Mutator ()
		{
		}

		public Type GenomeType {get; protected set; }
	}

	public abstract class Mutator<T> : Mutator// where T : IComparable
	{
		protected T Min { get; set; }
		protected T Max { get; set; }

		public Mutator ()
		{
			GenomeType = typeof(T);

			Initialiser = InitialiserProvider.GetInitialiser<T>();
		}
			
		protected IndividualInitialiser<T> Initialiser { get; set;}

		protected abstract void BeforeMutate ();

		public void Mutate(Population<T> population)
		{
			BeforeMutate ();

			population.Individuals.ForEach (ind => {
				for(int i=0;i<ind.Code.Length;i++)
				{
					if (ConfigurationProvider.Rnd.NextDouble()<=ConfigurationProvider.Mutation.ChanceOfMutation)
					{
						ind.Code[i] = PerformMutation(ind.Code[i]);
					}
				}
			});
		}

		protected virtual T PerformMutation(T value)
		{
			T newValue = default(T);
			//Ensure mutation
			do 
			{
				if (ConfigurationProvider.Mutation.MutationMethod == MutationConfiguration.MutationStyle.Variance) {
					newValue = Mutate (value);			
				} 
				else {
					newValue = GetRandomValue ();				
				}

			} while(
				IsNotCheckedVariance &&
				value.Equals(newValue));

			return newValue;
		}

		private bool IsNotCheckedVariance { get { return !(
			    !ConfigurationProvider.Mutation.AllowUncheckedVariance &&
			    ConfigurationProvider.Mutation.MutationMethod == MutationConfiguration.MutationStyle.Variance); } }

		protected virtual T GetRandomValue (){
			return Initialiser.GetRandomValue ();
		}

		protected virtual T ApplyVarianceControls (T value)
		{
			if (GreaterThanOrEqualTo (value, Min) && LessThanOrEqualTo(value, Max))
				return value;

			if (LessThan (value, Min)) {
				if (ConfigurationProvider.Mutation.AllowUncheckedVariance)
					return ApplyUncheckedUnderflow (value);
				else
					return Min;					
			}

			//We're greater than Max here
			if (ConfigurationProvider.Mutation.AllowUncheckedVariance)
				return ApplyUncheckedOverflow (value);
			else
				return Max;				
		}

		protected virtual T ApplyUncheckedUnderflow (T value)
		{
			T underflow = Subtract(Min, value);

			value = Subtract(Max, underflow);

			if (LessThan(value, Min))
				throw new UncheckedVarianceOutOfBoundsException ();

			return value;
		}

		protected virtual T ApplyUncheckedOverflow (T value)
		{		
			T overflow = Subtract(value, Max);

			value = Add(Min, overflow);

			if (GreaterThan(value, Max))
				throw new UncheckedVarianceOutOfBoundsException ();

			return value;
		}

		protected abstract T Mutate(T originalValue);

		static T Add(T x, T y) {
			dynamic dx = x, dy = y;
			return dx + dy;
		}

		static T Subtract(T x, T y) {
			dynamic dx = x, dy = y;
			return dx - dy;
		}

		static bool GreaterThan(T value, T comparison) {
			dynamic dx = value, dy = comparison;
			return dx > dy;
		}

		static bool LessThan(T value, T comparison) {
			dynamic dx = value, dy = comparison;
			return dx < dy;
		}

		static bool GreaterThanOrEqualTo(T value, T comparison) {
			dynamic dx = value, dy = comparison;
			return dx >= dy;
		}

		static bool LessThanOrEqualTo(T value, T comparison) {
			dynamic dx = value, dy = comparison;
			return dx <= dy;
		}
	}
}

