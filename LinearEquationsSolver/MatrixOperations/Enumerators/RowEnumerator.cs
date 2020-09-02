using System;
using System.Collections;
using System.Collections.Generic;

namespace MatrixOperations
{
    internal class RowEnumerator<Tsource> : IEnumerator<Tsource[]>
        where Tsource : struct, IEquatable<Tsource>
    {
        #region Fields

        private Matrix<Tsource> matrix;
        private int index = -1;

        #endregion

        #region Constructors

        public RowEnumerator(Matrix<Tsource> matrix) =>
            this.matrix = matrix ?? throw new ArgumentNullException();

        #endregion

        #region Properties

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

        #endregion

        #region Methods

        public void Dispose() => this.matrix = null;

        public bool MoveNext()
        {
            this.index++;
            if (this.index >= this.matrix.Rows.Count)
                return false;

            return true;
        }

        public void Reset() => this.index = -1;

        #endregion
    }
}
