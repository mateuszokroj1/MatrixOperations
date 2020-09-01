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
        public void Constructor3_ShouldRefToArray()
        {

        }

        #endregion
    }
}
