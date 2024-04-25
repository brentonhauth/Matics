using System.Runtime.InteropServices;

namespace Matics {
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4x2 {
        #region Static Readonly Matrix4x2 Variables
        #endregion

        public Vector2 Row0, Row1, Row2, Row3;

        #region Matrix4x2 Properties & Indexers
        public Matrix2x4 Transposed => new Matrix2x4(Row0, Row1, Row2, Row3);

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
        #endregion

        #region Matrix4x2 Constructors
        public Matrix4x2(Vector2 row0, Vector2 row1, Vector2 row2, Vector2 row3) {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
        }
        public Matrix4x2(Vector4 col0, Vector4 col1) {
            Row0 = new Vector2(col0.X, col1.X);
            Row1 = new Vector2(col0.Y, col1.Y);
            Row2 = new Vector2(col0.Z, col1.Z);
            Row3 = new Vector2(col0.W, col1.W);
        }
        #endregion

        #region Matrix4x2 Methods
        public static void Multiply(in Matrix4x2 left, in Matrix2x4 right, out Matrix4 prod) {
            Vector2 rightColumnN = right.Column0;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);
            Vector2.Dot(in left.Row3, in rightColumnN, out prod.Row3.X);

            rightColumnN = right.Column1;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);
            Vector2.Dot(in left.Row3, in rightColumnN, out prod.Row3.Y);

            rightColumnN = right.Column2;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.Z);
            Vector2.Dot(in left.Row3, in rightColumnN, out prod.Row3.Z);

            rightColumnN = right.Column3;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.W);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.W);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.W);
            Vector2.Dot(in left.Row3, in rightColumnN, out prod.Row3.W);
        }
        public static void Multiply(in Matrix4x2 left, in Matrix2x3 right, out Matrix4x3 prod) {
            Vector2 rightColumnN = right.Column0;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);
            Vector2.Dot(in left.Row3, in rightColumnN, out prod.Row3.X);

            rightColumnN = right.Column1;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);
            Vector2.Dot(in left.Row3, in rightColumnN, out prod.Row3.Y);

            rightColumnN = right.Column2;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.Z);
            Vector2.Dot(in left.Row3, in rightColumnN, out prod.Row3.Z);
        }
        public static void Multiply(in Matrix4x2 left, in Matrix2 right, out Matrix4x2 prod) {
            Vector2 rightColumnN = right.Column0;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);
            Vector2.Dot(in left.Row3, in rightColumnN, out prod.Row3.X);

            rightColumnN = right.Column1;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);
            Vector2.Dot(in left.Row3, in rightColumnN, out prod.Row3.Y);
        }
        public static void Multiply(in Matrix4x2 left, in Vector2 right, out Vector4 prod) {
            Vector2.Dot(in left.Row0, in right, out prod.X);
            Vector2.Dot(in left.Row0, in right, out prod.Y);
            Vector2.Dot(in left.Row0, in right, out prod.Z);
            Vector2.Dot(in left.Row0, in right, out prod.W);
        }
        #endregion

        #region Matrix4x2 Operators
        public static Matrix4 operator *(Matrix4x2 left, Matrix2x4 right) {
            Multiply(in left, in right, out Matrix4 prod);
            return prod;
        }
        public static Matrix4x3 operator *(Matrix4x2 left, Matrix2x3 right) {
            Multiply(in left, in right, out Matrix4x3 prod);
            return prod;
        }
        public static Matrix4x2 operator *(Matrix4x2 left, Matrix2 right) {
            Multiply(in left, in right, out Matrix4x2 prod);
            return prod;
        }
        public static Vector4 operator *(Matrix4x2 left, Vector2 right) {
            Multiply(in left, in right, out Vector4 prod);
            return prod;
        }
        #endregion

        #region Matrix4x2 Overrides
        #endregion
    }
}
