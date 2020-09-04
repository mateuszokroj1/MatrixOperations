using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        internal Tsource[][] value = new Tsource[0][];

        #endregion

        #region Constructors

        /// <summary>
        /// Creates empty matrix
        /// </summary>
        public Matrix()
        {
            Rows = new RowCollection<Tsource>(this);
            Columns = new ColumnCollection<Tsource>(this);
        }

        /// <summary>
        /// Copy existing array to new matrix
        /// </summary>
        /// <param name="array">Values to copy</param>
        /// <exception cref="ArgumentNullException" />
        public Matrix(Tsource[][] array) : this()
        {
            if (array == null)
                throw new ArgumentNullException();
            Tsource[][] newarr = new Tsource[array.Length][];
            array.CopyTo(newarr, 0);

            this.value = newarr;
        }

        /// <summary>
        /// Creates new matrix from referenced array (not copy)
        /// </summary>
        /// <param name="array">Reference to array</param>
        /// <exception cref="ArgumentNullException"/>
        public Matrix(ref Tsource[][] array) : this()
        {
            this.value = array ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Creates matrix with default values for <see cref="Tsource" /> 
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        public Matrix(int rows, int columns) : this()
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
        public Matrix(Matrix<Tsource> matrix) : this()
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

        public bool IsSquare => Rows.Count == Columns.Count && Rows.Count > 0;

        public bool IsVector => Rows.Count == 1 || Columns.Count == 1;

        /// <summary>
        /// Gets or sets the selected value of matrix
        /// </summary>
        /// <returns>Value of selected element of matrix</returns>
        /// <exception cref="IndexOutOfRangeException"/>
        public Tsource this[int rowIndex, int columnIndex]
        {
            get => this.value[rowIndex][columnIndex];
            set { this.value[rowIndex][columnIndex] = value; }
        }

        public RowCollection<Tsource> Rows { get; protected set; }

        public ColumnCollection<Tsource> Columns { get; protected set; }

        #endregion

        #region Static

        public static bool CheckIsSizeEqual(params Matrix<Tsource>[] matrices)
        {
            if (matrices == null || matrices.Where(matrix => matrix == null).Count() > 0)
                throw new ArgumentNullException();

            return matrices
                  .GroupBy(item => item.Rows.Count, item => item.Columns.Count)
                  .Count() == 1;
        }

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

            for (int i = 0; i < vector.Count(); i++)
            {
                arr[i] = new Tsource[vector.Count()];

                for (int j = 0; j < vector.Count(); j++)
                    arr[i][j] = i == j ? vector.ElementAt(i) : (default);
            }

            return new Matrix<Tsource>(arr);
        }

        #endregion

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

            var calculated = new Matrix<Tsource>(matrices[0]);

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

            var calculated = new Matrix<Tsource>(a.Rows.Count, b.Columns.Count);

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
                return new Matrix<Tsource>(matrix.Rows.Count, matrix.Columns.Count);

            var calculated = new Matrix<Tsource>(matrix.Rows.Count, matrix.Columns.Count);

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

        public static Matrix<Tsource> Multiply(Matrix<Tsource> matrix, Tsource scalar)
        => Multiply(scalar, matrix);

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
                    Parallel.For(0, vector.Count(), i =>
                    {
                        Tsource value = default;
                        for (int j = 0; j < vector.Count(); j++)
                            value += (dynamic)vector.ElementAt(j) * matrix[j, i];

                        newVector[i] = value;
                    });
                else
                    for(int i = 0; i < vector.Count(); i++)
                    {
                        Tsource value = default;
                        for (int j = 0; j < vector.Count(); j++)
                            value += (dynamic)vector.ElementAt(j) * matrix[j, i];

                        newVector[i] = value;
                    }
            }
            else if (vector.Count() == matrix.Columns.Count)
            {
                if (MatrixOperationsSettings.CheckIsParallelModeUseful(vector.Count()))
                    Parallel.For(0, vector.Count(), i =>
                    {
                        Tsource value = default;
                        for (int j = 0; j < vector.Count(); j++)
                            value += (dynamic)vector.ElementAt(j) * matrix[i, j];

                        newVector[i] = value;
                    });
                else
                    for (int i = 0; i < vector.Count(); i++)
                    {
                        Tsource value = default;
                        for (int j = 0; j < vector.Count(); j++)
                            value += (dynamic)vector.ElementAt(j) * matrix[i, j];

                        newVector[i] = value;
                    }
            }
            else
                throw new InvalidOperationException("Invalid sizes for multiplication.");

            return newVector;
        }

        #endregion

        #region Sum

        public static Matrix<Tsource> Sum(params Matrix<Tsource>[] matrices)
        {
            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to sum.");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            if (matrices.Length < 1)
                throw new ArgumentException("Minimum one matrix is required.");
            else if (matrices.Length == 1)
                return matrices[0];

            int rows = matrices[0].Rows.Count, columns = matrices[0].Columns.Count;

            var newmatrix = new Matrix<Tsource>(rows, columns);

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(rows))
                Parallel.For(0, (int)rows, row =>
                {
                    for(int column = 0; column < columns; column++)
                    {
                        Tsource value = default;
                        for (int matrixIndex = 0; matrixIndex < matrices.Length; matrixIndex++)
                            value += (dynamic) matrices[matrixIndex][row, column];

                        newmatrix[row, column] = value;
                    }
                });
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(columns))
                for (int row = 0; row < rows; row++)
                    Parallel.For(0, (int)columns, column =>
                    {
                        Tsource value = default;
                        for (int matrixIndex = 0; matrixIndex < matrices.Length; matrixIndex++)
                            value += (dynamic)matrices[matrixIndex][row, column];

                        newmatrix[row, column] = value;
                    });
            else
                for(int row = 0; row < rows; row++)
                    for (int column = 0; column < columns; column++)
                    {
                        Tsource value = default;
                        for (int matrixIndex = 0; matrixIndex < matrices.Length; matrixIndex++)
                            value += (dynamic)matrices[matrixIndex][row, column];

                        newmatrix[row, column] = value;
                    }

            return newmatrix;
        }

        #endregion

        #region Difference

        public static Matrix<Tsource> Difference(params Matrix<Tsource>[] matrices)
        {
            if (matrices == null)
                throw new ArgumentNullException();

            if (matrices.Length < 1)
                throw new InvalidOperationException("No matrices to difference.");

            if (!CheckIsSizeEqual(matrices))
                throw new InvalidOperationException("Sizes are not equal.");

            if (matrices.Length == 1)
                return matrices[0];

            int rows = matrices[0].Rows.Count, columns = matrices[0].Columns.Count;

            var calculated = new Matrix<Tsource>(rows, columns);

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(rows))
                Parallel.For(0, rows, row =>
                {
                    for (int column = 0; column < columns; column++)
                    {
                        Tsource value = default;

                        for (int i = 0; i < matrices.Length; i++)
                            value -= (dynamic)matrices[i][row, column];

                        calculated[row, column] = value;
                    }
                });
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(columns))
                for (int row = 0; row < rows; row++)
                    Parallel.For(0, columns, column =>
                    {
                        Tsource value = default;

                        for (int i = 0; i < matrices.Length; i++)
                            value -= (dynamic)matrices[i][row, column];

                        calculated[row, column] = value;
                    });
            else
                for (int row = 0; row < rows; row++)
                    for (int column = 0; column < columns; column++)
                    {
                        Tsource value = default;

                        for (int i = 0; i < matrices.Length; i++)
                            value -= (dynamic)matrices[i][row,column];

                        calculated[row, column] = value;
                    }

            return calculated;
        }

        #endregion

        #endregion

        #region Methods

        public Matrix<Toutput> ConvertTo<Toutput>()
            where Toutput : struct, IEquatable<Toutput>
        {
            if (typeof(Tsource) == typeof(Toutput))
                return new Matrix<Toutput>((dynamic)this);

            var newMatrix = new Matrix<Toutput>(Rows.Count, Columns.Count);

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

        public bool Equals(Matrix<Tsource> other)
        {
            if (other == null)
                throw new ArgumentNullException();

            if (Rows.Count != other.Rows.Count || Columns.Count != other.Columns.Count)
                return false;

            bool retValue = true;

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(Rows.Count))
                Parallel.For(0, Rows.Count, (row, state) =>
                {
                    for (int column = 0; column < Columns.Count; column++)
                    {
                        if (!this[row, column].Equals(other[row, column]))
                        {
                            retValue = false;
                            state.Break();
                            return;
                        }
                    }
                });
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(Columns.Count))
                for (int row = 0; row < Rows.Count; row++)
                    Parallel.For(0, Columns.Count, (column, state) =>
                    {
                        if (!this[row, column].Equals(other[row, column]))
                        {
                            retValue = false;
                            state.Break();
                            return;
                        }
                    });
            else
                for (int row = 0; row < Rows.Count; row++)
                    for (int column = 0; column < Columns.Count; column++)
                        if (!this[row, column].Equals(other[row, column]))
                            return false;

            return retValue;
        }

        public override bool Equals(object obj)
        => obj is Matrix<Tsource> obj2 ? Equals(obj2) : base.Equals(obj);

        public override int GetHashCode()
        => this.value.GetHashCode();

        public override string ToString()
        {
            var output = new StringBuilder();
            if ((this.value?.Length ?? 0) == 0 || (this.value[0]?.Length ?? 0) == 0)
                return string.Empty;

            output.Append("{");

            for (int row = 0; row < this.value.Length; row++)
            {
                foreach (var value in this.value[row])
                    output.Append($" {value}");

                if(row == this.value.Length+1)
                    output.Append(" |");
            }

            output.Append(" }");

            return output.ToString();
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

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(Rows.Count))
                Parallel.For(0, Rows.Count, row =>
                {
                    newarray[row] = new Tsource[Columns.Count - 1];
                    for (int column = 0; column < columnIndex; column++)
                        newarray[row][column] = this[row, column];

                    for (int column = (int)columnIndex; column < Columns.Count; column++)
                        newarray[row][column] = this[row, column + 1];
                });
            else
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
        /// Multiplies all matrix cells with scalar value
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public void MultiplyWithScalar(Tsource scalarValue)
        {
            if ((dynamic)scalarValue == 1)
                return;

            if (MatrixOperationsSettings.CheckIsParallelModeUseful(Rows.Count))
                Parallel.For(0, Rows.Count, rowIndex => Rows.MultiplyWithScalar(rowIndex, scalarValue));
            else if (MatrixOperationsSettings.CheckIsParallelModeUseful(Columns.Count))
            {
                Parallel.For(0, Columns.Count, columnIndex =>
                {
                    for (int rowIndex = 0; rowIndex < Rows.Count; rowIndex++)
                        this[rowIndex, columnIndex] *= (dynamic)scalarValue;
                });
            }
            else
                for (int rowIndex = 0; rowIndex < Rows.Count; rowIndex++)
                    Rows.MultiplyWithScalar(rowIndex, scalarValue);
        }

        /// <summary>
        /// Calculate determinant for square matrix or throw <see cref="InvalidOperationException"/> if <paramref name="matrix"/> is invalid.
        /// </summary>
        /// <returns>Determinant if exists</returns>
        /// <exception cref="InvalidOperationException"/>
        public Tsource CalculateDeterminant()
        {
            if (!IsSquare || Columns.Count < 1 || Rows.Count < 1)
                throw new InvalidOperationException("Matrix must be a square");

            if (Rows.Count == 1)
                return this[0, 0];

            if (Rows.Count == 2)
                return (dynamic)this[0, 0] * this[1, 1] - (dynamic)this[0, 1] * this[1, 0];

            if (Rows.Count == 3)
                return
                    (dynamic)this[0, 0] * this[1, 1] * this[2, 2]
                  + (dynamic)this[1, 0] * this[2, 1] * this[0, 2]
                  + (dynamic)this[2, 0] * this[0, 1] * this[1, 2]

                  - (dynamic)this[2, 0] * this[1, 1] * this[0, 2]
                  - (dynamic)this[0, 0] * this[2, 1] * this[1, 2]
                  - (dynamic)this[1, 0] * this[0, 1] * this[2, 2];
            else
            {
                Tsource sum = default;

                for (int column = 0; column < Columns.Count; column++)
                    sum += (dynamic)this[0, column]
                        * Math.Pow(-1, 2 + column)
                        * SkipRow(0).SkipColumn((uint)column).CalculateDeterminant();

                return sum;
            }
        }

        /// <summary>
        /// Calculate Matrix^-1 = Transpose(Matrix) / Determinant(Matrix)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Inverted matrix</returns>
        public Matrix<Tsource> Inversion()
            => (dynamic)Transpose() * ((dynamic)1 / CalculateDeterminant());

        /// <summary>
        /// Checks if the given matrix is diagonal.
        /// In a diagonal matrix, non-diagonal values must have a default value.
        /// </summary>
        public bool CheckIsDiagonal()
        {
            if (Rows.Count == 0 || !IsSquare)
                return false;

            for (int row = 0; row < Rows.Count; row++)
                for (int column = 0; column < Columns.Count; column++)
                    if (row != column && !this[row,column].Equals(default))
                        return false;

            return true;
        }

        /// <summary>
        /// Get vector array if matrix have only one row or one column or is diagonal
        /// </summary>
        /// <returns>Vector array</returns>
        public IEnumerable<Tsource> AsVector()
        {
            if (Rows.Count == 0)
                return new Tsource[0];

            if (!IsVector && !CheckIsDiagonal())
                throw new InvalidOperationException("Matrix is not vector or diagonal");

            if (Rows.Count == 1)
                return Rows[0];
            else if (Columns.Count == 1)
                return Columns[0];
            else
            {
                Tsource[] ret = new Tsource[Rows.Count];

                for (int i = 0; i < Rows.Count; i++)
                    ret[i] = this[i,i];

                return ret;
            }
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

        public object Clone() => new Matrix<Tsource>(this.value);

        #endregion
    }
}
