using System.Runtime.InteropServices;

namespace Matics {
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix3x4 {
        #region Static Readonly Matrix3x4 Variables
        #endregion

        public Vector4 Row0, Row1, Row2;

        #region Matrix3x4 Properties & Indexers
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
        public Vector3 Column3 {
            get => new Vector3(Row0.W, Row1.W, Row2.W);
            set {
                Row0.W = value.X;
                Row1.W = value.Y;
                Row2.W = value.Z;
            }
        }
        #endregion

        #region Matrix3x4 Constructors
        #endregion

        #region Matrix3x4 Methods
        public static void Multiply(in Matrix3x4 left, in Matrix4 right, out Matrix3x4 prod) {
            Vector4 rightColumnN = right.Column0;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);

            rightColumnN = right.Column1;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);

            rightColumnN = right.Column2;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.Z);

            rightColumnN = right.Column3;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.W);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.W);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.W);
        }
        public static void Multiply(in Matrix3x4 left, in Matrix4x3 right, out Matrix3 prod) {
            Vector4 rightColumnN = right.Column0;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);

            rightColumnN = right.Column1;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);

            rightColumnN = right.Column2;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.Z);
        }
        public static void Multiply(in Matrix3x4 left, in Matrix4x2 right, out Matrix3x2 prod) {
            Vector4 rightColumnN = right.Column0;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);

            rightColumnN = right.Column1;
            Vector4.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector4.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector4.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);
        }
        public static void Multiply(in Matrix3x4 left, in Vector4 right, out Vector3 prod) {
            Vector4.Dot(in left.Row0, in right, out prod.X);
            Vector4.Dot(in left.Row1, in right, out prod.Y);
            Vector4.Dot(in left.Row2, in right, out prod.Z);
        }
        #endregion

        #region Matrix3x4 Operators
        public static Matrix3x4 operator *(Matrix3x4 left, Matrix4 right) {
            Multiply(in left, in right, out Matrix3x4 prod);
            return prod;
        }
        public static Matrix3 operator *(Matrix3x4 left, Matrix4x3 right) {
            Multiply(in left, in right, out Matrix3 prod);
            return prod;
        }
        public static Matrix3x2 operator *(Matrix3x4 left, Matrix4x2 right) {
            Multiply(in left, in right, out Matrix3x2 prod);
            return prod;
        }
        public static Vector3 operator *(Matrix3x4 left, Vector4 right) {
            Multiply(in left, in right, out Vector3 prod);
            return prod;
        }
        #endregion

        #region Matrix3x4 Overrides
        #endregion
    }
}
