using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MatrixOperations
{
    public class ColumnCollection<Tsource> : IList<Tsource[]> where Tsource : struct
    {
        public ColumnCollection(Matrix<Tsource> matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException();

            this.matrix = matrix;
        }

        protected Matrix<Tsource> matrix;

        public int Count => this.matrix.value.GetUpperBound(1) + 1;

        public bool IsReadOnly => false;

        /// <summary>
        /// Access column with selected index
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="IndexOutOfRangeException"/>
        public Tsource[] this[int index]
        {
            get
            {
                Tsource[] ret = new Tsource[this.matrix.value.GetUpperBound(0) + 1];
                for (int i = 0; i <= this.matrix.value.GetUpperBound(0); i++)
                    ret[i] = this.matrix[i,index];
                return ret;
            }
            
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                if (value.Length != this.matrix.value.GetUpperBound(0) + 1)
                    throw new InvalidOperationException("New array must have the same length as exists.");

                if (index >= this.matrix.value.GetUpperBound(1))
                    throw new IndexOutOfRangeException();

                for (int i = 0; i < value.Length; i++)
                    this.matrix[i,index] = value[i];
            }
        }

        /// <summary>Search matrix for column equals to 'item' and return index of column in matrix or -1.</summary>
        /// <param name="item">Column to search</param>
        /// <exception cref="ArgumentNullException"/>
        public int IndexOf(Tsource[] item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (item.Length != this.matrix.Rows.Count)
                return -1;

            int column = 0;
        NextColumn:
            while (column <= this.matrix.Columns.Count-1)
            {
                for (int row = 0; row < Count; row++)
                    if (!this.matrix[row,column].Equals(item[row]))
                    {
                        column++;
                        goto NextColumn;
                    }
                return column;
            }

            return -1;
        }

        /// <summary>
        /// Insert new column in selected index
        /// </summary>
        /// <param name="index">Inserting place</param>
        /// <param name="item">New column</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="IndexOutOfRangeException" />
        public void Insert(int index, Tsource[] item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (index == this.matrix.Rows.Count)
                throw new IndexOutOfRangeException();

            Tsource[][] newarray = new Tsource[this.matrix.Rows.Count][];

            for(int i = 0; i < this.matrix.Rows.Count; i++)
            {
                newarray[i] = new Tsource[this.matrix.Columns.Count + 1];
                for(int j = 0; j < index; j++)
                    newarray[i][j] = this.matrix[i,j];

                newarray[i][index] = item[i];

                for (int j = index; j < this.matrix.Columns.Count; j++)
                    newarray[i][j + 1] = this.matrix[i,j];
            }
            this.matrix.value = newarray;
        }

        /// <summary>
        /// Remove column with selected index.
        /// </summary>
        /// <param name="index">Column index</param>
        /// <exception cref="IndexOutOfRangeException"/>
        public void RemoveAt(int index)
        {
            if (index >= this.matrix.Columns.Count-1)
                throw new IndexOutOfRangeException();

            this.matrix.SkipColumn((uint)index);
        }
        
        /// <summary>
        /// Add new column on the end of this matrix
        /// </summary>
        /// <param name="item">New column</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException" />
        public void Add(Tsource[] item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (item.Length != this.matrix.Rows.Count)
                throw new InvalidOperationException("New column must have the same length as other.");

            Tsource[,] newarray = new Tsource[this.matrix.Rows.Count,this.matrix.Columns.Count+1];

            for(int i = 0; i < this.matrix.Rows.Count; i++)
            {
                for (int j = 0; j < this.matrix.Columns.Count; j++)
                    newarray[i, j] = this.matrix[i][j];

                newarray[i, this.matrix.GetUpperBound(1) + 1] = item[i];
            }
            this.matrix = newarray as Tsource[][];
        }

        /// <summary></summary>
        /// <exception cref="NotSupportedException" />
        [Obsolete]
        public void Clear() => throw new NotSupportedException();

        /// <summary>
        /// Search matrix for column equals to 'item'
        /// </summary>
        /// <param name="item">Column to search</param>
        /// <returns>True if matrix contain column</returns>
        /// <exception cref="ArgumentNullException" />
        public bool Contains(Tsource[] item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (item.Length != Count)
                return false;
            
            int column = 0;
            NextColumn:
            while (column <= this.matrix.GetUpperBound(1))
            {
                for (int row = 0; row < Count; row++)
                    if (!this.matrix[row][column].Equals(item[row]))
                    {
                        column++;
                        goto NextColumn;
                    }
                return true;
            }
            
            return false;
        }

        public void CopyTo(Tsource[][] array, int arrayIndex) => throw new NotImplementedException();

        /// <summary></summary>
        /// <exception cref="NotSupportedException" />
        [Obsolete]
        public bool Remove(Tsource[] item) => throw new NotSupportedException();

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public IEnumerator<Tsource[]> GetEnumerator() => new ColumnEnumerator<Tsource>(this.matrix);
        IEnumerator IEnumerable.GetEnumerator() => new ColumnEnumerator<Tsource>(this.matrix);
    }
}
