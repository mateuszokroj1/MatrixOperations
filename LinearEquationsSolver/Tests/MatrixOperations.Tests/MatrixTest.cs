﻿using System;
using System.Linq;
using System.Numerics;
using Xunit;

namespace MatrixOperations.Tests
{
    public class MatrixTest
    {
        #region Constructors

        [Fact]
        public void Constructor1_ShouldCreateEmptyArray()
        {
            var matrix = new Matrix<bool>();
            Assert.NotNull(matrix);
            Assert.NotNull(matrix.value);

            Assert.NotNull(matrix.Rows);
            Assert.Empty(matrix.Rows);
            Assert.NotNull(matrix.Columns);
            Assert.Empty(matrix.Columns);
        }

        [Fact]
        public void Constructor2_ShouldCopyArray()
        {
            var array = new int[][]
            {
                new int[]{ 1, 2, 3, -1 },
                new int[]{ 0, -2, 30, 100 }
            };

            var matrix = new Matrix<int>(array);

            Assert.NotNull(matrix);
            Assert.NotNull(matrix.value);
            Assert.Equal(2, matrix.value?.Length ?? 0);

            for (byte row = 0; row <= 1; row++)
            {
                Assert.Equal(4, matrix.value[row].Length);

                for (byte column = 0; column <= 3; column++)
                    Assert.Contains(array[row][column], matrix.value[row]);
            }

            Assert.NotNull(matrix.Rows);
            Assert.Equal(2, matrix.Rows.Count);

            Assert.NotNull(matrix.Columns);
            Assert.Equal(4, matrix.Columns.Count);
        }

        [Fact]
        public void Constructor3_WhenRefIsNotNull_ShouldRefToArray()
        {
            float[][] values = new float[][]
            {
                new float[]{ 0, 1, -100.9f },
                new float[]{ -12.341f, 10.352f, -0.0000032f }
            };

            var matrix = new Matrix<float>(ref values);

            Assert.Equal(values, matrix.value);

            Assert.NotNull(matrix.Rows);
            Assert.Equal(2, matrix.Rows.Count);
            Assert.NotNull(matrix.Columns);
            Assert.Equal(3, matrix.Columns.Count);
        }

        [Fact]
        public void Constructor3_WhenRefIsNull_ShouldThrowArgumentNullException()
        {
            bool[][] array = null;
            Assert.Throws<ArgumentNullException>(() => new Matrix<bool>(ref array));
        }

