using Matics.Interfaces;
using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Matics {
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix3 : ISquareMatrix<Matrix3, Vector3> {
        #region Static Readonly Matrix3 Variables
        public static readonly Matrix3 Identity = new Matrix3(1f);
        #endregion

        public Vector3 Row0, Row1, Row2;

        #region Matrix3 Properties & Indexers
        public unsafe float this[int r, int c] {
            get {
                if (r < 0 || r > 2 || c < 0 || c > 2) {
                    throw new IndexOutOfRangeException();
                }
                fixed (float* p = &Row0.X) {
                    return *(p + (r * 3) + c);
                }
            }
            set {
                if (r < 0 || r > 2 || c < 0 || c > 2) {
                    throw new IndexOutOfRangeException();
                }
                fixed (float* p = &Row0.X) {
                    *(p + (r * 3) + c) = value;
                }
            }
        }

        public float Determinant {
            get => (Row0.X * (Row1.Y * Row2.Z - Row1.Z * Row2.Y))
                 - (Row0.Y * (Row1.X * Row2.Z - Row1.Z * Row2.X))
                 + (Row0.Z * (Row1.X * Row2.Y - Row1.Y * Row2.X));
        }

        public Matrix3 Transposed => new Matrix3(Column0, Column1, Column2);

        public Vector3 Diagonal {
            get => new Vector3(Row0.X, Row1.Y, Row2.Z);
            set {
                Row0.X = value.X;
                Row1.Y = value.Y;
                Row2.Z = value.Z;
            }
        }

        public Matrix3 Adjoint {
            get {
                Matrix3 adj = this;
                return adj;
            }
        }

        public float Trace => Row0.X + Row1.Y + Row2.Z;

        public Vector3 Column0 {
            get => new Vector3(Row0.X, Row1.X, Row2.X);
            set {
                Row0.X = value.X;
                Row1.X = value.Y;
                Row2.X = value.Z;
            }
        }
        public Vector3 Column1 {
            get => new Vector3(Row0.Y, Row1.Y, Row2.Y);
            set {
                Row0.Y = value.X;
                Row1.Y = value.Y;
                Row2.Y = value.Z;
            }
        }
        public Vector3 Column2 {
            get => new Vector3(Row0.Z, Row1.Z, Row2.Z);
            set {
                Row0.Z = value.X;
                Row1.Z = value.Y;
                Row2.Z = value.Z;
            }
        }

        public float M11 { get => Row0.X; set => Row0.X = value; }
        public float M12 { get => Row0.Y; set => Row0.Y = value; }
        public float M13 { get => Row0.Z; set => Row0.Z = value; }

        public float M21 { get => Row1.X; set => Row1.X = value; }
        public float M22 { get => Row1.Y; set => Row1.Y = value; }
        public float M23 { get => Row1.Z; set => Row1.Z = value; }

        public float M31 { get => Row2.X; set => Row2.X = value; }
        public float M32 { get => Row2.Y; set => Row2.Y = value; }
        public float M33 { get => Row2.Z; set => Row2.Z = value; }
        #endregion

        #region Matrix3 Constructors
        public Matrix3(Vector3 row0, Vector3 row1, Vector3 row2) {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
        }

        public Matrix3(
            float m00, float m01, float m02,
            float m10, float m11, float m12,
            float m20, float m21, float m22) {
            Row0 = new Vector3(m00, m01, m02);
            Row1 = new Vector3(m10, m11, m12);
            Row2 = new Vector3(m20, m21, m22);
        }

        public Matrix3(Matrix2 m00to22) {
            Row0 = m00to22.Row0.Z0;
            Row1 = m00to22.Row1.Z0;
            Row2 = Vector3.UnitZ;
        }

        public Matrix3(float diagonal) {
            Row0 = new Vector3(diagonal, 0, 0);
            Row1 = new Vector3(0, diagonal, 0);
            Row2 = new Vector3(0, 0, diagonal);
        }
        #endregion

        #region Matrix3 Methods
        public Vector3 Row(int index) {
            switch (index) {
                case 0: return Row0;
                case 1: return Row1;
                case 2: return Row2;
                default: throw new IndexOutOfRangeException();
            }
        }
        public void Row(int index, Vector3 row) => Row(index, ref row);
        public void Row(int index, ref Vector3 row) {
            switch (index) {
                case 0: Row0 = row; break;
                case 1: Row1 = row; break;
                case 2: Row2 = row; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        public Vector3 Column(int index) {
            switch (index) {
                case 0: return Column0;
                case 1: return Column1;
                case 2: return Column2;
                default: throw new IndexOutOfRangeException();
            }
        }
        public void Column(int index, Vector3 column) => Column(index, ref column);
        public void Column(int index, ref Vector3 column) {
            switch (index) {
                case 0: Column0 = column; break;
                case 1: Column1 = column; break;
                case 2: Column2 = column; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        public static void Invert(in Matrix3 matrix, out Matrix3 result) {
            // Original implementation can be found here:
            float row0X = matrix.Row0.X, row0Y = matrix.Row0.Y, row0Z = matrix.Row0.Z;
            float row1X = matrix.Row1.X, row1Y = matrix.Row1.Y, row1Z = matrix.Row1.Z;
            float row2X = matrix.Row2.X, row2Y = matrix.Row2.Y, row2Z = matrix.Row2.Z;

            float invRow0X = (+row1Y * row2Z) - (row1Z * row2Y);
            float invRow1X = (-row1X * row2Z) + (row1Z * row2X);
            float invRow2X = (+row1X * row2Y) - (row1Y * row2X);

            float det = (row0X * invRow0X) + (row0Y * invRow1X) + (row0Z * invRow2X);

            if (det == 0f) {
                throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
            }

            // Compute adjoint:
            result.Row0.X = invRow0X;
            result.Row0.Y = (-row0Y * row2Z) + (row0Z * row2Y);
            result.Row0.Z = (+row0Y * row1Z) - (row0Z * row1Y);
            result.Row1.X = invRow1X;
            result.Row1.Y = (+row0X * row2Z) - (row0Z * row2X);
            result.Row1.Z = (-row0X * row1Z) + (row0Z * row1X);
            result.Row2.X = invRow2X;
            result.Row2.Y = (-row0X * row2Y) + (row0Y * row2X);
            result.Row2.Z = (+row0X * row1Y) - (row0Y * row1X);

            // Multiply adjoint with reciprocal of determinant:
            det = 1.0f / det;
            Vector3.Scale(ref result.Row0, in det);
            Vector3.Scale(ref result.Row1, in det);
            Vector3.Scale(ref result.Row2, in det);
        }

        [Pure]
        public static Matrix3 CreateRotationX(float angle) {
            float c = MathF.Cos(angle), s = MathF.Sin(angle);
            return new Matrix3(
                1, 0, 0,
                0, c, -s,
                0, s, c);
        }
        [Pure]
        public static Matrix3 CreateRotationY(float angle) {
            float c = MathF.Cos(angle), s = MathF.Sin(angle);
            return new Matrix3(
                c, 0, s,
                0, 1, 0,
                -s, 0, c);
        }
        [Pure]
        public static Matrix3 CreateRotationZ(float angle) {
            float c = MathF.Cos(angle), s = MathF.Sin(angle);
            return new Matrix3(
                c, -s, 0,
                s, c, 0,
                0, 0, 1);
        }

        public static void Add(in Matrix3 left, in Matrix3 right, out Matrix3 sum) {
            Vector3.Add(in left.Row0, in right.Row0, out sum.Row0);
            Vector3.Add(in left.Row1, in right.Row1, out sum.Row1);
            Vector3.Add(in left.Row2, in right.Row2, out sum.Row2);
        }
        public static void Add(ref Matrix3 self, in Matrix3 other) {
            Vector3.Add(ref self.Row0, in other.Row0);
            Vector3.Add(ref self.Row1, in other.Row1);
            Vector3.Add(ref self.Row2, in other.Row2);
        }

        public static void Subtract(in Matrix3 left, in Matrix3 right, out Matrix3 diff) {
            Vector3.Subtract(in left.Row0, in right.Row0, out diff.Row0);
            Vector3.Subtract(in left.Row1, in right.Row1, out diff.Row1);
            Vector3.Subtract(in left.Row2, in right.Row2, out diff.Row2);
        }
        public static void Subtract(ref Matrix3 self, in Matrix3 other) {
            Vector3.Subtract(ref self.Row0, in other.Row0);
            Vector3.Subtract(ref self.Row1, in other.Row1);
            Vector3.Subtract(ref self.Row2, in other.Row2);
        }

        public static void Multiply(in Matrix3 left, in Matrix3x4 right, out Matrix3x4 prod) {
            Vector3 rightColumnN = right.Column0;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);

            rightColumnN = right.Column1;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);

            rightColumnN = right.Column2;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.Z);

            rightColumnN = right.Column3;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.W);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.W);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.W);
        }
        public static void Multiply(in Matrix3 left, in Matrix3 right, out Matrix3 prod) {
            Vector3 rightColumnN = right.Column0;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);

            rightColumnN = right.Column1;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);

            rightColumnN = right.Column2;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.Z);
        }
        public static void Multiply(in Matrix3 left, in Matrix3x2 right, out Matrix3x2 prod) {
            Vector3 rightColumnN = right.Column0;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);

            rightColumnN = right.Column1;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);
        }
        public static void Multiply(in Matrix3 left, in Vector3 right, out Vector3 prod) {
            Vector3.Dot(in left.Row0, in right, out prod.X);
            Vector3.Dot(in left.Row1, in right, out prod.Y);
            Vector3.Dot(in left.Row2, in right, out prod.Z);
        }

        public static void Scale(ref Matrix3 mat, in float scale) {
            Vector3.Scale(ref mat.Row0, in scale);
            Vector3.Scale(ref mat.Row1, in scale);
            Vector3.Scale(ref mat.Row2, in scale);
        }
        #endregion

        #region Matrix3 Operators
        public static Matrix3 operator +(Matrix3 left, Matrix3 right) {
            Add(ref left, in right);
            return left;
        }

        public static Matrix3 operator -(Matrix3 left, Matrix3 right) {
            Subtract(ref left, in right);
            return left;
        }

        public static Matrix3x4 operator *(Matrix3 left, Matrix3x4 right) {
            Multiply(in left, in right, out Matrix3x4 prod);
            return prod;
        }
        public static Matrix3 operator *(Matrix3 left, Matrix3 right) {
            Multiply(in left, in right, out Matrix3 prod);
            return prod;
        }
        public static Matrix3x2 operator *(Matrix3 left, Matrix3x2 right) {
            Multiply(in left, in right, out Matrix3x2 prod);
            return prod;
        }
        public static Vector3 operator *(Matrix3 left, Vector3 right) {
            Multiply(in left, in right, out Vector3 prod);
            return prod;
        }
        public static Matrix3 operator *(Matrix3 mat, float scale) {
            Scale(ref mat, in scale);
            return mat;
        }
        public static Matrix3 operator *(float scale, Matrix3 mat) {
            Scale(ref mat, in scale);
            return mat;
        }

        public static bool operator ==(Matrix3 left, Matrix3 right) => Equals(in left, in right);
        public static bool operator !=(Matrix3 left, Matrix3 right) => !Equals(in left, in right);

        public static explicit operator Matrix3(Matrix2 m) => new Matrix3(m);
        public static explicit operator Matrix3(Matrix4 m) => new Matrix3(m.Row0.XYZ, m.Row1.XYZ, m.Row2.XYZ);
