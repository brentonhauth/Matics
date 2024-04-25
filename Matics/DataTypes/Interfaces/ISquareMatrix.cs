using System;

namespace Matics.Interfaces {
    public interface ISquareMatrix<M, V> : ISquareMatrix, IEquatable<M>
        where M : ISquareMatrix
        where V : IVector<V> {
        M Transposed { get; }
        V Diagonal { get; set; }
    }
    
    public interface ISquareMatrix {
        float Determinant { get; }
        float Trace { get; }
        float this[int row, int col] { get; set; }
    }
}
