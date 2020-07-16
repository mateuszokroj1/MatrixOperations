using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixOperations
{
    public class Matrix<Tsource> : IEquatable<Matrix<Tsource>>, ICloneable
        where Tsource : struct, IEquatable<Tsource>
    {
        #region Fields

        internal Tsource[][] value;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates empty matrix
        /// </summary>
        public Matrix()
        {
            this.value = new Tsource[0][];
        }

        /// <summary>
        /// Copy existing array to new matrix
        /// </summary>
        /// <param name="array">Values to copy</param>
        /// <exception cref="ArgumentNullException" />
        public Matrix(Tsource[][] array)
        {
            if (array == null)
                throw new ArgumentNullException();
            Tsource[][] newarr = new Tsource[array.Length][];
            array.CopyTo(newarr, 0);

            value = newarr;
            Rows = new RowCollection<Tsource>(this);
            Columns = new ColumnCollection<Tsource>(this);
        }

        /// <summary>
        /// Creates new matrix with referenced array
        /// </summary>
        /// <param name="array"></param>
        public Matrix(ref Tsource[][] array)
        {
            value = array ?? throw new ArgumentNullException();

            Rows = new RowCollection<Tsource>(this);
            Columns = new ColumnCollection<Tsource>(this);
        }

        /// <summary>
        /// Creates matrix with default values for <see cref="Tsource" /> 
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        public Matrix(uint rows, uint columns)
        {
            Tsource[][] arr;
            if (rows == 0 || columns == 0)
                arr = new Tsource[0][];
            else
            {
                arr = new Tsource[rows][];

                if (MatrixOperationsSettings.CheckIsParallelModeUseful(rows))
                    Parallel.For(0, rows, rowIndex => arr[rowIndex] = new Tsource[columns]);
                else
                    for (int rowIndex = 0; rowIndex < rows; rowIndex++)
                        arr[rowIndex] = new Tsource[columns];
            }

            this.value = arr;
        }

        /// <summary>
        /// Copies values from other matrix to new instance
        /// </summary>
        /// <param name="matrix"></param>
        /// <exception cref="ArgumentNullException" />
        public Matrix(Matrix<Tsource> matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if(matrix.value == null)
            {
                this.value = null;
                return;
            }

            this.value = new Tsource[matrix.value.Length][];
            matrix.value.CopyTo(this.value, 0);
        }


        #endregion

        #region Properties

        public bool IsSquare => Rows.Count == Columns.Count;

        public bool IsVector => Rows.Count == 1 || Columns.Count == 1;

        /// <summary>
        /// Gets or sets the selected value of matrix
        /// </summary>
        /// <returns>Value of selected element of matrix</returns>
        public Tsource this[int rowIndex, int columnIndex]
        {
            get => this.value[rowIndex][columnIndex];
            set { this.value[rowIndex][columnIndex] = value; }
        }

        public RowCollection<Tsource> Rows { get; protected set; }

        public ColumnCollection<Tsource> Columns { get; protected set; }

        #endregion

        #region Static

        #region Generators
        /// <summary>
        /// Create identity square matrix countXcount size
        /// </summary>
        /// <param name="count">Size of matrix</param>
        /// <returns>Identity matrix</returns>
        public static Matrix<float> GenerateIdentity<T>(uint count)
        {
            if (count == 0)
                return new Matrix<float>(new float[0][]);

            var array = new float[count][];

            for (int i = 0; i < count; i++)
            {
                array[i] = new float[count];
                array[i][i] = 1;
            }

            return new Matrix<float>(array);
        }

        /// <summary>
        /// Create diagonal matrix from vector (array)
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <returns>Diagonal matrix</returns>
        public static Matrix<TSource> GenerateDiagonal<TSource>(TSource[] vector) where TSource : struct
        {
            if (vector == null)
                throw new ArgumentNullException();

            TSource[][] arr = new TSource[vector.Length][];

            for(int i = 0; i < vector.Length; i++)
            {
                arr[i] = new TSource[vector.Length];

                for (int j = 0; j < vector.Length; j++)
                    if (i == j)
                        arr[i][j] = vector[i];
                    else
                        arr[i][j] = default;
            }

            return new Matrix<TSource>(arr);
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

        #region Methods

        public static bool CheckIsSizeEqual<TSource>(params Matrix<TSource>[] matrices) where TSource : struct
        => matrices
                .GroupBy(item => item.Rows.Count, item => item.Columns.Count)
                .Count() == 1;

        public static Matrix<decimal> Multiply(params Matrix<decimal>[] matrices)
        {

        }

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

        public static Matrix<short> Multiply(params Matrix<short>[] matrices)
        {

        }

        public static Matrix<byte> Multiply(params Matrix<byte>[] matrices)
        {

        }

        public static Matrix<BigInteger> Multiply(params Matrix<BigInteger>[] matrices)
        {

        }

        public static Matrix<Complex> Multiply(params Matrix<Complex>[] matrices)
        {

        }

        public static Matrix<decimal> Multiply(decimal a, params Matrix<decimal>[] matrices)
        {

        }

        public static Matrix<double> Multiply(double a, params Matrix<double>[] matrices)
        {

        }

        public static Matrix<float> Multiply(float a, params Matrix<float>[] matrices)
        {

        }

        public static Matrix<long> Multiply(long a, params Matrix<long>[] matrices)
        {

        }

        public static Matrix<int> Multiply(int a, params Matrix<int>[] matrices)
        {

        }

        public static Matrix<short> Multiply(short a, params Matrix<short>[] matrices)
        {

        }

        public static Matrix<byte> Multiply(byte a, params Matrix<byte>[] matrices)
        {

        }

        public static Matrix<BigInteger> Multiply(BigInteger a, params Matrix<BigInteger>[] matrices)
        {

        }

        public static Matrix<Complex> Multiply(Complex a, params Matrix<Complex>[] matrices)
        {

        }

        public static Matrix<decimal> Multiply(Matrix<decimal> matrix, decimal b)
        {

        }

        public static Matrix<double> Multiply(Matrix<double> matrix, double b)
        {

        }

        public static Matrix<float> Multiply(Matrix<float> matrix, float b)
        {

        }

        public static Matrix<long> Multiply(Matrix<long> matrix, long b)
        {

        }

        public static Matrix<int> Multiply(Matrix<int> matrix, int b)
        {

        }

        public static Matrix<short> Multiply(Matrix<short> matrix, short b)
        {

        }

        public static Matrix<byte> Multiply(Matrix<byte> matrix, byte b)
        {

        }

        public static Matrix<BigInteger> Multiply(Matrix<BigInteger> matrix, BigInteger b)
        {

        }

        public static Matrix<Complex> Multiply(Matrix<Complex> matrix, Complex b)
        {

        }

        public static Matrix<decimal> Multiply(decimal a, Matrix<decimal> matrix)
        {

        }
        public static Matrix<double> Multiply(double a, Matrix<double> matrix)
        {

        }

        public static Matrix<float> Multiply(float a, Matrix<float> matrix)
        {

        }

        public static Matrix<long> Multiply(long a, Matrix<long> matrix)
        {

        }

        public static Matrix<int> Multiply(int a, Matrix<int> matrix)
        {

        }

        public static Matrix<short> Multiply(short a, Matrix<short> matrix)
        {

        }

        public static Matrix<byte> Multiply(byte a, Matrix<byte> matrix)
        {

        }

        public static Matrix<BigInteger> Multiply(BigInteger a, Matrix<BigInteger> matrix)
        {

        }

        public static Matrix<Complex> Multiply(Complex a, Matrix<Complex> matrix)
        {

        }

        public static IEnumerable<decimal> Multiply(IEnumerable<decimal> vector, Matrix<decimal> matrix)
        {

        }

        public static IEnumerable<double> Multiply(IEnumerable<double> vector, Matrix<double> matrix)
        {

        }

        public static IEnumerable<float> Multiply(IEnumerable<float> vector, Matrix<float> matrix)
        {

        }

        public static IEnumerable<long> Multiply(IEnumerable<long> vector, Matrix<long> matrix)
        {

        }

        public static IEnumerable<int> Multiply(IEnumerable<int> vector, Matrix<int> matrix)
        {

        }

        public static IEnumerable<short> Multiply(IEnumerable<short> vector, Matrix<short> matrix)
        {

        }

        public static IEnumerable<byte> Multiply(IEnumerable<byte> vector, Matrix<byte> matrix)
        {

        }

        public static IEnumerable<BigInteger> Multiply(IEnumerable<BigInteger> vector, Matrix<BigInteger> matrix)
        {

        }

        public static IEnumerable<Complex> Multiply(IEnumerable<Complex> vector, Matrix<Complex> matrix)
        {

        }

        public static IEnumerable<decimal> Multiply(Matrix<decimal> matrix, IEnumerable<decimal> vector)
        {

        }

        public static IEnumerable<double> Multiply(Matrix<double> matrix, IEnumerable<double> vector)
        {

        }

        public static IEnumerable<float> Multiply(Matrix<float> matrix, IEnumerable<float> vector)
        {

        }

        public static IEnumerable<long> Multiply(Matrix<long> matrix, IEnumerable<long> vector)
        {

        }

        public static IEnumerable<int> Multiply(Matrix<int> matrix, IEnumerable<int> vector)
        {

        }

        public static IEnumerable<short> Multiply(Matrix<short> matrix, IEnumerable<short> vector)
        {

        }

        public static IEnumerable<byte> Multiply(Matrix<byte> matrix, IEnumerable<byte> vector)
        {

        }

        public static IEnumerable<BigInteger> Multiply(Matrix<BigInteger> matrix, IEnumerable<BigInteger> vector)
        {

        }

        public static IEnumerable<Complex> Multiply(Matrix<Complex> matrix, IEnumerable<Complex> vector)
        {

        }

        public static Matrix<decimal> Sum(params Matrix<decimal>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<decimal>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix += matrices[i];

            return newmatrix;
        }

        public static Matrix<double> Sum(params Matrix<double>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var arr = new double[matrices[0].Rows.Count][];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new double[matrices[0].Columns.Count];

                for (int j = 0; j < arr[i].Length; j++)
                    arr[i][j] = matrices[0][i, j];
            }

            var newmatrix = new Matrix<double>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix += matrices[i];

            return newmatrix;
        }

        public static Matrix<float> Sum(params Matrix<float>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<float>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix += matrices[i];

            return newmatrix;
        }

        public static Matrix<long> Sum(params Matrix<long>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<long>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix += matrices[i];

            return newmatrix;
        }

        public static Matrix<int> Sum(params Matrix<int>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<int>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix += matrices[i];

            return newmatrix;
        }

        public static Matrix<short> Sum(params Matrix<short>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<short>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix += matrices[i];

            return newmatrix;
        }

        public static Matrix<BigInteger> Sum(params Matrix<BigInteger>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<BigInteger>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix += matrices[i];

            return newmatrix;
        }

        public static Matrix<Complex> Sum(params Matrix<Complex>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<Complex>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix += matrices[i];

            return newmatrix;
        }

        public static Matrix<decimal> Difference(params Matrix<decimal>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<decimal>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix -= matrices[i];

            return newmatrix;
        }

        public static Matrix<double> Difference(params Matrix<double>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<double>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix -= matrices[i];

            return newmatrix;
        }

        public static Matrix<float> Difference(params Matrix<float>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<float>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix -= matrices[i];

            return newmatrix;
        }

        public static Matrix<long> Difference(params Matrix<long>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<long>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix -= matrices[i];

            return newmatrix;
        }

        public static Matrix<int> Difference(params Matrix<int>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<int>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix -= matrices[i];

            return newmatrix;
        }

        public static Matrix<short> Difference(params Matrix<short>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<short>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix -= matrices[i];

            return newmatrix;
        }

        public static Matrix<byte> Difference(params Matrix<byte>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<byte>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix -= matrices[i];

            return newmatrix;
        }

        public static Matrix<BigInteger> Difference(params Matrix<BigInteger>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<BigInteger>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix -= matrices[i];

            return newmatrix;
        }

        public static Matrix<Complex> Difference(params Matrix<Complex>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            var newmatrix = new Matrix<Complex>(matrices[0].value);

            for (int i = 1; i < matrices.Length; i++)
                newmatrix -= matrices[i];

            return newmatrix;
        }

        #endregion

        #endregion

        #region Methods

        public bool Equals(Matrix<Tsource> other)
        {
            if (other == null)
                throw new ArgumentNullException();

            if (Rows.Count != other.Rows.Count || Columns.Count != other.Columns.Count)
                return false;

            bool retValue = true;

            if(MatrixOperationsSettings.CheckIsParallelModeUseful())

            for (int row = 0; row < Rows.Count; row++)
                for (int column = 0; column < Columns.Count; column++)
                    if (!this.value[row][column].Equals(other[row, column]))
                        return false;
            return true;
        }

        /// <summary>
        /// Create submatrix from this matrix in selected bounds
        /// </summary>
        /// <param name="firstRowIndex">Must be lower or equal than lastRowIndex</param>
        /// <param name="lastRowIndex">Must be lower than number of rows</param>
        /// <param name="firstColumnIndex">Must be lower or equal than lastColumnIndex</param>
        /// <param name="lastColumnIndex">Must be lower than number of columns</param>
        /// <returns>SubMatrix</returns>
        public Matrix<Tsource> GenerateSubMatrix(int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex)
        {
            if (firstRowIndex < 0 || firstRowIndex >= Rows.Count)
                throw new IndexOutOfRangeException(nameof(firstRowIndex));

            if (lastRowIndex < 0 || lastRowIndex >= Rows.Count)
                throw new IndexOutOfRangeException(nameof(lastRowIndex));

            if (firstColumnIndex < 0 || firstColumnIndex >= Columns.Count)
                throw new IndexOutOfRangeException(nameof(firstColumnIndex));

            if (lastRowIndex < 0 || lastColumnIndex >= Columns.Count)
                throw new IndexOutOfRangeException(nameof(lastColumnIndex));

            if (firstRowIndex > lastColumnIndex || firstColumnIndex > lastColumnIndex)
                throw new InvalidOperationException("First index must be lower than or equal to last index");

            Tsource[][] array = new Tsource[lastRowIndex-firstRowIndex+1][];

            for (int sourceRowIndex = firstRowIndex, destRowIndex = 0; sourceRowIndex <= lastRowIndex && destRowIndex < Rows.Count; sourceRowIndex++, destRowIndex++)
            {
                array[destRowIndex] = new Tsource[lastColumnIndex - firstColumnIndex + 1];
                for (int sourceColumnIndex = firstColumnIndex, destColumnIndex = 0; sourceColumnIndex <= lastColumnIndex && destColumnIndex < Columns.Count; sourceColumnIndex++, destColumnIndex++)
                    array[destRowIndex][destColumnIndex] = this[sourceRowIndex, sourceColumnIndex];
            }

            return new Matrix<Tsource>(ref array);
        }

        public Matrix<Tsource> SkipColumn(uint columnIndex)
        {
            if (columnIndex >= Columns.Count)
                throw new IndexOutOfRangeException();

            Tsource[][] newarray = new Tsource[Rows.Count][];

            for (int row = 0; row < Rows.Count; row++)
            {
                newarray[row] = new Tsource[Columns.Count - 1];
                for (int column = 0; column < columnIndex; column++)
                    newarray[row][column] = this[row,column];

                for (int column = (int)columnIndex; column < Columns.Count; column++)
                    newarray[row][column] = this[row,column+1];
            }
            return new Matrix<Tsource>(newarray);
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
            Tsource[][] newarray = new Tsource[Rows.Count][];
            for (int row = 0; row < Rows.Count; row++)
            {
                newarray[row] = new Tsource[Columns.Count];
                for (int column = 0; column < Columns.Count; column++)
                    newarray[row][column] = this[column, row];
            }

            return new Matrix<Tsource>(newarray);
        }

        public object Clone() => new Matrix<Tsource>(value);

        #endregion
    }

    public enum AngleMode { Radians = 0, Degrees = 1 }

    /// <summary>
    /// Extension methods for <see cref="Matrix{Tsource}"/> class in selected Tsource types
    /// </summary>
    public static class ExtensionMethods
    {
        #region CalculateDeterminant
        /// <summary>
        /// Calculate determinant for square matrix or throw <see cref="InvalidOperationException"/> if <paramref name="matrix"/> is invalid.
        /// </summary>
        /// <returns>Determinant if exists</returns>
        /// <exception cref="InvalidOperationException"/>
        public static decimal CalculateDeterminant(this Matrix<decimal> matrix)
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
                        * (decimal)Math.Pow(-1, 2 + column)
                        * (matrix.SkipRow(0).SkipColumn((uint)column).CalculateDeterminant());

                return sum;
            }
        }

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
        public static Matrix<decimal> Inversion(this Matrix<decimal> matrix) => Matrix matrix.Transpose() * (1 / matrix.CalculateDeterminant());

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

        #region CheckIsDiagonal
        /// <summary>
        /// Checks if the given matrix is diagonal.
        /// In a diagonal matrix, non-diagonal values must have a default value.
        /// </summary>
        public static bool CheckIsDiagonal<TSource>(this Matrix<TSource> matrix)
            where TSource : struct, IEquatable<TSource>
        {
            if (matrix.Rows.Count == 0 || !matrix.IsSquare)
                return false;

            for (int row = 0; row < matrix.Rows.Count; row++)
                for (int column = 0; column < matrix.Columns.Count; column++)
                    if (row != column && !matrix.value[row][column].Equals(default))
                        return false;

            return true;
        }
        #endregion

        #region AsVector
        /// <summary>
        /// Get vector array if matrix have only one row or one column or is diagonal
        /// </summary>
        /// <returns>Vector array</returns>
        public static IEnumerable<TSource> AsVector<TSource>(this Matrix<TSource> matrix)
            where TSource : struct, IEquatable<TSource>
        {
            if (matrix.Rows.Count == 0)
                return new TSource[0];

            if (!matrix.IsVector || !matrix.CheckIsDiagonal())
                throw new InvalidOperationException("Matrix is not vector or diagonal");

            if (matrix.Rows.Count == 1)
                return matrix.Rows[0];
            else if (matrix.Columns.Count == 1)
                return matrix.Columns[0];
            else
            {
                TSource[] ret = new TSource[matrix.Rows.Count];

                for (int i = 0; i < matrix.Rows.Count; i++)
                    ret[i] = matrix.value[i][i];

                return ret;
            }
        }
        #endregion

        #region MultiplyWithScalar

        /// <summary>
        /// Multiplies all matrix cells with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static void MultiplyWithScalar(this Matrix<decimal> matrix, decimal scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (scalarValue == 1.0M)
                return;

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Rows.Count))
                Parallel.For(0, matrix.Rows.Count, rowIndex => MultiplyColumnWithScalar(matrix, rowIndex, scalarValue));
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Columns.Count))
            {
                Parallel.For(0, matrix.Columns.Count, columnIndex =>
                {
                    for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                        matrix[rowIndex, columnIndex] *= scalarValue;
                });
            }
            else
                for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                    MultiplyColumnWithScalar(matrix, rowIndex, scalarValue);
        }

        /// <summary>
        /// Multiplies all matrix cells with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static void MultiplyWithScalar(this Matrix<double> matrix, double scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (scalarValue == 1.0d)
                return;

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Rows.Count))
                Parallel.For(0, matrix.Rows.Count, rowIndex => MultiplyColumnWithScalar(matrix, rowIndex, scalarValue));
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Columns.Count))
            {
                Parallel.For(0, matrix.Columns.Count, columnIndex =>
                {
                    for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                        matrix[rowIndex, columnIndex] *= scalarValue;
                });
            }
            else
                for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                    MultiplyColumnWithScalar(matrix, rowIndex, scalarValue);
        }

        /// <summary>
        /// Multiplies all matrix cells with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static void MultiplyWithScalar(this Matrix<float> matrix, float scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (scalarValue == 1.0f)
                return;

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Rows.Count))
                Parallel.For(0, matrix.Rows.Count, rowIndex => MultiplyColumnWithScalar(matrix, rowIndex, scalarValue));
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Columns.Count))
            {
                Parallel.For(0, matrix.Columns.Count, columnIndex =>
                {
                    for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                        matrix[rowIndex, columnIndex] *= scalarValue;
                });
            }
            else
                for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                    MultiplyColumnWithScalar(matrix, rowIndex, scalarValue);
        }

        /// <summary>
        /// Multiplies all matrix cells with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static void MultiplyWithScalar(this Matrix<long> matrix, long scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (scalarValue == 1)
                return;

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Rows.Count))
                Parallel.For(0, matrix.Rows.Count, rowIndex => MultiplyColumnWithScalar(matrix, rowIndex, scalarValue));
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Columns.Count))
            {
                Parallel.For(0, matrix.Columns.Count, columnIndex =>
                {
                    for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                        matrix[rowIndex, columnIndex] *= scalarValue;
                });
            }
            else
                for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                    MultiplyColumnWithScalar(matrix, rowIndex, scalarValue);
        }

        /// <summary>
        /// Multiplies all matrix cells with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static void MultiplyWithScalar(this Matrix<int> matrix, int scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (scalarValue == 1)
                return;

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Rows.Count))
                Parallel.For(0, matrix.Rows.Count, rowIndex => MultiplyColumnWithScalar(matrix, rowIndex, scalarValue));
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Columns.Count))
            {
                Parallel.For(0, matrix.Columns.Count, columnIndex =>
                {
                    for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                        matrix[rowIndex, columnIndex] *= scalarValue;
                });
            }
            else
                for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                    MultiplyColumnWithScalar(matrix, rowIndex, scalarValue);
        }

        /// <summary>
        /// Multiplies all matrix cells with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static void MultiplyWithScalar(this Matrix<short> matrix, short scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (scalarValue == 1.0M)
                return;

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Rows.Count))
                Parallel.For(0, matrix.Rows.Count, rowIndex => MultiplyColumnWithScalar(matrix, rowIndex, scalarValue));
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Columns.Count))
            {
                Parallel.For(0, matrix.Columns.Count, columnIndex =>
                {
                    for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                        matrix[rowIndex, columnIndex] *= scalarValue;
                });
            }
            else
                for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                    MultiplyColumnWithScalar(matrix, rowIndex, scalarValue);
        }

        /// <summary>
        /// Multiplies all matrix cells with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static void MultiplyWithScalar(this Matrix<byte> matrix, byte scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (scalarValue == 1.0M)
                return;

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Rows.Count))
                Parallel.For(0, matrix.Rows.Count, rowIndex => MultiplyColumnWithScalar(matrix, rowIndex, scalarValue));
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Columns.Count))
            {
                Parallel.For(0, matrix.Columns.Count, columnIndex =>
                {
                    for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                        matrix[rowIndex, columnIndex] *= scalarValue;
                });
            }
            else
                for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                    MultiplyColumnWithScalar(matrix, rowIndex, scalarValue);
        }

        /// <summary>
        /// Multiplies all matrix cells with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static void MultiplyWithScalar(this Matrix<BigInteger> matrix, BigInteger scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (scalarValue == 1)
                return;

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Rows.Count))
                Parallel.For(0, matrix.Rows.Count, rowIndex => MultiplyColumnWithScalar(matrix, rowIndex, scalarValue));
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Columns.Count))
            {
                Parallel.For(0, matrix.Columns.Count, columnIndex =>
                {
                    for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                        matrix[rowIndex, columnIndex] *= scalarValue;
                });
            }
            else
                for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                    MultiplyColumnWithScalar(matrix, rowIndex, scalarValue);
        }

        /// <summary>
        /// Multiplies all matrix cells with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static void MultiplyWithScalar(this Matrix<Complex> matrix, Complex scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (scalarValue == Complex.One)
                return;

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Rows.Count))
                Parallel.For(0, matrix.Rows.Count, rowIndex => MultiplyColumnWithScalar(matrix, rowIndex, scalarValue));
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Columns.Count))
            {
                Parallel.For(0, matrix.Columns.Count, columnIndex =>
                {
                    for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                        matrix[rowIndex, columnIndex] *= scalarValue;
                });
            }
            else
                for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                    MultiplyColumnWithScalar(matrix, rowIndex, scalarValue);
        }

        #endregion

        #region MultiplyColumnWithScalar

        /// <summary>
        /// Multiplies selected column with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="IndexOutOfRangeException" />
        public static void MultiplyColumnWithScalar(this Matrix<decimal> matrix, int rowIndex, decimal scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (rowIndex < 0 || rowIndex >= matrix.Rows.Count)
                throw new IndexOutOfRangeException();

            if (scalarValue == 1.0M)
                return;

            for (int columnIndex = 0; columnIndex < matrix.Columns.Count; columnIndex++)
                matrix[rowIndex, columnIndex] *= scalarValue;
        }

        /// <summary>
        /// Multiplies selected column with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="IndexOutOfRangeException" />
        public static void MultiplyColumnWithScalar(this Matrix<double> matrix, int rowIndex, double scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (rowIndex < 0 || rowIndex >= matrix.Rows.Count)
                throw new IndexOutOfRangeException();

            if (scalarValue == 1.0d)
                return;

            for (int columnIndex = 0; columnIndex < matrix.Columns.Count; columnIndex++)
                matrix[rowIndex, columnIndex] *= scalarValue;
        }

        /// <summary>
        /// Multiplies selected column with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="IndexOutOfRangeException" />
        public static void MultiplyColumnWithScalar(this Matrix<float> matrix, int rowIndex, float scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (rowIndex < 0 || rowIndex >= matrix.Rows.Count)
                throw new IndexOutOfRangeException();

            if (scalarValue == 1.0f)
                return;

            for (int columnIndex = 0; columnIndex < matrix.Columns.Count; columnIndex++)
                matrix[rowIndex, columnIndex] *= scalarValue;
        }

        /// <summary>
        /// Multiplies selected column with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="IndexOutOfRangeException" />
        public static void MultiplyColumnWithScalar(this Matrix<long> matrix, int rowIndex, long scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (rowIndex < 0 || rowIndex >= matrix.Rows.Count)
                throw new IndexOutOfRangeException();

            if (scalarValue == 1)
                return;

            for (int columnIndex = 0; columnIndex < matrix.Columns.Count; columnIndex++)
                matrix[rowIndex, columnIndex] *= scalarValue;
        }

        /// <summary>
        /// Multiplies selected column with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="IndexOutOfRangeException" />
        public static void MultiplyColumnWithScalar(this Matrix<int> matrix, int rowIndex, int scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (rowIndex < 0 || rowIndex >= matrix.Rows.Count)
                throw new IndexOutOfRangeException();

            if (scalarValue == 1)
                return;

            for (int columnIndex = 0; columnIndex < matrix.Columns.Count; columnIndex++)
                matrix[rowIndex, columnIndex] *= scalarValue;
        }

        /// <summary>
        /// Multiplies selected column with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="IndexOutOfRangeException" />
        public static void MultiplyColumnWithScalar(this Matrix<short> matrix, int rowIndex, short scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (rowIndex < 0 || rowIndex >= matrix.Rows.Count)
                throw new IndexOutOfRangeException();

            if (scalarValue == 1)
                return;

            for (int columnIndex = 0; columnIndex < matrix.Columns.Count; columnIndex++)
                matrix[rowIndex, columnIndex] *= scalarValue;
        }

        /// <summary>
        /// Multiplies selected column with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="IndexOutOfRangeException" />
        public static void MultiplyColumnWithScalar(this Matrix<byte> matrix, int rowIndex, byte scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (rowIndex < 0 || rowIndex >= matrix.Rows.Count)
                throw new IndexOutOfRangeException();

            if (scalarValue == 1)
                return;

            for (int columnIndex = 0; columnIndex < matrix.Columns.Count; columnIndex++)
                matrix[rowIndex, columnIndex] *= scalarValue;
        }

        /// <summary>
        /// Multiplies selected column with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="IndexOutOfRangeException" />
        public static void MultiplyColumnWithScalar(this Matrix<BigInteger> matrix, int rowIndex, BigInteger scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (rowIndex < 0 || rowIndex >= matrix.Rows.Count)
                throw new IndexOutOfRangeException();

            if (scalarValue == 1)
                return;

            for (int columnIndex = 0; columnIndex < matrix.Columns.Count; columnIndex++)
                matrix[rowIndex, columnIndex] *= scalarValue;
        }

        /// <summary>
        /// Multiplies selected column with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="IndexOutOfRangeException" />
        public static void MultiplyColumnWithScalar(this Matrix<Complex> matrix, int rowIndex, Complex scalarValue)
        {
            if (matrix == null)
                throw new ArgumentNullException();


            if (rowIndex < 0 || rowIndex >= matrix.Rows.Count)
                throw new IndexOutOfRangeException();

            if (scalarValue == Complex.One)
                return;

            for (int columnIndex = 0; columnIndex < matrix.Columns.Count; columnIndex++)
                matrix[rowIndex, columnIndex] *= scalarValue;
        }

        #endregion
    }
}
