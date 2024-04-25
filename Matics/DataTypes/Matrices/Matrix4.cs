using Matics.Interfaces;
using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Matics {
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4 : ISquareMatrix<Matrix4, Vector4> {
        #region Static Readonly Matrix4 Variables
        public static readonly Matrix4
            Identity = new Matrix4(1f),
            Zero = new Matrix4(0f);
        #endregion

        public Vector4 Row0, Row1, Row2, Row3;

        #region Matrix4 Indexers & Properties
        public unsafe float this[int r, int c] {
            get {
                if (r < 0 || r > 3 || c < 0 || c > 3) {
                    throw new IndexOutOfRangeException();
                }
                fixed (float* p = &Row0.X) {
                    return *(p + (r * 4) + c);
                }
            }
            set {
                if (r < 0 || r > 3 || c < 0 || c > 3) {
                    throw new IndexOutOfRangeException();
                }
                fixed (float* p = &Row0.X) {
                    *(p + (r * 4) + c) = value;
                }
            }
        }

        public float Determinant {
            get {
                float a = (Row2.Z * Row3.W) - (Row2.W * Row3.Z),
                      b = (Row2.Y * Row3.W) - (Row2.W * Row3.Y),
                      c = (Row2.Y * Row3.Z) - (Row2.Z * Row3.Y),
                      d = (Row2.X * Row3.W) - (Row2.W * Row3.X),
                      e = (Row2.X * Row3.Z) - (Row2.Z * Row3.X),
                      f = (Row2.X * Row3.Y) - (Row2.Y * Row3.X);

                return (Row0.X * ((Row1.Y * a) - (Row1.Z * b) + (Row1.W * c)))
                     - (Row0.Y * ((Row1.X * a) - (Row1.Z * d) + (Row1.W * e)))
                     + (Row0.Z * ((Row1.X * b) - (Row1.Y * d) + (Row1.W * f)))
                     - (Row0.W * ((Row1.X * c) - (Row1.Y * e) + (Row1.Z * f)));
            }
        }

        public Matrix4 Transposed => new Matrix4(Column0, Column1, Column2, Column3);

        public Vector4 Diagonal {
            get => new Vector4(M11, M22, M33, M44);
            set {
                M11 = value.X;
                M22 = value.Y;
                M33 = value.Z;
                M44 = value.W;
            }
        }

        public float Trace => M11 + M22 + M33 + M44;

        public Vector4 Column0 {
            get => new Vector4(Row0.X, Row1.X, Row2.X, Row3.X);
            set {
                Row0.X = value.X;
                Row1.X = value.Y;
                Row2.X = value.Z;
                Row3.X = value.W;
            }
        }
        public Vector4 Column1 {
            get => new Vector4(Row0.Y, Row1.Y, Row2.Y, Row3.Y);
            set {
                Row0.Y = value.X;
                Row1.Y = value.Y;
                Row2.Y = value.Z;
                Row3.Y = value.W;
            }
        }
        public Vector4 Column2 {
            get => new Vector4(Row0.Z, Row1.Z, Row2.Z, Row3.Z);
            set {
                Row0.Z = value.X;
                Row1.Z = value.Y;
                Row2.Z = value.Z;
                Row3.Z = value.W;
            }
        }
        public Vector4 Column3 {
            get => new Vector4(Row0.W, Row1.W, Row2.W, Row3.W);
            set {
                Row0.W = value.X;
                Row1.W = value.Y;
                Row2.W = value.Z;
                Row3.W = value.W;
            }
        }

        // M(row)(column)
        public float M11 { get => Row0.X; set => Row0.X = value; }
        public float M12 { get => Row0.Y; set => Row0.Y = value; }
        public float M13 { get => Row0.Z; set => Row0.Z = value; }
        public float M14 { get => Row0.W; set => Row0.W = value; }

        public float M21 { get => Row1.X; set => Row1.X = value; }
        public float M22 { get => Row1.Y; set => Row1.Y = value; }
        public float M23 { get => Row1.Z; set => Row1.Z = value; }
        public float M24 { get => Row1.W; set => Row1.W = value; }

        public float M31 { get => Row2.X; set => Row2.X = value; }
        public float M32 { get => Row2.Y; set => Row2.Y = value; }
        public float M33 { get => Row2.Z; set => Row2.Z = value; }
        public float M34 { get => Row2.W; set => Row2.W = value; }

        public float M41 { get => Row3.X; set => Row3.X = value; }
        public float M42 { get => Row3.Y; set => Row3.Y = value; }
        public float M43 { get => Row3.Z; set => Row3.Z = value; }
        public float M44 { get => Row3.W; set => Row3.W = value; }
        #endregion

        #region Matrix4 Constructors
        public Matrix4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3) {
            Row0 = row0; Row1 = row1;
            Row2 = row2; Row3 = row3;
        }

        public Matrix4(float diagonal) {
            Row0 = new Vector4(diagonal, 0, 0, 0);
            Row1 = new Vector4(0, diagonal, 0, 0);
            Row2 = new Vector4(0, 0, diagonal, 0);
            Row3 = new Vector4(0, 0, 0, diagonal);
        }

        public Matrix4(
            float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33) {
            Row0 = new Vector4(m00, m01, m02, m03);
            Row1 = new Vector4(m10, m11, m12, m13);
            Row2 = new Vector4(m20, m21, m22, m23);
            Row3 = new Vector4(m30, m31, m32, m33);
        }

        public Matrix4(Matrix3 m00to33) {
            Row0 = m00to33.Row0.W0;
            Row1 = m00to33.Row1.W0;
            Row2 = m00to33.Row2.W0;
            Row3 = Vector4.UnitW;
        }
        #endregion

        #region Matrix4 Methods
        public Vector4 Row(int index) {
            switch (index) {
                case 0: return Row0;
                case 1: return Row1;
                case 2: return Row2;
                case 3: return Row3;
                default: throw new IndexOutOfRangeException();
            }
        }
        public void Row(int index, Vector4 row) => Row(index, ref row);
        public void Row(int index, ref Vector4 row) {
            switch (index) {
                case 0: Row0 = row; break;
                case 1: Row1 = row; break;
                case 2: Row2 = row; break;
                case 3: Row3 = row; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        public Vector4 Column(int index) {
            switch (index) {
                case 0: return Column0;
                case 1: return Column1;
                case 2: return Column2;
                case 3: return Column3;
                default: throw new IndexOutOfRangeException();
            }
        }
        public void Column(int index, Vector4 column) => Column(index, ref column);
        public void Column(int index, ref Vector4 column) {
            switch (index) {
                case 0: Column0 = column; break;
                case 1: Column1 = column; break;
                case 2: Column2 = column; break;
                case 3: Column3 = column; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        public static Matrix4 LookAt(Vector3 eye, Vector3 at, Vector3 up) {
            Vector3.Subtract(in eye, in at, out Vector3 z);
            Vector3.Normalize(ref z);
            Vector3.Cross(in up, in z, out Vector3 x);
            Vector3.Normalize(ref x);
            Vector3.Cross(in z, in x, out Vector3 y);
            Vector3.Normalize(ref y);

            Vector3.Dot(in x, in eye, out float mx);
            Vector3.Dot(in y, in eye, out float my);
            Vector3.Dot(in z, in eye, out float mz);

            return new Matrix4(
                x.X, y.X, z.X, 0,
                x.Y, y.Y, z.Y, 0,
                x.Z, y.Z, z.Z, 0,
                -mx, -my, -mz, 1);
        }

        public static Matrix4 PerspectiveFOV(float fov, float aspect, float near, float far) {
            float y = 1f / Maths.Tan(fov * .5f),
                  x = y / aspect,
                  d = near - far;
            
            return new Matrix4(
                x, 0, 0, 0,
                0, y, 0, 0,
                0, 0, (far + near) / d, -1,
                0, 0, (2 * far * near) / d, 0);
        }

        public static Matrix4 CreateTranslation(Vector3 translate) {
            CreateTranslation(in translate, out Matrix4 matrix);
            return matrix;
        }
        public static void CreateTranslation(in Vector3 translate, out Matrix4 matrix) {
            matrix.Row0 = Vector4.UnitX;
            matrix.Row1 = Vector4.UnitY;
            matrix.Row2 = Vector4.UnitZ;
            matrix.Row3 = translate.W1;
        }

        public static Matrix4 CreateScale(Vector3 scale) {
            CreateScale(in scale, out Matrix4 matrix);
            return matrix;
        }
        public static void CreateScale(in Vector3 scale, out Matrix4 matrix) {
            matrix = new Matrix4 {
                Diagonal = scale.W1
            };
        }

        public static Matrix4 CreateRotation(Quaternion rotation) {
            CreateRotation(in rotation, out Matrix4 matrix);
            return matrix;
        }
        public static void CreateRotation(in Quaternion rotation, out Matrix4 matrix) {
            Vector3.Multiply(in rotation.XYZ, in rotation.XYZ, out Vector3 sq);
            float sqW = rotation.W * rotation.W,
                  xy = rotation.XYZ.X * rotation.XYZ.Y,
                  xz = rotation.XYZ.X * rotation.XYZ.Z,
                  xw = rotation.XYZ.X * rotation.W,
                  yz = rotation.XYZ.Y * rotation.XYZ.Z,
                  yw = rotation.XYZ.Y * rotation.W,
                  zw = rotation.XYZ.Z * rotation.W,
                  s = 2f / (sq.SumValues + sqW);

            matrix.Row0 = new Vector4(
                x: 1f - (s * (sq.Y + sq.Z)),
                y: s * (xy + zw),
                z: s * (xz - yw),
                w: 0);
            matrix.Row1 = new Vector4(
                x: s * (xy - zw),
                y: 1f - (s * (sq.X + sq.Z)),
                z: s * (yz + xw),
                w: 0);
            matrix.Row2 = new Vector4(
                x: s * (xz + yw),
                y: s * (yz - xw),
                z: 1f - (s * (sq.X + sq.Y)),
                w: 0);
            matrix.Row3 = Vector4.UnitW;
        }

        public static void Add(in Matrix4 left, in Matrix4 right, out Matrix4 sum) {
            Vector4.Add(in left.Row0, in right.Row0, out sum.Row0);
            Vector4.Add(in left.Row1, in right.Row1, out sum.Row1);
            Vector4.Add(in left.Row2, in right.Row2, out sum.Row2);
            Vector4.Add(in left.Row3, in right.Row3, out sum.Row3);
        }
        public static void Add(ref Matrix4 self, in Matrix4 other) {
            Vector4.Add(ref self.Row0, in other.Row0);
            Vector4.Add(ref self.Row1, in other.Row1);
            Vector4.Add(ref self.Row2, in other.Row2);
            Vector4.Add(ref self.Row3, in other.Row3);
        }

        public static void Subtract(in Matrix4 left, in Matrix4 right, out Matrix4 diff) {
            Vector4.Subtract(in left.Row0, in right.Row0, out diff.Row0);
            Vector4.Subtract(in left.Row1, in right.Row1, out diff.Row1);
            Vector4.Subtract(in left.Row2, in right.Row2, out diff.Row2);
            Vector4.Subtract(in left.Row3, in right.Row3, out diff.Row3);
        }
        public static void Subtract(ref Matrix4 self, in Matrix4 other) {
            Vector4.Subtract(ref self.Row0, in other.Row0);
            Vector4.Subtract(ref self.Row1, in other.Row1);
            Vector4.Subtract(ref self.Row2, in other.Row2);
            Vector4.Subtract(ref self.Row3, in other.Row3);
        }

        public static void Multiply(in Matrix4 left, in Matrix4 right, out Matrix4 prod) {
            Vector4 rightColumnN = right.Column0;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);
            Vector4.Dot(in left.Row3, in rightColumnN, out prod.Row3.X);

            rightColumnN = right.Column1;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);
            Vector4.Dot(in left.Row3, in rightColumnN, out prod.Row3.Y);

            rightColumnN = right.Column2;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.Z);
            Vector4.Dot(in left.Row3, in rightColumnN, out prod.Row3.Z);

            rightColumnN = right.Column3;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.W);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.W);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.W);
            Vector4.Dot(in left.Row3, in rightColumnN, out prod.Row3.W);
        }
        public static void Multiply(in Matrix4 left, in Matrix4x3 right, out Matrix4x3 prod) {
            Vector4 rightColumnN = right.Column0;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);
            Vector4.Dot(in left.Row3, in rightColumnN, out prod.Row3.X);

            rightColumnN = right.Column1;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);
            Vector4.Dot(in left.Row3, in rightColumnN, out prod.Row3.Y);

            rightColumnN = right.Column2;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.Z);
            Vector4.Dot(in left.Row3, in rightColumnN, out prod.Row3.Z);
        }
        public static void Multiply(in Matrix4 left, in Matrix4x2 right, out Matrix4x2 prod) {
            Vector4 rightColumnN = right.Column0;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);
            Vector4.Dot(in left.Row3, in rightColumnN, out prod.Row3.X);

            rightColumnN = right.Column1;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);
            Vector4.Dot(in left.Row3, in rightColumnN, out prod.Row3.Y);
        }
        public static void Multiply(in Matrix4 left, in Vector4 right, out Vector4 prod) {
            Vector4.Dot(in left.Row0, in right, out prod.X);
            Vector4.Dot(in left.Row1, in right, out prod.Y);
            Vector4.Dot(in left.Row2, in right, out prod.Z);
            Vector4.Dot(in left.Row3, in right, out prod.W);
        }

        public static void Scale(in Matrix4 mat, in float scale, out Matrix4 prod) {
            Vector4.Scale(in mat.Row0, in scale, out prod.Row0);
            Vector4.Scale(in mat.Row1, in scale, out prod.Row1);
            Vector4.Scale(in mat.Row2, in scale, out prod.Row2);
            Vector4.Scale(in mat.Row3, in scale, out prod.Row3);
        }
        public static void Scale(ref Matrix4 mat, in float scale) {
            Vector4.Scale(ref mat.Row0, in scale);
            Vector4.Scale(ref mat.Row1, in scale);
            Vector4.Scale(ref mat.Row2, in scale);
            Vector4.Scale(ref mat.Row3, in scale);
        }

        public static void Negate(ref Matrix4 mat) {
            Vector4.Negate(ref mat.Row0);
            Vector4.Negate(ref mat.Row1);
            Vector4.Negate(ref mat.Row2);
            Vector4.Negate(ref mat.Row3);
        }
        #endregion

        #region Matrix4 Operators
        public static Matrix4 operator +(Matrix4 left, Matrix4 right) {
            Add(ref left, in right);
            return left;
        }

        public static Matrix4 operator -(Matrix4 left, Matrix4 right) {
            Subtract(ref left, in right);
            return left;
        }
        public static Matrix4 operator -(Matrix4 mat) {
            Negate(ref mat);
            return mat;
        }

        public static Matrix4 operator *(Matrix4 left, Matrix4 right) {
            Multiply(in left, in right, out Matrix4 prod);
            return prod;
        }
        public static Matrix4x3 operator *(Matrix4 left, Matrix4x3 right) {
            Multiply(in left, in right, out Matrix4x3 prod);
            return prod;
        }
        public static Matrix4x2 operator *(Matrix4 left, Matrix4x2 right) {
            Multiply(in left, in right, out Matrix4x2 prod);
            return prod;
        }
        public static Vector4 operator *(Matrix4 left, Vector4 right) {
            Multiply(in left, in right, out Vector4 prod);
            return prod;
        }
        public static Matrix4 operator *(float scale, Matrix4 mat) {
            Scale(ref mat, in scale);
            return mat;
        }
        public static Matrix4 operator *(Matrix4 mat, float scale) {
            Scale(ref mat, in scale);
            return mat;
        }

        public static bool operator ==(Matrix4 left, Matrix4 right) {
            return left.Row0 == right.Row0 &&
                   left.Row1 == right.Row1 &&
                   left.Row2 == right.Row2 &&
                   left.Row3 == right.Row3;
        }
        public static bool operator !=(Matrix4 left, Matrix4 right) {
            return left.Row0 != right.Row0 ||
                   left.Row1 != right.Row1 ||
                   left.Row2 != right.Row2 ||
                   left.Row3 != right.Row3;
        }

        public static explicit operator Matrix4(Matrix3 m) => new Matrix4(m);

