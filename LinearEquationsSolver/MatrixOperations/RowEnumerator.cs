using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MatrixOperations
{
    public class RowEnumerator<Tsource> : IEnumerator<Tsource[]>
        where Tsource : struct, IEquatable<Tsource>
    {
        public RowEnumerator(Matrix<Tsource> matrix) =>
            this.matrix = matrix ?? throw new ArgumentNullException();

        protected Matrix<Tsource> matrix;
        protected int index = -1;

        public Tsource[] Current
        {
            get
            {
                if (index < 0)
                    throw new InvalidOperationException("Use first MoveNext method.");

                return this.matrix.value[index];
            }
        }

        object IEnumerator.Current => this.Current;

        public void Dispose() { this.matrix = null; }
        public bool MoveNext()
        {
            if (this.index >= this.matrix.Rows.Count)
                return false;

            this.index++;
            return true;
        }
        public void Reset() => this.index = -1;
    }
}
