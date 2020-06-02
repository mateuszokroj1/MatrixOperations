using System;
using System.Collections;
using System.Collections.Generic;

namespace MatrixOperations
{
    public class RowCollection<Tsource> : IList<Tsource[]>
        where Tsource : struct, IEquatable<Tsource>
    {
        #region Constructors
        public RowCollection(Matrix<Tsource> matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException();
        }
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

        protected Matrix<Tsource> matrix;

        public int Count => this.matrix.value.Length;

        public bool IsReadOnly => false;
        #endregion

        #region Methods
        /// <summary>
        /// Search matrix for row equals to 'item' and return index of row in matrix or -1.
        /// </summary>
        /// 
        public int IndexOf(Tsource[] item)
        {
            if (item == null)
                throw new ArgumentNullException();

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
            if (item == null)
                throw new ArgumentNullException();

            if (item.Length != this.matrix.Columns.Count)
                throw new ArgumentException("New row must have the same length as other.");

            if (index >= Count)
                throw new IndexOutOfRangeException(nameof(index));

            Tsource[][] newarray = new Tsource[Count+1][];

            int i = 0;
            while(i < index)
            {
                newarray[i] = this[i];
                i++;
            }

            newarray[i] = item;

            while (i < Count+1)
            {
                newarray[i] = this[i - 1];
                i++;
            }

            this.matrix.value = newarray;
        }

        /// <summary>
        /// Remove row with selected index
        /// </summary>
        /// <param name="index">Selected row to remove</param>
        /// <exception cref="IndexOutOfRangeException"/>
        public void RemoveAt(int index)
        {
            if (index >= Count)
                throw new IndexOutOfRangeException();

            Tsource[][] newarray = new Tsource[Count-1][];

            int i = 0;
            while (i < index)
            {
                newarray[i] = this[i];
                i++;
            }

            while(i < Count-1)
            {
                newarray[i] = this[i+1];
                i++;
            }

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

            if (item.Length != this.matrix.Columns.Count)
                throw new ArgumentException("New row must have the same length as other.");

            Tsource[][] newarray = new Tsource[Count+1][];
            for (int i = 0; i < Count; i++)
                newarray[i] = this[i];
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
        {
            if (item == null)
                throw new ArgumentNullException();

            return IndexOf(item) > -1;
        }

        /// <summary></summary>
        /// <exception cref="NotSupportedException"/>
        [Obsolete]
        public void CopyTo(Tsource[][] array, int arrayIndex) => throw new NotSupportedException();

        
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

        public IEnumerator<Tsource[]> GetEnumerator() => new RowEnumerator<Tsource>(this.matrix);
        
        IEnumerator IEnumerable.GetEnumerator() => new RowEnumerator<Tsource>(this.matrix);
        #endregion
    }
}
