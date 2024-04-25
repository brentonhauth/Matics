using Matics.Interfaces;
using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Matics {
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2 : IVector<Vector2> {
        #region Static Readonly Vector2 Variables
        public static readonly Vector2
            One = new Vector2(1f),
            Zero = new Vector2(0f),
            Max = new Vector2(float.MaxValue),
            Min = new Vector2(float.MinValue),
            PositiveInfinity = new Vector2(float.PositiveInfinity),
            NegativeInfinity = new Vector2(float.NegativeInfinity),
            UnitX = new Vector2(1, 0),
            UnitY = new Vector2(0, 1);
        #endregion

        public float X, Y;

        #region Vector2 Properties & Indexers
        public float this[int index] {
            get {
                switch (index) {
                    case 0: return X;
                    case 1: return Y;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set {
                switch (index) {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public float Magnitude => MathF.Sqrt(MagnitudeSquared);
        public float MagnitudeSquared => (X * X) + (Y * Y);
        public Vector2 Normalized {
            get {
                float m = Magnitude;
                if (m > 0) {
                    Divide(in this, in m, out Vector2 normalized);
                    return normalized;
                } else {
                    return Zero;
                }
            }
        }
        public Vector3 Z0 => new Vector3(this, 0);
        public Vector3 Z1 => new Vector3(this, 1);
        public float[] Array => new[] { X, Y };
        public unsafe Span<float> Span {
            get {
                fixed (float* p = &X) {
                    return new Span<float>(p, 2);
                }
            }
        }
        public float SumValues => X + Y;
        public unsafe float* Raw {
            get {
                fixed (float* p = &X) {
                    return p;
                }
            }
        }
        #endregion

        #region Vector2 Constructors
        public Vector2(float x, float y) {
            X = x; Y = y;
        }
        public Vector2(float xy) {
            X = Y = xy;
        }
        public unsafe Vector2(float* start) {
            X = *start++;
            Y = *start;
        }
        #endregion

        #region Vector2 Methods
        public unsafe void Write(float* p) {
            *p++ = X;
            *p = Y;
        }

        [Pure]
        public static float Distance(Vector2 point, Vector2 lineStart, Vector2 lineEnd) {
            Subtract(in lineStart, in point, out Vector2 AP);
            Subtract(ref lineEnd, in lineStart); // AB = lineEnd
            Projection(in AP, in lineEnd, out Vector2 proj); // Projection of AP on AB
            Subtract(ref lineEnd, in proj);
            return lineEnd.Magnitude;
        }
        [Pure]
        public static float Distance(Vector2 u, Vector2 v) {
            Subtract(ref u, in v);
            return u.Magnitude;
        }
        [Pure]
        public static float DistanceSquared(Vector2 point, Vector2 lineStart, Vector2 lineEnd) {
            Subtract(in lineStart, in point, out Vector2 AP);
            Subtract(ref lineEnd, in lineStart); // AB = lineEnd
            Projection(in AP, in lineEnd, out Vector2 proj); // Projection of AP on AB
            Subtract(ref lineEnd, in proj);
            return lineEnd.MagnitudeSquared;
        }
        [Pure]
        public static float DistanceSquared(Vector2 u, Vector2 v) {
            Subtract(ref u, in v);
            return u.MagnitudeSquared;
        }

        [Pure]
        public static bool WithinRange(Vector2 u, Vector2 v, float range) {
            return DistanceSquared(u, v) <= range * range;
        }
        public void Normalize() {
            Normalize(ref this);
        }
        public static void Normalize(ref Vector2 vec) {
            float m = vec.Magnitude;
            if (m > 0) {
                Divide(ref vec, in m);
            }
        }
        
        [Pure]
        public Vector2 Clamp(float min, float max) {
            return new Vector2(
                x: Maths.Clamp(X, min, max),
                y: Maths.Clamp(Y, min, max));
        }

        [Pure]
        public static float Dot(Vector2 u, Vector2 v) {
            Dot(in u, in v, out float dot);
            return dot;
        }
        public static void Dot(in Vector2 u, in Vector2 v, out float result) {
            result = (u.X * v.X) + (u.Y * v.Y);
        }

        public static float Angle(Vector2 u, Vector2 v) {
            float magx = u.MagnitudeSquared * v.MagnitudeSquared;
            if (magx == 0) return 0f;
            Dot(in u, in v, out float dot);
            return MathF.Acos(dot / MathF.Sqrt(magx));
        }

        public Vector2 Projection(Vector2 against) {
            Projection(in this, in against, out Vector2 proj);
            return proj;
        }
        public static Vector2 Projection(Vector2 vec2, Vector2 against) {
            Projection(in vec2, in against, out Vector2 proj);
            return proj;
        }
        public static void Projection(in Vector2 vec2, in Vector2 against, out Vector2 proj) {
            Dot(in vec2, in against, out float dot);
            dot /= against.MagnitudeSquared;
            Scale(in against, in dot, out proj);
        }

        public static void Add(in Vector2 left, in Vector2 right, out Vector2 sum) {
            sum.X = left.X + right.X;
            sum.Y = left.Y + right.Y;
        }
        public static void Add(ref Vector2 vec2, in Vector2 other) {
            vec2.X += other.X;
            vec2.Y += other.Y;
        }

        public static void Subtract(in Vector2 left, in Vector2 right, out Vector2 diff) {
            diff.X = left.X - right.X;
            diff.Y = left.Y - right.Y;
        }
        public static void Subtract(ref Vector2 vec2, in Vector2 other) {
            vec2.X -= other.X;
            vec2.Y -= other.Y;
        }

        public static void Multiply(in Vector2 left, in Matrix2x4 right, out Vector4 prod) {
            Dot(in left, right.Column0, out prod.X);
            Dot(in left, right.Column1, out prod.Y);
            Dot(in left, right.Column2, out prod.Z);
            Dot(in left, right.Column3, out prod.W);
        }
        public static void Multiply(in Vector2 left, in Matrix2x3 right, out Vector3 prod) {
            Dot(in left, right.Column0, out prod.X);
            Dot(in left, right.Column1, out prod.Y);
            Dot(in left, right.Column2, out prod.Z);
        }
        public static void Multiply(in Vector2 left, in Matrix2 right, out Vector2 prod) {
            Dot(in left, right.Column0, out prod.X);
            Dot(in left, right.Column1, out prod.Y);
        }
        public static void Multiply(in Vector2 left, in Vector2 right, out Matrix2 prod) {
            prod.Row0.X = left.X * right.X;
            prod.Row0.Y = left.X * right.Y;
            prod.Row1.X = left.Y * right.X;
            prod.Row1.Y = left.Y * right.Y;
        }
        public static void Multiply(in Vector2 left, in Vector2 right, out Vector2 prod) {
            prod.X = left.X * right.X;
            prod.Y = left.Y * right.Y;
        }
        public static void Multiply(ref Vector2 vec2, in Vector2 other) {
            vec2.X *= other.X;
            vec2.Y *= other.Y;
        }

        public static void Divide(in Vector2 left, in float right, out Vector2 quot) {
            quot.X = left.X / right;
            quot.Y = left.Y / right;
        }
        public static void Divide(ref Vector2 vec, in float by) {
            vec.X /= by;
            vec.Y /= by;
        }
        public static void Divide(in Vector2 left, in Vector2 right, out Vector2 quot) {
            quot.X = left.X / right.X;
            quot.Y = left.Y / right.Y;
        }
        public static void Divide(ref Vector2 vec2, in Vector2 other) {
            vec2.X /= other.X;
            vec2.Y /= other.Y;
        }

        public static void Scale(in Vector2 vec, in float scale, out Vector2 prod) {
            prod.X = vec.X * scale;
            prod.Y = vec.Y * scale;
        }
        public static void Scale(ref Vector2 vec, in float scale) {
            vec.X *= scale;
            vec.Y *= scale;
        }

        public static void Negate(ref Vector2 vec) {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
        }

        public static void Increment(ref Vector2 vec) {
            ++vec.X; ++vec.Y;
        }

        public static void Decrement(ref Vector2 vec) {
            --vec.X; --vec.Y;
        }
        #endregion

        #region Vector2 Operators
        public static Vector2 operator +(Vector2 left, Vector2 right) {
            Add(ref left, in right);
            return left;
        }
        public static Vector2 operator ++(Vector2 vec) {
            Increment(ref vec);
            return vec;
        }

        public static Vector2 operator -(Vector2 left, Vector2 right) {
            Subtract(ref left, in right);
            return left;
        }
        public static Vector2 operator -(Vector2 vec) {
            Negate(ref vec);
            return vec;
        }
        public static Vector2 operator --(Vector2 vec) {
            Decrement(ref vec);
            return vec;
        }

        public static Vector4 operator *(Vector2 left, Matrix2x4 right) {
            Multiply(in left, in right, out Vector4 prod);
            return prod;
        }
        public static Vector3 operator *(Vector2 left, Matrix2x3 right) {
            Multiply(in left, in right, out Vector3 prod);
            return prod;
        }
        public static Vector2 operator *(Vector2 left, Matrix2 right) {
            Multiply(in left, in right, out Vector2 prod);
            return prod;
        }
        public static Vector2 operator *(Vector2 left, Vector2 right) {
            Multiply(ref left, in right);
            return left;
        }
        public static Vector2 operator *(Vector2 vec, float scale) {
            Scale(ref vec, in scale);
            return vec;
        }
        public static Vector2 operator *(float scale, Vector2 vec) {
            Scale(ref vec, in scale);
            return vec;
        }

        public static Vector2 operator /(Vector2 vec, float by) {
            Divide(ref vec, in by);
            return vec;
        }
        public static Vector2 operator /(Vector2 left, Vector2 right) {
            Divide(ref left, in right);
            return left;
        }

        public static bool operator ==(Vector2 left, Vector2 right) {
            return left.X == right.X &&
                   left.Y == right.Y;
        }
        public static bool operator !=(Vector2 left, Vector2 right) {
            return left.X != right.X ||
                   left.Y != right.Y;
        }

        public static explicit operator Vector2(Vector3 v) => v.XY;
        public static explicit operator Vector2(Vector4 v) => v.XY;
        public static explicit operator float[](Vector2 v) => v.Array;
        public static explicit operator Vector2(float[] a) => new Vector2(a[0], a[1]);
        public static unsafe explicit operator float*(Vector2 v) => v.Raw;
        public static unsafe explicit operator Vector2(float* p) => new Vector2(p);
        public static explicit operator Matrix2(Vector2 v) => new Matrix2(v.X, -v.Y, v.Y, v.X);

        #if ANARCHY_LIBS
        public static implicit operator OpenTK.Vector2(Vector2 v) => new OpenTK.Vector2(v.X, v.Y);
        public static implicit operator Vector2(OpenTK.Vector2 v) => new Vector2(v.X, v.Y);
        public static implicit operator Vector2D(Vector2 v) => new Vector2D(v.X, v.Y);
        public static implicit operator Vector2(Vector2D v) => new Vector2(v.X, v.Y);
        #endif
        #endregion

        #region Vector2 Overrides
        public override bool Equals(object o) {
            return o is Vector2 v && Equals(v);
        }
        public bool Equals(Vector2 other) {
            return Equals(in this, in other);
        }
        public static bool Equals(in Vector2 left, in Vector2 right) {
            return left.X == right.X &&
                   left.Y == right.Y;
        }

        public override int GetHashCode() {
            const int n = -1521134295;
            int code = 1861411795;
            code = code * n + X.GetHashCode();
            code = code * n + Y.GetHashCode();
            return code;
        }
        public override string ToString() => $"({X}, {Y})";
        #endregion
    }
}
