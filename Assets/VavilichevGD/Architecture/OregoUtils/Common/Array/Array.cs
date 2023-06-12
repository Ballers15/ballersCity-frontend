using System;

namespace Orego.Util
{
    public sealed class Array<T>
    {
        private readonly T[] array;

        public int count => this.array.Length;
        
        public Array(int size, Func<T> initFunc = null)
        {
            this.array = new T[size];
            if (initFunc == null)
            {
                return;
            }

            for (var i = 0; i < size; i++)
            {
                this.array[i] = initFunc.Invoke();
            }
        }

        public T this[int index]
        {
            get { return this.array[index]; }
            set { this.array[index] = value; }
        }        
    }
}