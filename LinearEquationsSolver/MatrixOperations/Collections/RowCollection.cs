﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace MatrixOperations
{
    public class RowCollection<Tsource> : IRowCollection<Tsource>
        where Tsource : struct, IEquatable<Tsource>
    {
        #region Fields

        private readonly Matrix<Tsource> matrix;

        #endregion

        #region Constructors

        public RowCollection(Matrix<Tsource> matrix) => this.matrix = matrix ?? throw new ArgumentNullException();

        #endregion

        #region Properties
        /// <summary>
        /// Access to row with selected index
        /// </summary>
        /// <param name="index">Index of row</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="IndexOutOfRangeException"/>
        /// <exception cref="InvalidOperationException"/>
        public Tsource[] this[int index]
        {
            get => this.matrix.value[index];
            set
            {
                if (index >= Count)
                    throw new IndexOutOfRangeException();

                if (value == null)
                    throw new ArgumentNullException();

                if (value.Length != this.matrix.Columns.Count)
                    throw new InvalidOperationException("New row must have the same length as other.");

                this.matrix.value[index] = value;
            }
        }

        public int Count => this.matrix.value.Length;

        public long LongCount => this.matrix.value.LongLength;

        public bool IsReadOnly => false;
        #endregion

        #region Methods
        /// <summary>
        /// Search matrix for row equals to 'item' and return index of row in matrix or -1.
        /// </summary>
        public int IndexOf(Tsource[] item)
        {
            if (item == null)
                return -1;

            if (item.Length != this.matrix.Columns.Count)
                return -1;

            int row = 0;
        NextRow:
            while (row < Count)
            {
                for (int column = 0; column < this.matrix.Columns.Count; column++)
                {
                    if (!this.matrix[row, column].Equals(item[column]))
                    {
                        row++;
                        goto NextRow;
                    }
                }
                return row;
            }

            return -1;
        }

        /// <summary>
        /// Add new row in selected index
        /// </summary>
        /// <param name="index">Index of new row</param>
        /// <param name="item">New row</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="IndexOutOfRangeException"/>
        public void Insert(int index, Tsource[] item)
        {
            if (index >= Count || index < 0)
                throw new IndexOutOfRangeException();

            if (item == null)
                throw new ArgumentNullException();

            if (item.Length != this.matrix.Columns.Count)
                throw new ArgumentException("New row must have the same length as other.");

            Tsource[][] newarray = new Tsource[Count+1][];

            for (int i = 0; i < index; i++)
                newarray[i] = this.matrix.value[i];

            newarray[index] = item;

            for (int i = index; i < this.matrix.value.Length; i++)
                newarray[i+1] = this.matrix.value[i];

            this.matrix.value = newarray;
        }

        /// <summary>
        /// Remove row with selected index
        /// </summary>
        /// <param name="index">Selected row to remove</param>
        /// <exception cref="IndexOutOfRangeException"/>
        public void RemoveAt(int index)
        {
            if (index >= Count || index < 0)
                throw new IndexOutOfRangeException();

            Tsource[][] newarray = new Tsource[Count-1][];

            for (int i = 0; i < index; i++)
                newarray[i] = this.matrix.value[i];

            for (int i = index; i < this.matrix.value.Length-1; i++)
                newarray[i] = this.matrix.value[i+1];

            this.matrix.value = newarray;
        }

        /// <summary>
        /// Add new row to the end of this matrix
        /// </summary>
        /// <param name="item">New row</param>
        public void Add(Tsource[] item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (Count < 1 || item.Length != this.matrix.Columns.Count)
                throw new ArgumentException("New row must have the same length as other.");

            Tsource[][] newarray = new Tsource[Count+1][];

            for (int i = 0; i < Count; i++)
                newarray[i] = this.matrix.value[i];

            newarray[Count] = item;

            this.matrix.value = newarray;
        }

        /// <exception cref="NotSupportedException"/>
        [Obsolete]
        public void Clear() => throw new NotSupportedException();

        /// <summary>
        /// Search matrix for row equals to 'item'
        /// </summary>
        /// <param name="item">Row to search</param>
        public bool Contains(Tsource[] item)
        => IndexOf(item) > -1;

        /// <summary>
        /// Copies all values to new array of <typeparamref name="Tsource"/>[]
        /// </summary>
        /// <param name="array">Destination array</param>
        /// <param name="arrayIndex">Destination array start index</param>
        public void CopyTo(Tsource[][] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException();

            if (array.Length-arrayIndex != Count)
                throw new ArgumentException("Destination array is too small.");

            for(int i = 0, j = arrayIndex; i < Count && j < array.Length; i++, j++)
            {
                array[j] = new Tsource[this.matrix.value[0].Length];
                for (int columnIndex = 0; columnIndex < this.matrix.value[0].Length; columnIndex++)
                    array[j][columnIndex] = this.matrix.value[i][columnIndex];
            }
        }

        
        public bool Remove(Tsource[] item)
        {
            if (item == null)
                throw new ArgumentNullException();

            int index = IndexOf(item);

            if (index == -1)
                return false;

            RemoveAt(index);

            return true;
        }

        /// <summary>
        /// Multiplies selected row with scalar value
        /// </summary>
        /// <exception cref="IndexOutOfRangeException" />
        public void MultiplyWithScalar(int rowIndex, Tsource scalarValue)
        {
            if (rowIndex < 0 || rowIndex >= this.matrix.Rows.Count)
                throw new IndexOutOfRangeException();

            if ((dynamic)scalarValue == 1)
                return;

            for (int columnIndex = 0; columnIndex < this.matrix.Columns.Count; columnIndex++)
                this.matrix[rowIndex, columnIndex] *= (dynamic)scalarValue;
        }

        public IEnumerator<Tsource[]> GetEnumerator() => new RowEnumerator<Tsource>(this.matrix);
        
        IEnumerator IEnumerable.GetEnumerator() => new RowEnumerator<Tsource>(this.matrix);

        #endregion
    }
}
