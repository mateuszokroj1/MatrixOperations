using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MatrixOperations
{
    public class ColumnCollection<Tsource> : IList<Tsource[]> where Tsource : struct
    {

        public ColumnCollection(ref Tsource[][] array)
        {
            this.array = array;
        }

        protected Tsource[][] array;

        public int Count => this.array.GetUpperBound(1) + 1;

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
                Tsource[] ret = new Tsource[this.array.GetUpperBound(0) + 1];
                for (int i = 0; i <= this.array.GetUpperBound(0); i++)
                    ret[i] = this.array[i][index];
                return ret;
            }
            
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                if (value.Length != this.array.GetUpperBound(0) + 1)
                    throw new InvalidOperationException("New array must have the same length as exists.");

                if (index >= this.array.GetUpperBound(1))
                    throw new IndexOutOfRangeException();

                for (int i = 0; i < value.Length; i++)
                    this.array[i][index] = value[i];
            }
        }

        /// <exception cref="NotSupportedException" />
        [Obsolete]
        public int IndexOf(Tsource[] item) => throw new NotSupportedException();

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

            if (index >= this.array.GetUpperBound(0))
                throw new IndexOutOfRangeException();

            Tsource[,] newarray = new Tsource[this.array.GetUpperBound(0) + 1,this.array.GetUpperBound(1)+2];

            for(int i = 0; i <= this.array.GetUpperBound(0); i++)
            {
                for(int j = 0; j < index; j++)
                    newarray[i, j] = this.array[i][j];

                newarray[i, index] = item[i];

                for (int j = index; j <= this.array.GetUpperBound(1); j++)
                    newarray[i, j + 1] = this.array[i][j];
            }
            this.array = newarray as Tsource[][];
        }

        /// <summary>
        /// Remove column with selected index.
        /// </summary>
        /// <param name="index">Column index</param>
        /// <exception cref="IndexOutOfRangeException"/>
        public void RemoveAt(int index)
        {
            if (index >= this.array.GetUpperBound(1))
                throw new IndexOutOfRangeException();

            Tsource[,] newarray = new Tsource[this.array.GetUpperBound(0)+1,this.array.GetUpperBound(1)];

            for(int i = 0; i <= this.array.GetUpperBound(0); i++)
            {
                for (int j = 0; j < index; j++)
                    newarray[i, j] = this.array[i][j];

                for (int j = index; j <= this.array.GetUpperBound(1); j++)
                    newarray[i, j] = this.array[i][j+1];
            }
            this.array = newarray as Tsource[][];
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

            if (item.Length != this.array.GetUpperBound(0) + 1)
                throw new InvalidOperationException("New column must have the same length as other.");

            Tsource[,] newarray = new Tsource[this.array.GetUpperBound(0)+1,this.array.GetUpperBound(1)+2];

            for(int i = 0; i <= this.array.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= this.array.GetUpperBound(1); j++)
                    newarray[i, j] = this.array[i][j];

                newarray[i, this.array.GetUpperBound(1) + 1] = item[i];
            }
            this.array = newarray as Tsource[][];
        }

        /// <exception cref="NotSupportedException" />
        [Obsolete]
        public void Clear() => throw new NotSupportedException();

        /// <exception cref="NotSupportedException" />
        [Obsolete]
        public bool Contains(Tsource[] item) => throw new NotSupportedException();

        public void CopyTo(Tsource[][] array, int arrayIndex) => throw new NotImplementedException();

        /// <exception cref="NotSupportedException" />
        [Obsolete]
        public bool Remove(Tsource[] item) => throw new NotSupportedException();

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public IEnumerator<Tsource[]> GetEnumerator() => new ColumnEnumerator<Tsource>(this.array);
        IEnumerator IEnumerable.GetEnumerator() => new ColumnEnumerator<Tsource>(this.array);
    }
}
