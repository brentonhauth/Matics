using Matics.Interfaces;
using System;
using System.Runtime.InteropServices;

namespace Matics {
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3 : IVector<Vector3> {
        #region Static Readonly Vector3 Variables
        public static readonly Vector3
            One = new Vector3(1f),
            Zero = new Vector3(0f),
            Max = new Vector3(float.MaxValue),
            Min = new Vector3(float.MinValue),
            PositiveInfinity = new Vector3(float.PositiveInfinity),
            NegativeInfinity = new Vector3(float.NegativeInfinity),
            UnitX = new Vector3(1, 0, 0),
            UnitY = new Vector3(0, 1, 0),
            UnitZ = new Vector3(0, 0, 1),
            Right = UnitX,
            Up = UnitY,
            Forward = UnitZ,
            Left = -Right,
            Down = -Up,
            Back = -Forward;
        #endregion

        public float X, Y, Z;

        #region Vector3 Properties & Indexers
        public float this[int index] {
            get {
                switch (index) {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set {
                switch (index) {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public float Magnitude => MathF.Sqrt(MagnitudeSquared);
        public float MagnitudeSquared => (X * X) + (Y * Y) + (Z * Z);
        public Vector3 Normalized {
            get {
                float m = Magnitude;
                if (m > 0) {
                    Divide(in this, in m, out Vector3 normalized);
                    return normalized;
                } else {
                    return Zero;
                }
            }
        }

        public Vector2 XY {
            get => new Vector2(X, Y);
            set {
                X = value.X;
                Y = value.Y;
            }
        }
        public Vector2 XZ {
            get => new Vector2(X, Z);
            set {
                X = value.X;
                Z = value.Y;
            }
        }
        public Vector2 YZ {
            get => new Vector2(Y, Z);
            set {
                Y = value.X;
                Z = value.Y;
            }
        }

        public Vector4 W0 => new Vector4(this, 0);
        public Vector4 W1 => new Vector4(this, 1);
        public float[] Array => new[] { X, Y, Z };
        public float SumValues => X + Y + Z;
        public unsafe float* Raw {
            get {
                fixed (float* p = &X) {
                    return p;
                }
            }
        }
        #endregion

        #region Vector3 Constructors
        public Vector3(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3(Vector2 xy, float z) {
            X = xy.X;
            Y = xy.Y;
            Z = z;
        }
        public Vector3(float x, Vector2 yz) {
            X = x; Y = yz.X; Z = yz.Y;
        }
        public Vector3(float xyz) {
            X = Y = Z = xyz;
        }
        public unsafe Vector3(float* start) {
            X = *start++;
            Y = *start++;
            Z = *start;
        }
        #endregion

        #region Vector3 Methods
        public unsafe void Write(float* p) {
            *p++ = X;
            *p++ = Y;
            *p = Z;
        }

        public static float Distance(Vector3 point, Vector3 lineStart, Vector3 lineEnd) {
            Subtract(in lineStart, in point, out Vector3 AP);
            Subtract(ref lineEnd, in lineStart); // AB = lineEnd
            Projection(in AP, in lineEnd, out Vector3 proj); // Projection of AP on AB
            Subtract(ref lineEnd, in proj);
            return lineEnd.Magnitude;
        }
        public static float Distance(Vector3 u, Vector3 v) {
            Subtract(ref u, in v);
            return u.Magnitude;
        }

        public static float DistanceSquared(Vector3 point, Vector3 lineStart, Vector3 lineEnd) {
            Subtract(in lineStart, in point, out Vector3 AP);
            Subtract(ref lineEnd, in lineStart); // AB = lineEnd
            Projection(in AP, in lineEnd, out Vector3 proj); // Projection of AP on AB
            Subtract(ref lineEnd, in proj);
            return lineEnd.MagnitudeSquared;
        }
        public static float DistanceSquared(Vector3 u, Vector3 v) {
            Subtract(ref u, in v);
            return u.MagnitudeSquared;
        }

        public static bool WithinRange(Vector3 u, Vector3 v, float range) {
            return DistanceSquared(u, v) <= range * range;
        }
        public void Normalize() {
            Normalize(ref this);
        }
        public static void Normalize(ref Vector3 vec) {
            float m = vec.Magnitude;
            if (m > 0) {
                Divide(ref vec, in m);
            }
        }

        public Vector3 Clamp(float min, float max) {
            return new Vector3(
                x: Maths.Clamp(X, min, max),
                y: Maths.Clamp(Y, min, max),
                z: Maths.Clamp(Z, min, max));
        }

        public static float Dot(Vector3 u, Vector3 v) {
            Dot(in u, in v, out float result);
            return result;
        }
        public static void Dot(in Vector3 u, in Vector3 v, out float result) {
            result = (u.X * v.X) + (u.Y * v.Y) + (u.Z * v.Z);
        }

        public static Vector3 Cross(Vector3 u, Vector3 v) {
            Cross(in u, in v, out Vector3 prod);
            return prod;
        }
        public static void Cross(in Vector3 u, in Vector3 v, out Vector3 prod) {
            prod.X = (u.Y * v.Z) - (u.Z * v.Y);
            prod.Y = (u.Z * v.X) - (u.X * v.Z);
            prod.Z = (u.X * v.Y) - (u.Y * v.X);
        }

        public static float Angle(Vector3 u, Vector3 v) {
            float magx = u.MagnitudeSquared * v.MagnitudeSquared;
            if (magx == 0) return 0f;
            Dot(in u, in v, out float dot);
            return Maths.Acos(dot / MathF.Sqrt(magx));
        }

        public Vector3 Projection(Vector3 against) {
            Projection(in this, in against, out Vector3 proj);
            return proj;
        }
        public static Vector3 Projection(Vector3 vec3, Vector3 against) {
            Projection(in vec3, in against, out Vector3 proj);
            return proj;
        }
        public static void Projection(in Vector3 vec3, in Vector3 against, out Vector3 proj) {
            Dot(in vec3, in against, out float dot);
            dot /= against.MagnitudeSquared;
            Scale(in against, in dot, out proj);
        }

        public static void Add(in Vector3 left, in Vector3 right, out Vector3 sum) {
            sum.X = left.X + right.X;
            sum.Y = left.Y + right.Y;
            sum.Z = left.Z + right.Z;
        }
        public static void Add(ref Vector3 vec3, in Vector3 other) {
            vec3.X += other.X;
            vec3.Y += other.Y;
            vec3.Z += other.Z;
        }

        public static void Subtract(in Vector3 left, in Vector3 right, out Vector3 diff) {
            diff.X = left.X - right.X;
            diff.Y = left.Y - right.Y;
            diff.Z = left.Z - right.Z;
        }
        public static void Subtract(ref Vector3 vec3, in Vector3 other) {
            vec3.X -= other.X;
            vec3.Y -= other.Y;
            vec3.Z -= other.Z;
        }

        public static void Multiply(in Vector3 left, in Matrix3x4 right, out Vector4 prod) {
            Dot(in left, right.Column0, out prod.X);
            Dot(in left, right.Column1, out prod.Y);
            Dot(in left, right.Column2, out prod.Z);
            Dot(in left, right.Column3, out prod.W);
        }
        public static void Multiply(in Vector3 left, in Matrix3 right, out Vector3 prod) {
            Dot(in left, right.Column0, out prod.X);
            Dot(in left, right.Column1, out prod.Y);
            Dot(in left, right.Column2, out prod.Z);
        
        }
        // TODO: Matrix style multiplication 3x1 * 1x3 => 3x3
        public static void Multiply(in Vector3 left, in Matrix3x2 right, out Vector2 prod) {
            Dot(in left, right.Column0, out prod.X);
            Dot(in left, right.Column1, out prod.Y);
        }
        public static void Multiply(in Vector3 left, in Vector3 right, out Vector3 prod) {
            prod.X = left.X * right.X;
            prod.Y = left.Y * right.Y;
            prod.Z = left.Z * right.Z;
        }
        public static void Multiply(ref Vector3 vec3, in Vector3 other) {
            vec3.X *= other.X;
            vec3.Y *= other.Y;
            vec3.Z *= other.Z;
        }

        public static void Divide(in Vector3 left, in float right, out Vector3 quot) {
            quot.X = left.X / right;
            quot.Y = left.Y / right;
            quot.Z = left.Z / right;
        }
        public static void Divide(ref Vector3 vec, in float by) {
            vec.X /= by;
            vec.Y /= by;
            vec.Z /= by;
        }
        public static void Divide(in Vector3 left, in Vector3 right, out Vector3 quot) {
            quot.X = left.X / right.X;
            quot.Y = left.Y / right.Y;
            quot.Z = left.Z / right.Z;
        }
        public static void Divide(ref Vector3 vec3, in Vector3 other) {
            vec3.X /= other.X;
            vec3.Y /= other.Y;
            vec3.Z /= other.Z;
        }

        public static void Scale(in Vector3 vec, in float scale, out Vector3 prod) {
            prod.X = vec.X * scale;
            prod.Y = vec.Y * scale;
            prod.Z = vec.Z * scale;
        }
        public static void Scale(ref Vector3 vec, in float scale) {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
        }

        public static void Negate(ref Vector3 vec) {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            vec.Z = -vec.Z;
        }

        public static void Increment(ref Vector3 vec) {
            ++vec.X; ++vec.Y; ++vec.Z;
        }

        public static void Decrement(ref Vector3 vec) {
            --vec.X; --vec.Y; --vec.Z;
        }
        #endregion

        #region Vector3 Operators
        public static Vector3 operator +(Vector3 left, Vector3 right) {
            Add(ref left, in right);
            return left;
        }
        public static Vector3 operator ++(Vector3 vec) {
            Increment(ref vec);
            return vec;
        }

        public static Vector3 operator -(Vector3 left, Vector3 right) {
            Subtract(ref left, in right);
            return left;
        }
        public static Vector3 operator -(Vector3 vec) {
            Negate(ref vec);
            return vec;
        }
        public static Vector3 operator --(Vector3 vec) {
            Decrement(ref vec);
            return vec;
        }
        public static Vector4 operator *(Vector3 left, Matrix3x4 right) {
            Multiply(in left, in right, out Vector4 prod);
            return prod;
        }
        public static Vector3 operator *(Vector3 left, Matrix3 right) {
            Multiply(in left, in right, out Vector3 prod);
            return prod;
        }
        public static Vector2 operator *(Vector3 left, Matrix3x2 right) {
            Multiply(in left, in right, out Vector2 prod);
            return prod;
        }
        public static Vector3 operator *(Vector3 left, Vector3 right) {
            Multiply(ref left, in right);
            return left;
        }
        public static Vector3 operator *(Vector3 vec, float scale) {
            Scale(ref vec, in scale);
            return vec;
        }
        public static Vector3 operator *(float scale, Vector3 vec) {
            Scale(ref vec, in scale);
            return vec;
        }

        public static Vector3 operator /(Vector3 vec, float by) {
            Divide(ref vec, in by);
            return vec;
        }
        public static Vector3 operator /(Vector3 left, Vector3 right) {
            Divide(ref left, in right);
            return left;
        }

        public static bool operator ==(Vector3 left, Vector3 right) {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }
        public static bool operator !=(Vector3 left, Vector3 right) {
            return left.X != right.X ||
                   left.Y != right.Y ||
                   left.Z != right.Z;
        }

        public static explicit operator Vector3(Vector2 v) => v.Z0;
        public static explicit operator Vector3(Vector4 v) => v.XYZ;
        public static explicit operator float[](Vector3 v) => v.Array;
        public static explicit operator Vector3(float[] a) => new Vector3(a[0], a[1], a[2]);
        public static unsafe explicit operator float*(Vector3 v) => v.Raw;
        public static unsafe explicit operator Vector3(float* p) => new Vector3(p);
        #if ANARCHY_LIBS
        public static implicit operator OpenTK.Vector3(Vector3 v) => new OpenTK.Vector3(v.X, v.Y, v.Z);
        public static implicit operator Vector3(OpenTK.Vector3 v) => new Vector3(v.X, v.Y, v.Z);
        public static implicit operator JVector(Vector3 v) => new JVector(v.X, v.Y, v.Z);
        public static implicit operator Vector3(JVector v) => new Vector3(v.X, v.Y, v.Z);
        public static implicit operator Vector3D(Vector3 v) => new Vector3D(v.X, v.Y, v.Z);
        public static implicit operator Vector3(Vector3D v) => new Vector3(v.X, v.Y, v.Z);
        #endif
        #endregion

        #region Vector3 Overrides
        public override bool Equals(object o) {
            return o is Vector3 v && Equals(v);
        }
        public bool Equals(Vector3 other) {
            return Equals(in this, in other);
        }
        public static bool Equals(in Vector3 left, in Vector3 right) {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }
        public override int GetHashCode() {
            const int n = -1521134295;
            int code = -307843816;
            code = code * n + X.GetHashCode();
            code = code * n + Y.GetHashCode();
            code = code * n + Z.GetHashCode();
            return code;
        }
        public override string ToString() => $"({X}, {Y}, {Z})";
        #endregion
    }
}
