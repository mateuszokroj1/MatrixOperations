using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;


//[assembly: InternalsVisibleTo("MatrixOperations.Tests")]

namespace MatrixOperations
{
    /// <summary>
    /// Basic matrix class. Values are saved in 2-dimensional array
    /// </summary>
    /// <typeparam name="Tsource"></typeparam>
    public class Matrix<Tsource> : IMatrix<Tsource>
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
        /// Creates new matrix from referenced array (not copy)
        /// </summary>
        /// <param name="array">Reference to array</param>
        /// <exception cref="ArgumentNullException"/>
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

            if (matrix.value.Where(row => row == null).Count() < 0)
                throw new ArgumentException("Some rows in matrix is null.");

            if(matrix.value.Length < 1 || matrix.value[0].Length < 1)
            {
                this.value = new Tsource[0][];
                return;
            }

            this.value = new Tsource[matrix.value.Length][];

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.value.Length))
                Parallel.For(0, this.value.Length, index =>
                {
                    this.value[index] = new Tsource[matrix.value[index].Length];
                    for (int column = 0; column < this.value[index].Length; column++)
                        this.value[index][column] = matrix.value[index][column];
                });
            else
                for(int row = 0; row < this.value.Length; row++)
                {
                    this.value[row] = new Tsource[matrix.value[row].Length];
                    for(int column = 0; column < this.value[row].Length; column++)
                        this.value[row][column] = matrix.value[row][column];
                }
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
        /// Create identity square <see cref="Matrix{Tsource}"/> <paramref name="count"/> X <paramref name="count"/> size
        /// </summary>
        /// <param name="count">Size of matrix</param>
        /// <returns>Identity matrix</returns>
        public static Matrix<Tsource> GenerateIdentity(uint count)
        {
            if (count == 0)
                return new Matrix<Tsource>(new Tsource[0][]);

            var array = new Tsource[count][];

            for (int i = 0; i < count; i++)
            {
                array[i] = new Tsource[count];
                array[i][i] = (dynamic)1;
            }

            return new Matrix<Tsource>(array);
        }

        /// <summary>
        /// Create diagonal matrix from vector (array)
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <returns>Diagonal matrix</returns>
        public static Matrix<Tsource> GenerateDiagonal(IEnumerable<Tsource> vector)
        {
            if (vector == null)
                throw new ArgumentNullException();

            Tsource[][] arr = new Tsource[vector.Count()][];

            for(int i = 0; i < vector.Count(); i++)
            {
                arr[i] = new Tsource[vector.Count()];

                for (int j = 0; j < vector.Count(); j++)
                    arr[i][j] = i == j ? vector.ElementAt(i) : (default);
            }

            return new Matrix<Tsource>(arr);
        }

        #endregion

        #region Methods

        public Matrix<Toutput> ConvertTo<Toutput>()
            where Toutput : struct, IEquatable<Toutput>
        {
            if (typeof(Tsource) == typeof(Toutput))
                return new Matrix<Toutput>((dynamic)this);

            Matrix<Toutput> newMatrix = new Matrix<Toutput>((uint)Rows.Count, (uint)Columns.Count);

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(Rows.Count))
            {
                Parallel.For(0, Rows.Count, index =>
                {
                    for (int column = 0; column < Columns.Count; column++)
                        newMatrix.value[index][column] = (dynamic)this.value[index][column];
                });
            }
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(Columns.Count))
            {
                for (int row = 0; row < Rows.Count; row++)
                    Parallel.For(0, Columns.Count, index =>
                        newMatrix.value[row][index] = (dynamic)this.value[row][index]
                    );
            }
            else
                for (int row = 0; row < Rows.Count; row++)
                    for (int column = 0; column < Columns.Count; column++)
                        newMatrix.value[row][column] = (dynamic)this.value[row][column];

            return newMatrix;
        }

        public static bool CheckIsSizeEqual(params Matrix<Tsource>[] matrices)
        => matrices
                .GroupBy(item => item.Rows.Count, item => item.Columns.Count)
                .Count() == 1;

        private static bool CheckMatricesHaveValidSizeForMultiplication(params Matrix<Tsource>[] matrices)
        {
            if (matrices == null || matrices.Where(matrix => matrix == null).Count() > 0)
                throw new ArgumentNullException();

            if (matrices.Length < 2)
                return false;

            for(int index = 0; index < matrices.Length-1; index++)
            {
                if (matrices[index].Columns.Count != matrices[index + 1].Rows.Count)
                    return false;
            }

            return true;
        }

        #region Multiplication

        public static Matrix<Tsource> Multiply(params Matrix<Tsource>[] matrices)
        {
            if (matrices == null)
                throw new ArgumentNullException();

            if (matrices.Length < 1)
                throw new ArgumentException("Minimum one matrix is required.");

            if (!CheckMatricesHaveValidSizeForMultiplication(matrices))
                throw new InvalidOperationException("Matrices haven't valid sizes for multiplication.");

            if (matrices.Length == 1)
                return matrices[0];

            Matrix<Tsource> calculated = new Matrix<Tsource>(matrices[0]);

            for(int i = 0; i < matrices.Length-1; i++)
                calculated = Multiply(calculated, matrices[1]);

            return calculated;
        }

        public static Matrix<Tsource> Multiply(Matrix<Tsource> a, Matrix<Tsource> b)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));

            if (b == null)
                throw new ArgumentNullException(nameof(b));

            if (!CheckMatricesHaveValidSizeForMultiplication(a, b))
                throw new InvalidOperationException("Matrices haven't valid sizes for multiplication.");

            Matrix<Tsource> calculated = new Matrix<Tsource>((uint)a.Rows.Count, (uint)b.Columns.Count);

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(calculated.Rows.Count))
                Parallel.For(0, calculated.Rows.Count, row =>
                {
                    for (int column = 0; column < calculated.Columns.Count; column++)
                    {
                        Tsource value = default;

                        for (int index = 0; index < a.Columns.Count; index++)
                            value += (dynamic)a[row, index] * b[index, column];

                        calculated[row, column] = value;
                    }
                });
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(calculated.Columns.Count))
                for (int row = 0; row < calculated.Rows.Count; row++)
                    Parallel.For(0, calculated.Columns.Count, column =>
                    {
                        Tsource value = default;

                        for (int index = 0; index < a.Columns.Count; index++)
                            value += (dynamic)a[row, index] * b[index, column];

                        calculated[row, column] = value;
                    });
            else
                for (int row = 0; row < calculated.Rows.Count; row++)
                    for (int column = 0; column < calculated.Columns.Count; column++)
                    {
                        Tsource value = default;

                        for (int index = 0; index < a.Columns.Count; index++)
                            value += (dynamic)a[row, index] * b[index, column];

                        calculated[row, column] = value;
                    }

            return calculated;
        }

        public static Matrix<Tsource> Multiply(Tsource scalar, Matrix<Tsource> matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            if (scalar.Equals(default))
                return new Matrix<Tsource>((uint)matrix.Rows.Count, (uint)matrix.Columns.Count);

            Matrix<Tsource> calculated = new Matrix<Tsource>((uint)matrix.Rows.Count, (uint)matrix.Columns.Count);

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(calculated.Rows.Count))
                Parallel.For(0, calculated.Rows.Count, row =>
                {
                    for (int column = 0; column < calculated.Columns.Count; column++)
                        calculated[row, column] = (dynamic)scalar * matrix[row,column];
                });
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(calculated.Columns.Count))
                for (int row = 0; row < calculated.Rows.Count; row++)
                    Parallel.For(0, calculated.Columns.Count, column =>
                    {
                        calculated[row, column] = (dynamic)scalar * matrix[row, column];
                    });
            else
                for (int row = 0; row < calculated.Rows.Count; row++)
                    for (int column = 0; column < calculated.Columns.Count; column++)
                    {
                        calculated[row, column] = (dynamic)scalar * matrix[row, column];
                    }

            return calculated;
        }

        public static Matrix<Tsource> Multiply(Tsource scalar, params Matrix<Tsource>[] matrices)
        => Multiply(scalar, Multiply(matrices));

        public static IEnumerable<Tsource> Multiply(IEnumerable<Tsource> vector, Matrix<Tsource> matrix)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector));

            if (matrix == null)
                throw new ArgumentNullException(nameof(vector));

            Tsource[] newVector = new Tsource[vector.Count()];

            if (vector.Count() == matrix.Rows.Count)
            {
                if (MatrixOperationsSettings.CheckIsParallelModeUseful(vector.Count()))
                    Parallel.For();
                else
                    for()
            }
            else if (vector.Count() == matrix.Columns.Count)
            {

            }
            else
                throw new InvalidOperationException("Invalid sizes for multiplication.");
        }

        public static IEnumerable<Tsource> Multiply(Matrix<Tsource> matrix, IEnumerable<Tsource> vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector));

            if (matrix == null)
                throw new ArgumentNullException(nameof(vector));

            if (vector.Count() == matrix.Rows.Count)
            {

            }
            else if (vector.Count() == matrix.Columns.Count)
            {

            }
            else
                throw new InvalidOperationException("Invalid sizes for multiplication.");
        }

        public static Matrix<Tsource> Sum(params Matrix<Tsource>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            if (matrices.Length < 1)
                throw new ArgumentException("Minimum one matrix is required.");
            else if (matrices.Length == 1)
                return matrices[0];

            var newmatrix = new Matrix<Tsource>((uint)matrices[0].Rows.Count, (uint)matrices[0].Columns.Count);

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrices[0].Rows.Count))
                Parallel.For(0, matrices[0].Rows.Count, row =>
                {
                    for(int column = 0; column < matrices[0].Columns.Count; column++)
                    {
                        Tsource value = default;
                        for (int matrixIndex = 0; matrixIndex < matrices.Length; matrixIndex++)
                            value += (dynamic) matrices[matrixIndex][row, column];

                        newmatrix[row, column] = value;
                    }
                });
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrices[0].Columns.Count))
                for (int row = 0; row < matrices[0].Rows.Count; row++)
                    Parallel.For(0, matrices[0].Columns.Count, column =>
                    {
                        Tsource value = default;
                        for (int matrixIndex = 0; matrixIndex < matrices.Length; matrixIndex++)
                            value += (dynamic)matrices[matrixIndex][row, column];

                        newmatrix[row, column] = value;
                    });
            else
                for(int row = 0; row < matrices[0].Rows.Count; row++)
                    for (int column = 0; column < matrices[0].Columns.Count; column++)
                    {
                        Tsource value = default;
                        for (int matrixIndex = 0; matrixIndex < matrices.Length; matrixIndex++)
                            value += (dynamic)matrices[matrixIndex][row, column];

                        newmatrix[row, column] = value;
                    }

            return newmatrix;
        }

        #endregion

        public static Matrix<Tsource> Difference(params Matrix<Tsource>[] matrices)
        {
            if (matrices == null)
                throw new ArgumentNullException();

            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to multiply");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");


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

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(Rows.Count))
            {

            }
            else if(MatrixOperationsSettings.CheckIsParallelModeUseful())

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

        /// <summary>
        /// Calculate determinant for square matrix or throw <see cref="InvalidOperationException"/> if <paramref name="matrix"/> is invalid.
        /// </summary>
        /// <returns>Determinant if exists</returns>
        /// <exception cref="InvalidOperationException"/>
        public static Tsource CalculateDeterminant<Tsource>(this Matrix<Tsource> matrix)
            where Tsource : struct, IEquatable<Tsource>
        {
            if (!matrix.IsSquare || matrix.Columns.Count < 1 || matrix.Rows.Count < 1)
                throw new InvalidOperationException("Matrix must be a square");

            if (matrix.Rows.Count == 1)
                return matrix[0, 0];

            if (matrix.Rows.Count == 2)
                return (dynamic)matrix[0, 0] * matrix[1, 1] - (dynamic)matrix[0, 1] * matrix[1, 0];

            if (matrix.Rows.Count == 3)
                return
                    (dynamic)matrix[0, 0] * matrix[1, 1] * matrix[2, 2]
                  + (dynamic)matrix[1, 0] * matrix[2, 1] * matrix[0, 2]
                  + (dynamic)matrix[2, 0] * matrix[0, 1] * matrix[1, 2]

                  - (dynamic)matrix[2, 0] * matrix[1, 1] * matrix[0, 2]
                  - (dynamic)matrix[0, 0] * matrix[2, 1] * matrix[1, 2]
                  - (dynamic)matrix[1, 0] * matrix[0, 1] * matrix[2, 2];
            else
            {
                var sum = (dynamic)matrix[0, 0] - matrix[0, 0];

                for (int column = 0; column < matrix.Columns.Count; column++)
                    sum += (dynamic)matrix[0, column]
                        * Math.Pow(-1, 2 + column)
                        * (matrix.SkipRow(0).SkipColumn((uint)column).CalculateDeterminant());

                return sum;
            }
        }

        /// <summary>
        /// Calculate Matrix^-1 = Transpose(Matrix) / Determinant(Matrix)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Inverted matrix</returns>
        public static Matrix<Tsource> Inversion<Tsource>(this Matrix<Tsource> matrix)
            where Tsource : struct, IEquatable<Tsource>
            => (dynamic)matrix.Transpose() * ((dynamic)1 / matrix.CalculateDeterminant());

        /// <summary>
        /// Checks if the given matrix is diagonal.
        /// In a diagonal matrix, non-diagonal values must have a default value.
        /// </summary>
        public static bool CheckIsDiagonal<Tsource>(this Matrix<Tsource> matrix)
            where Tsource : struct, IEquatable<Tsource>
        {
            if (matrix.Rows.Count == 0 || !matrix.IsSquare)
                return false;

            for (int row = 0; row < matrix.Rows.Count; row++)
                for (int column = 0; column < matrix.Columns.Count; column++)
                    if (row != column && !matrix.value[row][column].Equals(default))
                        return false;

            return true;
        }

        /// <summary>
        /// Get vector array if matrix have only one row or one column or is diagonal
        /// </summary>
        /// <returns>Vector array</returns>
        public static IEnumerable<Tsource> AsVector<Tsource>(this Matrix<Tsource> matrix)
            where Tsource : struct, IEquatable<Tsource>
        {
            if (matrix.Rows.Count == 0)
                return new Tsource[0];

            if (!matrix.IsVector || !matrix.CheckIsDiagonal())
                throw new InvalidOperationException("Matrix is not vector or diagonal");

            if (matrix.Rows.Count == 1)
                return matrix.Rows[0];
            else if (matrix.Columns.Count == 1)
                return matrix.Columns[0];
            else
            {
                Tsource[] ret = new Tsource[matrix.Rows.Count];

                for (int i = 0; i < matrix.Rows.Count; i++)
                    ret[i] = matrix.value[i][i];

                return ret;
            }
        }

        /// <summary>
        /// Multiplies all matrix cells with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static void MultiplyWithScalar<Tsource>(this Matrix<Tsource> matrix, Tsource scalarValue)
            where Tsource : struct, IEquatable<Tsource>
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if ((dynamic)scalarValue == 1)
                return;

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Rows.Count))
                Parallel.For(0, matrix.Rows.Count, rowIndex => MultiplyColumnWithScalar(matrix, rowIndex, scalarValue));
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(matrix.Columns.Count))
            {
                Parallel.For(0, matrix.Columns.Count, columnIndex =>
                {
                    for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                        matrix[rowIndex, columnIndex] *= (dynamic)scalarValue;
                });
            }
            else
                for (int rowIndex = 0; rowIndex < matrix.Rows.Count; rowIndex++)
                    MultiplyColumnWithScalar(matrix, rowIndex, scalarValue);
        }

        /// <summary>
        /// Multiplies selected column with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="IndexOutOfRangeException" />
        public static void MultiplyColumnWithScalar<Tsource>(this Matrix<Tsource> matrix, int rowIndex, Tsource scalarValue)
            where Tsource : struct, IEquatable<Tsource>
        {
            if (matrix == null)
                throw new ArgumentNullException();

            if (rowIndex < 0 || rowIndex >= matrix.Rows.Count)
                throw new IndexOutOfRangeException();

            if ((dynamic)scalarValue == 1)
                return;

            for (int columnIndex = 0; columnIndex < matrix.Columns.Count; columnIndex++)
                matrix[rowIndex, columnIndex] *= (dynamic)scalarValue;
        }
    }
}
