using System;

namespace MatrixOperations
{
    public class Matrix<Tsource> where Tsource : struct
    {
        protected Tsource[][] value;

        public Matrix(Tsource[][] array)
        {
            if (array == null)
                throw new ArgumentNullException();

            value = array;
        }

        #region Properties

        public bool IsSquare => this.value.GetUpperBound(0) == this.value.GetUpperBound(1);

        public Tsource this[int index1, int index2]
        {
            get => this.value[index1][index2];
            set { this.value[index1][index2] = value; }
        }

        #endregion
    }
}
