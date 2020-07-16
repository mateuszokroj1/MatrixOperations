using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MatrixOperations.Readers
{
    public interface IMatrixReader
    {
        /// <summary>
        /// Read <see cref="Matrix{Tsource}"/> from current source
        /// </summary>
        Matrix<Tsource> ReadMatrix<Tsource>()
            where Tsource : struct, IEquatable<Tsource>;

        /// <summary>
        /// Read <see cref="Matrix{Tsource}"/> from current source
        /// </summary>
        Task<Matrix<Tsource>> ReadMatrixAsync<Tsource>()
            where Tsource : struct, IEquatable<Tsource>;
    }
}
