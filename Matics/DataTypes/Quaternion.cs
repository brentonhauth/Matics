using System;
using System.Runtime.InteropServices;

namespace Matics {
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion {
        #region Static Readonly Quaternion Variables
        public static readonly Quaternion Identity = new Quaternion(Vector3.Zero, 1f);
        public static readonly Quaternion Zero = new Quaternion(Vector4.Zero);
        public static readonly Quaternion I = new Quaternion(Vector4.UnitX);
        public static readonly Quaternion J = new Quaternion(Vector4.UnitY);
        public static readonly Quaternion K = new Quaternion(Vector4.UnitZ);
        #endregion

        public Vector3 XYZ;
        public float W;

        #region Quaternion Properties & Indexers
        public float X {
            get => XYZ.X;
            set => XYZ.X = value;
        }
        public float Y {
            get => XYZ.Y;
            set => XYZ.Y = value;
        }
        public float Z {
            get => XYZ.Z;
            set => XYZ.Z = value;
        }

        public float MagnitudeSquared => XYZ.MagnitudeSquared + (W * W);
        public float Magnitude => MathF.Sqrt(MagnitudeSquared);

        public Quaternion Inverse {
            get {
                float m = MagnitudeSquared;
                return m != 0f ? (1f / m) * Conjugate : Zero;
            }
        }
        public Quaternion Conjugate { 
            get {
                Quaternion q = this;
                Vector3.Negate(ref q.XYZ);
                return q;
            }
        }
        #endregion

        #region Quaternion Constructors
        public Quaternion(float x, float y, float z, float w) {
            XYZ = new Vector3(x, y, z);
            W = w;
        }

        public Quaternion(Vector3 xyz, float w) {
            XYZ = xyz;
            W = w;
        }

        public Quaternion(Vector4 xyzw) {
            XYZ = xyzw.XYZ;
            W = xyzw.W;
        }

        public Quaternion(Vector3 angles) {
            FromEulerAngles(in angles, out this);
        }
        #endregion

        #region Quaternion Methods
        public static Quaternion FromEulerAngles(Vector3 angles) {
            FromEulerAngles(in angles, out Quaternion quat);
            return quat;
        }
        public static void FromEulerAngles(in Vector3 angles, out Quaternion quat) {
            Vector3.Scale(in angles, .5f, out Vector3 s);

            Vector3 c = new Vector3(
                x: Maths.Cos(s.X),
                y: Maths.Cos(s.Y),
                z: Maths.Cos(s.Z));

            s.X = Maths.Sin(s.X);
            s.Y = Maths.Sin(s.Y);
            s.Z = Maths.Sin(s.Z);
            
            quat.XYZ.X = (s.X * c.Y * c.Z) + (c.X * s.Y * s.Z);
            quat.XYZ.Y = (c.X * s.Y * c.Z) - (s.X * c.Y * s.Z);
            quat.XYZ.Z = (c.X * c.Y * s.Z) + (s.X * s.Y * c.Z);
            quat.W = (c.X * c.Y * c.Z) - (s.X * s.Y * s.Z);
        }

        public static Quaternion Exp(Quaternion q) {
            Quaternion result;
            float m = q.XYZ.Magnitude;
            float x = MathF.Sin(m);
            result.W = MathF.Cos(m);
            Vector3.Divide(in q.XYZ, in m, out result.XYZ);
            Vector3.Scale(ref result.XYZ, in x);
            x = MathF.Exp(q.W);
            Scale(ref result, in x);
            return result;
        }

        public static Quaternion Log(Quaternion q) {
            float m = q.Magnitude;
            Vector3.Normalize(ref q.XYZ);
            Vector3.Scale(ref q.XYZ, MathF.Acos(q.W / m));
            q.W = MathF.Log(m);
            return q;
        }

        public static void Add(in Quaternion left, in Quaternion right, out Quaternion sum) {
            Vector3.Add(in left.XYZ, in right.XYZ, out sum.XYZ);
            sum.W = left.W + right.W;
        }
        public static void Add(ref Quaternion self, in Quaternion other) {
            Vector3.Add(ref self.XYZ, in other.XYZ);
            self.W += other.W;
        }

        public static void Subtract(in Quaternion left, in Quaternion right, out Quaternion diff) {
            Vector3.Subtract(in left.XYZ, in right.XYZ, out diff.XYZ);
            diff.W = left.W - right.W;
        }
        public static void Subtract(ref Quaternion self, in Quaternion other) {
            Vector3.Subtract(ref self.XYZ, in other.XYZ);
            self.W -= other.W;
        }

        public static void Multiply(in Quaternion left, in Quaternion right, out Quaternion prod) {
            /*
             * XYZ = (right.w * left.xyz) + (left.w * right.xyz) + (left.xyz X right.xyz);
             *   W = (left.w * right.w) - (left.xyz · right.xyz);
             */
            Vector3.Scale(in left.XYZ, in right.W, out prod.XYZ);
            Vector3.Scale(in right.XYZ, in left.W, out Vector3 other);
            Vector3.Add(ref prod.XYZ, in other);
            Vector3.Cross(in left.XYZ, in right.XYZ, out other);
            Vector3.Add(ref prod.XYZ, in other);
            prod.W = left.W * right.W;
            Vector3.Dot(in left.XYZ, in right.XYZ, out float dot);
            prod.W -= dot;
        }

        public static void Scale(in Quaternion quat, in float scale, out Quaternion prod) {
            Vector3.Scale(in quat.XYZ, in scale, out prod.XYZ);
            prod.W = quat.W * scale;
        }
        public static void Scale(ref Quaternion quat, in float scale) {
            Vector3.Scale(ref quat.XYZ, in scale);
            quat.W *= scale;
        }

        public static void Divide(ref Quaternion quat, in float by) {
            if (by != 0f) {
                Vector3.Divide(ref quat.XYZ, in by);
                quat.W /= by;
            } else {
                throw new DivideByZeroException();
            }
        }

        public static void Negate(ref Quaternion quat) {
            Vector3.Negate(ref quat.XYZ);
            quat.W = -quat.W;
        }
        #endregion

        #region Quaternion Operators
        public static Quaternion operator +(Quaternion left, Quaternion right) {
            Add(ref left, in right);
            return left;
        }

        public static Quaternion operator -(Quaternion left, Quaternion right) {
            Subtract(ref left, in right);
            return left;
        }
        public static Quaternion operator -(Quaternion quat) {
            Negate(ref quat);
            return quat;
        }

        public static Quaternion operator *(Quaternion left, Quaternion right) {
            Multiply(in left, in right, out Quaternion prod);
            return prod;
        }
        public static Quaternion operator *(float scale, Quaternion quat) {
            Scale(ref quat, in scale);
            return quat;
        }
        public static Quaternion operator *(Quaternion quat, float scale) {
            Scale(ref quat, in scale);
            return quat;
        }

        public static Quaternion operator /(Quaternion quat, float by) {
            Divide(ref quat, in by);
            return quat;
        }

        public static bool operator ==(Quaternion left, Quaternion right) {
            return left.XYZ == right.XYZ && left.W == right.W;
        }
        public static bool operator !=(Quaternion left, Quaternion right) {
            return left.XYZ != right.XYZ || left.W != right.W;
        }

        public static implicit operator System.Numerics.Quaternion(Quaternion q) {
            return new System.Numerics.Quaternion(q.X, q.Y, q.Z, q.W);
        }
        public static implicit operator Quaternion(System.Numerics.Quaternion q) {
            return new Quaternion(q.X, q.Y, q.Z, q.W);
        }
        #if ANARCHY_LIBS
        public static implicit operator OpenTK.Quaternion(Quaternion q) => new OpenTK.Quaternion(q.XYZ, q.W);
        public static implicit operator Quaternion(OpenTK.Quaternion q) => new Quaternion(q.Xyz, q.W);
        #endif
        #endregion

        #region Quaternion Overrides
        public override bool Equals(object o) {
            return o is Quaternion q && Equals(q);
        }
        public bool Equals(Quaternion other) {
            return Vector3.Equals(in XYZ, in other.XYZ) && W == other.W;
        }

        public override int GetHashCode() {
            return HashCode.Combine(XYZ, W);
        }

        public override string ToString() {
            return $"{W}{XYZ.X:+#;-#;+0}i{XYZ.Y:+#;-#;+0}j{XYZ.Z:+#;-#;+0}k";
        }
        #endregion
    }
}
