using System;
using System.Collections.Generic;

namespace MatrixOperations
{
    public interface IRowCollection<Tsource> : IList<Tsource[]>
        where Tsource : struct, IEquatable<Tsource>
    {
        long LongCount { get; }

        void MultiplyWithScalar(int rowIndex, Tsource scalar);
    }
}