        [Theory]
        [InlineData(1, 3)]
        [InlineData(3, 3)]
        [InlineData(8, 6)]
        [InlineData(10, 2)]
        [InlineData(40, 1)]
        public void Constructor4_WhenValuesAreValid_ShouldCreateArrayWithDefaultValues(int rows, int columns)
        {
            var matrix1 = new Matrix<bool>(rows, columns);
            Assert.Equal(rows, matrix1.value?.Length ?? 0);
            Assert.Equal(columns, matrix1.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix1.Rows);
            Assert.Equal(rows, matrix1.Rows.Count);
            Assert.NotNull(matrix1.Columns);
            Assert.Equal(columns, matrix1.Columns.Count);

            var matrix2 = new Matrix<sbyte>(rows, columns);
            Assert.Equal(rows, matrix2.value?.Length ?? 0);
            Assert.Equal(columns, matrix2.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix2.Rows);
            Assert.Equal(rows, matrix2.Rows.Count);
            Assert.NotNull(matrix2.Columns);
            Assert.Equal(columns, matrix2.Columns.Count);

            var matrix3 = new Matrix<short>(rows, columns);
            Assert.Equal(rows, matrix3.value?.Length ?? 0);
            Assert.Equal(columns, matrix3.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix3.Rows);
            Assert.Equal(rows, matrix3.Rows.Count);
            Assert.NotNull(matrix3.Columns);
            Assert.Equal(columns, matrix3.Columns.Count);

            var matrix4 = new Matrix<ushort>(rows, columns);
            Assert.Equal(rows, matrix4.value?.Length ?? 0);
            Assert.Equal(columns, matrix4.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix4.Rows);
            Assert.Equal(rows, matrix4.Rows.Count);
            Assert.NotNull(matrix4.Columns);
            Assert.Equal(columns, matrix4.Columns.Count);

            var matrix5 = new Matrix<int>(rows, columns);
            Assert.Equal(rows, matrix5.value?.Length ?? 0);
            Assert.Equal(columns, matrix5.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix5.Rows);
            Assert.Equal(rows, matrix5.Rows.Count);
            Assert.NotNull(matrix5.Columns);
            Assert.Equal(columns, matrix5.Columns.Count);

            var matrix6 = new Matrix<uint>(rows, columns);
            Assert.Equal(rows, matrix6.value?.Length ?? 0);
            Assert.Equal(columns, matrix6.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix6.Rows);
            Assert.Equal(rows, matrix6.Rows.Count);
            Assert.NotNull(matrix6.Columns);
            Assert.Equal(columns, matrix6.Columns.Count);

            var matrix7 = new Matrix<long>(rows, columns);
            Assert.Equal(rows, matrix7.value?.Length ?? 0);
            Assert.Equal(columns, matrix7.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix7.Rows);
            Assert.Equal(rows, matrix7.Rows.Count);
            Assert.NotNull(matrix7.Columns);
            Assert.Equal(columns, matrix7.Columns.Count);

            var matrix8 = new Matrix<ulong>(rows, columns);
            Assert.Equal(rows, matrix8.value?.Length ?? 0);
            Assert.Equal(columns, matrix8.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix8.Rows);
            Assert.Equal(rows, matrix8.Rows.Count);
            Assert.NotNull(matrix8.Columns);
            Assert.Equal(columns, matrix8.Columns.Count);

            var matrix9 = new Matrix<float>(rows, columns);
            Assert.Equal(rows, matrix9.value?.Length ?? 0);
            Assert.Equal(columns, matrix9.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix9.Rows);
            Assert.Equal(rows, matrix9.Rows.Count);
            Assert.NotNull(matrix9.Columns);
            Assert.Equal(columns, matrix9.Columns.Count);

            var matrix10 = new Matrix<double>(rows, columns);
            Assert.Equal(rows, matrix10.value?.Length ?? 0);
            Assert.Equal(columns, matrix10.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix10.Rows);
            Assert.Equal(rows, matrix10.Rows.Count);
            Assert.NotNull(matrix10.Columns);
            Assert.Equal(columns, matrix10.Columns.Count);

            var matrix11 = new Matrix<decimal>(rows, columns);
            Assert.Equal(rows, matrix11.value?.Length ?? 0);
            Assert.Equal(columns, matrix11.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix11.Rows);
            Assert.Equal(rows, matrix11.Rows.Count);
            Assert.NotNull(matrix11.Columns);
            Assert.Equal(columns, matrix11.Columns.Count);

            var matrix12 = new Matrix<BigInteger>(rows, columns);
            Assert.Equal(rows, matrix12.value?.Length ?? 0);
            Assert.Equal(columns, matrix12.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix12.Rows);
            Assert.Equal(rows, matrix12.Rows.Count);
            Assert.NotNull(matrix12.Columns);
            Assert.Equal(columns, matrix12.Columns.Count);

            var matrix13 = new Matrix<Complex>(rows, columns);
            Assert.Equal(rows, matrix13.value?.Length ?? 0);
            Assert.Equal(columns, matrix13.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix13.Rows);
            Assert.Equal(rows, matrix13.Rows.Count);
            Assert.NotNull(matrix13.Columns);
            Assert.Equal(columns, matrix13.Columns.Count);
        }

        [Theory]
        [InlineData(0, 20)]
        [InlineData(100, 0)]
        [InlineData(0, 0)]
        public void Constructor4_WhenParameterEquals0_ShouldCreateEmptyArray(int rows, int columns)
        {
            var matrix = new Matrix<bool>(rows, columns);
            Assert.NotNull(matrix?.value);
            Assert.Empty(matrix.value);
            Assert.NotNull(matrix.Rows);
            Assert.Empty(matrix.Rows);
            Assert.NotNull(matrix.Columns);
            Assert.Empty(matrix.Columns);
        }

        [Fact]
        public void Constructor5_WhenArgumentIsNotEmpty_ShouldCopyArray()
        {
            var matrix1 = new Matrix<int>(4,4);
            var matrix2 = new Matrix<int>(matrix1);

            Assert.NotNull(matrix2?.value);

            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(4, matrix2.value[i]?.Length ?? 0);

                for (int j = 0; j < 3; j++)
                    Assert.Equal(default(int), matrix2.value[i][j]);
            }

