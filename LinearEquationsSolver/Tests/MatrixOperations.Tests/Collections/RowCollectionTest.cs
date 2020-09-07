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
        public void Indexer_Set_WhenArgumentIsOutOfRange_ShouldThrowIndexOutOfRange()
        {

        }

        #endregion
    }
}
