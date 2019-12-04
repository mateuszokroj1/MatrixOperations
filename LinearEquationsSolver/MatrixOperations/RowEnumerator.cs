using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MatrixOperations
{
    public class RowEnumerator<Tsource> : IEnumerator<Tsource[]> where Tsource : struct
    {
        public RowEnumerator(Tsource[][] array)
        {
            if (array == null)
                throw new ArgumentNullException();
            if (this.array.GetUpperBound(1)+1 < 1)
                throw new ArgumentException("Matrix row is too small.");
            this.array = array;
        }

        protected Tsource[][] array;
        protected int index = -1;

        public Tsource[] Current
        {
            get
            {
                if (index < 0)
                    throw new InvalidOperationException("Use first MoveNext method.");
                uint length = (uint)this.array.GetUpperBound(1)+1;
                Tsource[] ret = new Tsource[length];
                for (uint i = 0; i < length; i++)
                    ret[i] = this.array[index][(int)i];
                return ret;
            }
        }

        object IEnumerator.Current => this.Current;

        public void Dispose() { this.array = null; }
        public bool MoveNext()
        {
            if (this.index >= this.array.GetUpperBound(0))
                return false;

            this.index++;
            return true;

        }
        public void Reset() => this.index = -1;
    }
}
