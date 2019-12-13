using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MatrixOperations
{
    public class Matrix<Tsource> : IEquatable<Matrix<Tsource>> where Tsource : struct, IEquatable<Tsource>
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

        public bool IsVector => this.value.GetUpperBound(1) == 0 || this.value.GetUpperBound(0) == 0;

        public Tsource this[int index1, int index2]
        {
            get => this.value[index1][index2];
            set { this.value[index1][index2] = value; }
        }

        public RowCollection<Tsource> Rows { get; protected set; }

        public ColumnCollection<Tsource> Columns {get; protected set;}

        #endregion

        #region Static

        #region Generators

        public static Matrix<double> GenerateIdentity(uint count)
        {
            if (count == 0)
                return new Matrix<double>(new double[0][]);

            double[][] array = new double[count][];

            for (int i = 0; i < count; i++)
            {
                array[i] = new double[count];
                array[i][i] = 1;
            }

            return new Matrix<double>(array);
        }

        public static Matrix<TSource> GenerateDiagonal<TSource>(TSource[] vector)
        {
            if (vector == null)
                throw new ArgumentNullException();

            if(vector.Length == 0)
        }

        #region Transformation matrix
        public static Matrix<double> GenerateTranslate2D(double moveX, double moveY)
        {

        }

        public static Matrix<float> GenerateTranslate2D(float moveX, float moveY)
        {

        }

        public static Matrix<long> GenerateTranslate2D(long moveX, long moveY)
        {

        }

        public static Matrix<int> GenerateTranslate2D(int moveX, int moveY)
        {

        }

        public static Matrix<short> GenerateTranslate2D(short moveX, short moveY)
        {

        }

        public static Matrix<Complex> GenerateTranslate2D(Complex moveX, Complex moveY)
        {

        }

        public static Matrix<double> GenerateTranslate3D(double moveX, double moveY, double moveZ)
        {

        }

        public static Matrix<double> GenerateRotate2D(double angle, AngleMode angleMode, double centerX = 0, double centerY = 0)
        {

        }

        #endregion

        #endregion

        #region Operations

        public static Matrix<double> Multiply(params Matrix<double>[] matrices)
        {

        }

        public static Matrix<float> Multiply(params Matrix<float>[] matrices)
        {

        }

        public static Matrix<long> Multiply(params Matrix<long>[] matrices)
        {

        }

        public static Matrix<int> Multiply(params Matrix<int>[] matrices)
        {

        }

        #endregion

        #region Operators overload

            public static Matrix<double> operator+(Matrix<double> a, Matrix<double> b)
            {
                if (a.Rows.Count != b.Rows.Count || a.Columns.Count != b.Columns.Count)
                    throw new InvalidOperationException("Matrices must have the same size");
            }

            public static Matrix<float> operator +(Matrix<float> a, Matrix<float> b)
            {
                if (a.Rows.Count != b.Rows.Count || a.Columns.Count != b.Columns.Count)
                    throw new InvalidOperationException("Matrices must have the same size");
            }

            public static Matrix<double> operator-(Matrix<double> a, Matrix<double> b)
            {
                if (a.Rows.Count != b.Rows.Count || a.Columns.Count != b.Columns.Count)
                    throw new InvalidOperationException("Matrices must have the same size");
            }

            public static Matrix<float> operator -(Matrix<float> a, Matrix<float> b)
            {
                if (a.Rows.Count != b.Rows.Count || a.Columns.Count != b.Columns.Count)
                    throw new InvalidOperationException("Matrices must have the same size");
            }

            public static Matrix<double> operator*(Matrix<double> a, Matrix<double> b)
            {
                if (a.Columns.Count != b.Rows.Count)
                    throw new InvalidOperationException("Matrix A must have the same number of columns as matrix B have rows.");
            }

            public static Matrix<double> operator*(Matrix<double> a, double b)
            {
                Matrix<double> ret = a;
                for (int row = 0; row < a.Rows.Count; row++)
                    for (int column = 0; column < a.Columns.Count; column++)
                        ret[row, column] *= b;
                return ret;
            }

            public static Matrix<double> operator*(double a, Matrix<double> b)
            {

            }

            #endregion

        #endregion

        #region Methods

        public bool Equals(Matrix<Tsource> other)
        {
            if (other == null)
                throw new ArgumentNullException();

            if (this.value.GetUpperBound(0) != other.Rows.Count - 1 || this.value.GetUpperBound(1) != other.Columns.Count - 1)
                return false;

            for (int row = 0; row <= this.value.GetUpperBound(0); row++)
                for (int column = 0; column <= this.value.GetUpperBound(1); column++)
                    if (!this.value[row][column].Equals(other[row, column]))
                        return false;
            return true;
        }

        /// <summary>
        /// Create submatrix from this matrix in selected bounds
        /// </summary>
        /// <param name="firstRowIndex">Must be lower or equal than lastRowIndex</param>
        /// <param name="lastRowIndex"></param>
        /// <param name="firstColumnIndex"></param>
        /// <param name="lastColumnIndex"></param>
        /// <returns>SubMatrix</returns>
        public Matrix<Tsource> GenerateSubMatrix(uint firstRowIndex, uint lastRowIndex, uint firstColumnIndex, uint lastColumnIndex)
        {
            if (firstRowIndex >= Rows.Count)
                throw new IndexOutOfRangeException(nameof(firstRowIndex));

            if (lastRowIndex >= Rows.Count)
                throw new IndexOutOfRangeException(nameof(lastRowIndex));

            if (firstColumnIndex >= Columns.Count)
                throw new IndexOutOfRangeException(nameof(firstColumnIndex));

            if (lastColumnIndex >= Columns.Count)
                throw new IndexOutOfRangeException(nameof(lastColumnIndex));

            if (firstRowIndex > lastColumnIndex || firstColumnIndex > lastColumnIndex)
                return new Matrix<Tsource>(new Tsource[0][]);

            Tsource[,] array = new Tsource[lastRowIndex-firstRowIndex+1,lastColumnIndex-firstColumnIndex+1];

            for (int sourceRowIndex = (int)firstRowIndex, destRowIndex = 0; sourceRowIndex <= lastRowIndex && destRowIndex < Rows.Count; sourceRowIndex++, destRowIndex++)
                for (int sourceColumnIndex = (int)firstColumnIndex, destColumnIndex = 0; sourceColumnIndex <= lastColumnIndex && destColumnIndex < Columns.Count; sourceColumnIndex++, destColumnIndex++)
                    array[destRowIndex, destColumnIndex] = this[sourceRowIndex, sourceColumnIndex];

            return new Matrix<Tsource>(array as Tsource[][]);
        }

        /// <summary>
        /// Get vector array if matrix have only one row or one column
        /// </summary>
        /// <returns>Vector array</returns>
        public Tsource[] AsVector()
        {
            if (this.value.GetUpperBound(0) == 0)
                return Rows[0];
            else if (this.value.GetUpperBound(1) == 0)
                return Columns[0];
            else
                throw new InvalidOperationException("Matrix is not like vector");
        }

        public Matrix<Tsource> SkipColumn(uint columnIndex)
        {
            if (columnIndex >= Columns.Count)
                throw new IndexOutOfRangeException();

            Tsource[,] newarray = new Tsource[Rows.Count,Columns.Count-1];

            for (int row = 0; row < Rows.Count; row++)
            {
                for (int column = 0; column < columnIndex; column++)
                    newarray[row, column] = this[row,column];

                for (int column = (int)columnIndex; column < Columns.Count; column++)
                    newarray[row, column] = this[row,column+1];
            }
            return new Matrix<Tsource>(newarray as Tsource[][]);
        }

        public Matrix<Tsource> SkipRow(uint rowIndex)
        {
            if (rowIndex >= Rows.Count)
                throw new IndexOutOfRangeException();

            Tsource[][] newarray = new Tsource[Rows.Count-1][];
            
            for(int row = 0; row < rowIndex; row++)
                newarray[row] = Rows[row];

            for (int row = (int)rowIndex; row < Rows.Count; row++)
                newarray[row] = Rows[row + 1];

            return new Matrix<Tsource>(newarray);
        }

        /// <summary>
        /// Returns transposed matrix by replace row index with column index
        /// </summary>
        /// <returns></returns>
        public Matrix<Tsource> Transpose()
        {
            Tsource[,] newarray = new Tsource[Rows.Count,Columns.Count];
            for (int row = 0; row < Rows.Count; row++)
                for (int column = 0; column < Columns.Count; column++)
                    newarray[row, column] = this[column, row];

            return new Matrix<Tsource>(newarray as Tsource[][]);
        }

        #endregion
    }

    public enum AngleMode { Radians = 0, Degrees = 1 }

    public static class ExtensionMethods
    {
        #region CalculateDeterminant
        /// <summary>
        /// Calculate determinant for square matrix or throw InvalidOperationException if matrix is invalid.
        /// </summary>
        /// <returns>Determinant if exists</returns>
        /// <exception cref="InvalidOperationException"/>
        public static double CalculateDeterminant(this Matrix<double> matrix)
        {
            if (!matrix.IsSquare || matrix.Columns.Count < 1 || matrix.Rows.Count < 1)
                throw new InvalidOperationException("Matrix must be a square");

            if (matrix.Rows.Count == 1)
                return matrix[0,0];
            
            if (matrix.Rows.Count == 2)
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1,0];

            if (matrix.Rows.Count == 3)
                return
                    matrix[0,0]*matrix[1,1]*matrix[2,2]
                  + matrix[1,0]*matrix[2,1]*matrix[0,2]
                  + matrix[2,0]*matrix[0,1]*matrix[1,2]
                    
                  - matrix[2,0]*matrix[1,1]*matrix[0,2]
                  - matrix[0,0]*matrix[2,1]*matrix[1,2]
                  - matrix[1,0]*matrix[0,1]*matrix[2,2];
            else
            {
                var sum = matrix[0,0]-matrix[0,0];

                for(int column = 0; column < matrix.Columns.Count; column++)
                    sum += matrix[0, column]
                        * Math.Pow(-1, 2 + column)
                        * (matrix.SkipRow(0).SkipColumn((uint)column).CalculateDeterminant());

                return sum;
            }
        }

        /// <summary>
        /// Calculate determinant for square matrix or throw InvalidOperationException if matrix is invalid.
        /// </summary>
        /// <returns>Determinant if exists</returns>
        /// <exception cref="InvalidOperationException"/>
        public static float CalculateDeterminant(this Matrix<float> matrix)
        {
            if (!matrix.IsSquare || matrix.Columns.Count < 1 || matrix.Rows.Count < 1)
                throw new InvalidOperationException("Matrix must be a square");

            if (matrix.Rows.Count == 1)
                return matrix[0, 0];

            if (matrix.Rows.Count == 2)
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            if (matrix.Rows.Count == 3)
                return
                    matrix[0, 0] * matrix[1, 1] * matrix[2, 2]
                  + matrix[1, 0] * matrix[2, 1] * matrix[0, 2]
                  + matrix[2, 0] * matrix[0, 1] * matrix[1, 2]

                  - matrix[2, 0] * matrix[1, 1] * matrix[0, 2]
                  - matrix[0, 0] * matrix[2, 1] * matrix[1, 2]
                  - matrix[1, 0] * matrix[0, 1] * matrix[2, 2];
            else
            {
                var sum = matrix[0, 0] - matrix[0, 0];

                for (int column = 0; column < matrix.Columns.Count; column++)
                    sum += matrix[0, column]
                        * (float)Math.Pow(-1, 2 + column)
                        * (matrix.SkipRow(0).SkipColumn((uint)column).CalculateDeterminant());

                return sum;
            }
        }

        /// <summary>
        /// Calculate determinant for square matrix or throw InvalidOperationException if matrix is invalid.
        /// </summary>
        /// <returns>Determinant if exists</returns>
        /// <exception cref="InvalidOperationException"/>
        public static long CalculateDeterminant(this Matrix<long> matrix)
        {
            if (!matrix.IsSquare || matrix.Columns.Count < 1 || matrix.Rows.Count < 1)
                throw new InvalidOperationException("Matrix must be a square");

            if (matrix.Rows.Count == 1)
                return matrix[0, 0];

            if (matrix.Rows.Count == 2)
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            if (matrix.Rows.Count == 3)
                return
                    matrix[0, 0] * matrix[1, 1] * matrix[2, 2]
                  + matrix[1, 0] * matrix[2, 1] * matrix[0, 2]
                  + matrix[2, 0] * matrix[0, 1] * matrix[1, 2]

                  - matrix[2, 0] * matrix[1, 1] * matrix[0, 2]
                  - matrix[0, 0] * matrix[2, 1] * matrix[1, 2]
                  - matrix[1, 0] * matrix[0, 1] * matrix[2, 2];
            else
            {
                var sum = matrix[0, 0] - matrix[0, 0];

                for (int column = 0; column < matrix.Columns.Count; column++)
                    sum += matrix[0, column]
                        * (long)Math.Pow(-1, 2 + column)
                        * (matrix.SkipRow(0).SkipColumn((uint)column).CalculateDeterminant());

                return sum;
            }
        }

        /// <summary>
        /// Calculate determinant for square matrix or throw InvalidOperationException if matrix is invalid.
        /// </summary>
        /// <returns>Determinant if exists</returns>
        /// <exception cref="InvalidOperationException"/>
        public static int CalculateDeterminant(this Matrix<int> matrix)
        {
            if (!matrix.IsSquare || matrix.Columns.Count < 1 || matrix.Rows.Count < 1)
                throw new InvalidOperationException("Matrix must be a square");

            if (matrix.Rows.Count == 1)
                return matrix[0, 0];

            if (matrix.Rows.Count == 2)
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            if (matrix.Rows.Count == 3)
                return
                    matrix[0, 0] * matrix[1, 1] * matrix[2, 2]
                  + matrix[1, 0] * matrix[2, 1] * matrix[0, 2]
                  + matrix[2, 0] * matrix[0, 1] * matrix[1, 2]

                  - matrix[2, 0] * matrix[1, 1] * matrix[0, 2]
                  - matrix[0, 0] * matrix[2, 1] * matrix[1, 2]
                  - matrix[1, 0] * matrix[0, 1] * matrix[2, 2];
            else
            {
                var sum = matrix[0, 0] - matrix[0, 0];

                for (int column = 0; column < matrix.Columns.Count; column++)
                    sum += matrix[0, column]
                        * (int)Math.Pow(-1, 2 + column)
                        * (matrix.SkipRow(0).SkipColumn((uint)column).CalculateDeterminant());

                return sum;
            }
        }

        /// <summary>
        /// Calculate determinant for square matrix or throw InvalidOperationException if matrix is invalid.
        /// </summary>
        /// <returns>Determinant if exists</returns>
        /// <exception cref="InvalidOperationException"/>
        public static short CalculateDeterminant(this Matrix<short> matrix)
        {
            if (!matrix.IsSquare || matrix.Columns.Count < 1 || matrix.Rows.Count < 1)
                throw new InvalidOperationException("Matrix must be a square");

            if (matrix.Rows.Count == 1)
                return matrix[0, 0];

            if (matrix.Rows.Count == 2)
                return (short)(matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]);

            if (matrix.Rows.Count == 3)
                return (short)
                    (matrix[0, 0] * matrix[1, 1] * matrix[2, 2]
                  + matrix[1, 0] * matrix[2, 1] * matrix[0, 2]
                  + matrix[2, 0] * matrix[0, 1] * matrix[1, 2]

                  - matrix[2, 0] * matrix[1, 1] * matrix[0, 2]
                  - matrix[0, 0] * matrix[2, 1] * matrix[1, 2]
                  - matrix[1, 0] * matrix[0, 1] * matrix[2, 2]);
            else
            {
                var sum = matrix[0, 0] - matrix[0, 0];

                for (int column = 0; column < matrix.Columns.Count; column++)
                    sum += matrix[0, column]
                        * (short)Math.Pow(-1, 2 + column)
                        * (matrix.SkipRow(0).SkipColumn((uint)column).CalculateDeterminant());

                return (short)sum;
            }
        }

        /// <summary>
        /// Calculate determinant for square matrix or throw InvalidOperationException if matrix is invalid.
        /// </summary>
        /// <returns>Determinant if exists</returns>
        /// <exception cref="InvalidOperationException"/>
        public static Complex CalculateDeterminant(this Matrix<Complex> matrix)
        {
            if (!matrix.IsSquare || matrix.Columns.Count < 1 || matrix.Rows.Count < 1)
                throw new InvalidOperationException("Matrix must be a square");

            if (matrix.Rows.Count == 1)
                return matrix[0, 0];

            if (matrix.Rows.Count == 2)
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            if (matrix.Rows.Count == 3)
                return
                    matrix[0, 0] * matrix[1, 1] * matrix[2, 2]
                  + matrix[1, 0] * matrix[2, 1] * matrix[0, 2]
                  + matrix[2, 0] * matrix[0, 1] * matrix[1, 2]

                  - matrix[2, 0] * matrix[1, 1] * matrix[0, 2]
                  - matrix[0, 0] * matrix[2, 1] * matrix[1, 2]
                  - matrix[1, 0] * matrix[0, 1] * matrix[2, 2];
            else
            {
                var sum = matrix[0, 0] - matrix[0, 0];

                for (int column = 0; column < matrix.Columns.Count; column++)
                    sum += matrix[0, column]
                        * Math.Pow(-1, 2 + column)
                        * (matrix.SkipRow(0).SkipColumn((uint)column).CalculateDeterminant());

                return sum;
            }
        }

        /// <summary>
        /// Calculate determinant for square matrix or throw InvalidOperationException if matrix is invalid.
        /// </summary>
        /// <returns>Determinant if exists</returns>
        /// <exception cref="InvalidOperationException"/>
        public static BigInteger CalculateDeterminant(this Matrix<BigInteger> matrix)
        {
            if (!matrix.IsSquare || matrix.Columns.Count < 1 || matrix.Rows.Count < 1)
                throw new InvalidOperationException("Matrix must be a square");

            if (matrix.Rows.Count == 1)
                return matrix[0, 0];

            if (matrix.Rows.Count == 2)
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            if (matrix.Rows.Count == 3)
                return
                    matrix[0, 0] * matrix[1, 1] * matrix[2, 2]
                  + matrix[1, 0] * matrix[2, 1] * matrix[0, 2]
                  + matrix[2, 0] * matrix[0, 1] * matrix[1, 2]

                  - matrix[2, 0] * matrix[1, 1] * matrix[0, 2]
                  - matrix[0, 0] * matrix[2, 1] * matrix[1, 2]
                  - matrix[1, 0] * matrix[0, 1] * matrix[2, 2];
            else
            {
                var sum = matrix[0, 0] - matrix[0, 0];

                for (int column = 0; column < matrix.Columns.Count; column++)
                    sum += matrix[0, column]
                        * (int)Math.Pow(-1, 2 + column)
                        * (matrix.SkipRow(0).SkipColumn((uint)column).CalculateDeterminant());

                return sum;
            }
        }

        #endregion

        #region Inversion

        /// <summary>
        /// Calculate Matrix^-1 = Transpose(Matrix) / Determinant(Matrix)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Inverted matrix</returns>
        public static Matrix<double> Inversion(this Matrix<double> matrix) => matrix.Transpose() * (1 / matrix.CalculateDeterminant());

        /// <summary>
        /// Calculate Matrix^-1 = Transpose(Matrix) / Determinant(Matrix)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Inverted matrix</returns>
        public static Matrix<float> Inversion(this Matrix<float> matrix) => matrix.Transpose() * (1 / matrix.CalculateDeterminant());

        /// <summary>
        /// Calculate Matrix^-1 = Transpose(Matrix) / Determinant(Matrix)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Inverted matrix</returns>
        public static Matrix<long> Inversion(this Matrix<long> matrix) => matrix.Transpose() * (1 / matrix.CalculateDeterminant());

        /// <summary>
        /// Calculate Matrix^-1 = Transpose(Matrix) / Determinant(Matrix)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Inverted matrix</returns>
        public static Matrix<int> Inversion(this Matrix<int> matrix) => matrix.Transpose() * (1 / matrix.CalculateDeterminant());

        /// <summary>
        /// Calculate Matrix^-1 = Transpose(Matrix) / Determinant(Matrix)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Inverted matrix</returns>
        public static Matrix<short> Inversion(this Matrix<short> matrix) => matrix.Transpose() * (1 / matrix.CalculateDeterminant());

        /// <summary>
        /// Calculate Matrix^-1 = Transpose(Matrix) / Determinant(Matrix)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Inverted matrix</returns>
        public static Matrix<Complex> Inversion(this Matrix<Complex> matrix) => matrix.Transpose() * (1 / matrix.CalculateDeterminant());

        /// <summary>
        /// Calculate Matrix^-1 = Transpose(Matrix) / Determinant(Matrix)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Inverted matrix</returns>
        public static Matrix<BigInteger> Inversion(this Matrix<BigInteger> matrix) => matrix.Transpose() * (1 / matrix.CalculateDeterminant());

        #endregion
    }
}
