using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MatrixOperations.Readers
{
    public class CsvReader : IMatrixReader
    {
        #region Fields

        private string content;
        private string rowsSeparator;
        private string columnsSeparator;

        #endregion

        #region Constructors

        public CsvReader(string csvString, string rowsSeparator = "\n\r", string columnsSeparator = ";")
        {
            this.content = csvString ?? throw new ArgumentNullException();

            if (string.IsNullOrEmpty(rowsSeparator))
                throw new ArgumentException(nameof(rowsSeparator));

            if (string.IsNullOrEmpty(columnsSeparator))
                throw new ArgumentException(nameof(columnsSeparator));

            this.rowsSeparator = rowsSeparator;
            this.columnsSeparator = columnsSeparator;
        }

        public CsvReader(Stream textStream, Encoding textEncoding, string rowsSeparator = "\n\r", string columnsSeparator = ";")
        {
            if (textStream == null)
                throw new ArgumentNullException(nameof(textStream));

            if (textEncoding == null)
                throw new ArgumentNullException(nameof(textEncoding));

            if (!textStream.CanRead)
                throw new IOException("Stream is not readable.");

            if (string.IsNullOrEmpty(rowsSeparator))
                throw new ArgumentException(nameof(rowsSeparator));

            if (string.IsNullOrEmpty(columnsSeparator))
                throw new ArgumentException(nameof(columnsSeparator));

            this.rowsSeparator = rowsSeparator;
            this.columnsSeparator = columnsSeparator;

            if (textStream.Length < 1)
            {
                this.content = string.Empty;
                return;
            }

            textStream.Position = 0;
            byte[] buffer = new byte[textStream.Length];
            textStream.Read(buffer, 0, buffer.Length);

            this.content = textEncoding.GetString(buffer);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public string RowsSeparator
        {
            get => this.rowsSeparator;
            set
            {
                if(this.rowsSeparator != value)
                {
                    if (string.IsNullOrEmpty(value))
                        throw new ArgumentException();

                    this.rowsSeparator = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentException" />
        public string ColumnsSeparator
        {
            get => this.columnsSeparator;
            set
            {
                if(this.columnsSeparator != value)
                {
                    if (string.IsNullOrEmpty(value))
                        throw new ArgumentException();

                    this.columnsSeparator = value;
                }
            }
        }

        #endregion

        #region Methods

        public Matrix<Tsource> ReadMatrix<Tsource>()
            where Tsource : struct, IEquatable<Tsource> => ReadMatrixAsync<Tsource>().Result;

        public async Task<Matrix<Tsource>> ReadMatrixAsync<Tsource>()
            where Tsource : struct, IEquatable<Tsource>
        {
            string[][] values = null;

            lock (this.rowsSeparator)
            {
                lock(this.columnsSeparator)
                {
                    lock(this.content)
                    {
                        values = this.content
                            .Split(new string[] { RowsSeparator }, StringSplitOptions.None)
                            .Select(row => row.Split(new string[] { columnsSeparator }, StringSplitOptions.None))
                            .ToArray();
                    }
                }
            }

            Tsource[][] parsedValues = values.Select(row => row.Select(cell => (Tsource)Convert.ChangeType(cell, typeof(Tsource))).ToArray()
            ).ToArray();
        }

        #endregion
    }
}
