using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MatrixOperations.Tests.Collections
{
    public class RowCollectionTest
    {
        #region Constructors

        [Fact]
        public void Constructor_WhenArgumentIsNull_ShouldThrowArgumentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RowCollection<int>(null));
        }

        [Fact]
        public void Constructor_WhenArgumentIsEmpty_ShouldBeInitialized()
        {
            var collection = new RowCollection<float>(new Matrix<float>());
            Assert.NotNull(collection);
            Assert.Empty(collection);
        }

        [Fact]
        public void Constructor_WhenArgumentIsNotEmpty_ShouldHaveEqualItems()
        {
            var array = new short[][]
            {
                new short[] { 2, 3, 1 },
                new short[] { -2, 0, 10 }
            };
            var matrix = new Matrix<short>(array);
            var collection = new RowCollection<short>(matrix);

            Assert.Equal(2, collection.Count);

            for (byte row = 0; row < 2; row++)
            {
                Assert.NotNull(collection.ElementAt(row));
                for (byte column = 0; column < 3; column++)
                    Assert.Equal(array[row][column], collection.ElementAt(row)[column]);
            }
        }

        #endregion

        #region Properties

        [Fact]
        public void Indexer_Get_ShouldReturnRow()
        {
            var matrix = new Matrix<double>(4,4);
            var collection = new RowCollection<double>(matrix);
            for (byte row = 0; row < 4; row++)
                Assert.Equal(matrix.value[row], collection[row]);
        }

        [Fact]
        public void Indexer_GetSet_WhenArgumentIsOutOfRange_ShouldThrowIndexOutOfRange()
        {
            var matrix = new Matrix<double>(4, 4);
            var collection = new RowCollection<double>(matrix);

            Assert.Throws<IndexOutOfRangeException>(() => collection[4]);
            Assert.Throws<IndexOutOfRangeException>(() => collection[-1]);
            Assert.Throws<IndexOutOfRangeException>(() => collection[int.MaxValue] = new double[] { 2 });
        }

        [Fact]
        public void Indexer_Set_WhenIndexIsValidAndRowIsInvalid_ShouldThrowInvalidOperationException()
        {
            var matrix = new Matrix<double>(4, 4);
            var collection = new RowCollection<double>(matrix);

            Assert.Throws<InvalidOperationException>(() => collection[1] = new double[] { 2 });
        }

        [Fact]
        public void Indexer_Set_WhenArgumentsIsValid_ShouldSetValue()
        {
            var matrix = new Matrix<double>(4, 4);
            var collection = new RowCollection<double>(matrix);

            collection[0] = new double[] { 1, 2, 3, 4 };
        }

        [Fact]
        public void Count_ShouldReturnArrayLength()
        {
            var matrix = new Matrix<double>(10, 4);
            var collection = new RowCollection<double>(matrix);
            Assert.Equal(matrix.value.Length, collection.Count);
        }

        [Fact]
        public void LongCount_ShouldReturnArrayLength()
        {
            var matrix = new Matrix<double>(20, 20);
            var collection = new RowCollection<double>(matrix);
            Assert.Equal(matrix.value.LongLength, collection.LongCount);
        }

        [Fact]
        public void IsReadOnly_ShouldReturnFalse()
        {
            var matrix = new Matrix<double>(20, 20);
            var collection = new RowCollection<double>(matrix);
            Assert.False(collection.IsReadOnly);
        }

        #endregion

        #region Methods

        [Fact]
        public void IndexOf_WhenArgumentIsNull_ShouldThrowArgumentNullException()
        {
            var matrix = new Matrix<double>(20, 20);
            var collection = new RowCollection<double>(matrix);

            Assert.Throws<ArgumentNullException>(() => collection.IndexOf(null));

            matrix = new Matrix<double>();
            collection = new RowCollection<double>(matrix);
            Assert.Throws<ArgumentNullException>(() => collection.IndexOf(null));
        }

        [Theory]
        [InlineData(new double[] { 2, 3 })]
        [InlineData(new double[] { 3, 4, 1, 0 })]
        public void IndexOf_WhenRowNotExists_ShouldReturnMinus1(double[] rowsToTest)
        {
            var matrix = new Matrix<double>(20, 5);
            var collection = new RowCollection<double>(matrix);

            Assert.Equal(-1, collection.IndexOf(rowsToTest));
        }

        [Theory]
        [InlineData(0, new int[] { 1, 2, 3, 4 })]
        [InlineData(1, new int[] { 10, 9, 8, 7 })]
        public void IndexOf_WhenRowExists_ShouldReturnFirstIndex(int expectedValue, int[] rowToTest)
        {
            var matrix = new Matrix<int>(new int[][]
            {
                new int[] { 1, 2, 3, 4 },
                new int[] { 10, 9, 8, 7 }
            });

            var collection = new RowCollection<int>(matrix);

            Assert.Equal(expectedValue, collection.IndexOf(rowToTest));
        }

        [Fact]
        public void Insert_WhenIndexIsOutOfRange_ShouldThrowIndexOutOfRangeException()
        {

        }

        #endregion
    }
}
