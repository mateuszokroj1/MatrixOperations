using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MatrixOperations
{
    public class ColumnEnumerator<Tsource> : IEnumerator<Tsource[]>
    {
        public ColumnEnumerator(Tsource[][] array)
        {
            if (array == null)
                throw new ArgumentNullException();
            if (this.array.GetUpperBound(0) < 0)
                throw new ArgumentException("Matrix column is too small.");
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
                uint length = (uint)this.array.GetUpperBound(0) + 1;
                Tsource[] ret = new Tsource[length];
                for (uint i = 0; i < length; i++)
                    ret[i] = this.array[(int)i][index];
                return ret;
            }
        }

        object IEnumerator.Current => this.Current;

        public void Dispose() { this.array = null; }
        public bool MoveNext()
        {
            if (this.index >= this.array.GetUpperBound(1))
                return false;

            this.index++;
            return true;

        }
        public void Reset() => this.index = -1;
    }
}
