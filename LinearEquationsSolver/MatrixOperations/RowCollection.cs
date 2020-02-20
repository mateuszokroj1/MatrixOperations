using System;
using System.Collections;
using System.Collections.Generic;

namespace MatrixOperations
{
    public class RowCollection<Tsource> : IList<Tsource[]> where Tsource : struct
    {
        public RowCollection(Matrix<Tsource> matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException();
        }

        /// <summary>
        /// Access to row with selected index
        /// </summary>
        /// <param name="index">Index of row</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="IndexOutOfRangeException"/>
        /// <exception cref="InvalidOperationException"/>
        public Tsource[] this[int index]
        {
            get
            {
                Tsource[] ret = new Tsource[this.array.GetUpperBound(1)+1];
                for (int i = 0; i < this.array.GetUpperBound(1); i++)
                    ret[i] = this.array[index][i];
                return ret;
            }
            set
            {
                if (index >= this.array.GetUpperBound(0) + 1)
                    throw new IndexOutOfRangeException();

                if (value == null)
                    throw new ArgumentNullException();

                if (value.Length != this.array.GetUpperBound(1) + 1)
                    throw new InvalidOperationException("New row must have the same length as other.");

                for (int i = 0; i < value.Length; i++)
                    this.array[index][i] = value[i];
            }
        }

        protected Matrix<Tsource> matrix;

        public int Count => this.array.GetUpperBound(0)+1;

        public bool IsReadOnly => false;

        /// <summary>
        /// Search matrix for row equals to 'item' and return index of row in matrix or -1.
        /// </summary>
        /// 
        public int IndexOf(Tsource[] item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (item.Length != this.array.GetUpperBound(1) + 1)
                return -1;

            int row = 0;
        NextRow:
            while (row <= this.array.GetUpperBound(0))
                for (int column = 0; column <= this.array.GetUpperBound(1); column++)
                {
                    if (!this.array[row][column].Equals(item[column]))
                    {
                        row++;
                        goto NextRow;
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

            if (item.Length != this.array.GetUpperBound(1) + 1)
                throw new ArgumentException("New row must have the same length as other.");

            if (index >= this.array.GetUpperBound(0))
                throw new IndexOutOfRangeException(nameof(index));

            Tsource[][] newarray = new Tsource[this.array.GetUpperBound(0)+2][];

            int i = 0;
            while(i < index)
            {
                newarray[i] = this.array[i];
                i++;
            }

            newarray[i] = item;

            while (i <= this.array.GetUpperBound(0))
            {
                newarray[i] = this.array[i - 1];
                i++;
            }

            this.array = newarray;
        }

        /// <summary>
        /// Remove row with selected index
        /// </summary>
        /// <param name="index">Selected row to remove</param>
        /// <exception cref="IndexOutOfRangeException"/>
        public void RemoveAt(int index)
        {
            if (index >= this.array.GetUpperBound(0))
                throw new IndexOutOfRangeException();

            Tsource[][] newarray = new Tsource[this.array.GetUpperBound(0)+1][];

            for (int i = 0; i < index; i++)
                newarray[i] = this.array[i];

            for (int i = index; i <= this.array.GetUpperBound(0); i++)
                newarray[i] = this.array[i+1];

            this.array = newarray;
        }

        /// <summary>
        /// Add new row to the end of this matrix
        /// </summary>
        /// <param name="item">New row</param>
        public void Add(Tsource[] item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (item.Length != this.array.GetUpperBound(1)+1)
                throw new ArgumentException("New row must have the same length as other.");

            Tsource[][] newarray = new Tsource[this.array.GetUpperBound(0)+2][];
            for (int i = 0; i < this.array.GetUpperBound(0) + 1; i++)
                newarray[i] = this.array[i];
            newarray[this.array.Length] = item;
            this.array = newarray;
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

            if (item.Length != this.array.GetUpperBound(1) + 1)
                return false;

            int row = 0;
        NextRow:
            while (row <= this.array.GetUpperBound(0))
                for (int column = 0; column <= this.array.GetUpperBound(1); column++)
                {
                    if (!this.array[row][column].Equals(item[column]))
                    {
                        row++;
                        goto NextRow;
                    }
                    return true;
                }

            return false;
        }

        /// <summary></summary>
        /// <exception cref="NotSupportedException"/>
        [Obsolete]
        public void CopyTo(Tsource[][] array, int arrayIndex) => throw new NotSupportedException();

        /// <exception cref="NotSupportedException"/>
        [Obsolete]
        public bool Remove(Tsource[] item) => throw new NotSupportedException();

        public IEnumerator<Tsource[]> GetEnumerator() => new RowEnumerator<Tsource>(this.array);
        
        IEnumerator IEnumerable.GetEnumerator() => new RowEnumerator<Tsource>(this.array);
    }
}