#if ANARCHY_LIBS
        public static implicit operator JMatrix(Matrix3 m) => new JMatrix(
            m.M11, m.M12, m.M13,
            m.M21, m.M22, m.M23,
            m.M31, m.M32, m.M33);
        public static implicit operator Matrix3(JMatrix m) => new Matrix3(
            m.M11, m.M12, m.M13,
            m.M21, m.M22, m.M23,
            m.M31, m.M32, m.M33);
        public static implicit operator OpenTK.Matrix3(Matrix3 m) => new OpenTK.Matrix3(m.Row0, m.Row1, m.Row2);
        public static implicit operator Matrix3(OpenTK.Matrix3 m) => new Matrix3(m.Row0, m.Row1, m.Row2);
        public static implicit operator Assimp.Matrix3x3(Matrix3 m) => new Assimp.Matrix3x3(
            m.M11, m.M12, m.M13,
            m.M21, m.M22, m.M23,
            m.M31, m.M32, m.M33);
        public static implicit operator Matrix3(Assimp.Matrix3x3 m) => new Matrix3(
            m.A1, m.A2, m.A3,
            m.B1, m.B2, m.B3,
            m.C1, m.C2, m.C3);
#endif
        #endregion

        #region Matrix3 Overrides
        public override bool Equals(object o) {
            return o is Matrix3 m && Equals(in this, in m);
        }
        public bool Equals(Matrix3 other) {
            return Equals(in this, in other);
        }
        public static bool Equals(in Matrix3 left, in Matrix3 right) {
            return Vector3.Equals(in left.Row0, in right.Row0) &&
                   Vector3.Equals(in left.Row1, in right.Row1) &&
                   Vector3.Equals(in left.Row2, in right.Row2);
        }

        public override int GetHashCode() {
            const int n = -1521134295;
            var code = -99944038;
            code = code * n + Row0.GetHashCode();
            code = code * n + Row1.GetHashCode();
            code = code * n + Row2.GetHashCode();
            return code;
        }

        public override string ToString() {
            return $"{Row0}\n{Row1}\n{Row2}";
        }
        #endregion
    }
}
