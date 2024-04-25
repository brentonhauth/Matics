using System;

namespace Matics.Interfaces {
    public interface IVector<T> : IVector, IEquatable<T>
        where T : IVector {
        T Normalized { get; }
    }

    public interface IVector {
        float Magnitude { get; }
        float MagnitudeSquared { get; }
        float SumValues { get; }
        float[] Array { get; }
        float this[int index] { get; set; }
        unsafe float* Raw { get; }

        void Normalize();
    }
}