            Assert.NotNull(matrix2.Rows);
            Assert.Equal(4, matrix2.Rows.Count);
            Assert.NotNull(matrix2.Columns);
            Assert.Equal(4, matrix2.Columns.Count);
        }

        [Fact]
        public void Constructor5_WhenArgumentIsEmpty_ShouldCopyArray()
        {
            var matrix1 = new Matrix<float>();
            var matrix2 = new Matrix<float>(matrix1);

            Assert.NotNull(matrix2?.value);
            Assert.Empty(matrix2.value);

            Assert.NotNull(matrix2.Rows);
            Assert.Empty(matrix2.Rows);
            Assert.NotNull(matrix2.Columns);
            Assert.Empty(matrix2.Columns);
        }

        [Fact]
        public void Constructor5_WhenRefIsNull_ShouldThrowArgumentNullException()
        {
            Matrix<int> matrix1 = null;

            Assert.Throws<ArgumentNullException>(() => new Matrix<int>(matrix1));
        }

        #endregion

        #region Properties

        [Fact]
        public void IsSquare_ShouldReturnTrue()
        {
            var matrix1 = new Matrix<bool>(10,10);
            Assert.True(matrix1.IsSquare);

            var matrix2 = new Matrix<int>(new int[][]
            {
                new int[] { 1, -100 },
                new int[] { 0, 200 }
            });
            Assert.True(matrix2.IsSquare);
        }

        [Fact]
        public void IsSquare_ShouldReturnFalse()
        {
            var matrix1 = new Matrix<float>(100,200);
            Assert.False(matrix1.IsSquare);

            var matrix2 = new Matrix<double>(new double[][]
            {
                new double[] { -1200, 0.234 }
            });
            Assert.False(matrix2.IsSquare);

            var matrix3 = new Matrix<bool>();
            Assert.False(matrix3.IsSquare);
        }

        [Fact]
        public void IsVector_ShouldReturnTrue()
        {
            var matrix1 = new Matrix<int>(new int[][]
            {
                new int[] { 1 },
                new int[] { 2 },
                new int[] { 4 }
            });
            Assert.True(matrix1.IsVector);

            var matrix2 = new Matrix<float>(new float[][] { new float[] { 2.214f, -.121200f, 0.12231f, float.MaxValue, float.Epsilon } });
            Assert.True(matrix2.IsVector);
        }

        [Fact]
        public void IsVector_ShouldReturnFalse()
        {
            var matrix1 = new Matrix<bool>(2,2);
            Assert.False(matrix1.IsVector);

            var matrix2 = new Matrix<int>(new int[][]
            {
                new int [] { 0, 20, -123200 },
                new int [] { -100, 20000, 1000 }
            });
            Assert.False(matrix2.IsVector);
        }

        [Fact]
        public void Rows_ShouldReturnCollection()
        {
            var matrix1 = new Matrix<int>(20,20);
            Assert.IsType<RowCollection<int>>(matrix1.Rows);
            Assert.Equal(20, matrix1.Rows.Count);
            foreach(var row in matrix1.Rows)
            {
                Assert.NotNull(row);
                Assert.Equal(20, row.Length);
                foreach (var cell in row)
                    Assert.Equal(default(int), cell);
            }

            var matrix2 = new Matrix<float>();
            Assert.IsType<RowCollection<float>>(matrix2.Rows);
            Assert.Empty(matrix2.Rows);
        }

        [Fact]
        public void Columns_ShouldReturnCollection()
        {
            var matrix1 = new Matrix<float>(5,5);
            Assert.IsType<ColumnCollection<float>>(matrix1.Columns);
            Assert.Equal(5, matrix1.Columns.Count);
            foreach(var column in matrix1.Columns)
            {
                Assert.NotNull(column);
                Assert.Equal(5, column.Length);
                foreach (var cell in column)
                    Assert.Equal(default(float), cell);
            }

            var matrix2 = new Matrix<long>();
            Assert.IsType<ColumnCollection<long>>(matrix2.Columns);
            Assert.Empty(matrix2.Columns);
        }

