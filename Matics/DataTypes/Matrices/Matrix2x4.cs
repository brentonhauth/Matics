using System.Runtime.InteropServices;

namespace Matics {
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix2x4 {
        #region Static Readonly Matrix2x4 Variables
        #endregion

        public Vector4 Row0, Row1;

        #region Matrix2x4 Properties & Indexers
        public Matrix4x2 Transposed => new Matrix4x2(Row0, Row1);
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
        public Vector2 Column2 {
            get => new Vector2(Row0.Z, Row1.Z);
            set {
                Row0.Z = value.X;
                Row1.Z = value.Y;
            }
        }
        public Vector2 Column3 {
            get => new Vector2(Row0.W, Row1.W);
            set {
                Row0.W = value.X;
                Row1.W = value.Y;
            }
        }
        #endregion

        #region Matrix2x4 Constructors
        public Matrix2x4(
            float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13) {
            Row0 = new Vector4(m00, m01, m02, m03);
            Row1 = new Vector4(m10, m11, m12, m13);
        }
        public Matrix2x4(Vector4 row0, Vector4 row1) {
            Row0 = row0;
            Row1 = row1;
        }
        public Matrix2x4(Vector2 col0, Vector2 col1, Vector2 col2, Vector2 col3) {
            Row0 = new Vector4(col0.X, col1.X, col2.X, col3.X);
            Row1 = new Vector4(col0.Y, col1.Y, col2.Y, col3.Y);
        }
        public Matrix2x4(Matrix2 m00to11, Matrix2 m02to13) {
            Row0 = new Vector4(m00to11.Row0, m02to13.Row0);
            Row1 = new Vector4(m00to11.Row1, m02to13.Row1);
        }
        #endregion

        #region Matrix2x4 Methods
        #endregion

        #region Matrix2x4 Operators
        public static bool operator ==(Matrix2x4 left, Matrix2x4 right) {
            return left.Row0 == right.Row0 && left.Row1 == right.Row1;
        }
        public static bool operator !=(Matrix2x4 left, Matrix2x4 right) {
            return left.Row0 != right.Row0 || left.Row1 != right.Row1;
        }
        #endregion

        #region Matrix2x4 Overrides
        public override bool Equals(object o) {
            return o is Matrix2x4 m && this == m;
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
