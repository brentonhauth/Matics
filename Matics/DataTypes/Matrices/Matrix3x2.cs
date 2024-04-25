using System.Runtime.InteropServices;

namespace Matics {
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix3x2 {
        #region Static Readonly Matrix3x2 Variables
        #endregion

        public Vector2 Row0, Row1, Row2;

        #region Matrix3x2 Properties & Indexers
        public Matrix2x3 Transposed => new Matrix2x3(Column0, Column1);

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
        #endregion

        #region Matrix3x2 Constructors
        public Matrix3x2(Vector2 row0, Vector2 row1, Vector2 row2) {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
        }

        public Matrix3x2(Vector3 column0, Vector3 column1) {
            Row0.X = column0.X; Row0.Y = column1.X;
            Row1.X = column0.Y; Row1.Y = column1.Y;
            Row2.X = column0.Z; Row2.Y = column1.Z;
        }
        public Matrix3x2(Matrix2 row0to1, Vector2 row2) {
            Row0 = row0to1.Row0;
            Row1 = row0to1.Row1;
            Row2 = row2;
        }
        public Matrix3x2(
            float m00, float m01,
            float m10, float m11,
            float m20, float m21) {
            Row0 = new Vector2(m00, m01);
            Row1 = new Vector2(m10, m11);
            Row2 = new Vector2(m20, m21);
        }
        #endregion

        #region Matrix3x2 Methods
        public static void Subtract(ref Matrix3x2 self, in Matrix3x2 other) {
            Vector2.Subtract(ref self.Row0, in other.Row0);
            Vector2.Subtract(ref self.Row1, in other.Row1);
            Vector2.Subtract(ref self.Row2, in other.Row2);
        }

        public static void Multiply(in Matrix3x2 left, in Matrix2x4 right, out Matrix3x4 prod) {
            Vector2 rightColumnN = right.Column0;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);
            
            rightColumnN = right.Column1;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);
            
            rightColumnN = right.Column2;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.Z);
            
            rightColumnN = right.Column3;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.W);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.W);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.W);
        }
        public static void Multiply(in Matrix3x2 left, in Matrix2x3 right, out Matrix3 prod) {
            Vector2 rightColumnN = right.Column0;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);

            rightColumnN = right.Column1;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);
            
            rightColumnN = right.Column2;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.Z);
        }
        public static void Multiply(in Matrix3x2 left, in Matrix2 right, out Matrix3x2 prod) {
            Vector2 rightColumnN = right.Column0;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);

            rightColumnN = right.Column1;
            Vector2.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector2.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector2.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);
        }
        /// <summary>
        /// Like multiplying a 3x2 matrix by a 2x1 matrix
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="prod"></param>
        public static void Multiply(in Matrix3x2 left, in Vector2 right, out Vector3 prod) {
            Vector2.Dot(in left.Row0, in right, out prod.X);
            Vector2.Dot(in left.Row1, in right, out prod.Y);
            Vector2.Dot(in left.Row2, in right, out prod.Z);
        }
        #endregion

        #region Matrix3x2 Operators

        public static Matrix3x2 operator -(Matrix3x2 left, Matrix3x2 right) {
            Subtract(ref left, in right);
            return left;
        }

        public static Matrix3x4 operator *(Matrix3x2 left, Matrix2x4 right) {
            Multiply(in left, in right, out Matrix3x4 prod);
            return prod;
        }
        public static Matrix3 operator *(Matrix3x2 left, Matrix2x3 right) {
            Multiply(in left, in right, out Matrix3 prod);
            return prod;
        }
        public static Matrix3x2 operator *(Matrix3x2 left, Matrix2 right) {
            Multiply(in left, in right, out Matrix3x2 prod);
            return prod;
        }
        public static Vector3 operator *(Matrix3x2 left, Vector2 right) {
            Multiply(in left, in right, out Vector3 prod);
            return prod;
        }
        #endregion

        #region Matrix3x2 Overrides
        #endregion
    }
}
