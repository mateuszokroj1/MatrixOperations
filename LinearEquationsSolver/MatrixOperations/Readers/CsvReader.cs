using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MatrixOperations.Readers
{
    public class CsvReader : IMatrixReader
    {
        #region Fields

        private StringReader reader;
        private string rowSeparator;
        private string columnSeparator;

        #endregion

        #region Constructors

        public CsvReader(string csvString, string rowSeparator = "\n\r", string columnSeparator = ";")
        {
            if (csvString == null)
                throw new ArgumentNullException();

            if (string.IsNullOrEmpty(rowSeparator))
                throw new ArgumentException(nameof(rowSeparator));

            if (string.IsNullOrEmpty(columnSeparator))
                throw new ArgumentException(nameof(columnSeparator));

            this.reader = new StringReader(csvString);

            this.rowSeparator = rowSeparator;
            this.columnSeparator = columnSeparator;
        }

        public CsvReader(Stream textStream, Encoding textEncoding)
        {
            if (textStream == null)
                throw new ArgumentNullException(nameof(textStream));

            if (textEncoding == null)
                throw new ArgumentNullException(nameof(textEncoding));

            if (!textStream.CanRead)
                throw new IOException("Stream is not readable.");

            if(textStream.Length < 1)
            {
                this.reader = new StringReader(string.Empty);
                return;
            }

            textStream.Position = 0;
            byte[] buffer = new byte[textStream.Length];
            textStream.Read(buffer, 0, buffer.Length);
            string readedText = textEncoding.GetString(buffer);

            this.reader = new StringReader(readedText);
        }

        #endregion

        #region Properties

        public string 

        #endregion

        #region Methods

        public Matrix<Tsource> ReadMatrix<Tsource>()
            where Tsource : struct, IEquatable<Tsource>
        {
            throw new NotImplementedException();
        }

        public Task<Matrix<Tsource>> ReadMatrixAsync<Tsource>()
            where Tsource : struct, IEquatable<Tsource>
        {
            throw new NotImplementedException();
        }

        protected 

        #endregion
    }
}
