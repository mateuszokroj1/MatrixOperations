using System;
using System.Collections.Generic;

namespace MatrixOperations
{
    public interface IMatrix<Tsource> : IEquatable<Matrix<Tsource>>, ICloneable
        where Tsource : struct, IEquatable<Tsource>
    {
        Tsource this[int rowIndex, int columnIndex] { get; set; }

        ColumnCollection<Tsource> Columns { get; }
        bool IsSquare { get; }
        bool IsVector { get; }
        RowCollection<Tsource> Rows { get; }

        Matrix<Toutput> ConvertTo<Toutput>() where Toutput : struct, IEquatable<Toutput>;
        Matrix<Tsource> GenerateSubMatrix(int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex);
        Matrix<Tsource> SkipColumn(uint columnIndex);
        Matrix<Tsource> SkipRow(uint rowIndex);
        Matrix<Tsource> Transpose();
        void MultiplyRowWithScalar(int rowIndex, Tsource scalarValue);
        void MultiplyColumnWithScalar(int columnIndex, Tsource scalarValue);
        void MultiplyWithScalar(Tsource scalarValue);
        Tsource CalculateDeterminant();
        Matrix<Tsource> Inversion();
        bool CheckIsDiagonal();
        IEnumerable<Tsource> AsVector();
    }
}
