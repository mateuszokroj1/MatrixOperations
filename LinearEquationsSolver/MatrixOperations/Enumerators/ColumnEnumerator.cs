using System;
using System.Collections;
using System.Collections.Generic;

namespace MatrixOperations
{
    internal class ColumnEnumerator<Tsource> : IEnumerator<Tsource[]>
        where Tsource : struct, IEquatable<Tsource>
    {
        #region Fields

        private Matrix<Tsource> matrix;
        private int index = -1;

        #endregion

        #region Constructors

        public ColumnEnumerator(Matrix<Tsource> matrix) =>
            this.matrix = matrix ?? throw new ArgumentNullException();

        #endregion

        #region Properties

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

        #endregion

        #region Methods

        public void Dispose() => this.matrix = null;

        public bool MoveNext()
        {
            this.index++;
            if (this.index >= this.matrix.Columns.Count)
                return false;

            return true;
        }

        public void Reset() => this.index = -1;

        #endregion
    }
}
