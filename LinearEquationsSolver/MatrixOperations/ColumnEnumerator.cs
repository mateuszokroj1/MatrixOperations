using System;
using System.Collections;
using System.Collections.Generic;

namespace MatrixOperations
{
    public class ColumnEnumerator<Tsource> : IEnumerator<Tsource[]> where Tsource : struct
    {
        public ColumnEnumerator(Matrix<Tsource> matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            this.matrix = matrix;
        }

        protected Matrix<Tsource> matrix;
        protected int index = -1;

        public Tsource[] Current
        {
            get
            {
                if (index < 0)
                    throw new InvalidOperationException("Use first MoveNext method.");

                Tsource[] ret = new Tsource[this.matrix.Rows.Count];
                for (int i = 0; i < this.matrix.Rows.Count; i++)
                    ret[i] = this.matrix[i,index];

                return ret;
            }
        }

        object IEnumerator.Current => this.Current;

        public void Dispose() { this.matrix = null; }
        public bool MoveNext()
        {
            if (this.index >= this.matrix.Columns.Count)
                return false;

            this.index++;
            return true;
        }
        public void Reset() => this.index = -1;
    }
}
