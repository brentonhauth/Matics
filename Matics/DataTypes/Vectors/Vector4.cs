using Matics.Interfaces;
using System;
using System.Runtime.InteropServices;

namespace Matics {
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4 : IVector<Vector4> {
        #region Static Readonly Vector4 Variables
        public static readonly Vector4
            One = new Vector4(1f),
            Zero = new Vector4(0f),
            Max = new Vector4(float.MaxValue),
            Min = new Vector4(float.MinValue),
            PositiveInfinity = new Vector4(float.PositiveInfinity),
            NegativeInfinity = new Vector4(float.NegativeInfinity),
            UnitX = new Vector4(1, 0, 0, 0),
            UnitY = new Vector4(0, 1, 0, 0),
            UnitZ = new Vector4(0, 0, 1, 0),
            UnitW = new Vector4(0, 0, 0, 1);
        #endregion

        public float X, Y, Z, W;

        #region Vector4 Properties & Indexers
        public float this[int index] {
            get {
                switch (index) {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    case 3: return W;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set {
                switch (index) {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    case 3: W = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public float Magnitude => MathF.Sqrt(MagnitudeSquared);
        public float MagnitudeSquared => (X * X) + (Y * Y) + (Z * Z) + (W * W);
        
        public Vector4 Normalized {
            get {
                float m = Magnitude;
                return m == 0 ? Zero : (this / m);
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
        public Vector2 XW {
            get => new Vector2(X, W);
            set {
                X = value.X;
                W = value.Y;
            }
        }
        public Vector2 YZ {
            get => new Vector2(Y, Z);
            set {
                Y = value.X;
                Z = value.Y;
            }
        }
        public Vector2 YW {
            get => new Vector2(Y, W);
            set {
                Y = value.X;
                W = value.Y;
            }
        }
        public Vector2 ZW {
            get => new Vector2(Z, W);
            set {
                Z = value.X;
                W = value.Y;
            }
        }
        public Vector3 XYZ {
            get => new Vector3(X, Y, Z);
            set {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }
        public Vector3 XZW {
            get => new Vector3(X, Z, W);
            set {
                X = value.X;
                Z = value.Y;
                W = value.Z;
            }
        }
        public Vector3 XYW {
            get => new Vector3(X, Y, W);
            set {
                X = value.X;
                Y = value.Y;
                W = value.Z;
            }
        }
        public Vector3 YZW {
            get => new Vector3(Y, Z, W);
            set {
                Y = value.X;
                Z = value.Y;
                W = value.Z;
            }
        }


        public float[] Array => new[] { X, Y, Z, W };
        public float SumValues => X + Y + Z + W;
        public unsafe float* Raw {
            get {
                fixed (float* p = &X) {
                    return p;
                }
            }
        }
        #endregion

        #region Vector4 Constructors
        public Vector4(float x, float y, float z, float w) {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Vector4(Vector2 xy, float z, float w) {
            X = xy.X; Y = xy.Y; Z = z; W = w;
        }
        public Vector4(float x, float y, Vector2 zw) {
            X = x; Y = y; Z = zw.X; W = zw.Y;
        }
        public Vector4(float x, Vector2 yz, float w) {
            X = x; Y = yz.X; Z = yz.Y; W = w;
        }
        public Vector4(Vector2 xy, Vector2 zw) {
            X = xy.X; Y = xy.Y;
            Z = zw.X; W = zw.Y;
        }
        public Vector4(Vector3 xyz, float w) {
            X = xyz.X;
            Y = xyz.Y;
            Z = xyz.Z;
            W = w;
        }
        public Vector4(float x, Vector3 yzw) {
            X = x;
            Y = yzw.X;
            Z = yzw.Y;
            W = yzw.Z;
        }
        public Vector4(float xyzw) {
            X = Y = Z = W = xyzw;
        }
        public unsafe Vector4(float* start) {
            X = *start++;
            Y = *start++;
            Z = *start++;
            W = *start;
        }
        #endregion

        #region Vector4 Methods
        public unsafe void Write(float* p) {
            *p++ = X;
            *p++ = Y;
            *p++ = Z;
            *p = W;
        }

        public static float Distance(Vector4 point, Vector4 lineStart, Vector4 lineEnd) {
            Subtract(in lineStart, in point, out Vector4 AP);
            Subtract(ref lineEnd, in lineStart); // AB = lineEnd
            Projection(in AP, in lineEnd, out Vector4 proj); // Projection of AP on AB
            Subtract(ref lineEnd, in proj);
            return lineEnd.Magnitude;
        }
        public static float Distance(Vector4 u, Vector4 v) {
            Subtract(ref u, in v);
            return u.Magnitude;
        }
        public static float DistanceSquared(Vector4 point, Vector4 lineStart, Vector4 lineEnd) {
            Subtract(in lineStart, in point, out Vector4 AP);
            Subtract(ref lineEnd, in lineStart); // AB = lineEnd
            Projection(in AP, in lineEnd, out Vector4 proj); // Projection of AP on AB
            Subtract(ref lineEnd, in proj);
            return lineEnd.MagnitudeSquared;
        }
        public static float DistanceSquared(Vector4 u, Vector4 v) {
            Subtract(ref u, in v);
            return u.MagnitudeSquared;
        }

        public static bool WithinRange(Vector4 u, Vector4 v, float range) {
            return DistanceSquared(u, v) <= range * range;
        }

        public void Normalize() {
            Normalize(ref this);
        }
        public static void Normalize(ref Vector4 vec4) {
            float m = vec4.Magnitude;
            if (m > 0) {
                Divide(ref vec4, in m);
            }
        }

        public Vector4 Clamp(float min, float max) {
            return new Vector4(
                x: Maths.Clamp(X, min, max),
                y: Maths.Clamp(Y, min, max),
                z: Maths.Clamp(Z, min, max),
                w: Maths.Clamp(W, min, max));
        }

        public static float Dot(Vector4 u, Vector4 v) {
            Dot(in u, in v, out float result);
            return result;
        }
        public static void Dot(in Vector4 u, in Vector4 v, out float result) {
            result = (u.X * v.X) + (u.Y * v.Y) + (u.Z * v.Z) + (u.W * v.W);
        }

        public static float Angle(Vector4 u, Vector4 v) {
            float m = u.MagnitudeSquared * v.MagnitudeSquared;
            if (m == 0f) return 0f;
            m = MathF.Sqrt(m);
            return Maths.Acos(Dot(u, v) / m);
        }

        public Vector4 Projection(Vector4 against) {
            Projection(in this, in against, out Vector4 proj);
            return proj;
        }
        public static Vector4 Projection(Vector4 vec4, Vector4 against) {
            Projection(in vec4, in against, out Vector4 proj);
            return proj;
        }
        public static void Projection(in Vector4 vec4, in Vector4 against, out Vector4 proj) {
            Dot(in vec4, in against, out float dot);
            Scale(in against, dot / against.MagnitudeSquared, out proj);
        }

        public static void Add(in Vector4 left, in Vector4 right, out Vector4 sum) {
            sum.X = left.X + right.X;
            sum.Y = left.Y + right.Y;
            sum.Z = left.Z + right.Z;
            sum.W = left.W + right.W;
        }
        public static void Add(ref Vector4 vec4, in Vector4 other) {
            vec4.X += other.X;
            vec4.Y += other.Y;
            vec4.Z += other.Z;
            vec4.W += other.W;
        }

        public static void Subtract(in Vector4 left, in Vector4 right, out Vector4 diff) {
            diff.X = left.X - right.X;
            diff.Y = left.Y - right.Y;
            diff.Z = left.Z - right.Z;
            diff.W = left.W - right.W;
        }
        public static void Subtract(ref Vector4 vec4, in Vector4 other) {
            vec4.X -= other.X;
            vec4.Y -= other.Y;
            vec4.Z -= other.Z;
            vec4.W -= other.W;
        }
        public static void Multiply(in Vector4 left, in Matrix4 right, out Vector4 prod) {
            Dot(in left, right.Column0, out prod.X);
            Dot(in left, right.Column1, out prod.Y);
            Dot(in left, right.Column2, out prod.Z);
            Dot(in left, right.Column3, out prod.W);
        }
        public static void Multiply(in Vector4 left, in Matrix4x3 right, out Vector3 prod) {
            Dot(in left, right.Column0, out prod.X);
            Dot(in left, right.Column1, out prod.Y);
            Dot(in left, right.Column2, out prod.Z);
        }
        public static void Multiply(in Vector4 left, in Matrix4x2 right, out Vector2 prod) {
            Dot(in left, right.Column0, out prod.X);
            Dot(in left, right.Column1, out prod.Y);
        }
        public static void Multiply(in Vector4 left, in Vector4 right, out Vector4 prod) {
            prod.X = left.X * right.X;
            prod.Y = left.Y * right.Y;
            prod.Z = left.Z * right.Z;
            prod.W = left.W * right.W;
        }
        public static void Multiply(ref Vector4 vec4, in Vector4 other) {
            vec4.X *= other.X;
            vec4.Y *= other.Y;
            vec4.Z *= other.Z;
            vec4.W *= other.W;
        }

        public static void Divide(in Vector4 left, in float right, out Vector4 quot) {
            quot.X = left.X / right;
            quot.Y = left.Y / right;
            quot.Z = left.Z / right;
            quot.W = left.W / right;
        }
        public static void Divide(ref Vector4 vec4, in float by) {
            vec4.X /= by;
            vec4.Y /= by;
            vec4.Z /= by;
            vec4.W /= by;
        }
        public static void Divide(in Vector4 left, in Vector4 right, out Vector4 quot) {
            quot.X = left.X / right.X;
            quot.Y = left.Y / right.Y;
            quot.Z = left.Z / right.Z;
            quot.W = left.W / right.W;
        }
        public static void Divide(ref Vector4 vec4, in Vector4 other) {
            vec4.X /= other.X;
            vec4.Y /= other.Y;
            vec4.Z /= other.Z;
            vec4.W /= other.W;
        }

        public static void Scale(in Vector4 vec4, in float scale, out Vector4 prod) {
            prod.X = vec4.X * scale;
            prod.Y = vec4.Y * scale;
            prod.Z = vec4.Z * scale;
            prod.W = vec4.W * scale;
        }
        public static void Scale(ref Vector4 vec4, in float scale) {
            vec4.X *= scale;
            vec4.Y *= scale;
            vec4.Z *= scale;
            vec4.W *= scale;
        }

        public static void Negate(ref Vector4 vec4) {
            vec4.X = -vec4.X;
            vec4.Y = -vec4.Y;
            vec4.Z = -vec4.Z;
            vec4.W = -vec4.W;
        }

        public static void Increment(ref Vector4 vec4) {
            ++vec4.X; ++vec4.Y; ++vec4.Z; ++vec4.W;
        }

        public static void Decrement(ref Vector4 vec4) {
            --vec4.X; --vec4.Y; --vec4.Z; --vec4.W;
        }
        #endregion

        #region Vector4 Operators
        public static Vector4 operator +(Vector4 left, Vector4 right) {
            Add(ref left, in right);
            return left;
        }
        public static Vector4 operator ++(Vector4 vec4) {
            Increment(ref vec4);
            return vec4;
        }

        public static Vector4 operator -(Vector4 left, Vector4 right) {
            Subtract(ref left, in right);
            return left;
        }
        public static Vector4 operator -(Vector4 vec4) {
            Negate(ref vec4);
            return vec4;
        }
        public static Vector4 operator --(Vector4 vec4) {
            Decrement(ref vec4);
            return vec4;
        }
        public static Vector4 operator *(Vector4 left, Matrix4 right) {
            Multiply(in left, in right, out Vector4 prod);
            return prod;
        }
        public static Vector3 operator *(Vector4 left, Matrix4x3 right) {
            Multiply(in left, in right, out Vector3 prod);
            return prod;
        }
        public static Vector2 operator *(Vector4 left, Matrix4x2 right) {
            Multiply(in left, in right, out Vector2 prod);
            return prod;
        }
        public static Vector4 operator *(Vector4 left, Vector4 right) {
            Multiply(ref left, in right);
            return left;
        }
        public static Vector4 operator *(Vector4 vec4, float scale) {
            Scale(ref vec4, in scale);
            return vec4;
        }
        public static Vector4 operator *(float scale, Vector4 vec4) {
            Scale(ref vec4, in scale);
            return vec4;
        }

        public static Vector4 operator /(Vector4 vec4, float by) {
            Divide(ref vec4, in by);
            return vec4;
        }
        public static Vector4 operator /(Vector4 left, Vector4 right) {
            Divide(ref left, in right);
            return left;
        }

        public static bool operator ==(Vector4 left, Vector4 right) {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z &&
                   left.W == right.W;
        }
        public static bool operator !=(Vector4 left, Vector4 right) {
            return left.X != right.X ||
                   left.Y != right.Y ||
                   left.Z != right.Z ||
                   left.W != right.W;
        }

        public static explicit operator Vector4(Vector2 v) => new Vector4(v, Vector2.Zero);
        public static explicit operator Vector4(Vector3 v) => v.W0;
        public static explicit operator float[](Vector4 v) => v.Array;
        public static explicit operator Vector4(float[] a) => new Vector4(a[0], a[1], a[2], a[3]);
        public static unsafe explicit operator float*(Vector4 v) => v.Raw;
        public static unsafe explicit operator Vector4(float* p) => new Vector4(p);

        #if ANARCHY_LIBS
        public static implicit operator OpenTK.Vector4(Vector4 v) => new OpenTK.Vector4(v.X, v.Y, v.Z, v.W);
        public static implicit operator Vector4(OpenTK.Vector4 v) => new Vector4(v.X, v.Y, v.Z, v.W);
        #endif
        public static implicit operator System.Numerics.Vector4(Vector4 v) {
            return new System.Numerics.Vector4(v.X, v.Y, v.Z, v.W);
        }
        public static implicit operator Vector4(System.Numerics.Vector4 v) {
            return new Vector4(v.X, v.Y, v.Z, v.W);
        }
        #endregion

        #region Vector4 Overrides
        public override bool Equals(object o) {
            return o is Vector4 v && Equals(v);
        }
        public bool Equals(Vector4 other) {
            return Equals(in this, in other);
        }
        public static bool Equals(in Vector4 left, in Vector4 right) {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z &&
                   left.W == right.W;
        }
        public override int GetHashCode() {
            const int n = -1521134295;
            int code = 707706286;
            code = code * n + X.GetHashCode();
            code = code * n + Y.GetHashCode();
            code = code * n + Z.GetHashCode();
            code = code * n + W.GetHashCode();
            return code;
        }
        public override string ToString() {
            return $"({X}, {Y}, {Z}, {W})";
        }
        #endregion
    }
}
