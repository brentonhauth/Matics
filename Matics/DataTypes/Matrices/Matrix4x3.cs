using System.Runtime.InteropServices;

namespace Matics {
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4x3 {
        #region Static Readonly Matrix4x3 Variables
        #endregion

        public Vector3 Row0, Row1, Row2, Row3;

        #region Matrix4x3 Properties & Indexers
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
        #endregion

        #region Matrix4x3 Constructors
        #endregion

        #region Matrix4x3 Methods
        public static void Multiply(in Matrix4x3 left, in Matrix3x4 right, out Matrix4 prod) {
            Vector3 rightColumnN = right.Column0;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);
            Vector3.Dot(in left.Row3, in rightColumnN, out prod.Row3.X);

            rightColumnN = right.Column1;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);
            Vector3.Dot(in left.Row3, in rightColumnN, out prod.Row3.Y);

            rightColumnN = right.Column2;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.Z);
            Vector3.Dot(in left.Row3, in rightColumnN, out prod.Row3.Z);

            rightColumnN = right.Column3;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.W);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.W);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.W);
            Vector3.Dot(in left.Row3, in rightColumnN, out prod.Row3.W);
        }
        public static void Multiply(in Matrix4x3 left, in Matrix3 right, out Matrix4x3 prod) {
            Vector3 rightColumnN = right.Column0;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);
            Vector3.Dot(in left.Row3, in rightColumnN, out prod.Row3.X);

            rightColumnN = right.Column1;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);
            Vector3.Dot(in left.Row3, in rightColumnN, out prod.Row3.Y);

            rightColumnN = right.Column2;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.Z);
            Vector3.Dot(in left.Row3, in rightColumnN, out prod.Row3.Z);
        }
        public static void Multiply(in Matrix4x3 left, in Matrix3x2 right, out Matrix4x2 prod) {
            Vector3 rightColumnN = right.Column0;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.X);
            Vector3.Dot(in left.Row3, in rightColumnN, out prod.Row3.X);

            rightColumnN = right.Column1;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
            Vector3.Dot(in left.Row2, in rightColumnN, out prod.Row2.Y);
            Vector3.Dot(in left.Row3, in rightColumnN, out prod.Row3.Y);
        }
        public static void Multiply(in Matrix4x3 left, in Vector3 right, out Vector4 prod) {
            Vector3.Dot(in left.Row0, in right, out prod.X);
            Vector3.Dot(in left.Row1, in right, out prod.Y);
            Vector3.Dot(in left.Row2, in right, out prod.Z);
            Vector3.Dot(in left.Row3, in right, out prod.W);
        }
        #endregion

        #region Matrix4x3 Operators
        public static Matrix4 operator *(Matrix4x3 left, Matrix3x4 right) {
            Multiply(in left, in right, out Matrix4 prod);
            return prod;
        }
        public static Matrix4x3 operator *(Matrix4x3 left, Matrix3 right) {
            Multiply(in left, in right, out Matrix4x3 prod);
            return prod;
        }
        public static Matrix4x2 operator *(Matrix4x3 left, Matrix3x2 right) {
            Multiply(in left, in right, out Matrix4x2 prod);
            return prod;
        }
        public static Vector4 operator *(Matrix4x3 left, Vector3 right) {
            Multiply(in left, in right, out Vector4 prod);
            return prod;
        }
        #endregion

        #region Matrix4x3 Overrides
        #endregion
    }
}
