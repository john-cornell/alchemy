using System;
using System.Collections.Generic;

namespace ObviousCode.Alchemy.Creatures.DecisionProcessing
{
	public class StackArray<T>
	{
		int _cursor = 0;
		T[] _stack;

		public StackArray (int length)
		{
			_stack = new T[length];
		}

		public void Clear ()
		{
			_cursor = 0;
		}

		public void Push (T item)
		{
			if (_cursor >= _stack.Length)
				throw new IndexOutOfRangeException ("StackArray stack is full");

			_stack [_cursor] = item;

			_cursor++;
		}

		public T Peek ()
		{
			if (_cursor == 0)
				throw new IndexOutOfRangeException ("StackArray stack is empty");

			return _stack [_cursor - 1];
		}

		public T Pop ()
		{
			T value = Peek ();

			_cursor -= 1;

			_stack [_cursor] = default(T);

			return value;
		}

		public T this [int index] {
			get { 

				if (_cursor <= index)
					throw new IndexOutOfRangeException ();

				return _stack [index]; 
			}
		}
	}
}

