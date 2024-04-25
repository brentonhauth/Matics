using Matics.Interfaces;
using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Matics {
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix2 : ISquareMatrix<Matrix2, Vector2> {
        #region Static Readonly Matrix2 Variables
        public static readonly Matrix2
            Identity = new Matrix2(1f),
            Zero = new Matrix2(0f);
        #endregion

        public Vector2 Row0, Row1;

        #region Matrix2 Properties & Indexers
        public float this[int r, int c] {
            get {
                if (r == 0) {
                    if (c == 0) {
                        return Row0.X;
                    } else if (c == 1) {
                        return Row0.Y;
                    }
                } else if (r == 1) {
                    if (c == 0) {
                        return Row1.X;
                    } else if (c == 1) {
                        return Row1.Y;
                    }
                }
                throw new IndexOutOfRangeException();
            }
            set {
                if (r == 0) {
                    if (c == 0) {
                        Row0.X = value;
                    } else if (c == 1) {
                        Row0.Y = value;
                    }
                } else if (r == 1) {
                    if (c == 0) {
                        Row1.X = value;
                    } else if (c == 1) {
                        Row1.Y = value;
                    }
                }
                throw new IndexOutOfRangeException();
            }
        }

        public float Determinant {
            get => (Row0.X * Row1.Y) - (Row0.Y * Row1.X);
        }

        public Matrix2 Transposed => new Matrix2(Column0, Column1);

        public Vector2 Diagonal {
            get => new Vector2(M11, M22);
            set {
                M11 = value.X;
                M22 = value.Y;
            }
        }

        public Matrix2 Adjoint => new Matrix2(Row1.Y, -Row0.Y, -Row1.X, Row0.X);

        public Matrix2 Inverse {
            get {
                float det = Determinant;
                if (det != 0f) {
                    det = 1f / det;
                    Matrix2 adj = Adjoint;
                    Scale(ref adj, in det);
                    return adj;
                }
                return Zero;
            }
        }

        public float Trace => M11 + M22;

        public Vector2 Column0 {
            get => new Vector2(Row0.X, Row1.X);
            set {
                Row0.X = value.X;
                Row1.X = value.Y;
            }
        }
        public Vector2 Column1 {
            get => new Vector2(Row0.Y, Row1.Y);
            set {
                Row0.Y = value.X;
                Row1.Y = value.Y;
            }
        }

        public float M11 { get => Row0.X; set => Row0.X = value; }
        public float M12 { get => Row0.Y; set => Row0.Y = value; }

        public float M21 { get => Row1.X; set => Row1.X = value; }
        public float M22 { get => Row1.Y; set => Row1.Y = value; }
        #endregion

        #region Matrix2 Constructors
        public Matrix2(Vector2 row0, Vector2 row1) {
            Row0 = row0;
            Row1 = row1;
        }

        public Matrix2(float diagonal) {
            Row0 = new Vector2(diagonal, 0);
            Row1 = new Vector2(0, diagonal);
        }

        public Matrix2(
            float m00, float m01,
            float m10, float m11) {
            Row0 = new Vector2(m00, m01);
            Row1 = new Vector2(m10, m11);
        }
        #endregion

        #region Matrix2 Methods
        [Pure]
        public static Matrix2 CreateRotation(float angle) {
            CreateRotation(in angle, out Matrix2 rotation);
            return rotation;
        }
        public static void CreateRotation(in float angle, out Matrix2 rotation) {
            float sin = MathF.Sin(angle), cos = MathF.Cos(angle);
            rotation.Row0.X = cos;
            rotation.Row0.Y = -sin;
            rotation.Row1.X = sin;
            rotation.Row1.Y = cos;
        }

        public static float Det(in float m00, in float m01, in float m10, in float m11) {
            return (m00 * m11) - (m01 * m10);
        }

        public Vector2 Row(int index) {
            switch (index) {
                case 0: return Row0;
                case 1: return Row1;
                default: throw new IndexOutOfRangeException();
            }
        }
        public void Row(int index, Vector2 row) => Row(index, ref row);
        public void Row(int index, ref Vector2 row) {
            switch (index) {
                case 0: Row0 = row; break;
                case 1: Row1 = row; break;
                default: throw new IndexOutOfRangeException();
            }
        }
        public Vector2 Column(int index) {
            switch (index) {
                case 0: return Column0;
                case 1: return Column1;
            }
            throw new IndexOutOfRangeException();
        }
        public void Column(int index, Vector2 column) => Column(index, ref column);
        public void Column(int index, ref Vector2 column) {
            switch (index) {
                case 0: Column0 = column; break;
                case 1: Column1 = column; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        public static void Add(in Matrix2 left, in Matrix2 right, out Matrix2 sum) {
            Vector2.Add(in left.Row0, in right.Row0, out sum.Row0);
            Vector2.Add(in left.Row1, in right.Row1, out sum.Row1);
        }
        public static void Add(ref Matrix2 self, in Matrix2 other) {
            Vector2.Add(ref self.Row0, in other.Row0);
            Vector2.Add(ref self.Row1, in other.Row1);
        }

        public static void Subtract(in Matrix2 left, in Matrix2 right, out Matrix2 diff) {
            Vector2.Subtract(in left.Row0, in right.Row0, out diff.Row0);
            Vector2.Subtract(in left.Row1, in right.Row1, out diff.Row1);
        }
        public static void Subtract(ref Matrix2 self, in Matrix2 other) {
            Vector2.Subtract(ref self.Row0, in other.Row0);
            Vector2.Subtract(ref self.Row1, in other.Row1);
        }

        public static void Multiply(in Matrix2 left, in Matrix2x4 right, out Matrix2x4 prod) {
            Vector2 rightColumnN = right.Column0;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);

            rightColumnN = right.Column1;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);

            rightColumnN = right.Column2;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);

            rightColumnN = right.Column3;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.W);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.W);

        }
        public static void Multiply(in Matrix2 left, in Matrix2x3 right, out Matrix2x3 prod) {
            Vector2 rightColumnN = right.Column0;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);

            rightColumnN = right.Column1;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);

            rightColumnN = right.Column2;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);
        }
        public static void Multiply(in Matrix2 left, in Matrix2 right, out Matrix2 prod) {
            Vector2 rightColumnN = right.Column0;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);

            rightColumnN = right.Column1;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
        }
        /// <summary>
        /// Like multiplying a 2x2 matrix by a 2x1 matrix to get a 2x1 matrix
        /// </summary>
        /// <param name="left">The 2x2 matrix</param>
        /// <param name="right">The 2x1 (column) matrix</param>
        /// <param name="prod">The product, a 2x1 (column) matrix</param>
        public static void Multiply(in Matrix2 left, in Vector2 right, out Vector2 prod) {
            Vector2.Dot(in left.Row0, in right, out prod.X);
            Vector2.Dot(in left.Row1, in right, out prod.Y);
        }

        public static void Scale(in Matrix2 mat, in float scale, out Matrix2 prod) {
            Vector2.Scale(in mat.Row0, in scale, out prod.Row0);
            Vector2.Scale(in mat.Row1, in scale, out prod.Row1);
        }
        public static void Scale(ref Matrix2 mat, in float scale) {
            Vector2.Scale(ref mat.Row0, in scale);
            Vector2.Scale(ref mat.Row1, in scale);
        }
        
        public static void Negate(ref Matrix2 mat) {
            Vector2.Negate(ref mat.Row0);
            Vector2.Negate(ref mat.Row1);
        }
        #endregion

        #region Matrix2 Operators
        public static Matrix2 operator +(Matrix2 left, Matrix2 right) {
            Add(ref left, in right);
            return left;
        }

        public static Matrix2 operator -(Matrix2 left, Matrix2 right) {
            Subtract(ref left, in right);
            return left;
        }
        public static Matrix2 operator -(Matrix2 mat) {
            Negate(ref mat);
            return mat;
        }

        public static Matrix2x4 operator *(Matrix2 left, Matrix2x4 right) {
            Multiply(in left, in right, out Matrix2x4 prod);
            return prod;
        }
        public static Matrix2x3 operator *(Matrix2 left, Matrix2x3 right) {
            Multiply(in left, in right, out Matrix2x3 prod);
            return prod;
        }
        public static Matrix2 operator *(Matrix2 left, Matrix2 right) {
            Multiply(in left, in right, out Matrix2 prod);
            return prod;
        }
        public static Vector2 operator *(Matrix2 left, Vector2 right) {
            Multiply(in left, in right, out Vector2 prod);
            return prod;
        }

        public static Matrix2 operator *(float scale, Matrix2 mat) {
            Scale(ref mat, in scale);
            return mat;
        }
        public static Matrix2 operator *(Matrix2 mat, float scale) {
            Scale(ref mat, in scale);
            return mat;
        }

        public static Matrix2 operator /(Matrix2 left, float right) {
            right = 1f / right;
            Scale(ref left, in right);
            return left;
        }

        public static bool operator ==(Matrix2 left, Matrix2 right) {
            return left.Row0 == right.Row0 &&
                   left.Row1 == right.Row1;
        }
        public static bool operator !=(Matrix2 left, Matrix2 right) {
            return left.Row0 != right.Row0 ||
                   left.Row1 != right.Row1;
        }
        #if ANARCHY_LIBS
        public static implicit operator OpenTK.Matrix2(Matrix2 m) => new OpenTK.Matrix2(m.Row0, m.Row1);
        public static implicit operator Matrix2(OpenTK.Matrix2 m) => new Matrix2(m.Row0, m.Row1);
        #endif
        #endregion

        #region Matrix2 Overrides
        public override bool Equals(object o) {
            return o is Matrix2 m && Equals(m);
        }
        public bool Equals(Matrix2 other) {
            return Equals(in this, in other);
        }
        public static bool Equals(in Matrix2 left, in Matrix2 right) {
            return Vector2.Equals(in left.Row0, in right.Row0) &&
                   Vector2.Equals(in left.Row1, in right.Row1);
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string ToString() {
            return $"{Row0}\n{Row1}";
        }
        #endregion
    }
}
