using System;

namespace MatrixOperations
{
    public class Matrix<Tsource> : IEquatable<Matrix<Tsource>> where Tsource : struct
    {
        protected Tsource[][] value;

        public Matrix(Tsource[][] array)
        {
            if (array == null)
                throw new ArgumentNullException();

            value = array;
            Rows = new RowCollection<Tsource>(ref array);
            Columns = new ColumnCollection<Tsource>(ref array);
        }

        #region Properties

        public bool IsSquare => this.value.GetUpperBound(0) == this.value.GetUpperBound(1);

        public Tsource this[int index1, int index2]
        {
            get => this.value[index1][index2];
            set { this.value[index1][index2] = value; }
        }

        public RowCollection<Tsource> Rows { get; protected set; }

        public ColumnCollection<Tsource> Columns {get; protected set;}

        #endregion

        #region Static

        public static Matrix<double> GenerateIdentity(uint count)
        {

        }

        public static Matrix<TSource> GenerateDiagonal<TSource>(TSource[] vector)
        {

        }





        #endregion

        public bool Equals(Matrix<Tsource> other) => throw new NotImplementedException();
    }
}