        [Fact]
        public void Indexer_WhenIndexesAreInRange_ShouldReadAndWriteCell()
        {
            var matrix = new Matrix<int>(2,2)
            {
                [0,0] = 1,
                [0,1] = 10,
                [1,0] = 100,
                [1,1] = 1000
            };

            Assert.Equal(1, matrix[0,0]);
            Assert.Equal(10, matrix[0,1]);
            Assert.Equal(100, matrix[1,0]);
            Assert.Equal(1000, matrix[1,1]);
        }

        [Fact]
        public void Indexer_WhenIndexesAreOutOfRange_ShouldThrowIndexOutOfRangeException()
        {
            var matrix = new Matrix<bool>(2,2);
            Assert.Throws<IndexOutOfRangeException>(() => matrix[2, 2] = true);
            Assert.Throws<IndexOutOfRangeException>(() => matrix[int.MaxValue, int.MaxValue]);
        }

        #endregion

        #region Static

        [Fact]
        public void CheckIsSizeEqual_WhenArgumentIsNull_ShouldThrowArgumentNullException()
        {
            Matrix<int>[] matrices = null;
            Assert.Throws<ArgumentNullException>(() => Matrix<int>.CheckIsSizeEqual(matrices));
        }

        [Fact]
        public void CheckIsSizeEqual_WhenOneMatrixIsNull_ShouldThrowArgumentNullException()
        {
            var m1 = new Matrix<bool>(2, 2);
            Assert.Throws<ArgumentNullException>(() => Matrix<bool>.CheckIsSizeEqual(m1, null));
        }

        [Fact]
        public void CheckIsSizeEqual_WhenMatricesHaveTheSameSize_ShouldReturnTrue()
        {
            var matrices = new Matrix<float>[]
            {
                new Matrix<float>(10,10),
                new Matrix<float>(10,10),
                new Matrix<float>(10,10),
                new Matrix<float>(10,10)
            };

            Assert.True(Matrix<float>.CheckIsSizeEqual(matrices));
        }

        [Fact]
        public void CheckIsSizeEqual_WhenMatricesHaveRandomSizes_ShouldReturnFalse()
        {
            var matrices = new Matrix<long>[]
            {
                new Matrix<long>(5,3),
                new Matrix<long>(32,12),
                new Matrix<long>(10,1),
                new Matrix<long>(100,10)
            };

            Assert.False(Matrix<long>.CheckIsSizeEqual(matrices));
        }

        #region Generators

        [Fact]
        public void GenerateIdentity_ShouldReturnMatrix()
        {
            var matrix = Matrix<decimal>.GenerateIdentity(5);
            Assert.Equal(5, matrix.Rows.Count);
            Assert.Equal(5, matrix.Columns.Count);
            Assert.True(matrix.CheckIsDiagonal());

            var vector = matrix.AsVector();
            Assert.NotNull(vector);
            Assert.Equal(5, vector.Count());
            foreach (var item in vector)
                Assert.Equal(1M, item);

            var matrix2 = Matrix<int>.GenerateIdentity(0);
            Assert.Empty(matrix2.Rows);
            Assert.Empty(matrix2.Columns);
        }

        [Fact]
        public void GenerateDiagonal_WhenArgumentIsNull_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Matrix<int>.GenerateDiagonal(null));
        }

        [Fact]
        public void GenerateDiagonal_WhenVectorIsEmpty_ShouldReturnEmptyMatrix()
        {
            var matrix = Matrix<bool>.GenerateDiagonal(new bool[0]);
            Assert.NotNull(matrix);
            Assert.Empty(matrix.Rows);
            Assert.Empty(matrix.Columns);
        }

        [Fact]
        public void GenerateDiagonal_WhenVectorIsValid_ShouldReturnMatrix()
        {
            var vector = Enumerable.Range(1, 5);
            var matrix = Matrix<int>.GenerateDiagonal(vector);
            Assert.Equal(5, matrix.Rows.Count);
            Assert.Equal(5, matrix.Columns.Count);

            for (int row = 0; row < 5; row++)
                for (int column = 0; column < 5; column++)
                    if (row == column)
                        Assert.Equal(vector.ElementAt(row), matrix[row, column]);
                    else
                        Assert.Equal(0, matrix[row,column]);
        }

        #endregion

        #endregion
    }
}
