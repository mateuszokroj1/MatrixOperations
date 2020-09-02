using System;
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
            Assert.Empty(matrix.value);

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
        public void Constructor4_WhenValuesAreValid_ShouldCreateArrayWithDefaultValues(uint rows, uint columns)
        {
            var matrix1 = new Matrix<bool>(rows, columns);
            Assert.Equal(rows, (uint)(matrix1.value?.Length ?? 0));
            Assert.Equal((int)columns, matrix1.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix1.Rows);
            Assert.Equal(rows, (uint)matrix1.Rows.Count);
            Assert.NotNull(matrix1.Columns);
            Assert.Equal(columns, (uint)matrix1.Columns.Count);

            var matrix2 = new Matrix<sbyte>(rows, columns);
            Assert.Equal(rows, (uint)(matrix2.value?.Length ?? 0));
            Assert.Equal((int)columns, matrix2.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix2.Rows);
            Assert.Equal(rows, (uint)matrix2.Rows.Count);
            Assert.NotNull(matrix2.Columns);
            Assert.Equal(columns, (uint)matrix2.Columns.Count);

            var matrix3 = new Matrix<short>(rows, columns);
            Assert.Equal(rows, (uint)(matrix3.value?.Length ?? 0));
            Assert.Equal((int)columns, matrix3.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix3.Rows);
            Assert.Equal(rows, (uint)matrix3.Rows.Count);
            Assert.NotNull(matrix3.Columns);
            Assert.Equal(columns, (uint)matrix3.Columns.Count);

            var matrix4 = new Matrix<ushort>(rows, columns);
            Assert.Equal(rows, (uint)(matrix4.value?.Length ?? 0));
            Assert.Equal((int)columns, matrix4.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix4.Rows);
            Assert.Equal(rows, (uint)matrix4.Rows.Count);
            Assert.NotNull(matrix4.Columns);
            Assert.Equal(columns, (uint)matrix4.Columns.Count);

            var matrix5 = new Matrix<int>(rows, columns);
            Assert.Equal(rows, (uint)(matrix5.value?.Length ?? 0));
            Assert.Equal((int)columns, matrix5.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix5.Rows);
            Assert.Equal(rows, (uint)matrix5.Rows.Count);
            Assert.NotNull(matrix5.Columns);
            Assert.Equal(columns, (uint)matrix5.Columns.Count);

            var matrix6 = new Matrix<uint>(rows, columns);
            Assert.Equal(rows, (uint)(matrix6.value?.Length ?? 0));
            Assert.Equal((int)columns, matrix6.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix6.Rows);
            Assert.Equal(rows, (uint)matrix6.Rows.Count);
            Assert.NotNull(matrix6.Columns);
            Assert.Equal(columns, (uint)matrix6.Columns.Count);

            var matrix7 = new Matrix<long>(rows, columns);
            Assert.Equal(rows, (uint)(matrix7.value?.Length ?? 0));
            Assert.Equal((int)columns, matrix7.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix7.Rows);
            Assert.Equal(rows, (uint)matrix7.Rows.Count);
            Assert.NotNull(matrix7.Columns);
            Assert.Equal(columns, (uint)matrix7.Columns.Count);

            var matrix8 = new Matrix<ulong>(rows, columns);
            Assert.Equal(rows, (uint)(matrix8.value?.Length ?? 0));
            Assert.Equal((int)columns, matrix8.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix8.Rows);
            Assert.Equal(rows, (uint)matrix8.Rows.Count);
            Assert.NotNull(matrix8.Columns);
            Assert.Equal(columns, (uint)matrix8.Columns.Count);

            var matrix9 = new Matrix<float>(rows, columns);
            Assert.Equal(rows, (uint)(matrix9.value?.Length ?? 0));
            Assert.Equal((int)columns, matrix9.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix9.Rows);
            Assert.Equal(rows, (uint)matrix9.Rows.Count);
            Assert.NotNull(matrix9.Columns);
            Assert.Equal(columns, (uint)matrix9.Columns.Count);

            var matrix10 = new Matrix<double>(rows, columns);
            Assert.Equal(rows, (uint)(matrix10.value?.Length ?? 0));
            Assert.Equal((int)columns, matrix10.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix10.Rows);
            Assert.Equal(rows, (uint)matrix10.Rows.Count);
            Assert.NotNull(matrix10.Columns);
            Assert.Equal(columns, (uint)matrix10.Columns.Count);

            var matrix11 = new Matrix<decimal>(rows, columns);
            Assert.Equal(rows, (uint)(matrix11.value?.Length ?? 0));
            Assert.Equal((int)columns, matrix11.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix11.Rows);
            Assert.Equal(rows, (uint)matrix11.Rows.Count);
            Assert.NotNull(matrix11.Columns);
            Assert.Equal(columns, (uint)matrix11.Columns.Count);

            var matrix12 = new Matrix<BigInteger>(rows, columns);
            Assert.Equal(rows, (uint)(matrix12.value?.Length ?? 0));
            Assert.Equal((int)columns, matrix12.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix12.Rows);
            Assert.Equal(rows, (uint)matrix12.Rows.Count);
            Assert.NotNull(matrix12.Columns);
            Assert.Equal(columns, (uint)matrix12.Columns.Count);

            var matrix13 = new Matrix<Complex>(rows, columns);
            Assert.Equal(rows, (uint)(matrix13.value?.Length ?? 0));
            Assert.Equal((int)columns, matrix13.value.Where(row => row != null).GroupBy(row => row.Length).First().Key);
            Assert.NotNull(matrix13.Rows);
            Assert.Equal(rows, (uint)matrix13.Rows.Count);
            Assert.NotNull(matrix13.Columns);
            Assert.Equal(columns, (uint)matrix13.Columns.Count);
        }

        [Theory]
        [InlineData(0, 20)]
        [InlineData(100, 0)]
        [InlineData(0, 0)]
        public void Constructor4_WhenParameterEquals0_ShouldCreateEmptyArray(uint rows, uint columns)
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
    }
}
