using System;
using System.Collections;
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

        #region IndexOf

        [Theory]
        [InlineData(new double[] { 2, 3 })]
        [InlineData(new double[] { 3, 4, 1, 0 })]
        [InlineData(null)]
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

        #endregion

        #region Insert

        [Fact]
        public void Insert_WhenIndexIsOutOfRange_ShouldThrowIndexOutOfRangeException()
        {
            var matrix = new Matrix<bool>(2,2);
            var collection = new RowCollection<bool>(matrix);

            Assert.Throws<IndexOutOfRangeException>(() => collection.Insert(-1, null));
            Assert.Throws<IndexOutOfRangeException>(() => collection.Insert(2, null));
        }

        [Fact]
        public void Insert_WhenArgumentIsNull_ShouldThrowArgumentNullException()
        {
            var matrix = new Matrix<bool>(2, 2);
            var collection = new RowCollection<bool>(matrix);

            Assert.Throws<ArgumentNullException>(() => collection.Insert(0, null));
        }

        [Fact]
        public void Insert_WhenArgumentHaveInvalid_ShouldThrowArgumentException()
        {
            var matrix = new Matrix<bool>(2, 2);
            var collection = new RowCollection<bool>(matrix);

            Assert.Throws<ArgumentException>(() => collection.Insert(0, new bool[] { true, false, true }));
        }

        [Fact]
        public void Insert_WhenArgumentIsValid_ShouldAddNewRow()
        {
            var matrix = new Matrix<int>(2, 2);
            var collection = new RowCollection<int>(matrix);

            collection.Insert(0, new int[] { 1,1 });
            Assert.Equal(3, collection.Count);
            Assert.Equal(3, collection.LongCount);
            Assert.Equal(0, collection.IndexOf(new int[] { 1,1 }));
        }

        #endregion

        #region RemoveAt

        [Fact]
        public void RemoveAt_WhenIndexIsOutOfRange_ShouldThrowIndexOutOfRangeException()
        {
            var matrix = new Matrix<int>(4,4);
            var collection = new RowCollection<int>(matrix);

            Assert.Throws<IndexOutOfRangeException>(() => collection.RemoveAt(4));
            Assert.Throws<IndexOutOfRangeException>(() => collection.RemoveAt(-1));
        }

        [Fact]
        public void RemoveAt_WhenIndexIsValid_ShouldRemoveRow()
        {
            var matrix = new Matrix<int>(4, 4);
            var collection = new RowCollection<int>(matrix);

            collection.RemoveAt(0);
            Assert.Equal(3, collection.Count);
            Assert.Equal(3, collection.LongCount);
        }

        #endregion

        #region Remove

        [Fact]
        public void Remove_WhenRowIsNull_ShouldTHrowArgumentNullException()
        {
            var matrix = new Matrix<int>(new int[][]
            {
                new int [] { 0, 1, 2, 3 },
                new int [] { 1, -2, 3, 10 }
            });
            var collection = new RowCollection<int>(matrix);

            Assert.Throws<ArgumentNullException>(() => collection.Remove(null));
        }

        [Fact]
        public void Remove_WhenRowNotExists_ShouldReturnFalse()
        {
            var matrix = new Matrix<int>(new int[][]
            {
                new int [] { 0, 1, 2, 3 },
                new int [] { 1, -2, 3, 10 }
            });
            var collection = new RowCollection<int>(matrix);

            Assert.False(collection.Remove(new int[] { 0, 1 }));
            Assert.Equal(2, collection.Count);
            Assert.Equal(2, collection.LongCount);
        }

        [Fact]
        public void Remove_WhenRowExists_ShouldReturnTrueAndRemoveRow()
        {
            var matrix = new Matrix<int>(new int[][]
            {
                new int [] { 0, 1, 2, 3 },
                new int [] { 1, -2, 3, 10 }
            });
            var collection = new RowCollection<int>(matrix);

            Assert.True(collection.Remove(new int[] { 0, 1, 2, 3 }));

            Assert.Equal(1, collection.Count);
            Assert.Equal(1, collection.LongCount);
        }

        #endregion

        #region Contains

        [Fact]
        public void Contains_WhenRowNotExists_ShouldReturnFalse()
        {
            var matrix = new Matrix<int>(new int[][]
            {
                new int [] { 0, 1, 2, 3 },
                new int [] { 1, -2, 3, 10 }
            });
            var collection = new RowCollection<int>(matrix);

            Assert.False(collection.Contains(new int[] { 0, 1, 2, 4 }));
            Assert.False(collection.Contains(null));
        }

        [Fact]
        public void Contains_WhenRowExists_ShouldReturnTrue()
        {
            var matrix = new Matrix<int>(new int[][]
            {
                new int [] { 0, 1, 2, 3 },
                new int [] { 1, -2, 3, 10 }
            });
            var collection = new RowCollection<int>(matrix);

            Assert.True(collection.Contains(new int[] { 0, 1, 2, 3 }));
        }

        #endregion

        #region Add

        [Fact]
        public void Add_WhenArgumentIsNull_ShouldThrowArgumentNullException()
        {
            var matrix = new Matrix<int>(new int[][]
            {
                new int [] { 0, 1, 2, 3 },
                new int [] { 1, -2, 3, 10 }
            });
            var collection = new RowCollection<int>(matrix);

            Assert.Throws<ArgumentNullException>(() => collection.Add(null));
        }

        [Fact]
        public void Add_WhenArgumentIsInvalid_ShouldThrowArgumentException()
        {
            var matrix = new Matrix<int>(new int[][]
            {
                new int [] { 0, 1, 2, 3 },
                new int [] { 1, -2, 3, 10 }
            });
            var collection = new RowCollection<int>(matrix);

            Assert.Throws<ArgumentException>(() => collection.Add(new int[] { 1, 3, 2 }));
        }

        [Fact]
        public void Add_WhenArgumentIsValid_ShouldAddRow()
        {
            var matrix = new Matrix<float>(new float[][]
            {
                new float [] { 0.12f, 0.00024f, 1, 1000 },
                new float [] { 0, 0, 1, -0.00134f }
            });
            var collection = new RowCollection<float>(matrix);

            collection.Add(new float[] { 0.23f, -0.000013f, 123f, 0.00003f });

            Assert.Equal(3, collection.Count);
            Assert.Equal(3, collection.LongCount);
            Assert.Contains(new float[] { 0.23f, -0.000013f, 123f, 0.00003f }, collection);
        }

        #endregion

        #region CopyTo



        #endregion

        #region MultiplyWithScalar

        [Fact]
        public void MultiplyWithScalar_()
        {

        }

        #endregion

        #region GetEnumerator

        [Fact]
        public void GetEnumerator_ShouldReturnEnumerator()
        {
            var matrix = new Matrix<float>(new float[][]
            {
                new float [] { 0.12f, 0.00024f, 1, 1000 },
                new float [] { 0, 0, 1, -0.00134f }
            });
            var collection = new RowCollection<float>(matrix);

            IEnumerator<float[]> enumerator = collection.GetEnumerator();
            Assert.NotNull(enumerator);
        }

        #endregion

        #endregion
    }
}