#if ANARCHY_LIBS
        public static implicit operator OpenTK.Matrix4(Matrix4 m) => new OpenTK.Matrix4(m.Row0, m.Row1, m.Row2, m.Row3);
        public static implicit operator Matrix4(OpenTK.Matrix4 m) => new Matrix4(m.Row0, m.Row1, m.Row2, m.Row3);
#endif

        public static implicit operator Matrix4x4(Matrix4 m) => new Matrix4x4(
            m.M11, m.M12, m.M13, m.M14,
            m.M21, m.M22, m.M23, m.M24,
            m.M31, m.M32, m.M33, m.M34,
            m.M41, m.M42, m.M43, m.M44);
        public static implicit operator Matrix4(Matrix4x4 m) => new Matrix4(
            m.M11, m.M12, m.M13, m.M14,
            m.M21, m.M22, m.M23, m.M24,
            m.M31, m.M32, m.M33, m.M34,
            m.M41, m.M42, m.M43, m.M44);
        #endregion

        #region Matrix4 Overrides
        public override bool Equals(object o) => o is Matrix4 m && Equals(m);
        public bool Equals(Matrix4 other) {
            return Row0.Equals(other.Row0) &&
                   Row1.Equals(other.Row1) &&
                   Row2.Equals(other.Row2) &&
                   Row3.Equals(other.Row3);
        }
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() {
            return $"{Row0}\n{Row1}\n{Row2}\n{Row3}";
        }
        #endregion
    }
}
