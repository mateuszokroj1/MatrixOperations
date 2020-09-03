using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixOperations.Collections
{
    public interface IColumnCollection<Tsource> : IList<Tsource[]>
        where Tsource : struct, IEquatable<Tsource>
    {
        long LongCount { get; }
    }
}
