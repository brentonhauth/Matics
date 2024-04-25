using System.Runtime.InteropServices;

namespace Matics {
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix2x3 {
        #region Static Readonly Matrix2x3 Variables
        #endregion

        public Vector3 Row0, Row1;

        #region Matrix2x3 Properties & Indexers

        public Matrix3x2 Transposed => new Matrix3x2(Column0, Column1, Column2);

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

        public float M11 { get => Row0.X; set => Row0.X = value; }
        public float M12 { get => Row0.Y; set => Row0.Y = value; }
        public float M13 { get => Row0.Z; set => Row0.Z = value; }

        public float M21 { get => Row1.X; set => Row1.X = value; }
        public float M22 { get => Row1.Y; set => Row1.Y = value; }
        public float M23 { get => Row1.Z; set => Row1.Z = value; }
        #endregion

        #region Matrix2x3 Constructors
        public Matrix2x3(Vector3 row0, Vector3 row1) {
            Row0 = row0;
            Row1 = row1;
        }

        public Matrix2x3(
            float m00, float m01, float m02,
            float m10, float m11, float m12) {
            Row0 = new Vector3(m00, m01, m02);
            Row1 = new Vector3(m10, m11, m12);
        }
        #endregion

        #region Matrix2x3 Methods
        public static void Add(in Matrix2x3 left, in Matrix2x3 right, out Matrix2x3 sum) {
            Vector3.Add(in left.Row0, in right.Row0, out sum.Row0);
            Vector3.Add(in left.Row1, in right.Row1, out sum.Row1);
        }
        public static void Add(ref Matrix2x3 self, in Matrix2x3 other) {
            Vector3.Add(ref self.Row0, in other.Row0);
            Vector3.Add(ref self.Row1, in other.Row1);
        }

        public static void Subtract(in Matrix2x3 left, in Matrix2x3 right, out Matrix2x3 diff) {
            Vector3.Subtract(in left.Row0, in right.Row0, out diff.Row0);
            Vector3.Subtract(in left.Row1, in right.Row1, out diff.Row1);
        }
        public static void Subtract(ref Matrix2x3 self, in Matrix2x3 other) {
            Vector3.Subtract(ref self.Row0, in other.Row0);
            Vector3.Subtract(ref self.Row1, in other.Row1);
        }

        public static void Multiply(in Matrix2x3 left, in Matrix3x4 right, out Matrix2x4 prod) {
            Vector3 rightColumnN = right.Column0;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);

            rightColumnN = right.Column1;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);

            rightColumnN = right.Column2;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);

            rightColumnN = right.Column3;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.W);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.W);
        }
        public static void Multiply(in Matrix2x3 left, in Matrix3 right, out Matrix2x3 prod) {
            Vector3 rightColumnN = right.Column0;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);

            rightColumnN = right.Column1;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);

            rightColumnN = right.Column2;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Z);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Z);
        }
        public static void Multiply(in Matrix2x3 left, in Matrix3x2 right, out Matrix2 prod) {
            Vector3 rightColumnN = right.Column0;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.X);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.X);

            rightColumnN = right.Column1;
            Vector3.Dot(in left.Row0, in rightColumnN, out prod.Row0.Y);
            Vector3.Dot(in left.Row1, in rightColumnN, out prod.Row1.Y);
        }
        public static void Multiply(in Matrix2x3 left, in Vector3 right, out Vector2 prod) {
            Vector3.Dot(in left.Row0, in right, out prod.X);
            Vector3.Dot(in left.Row1, in right, out prod.Y);
        }

        public static void Scale(in Matrix2x3 mat, in float scale, out Matrix2x3 prod) {
            Vector3.Scale(in mat.Row0, in scale, out prod.Row0);
            Vector3.Scale(in mat.Row1, in scale, out prod.Row1);
        }
        public static void Scale(ref Matrix2x3 mat, in float scale) {
            Vector3.Scale(ref mat.Row0, in scale);
            Vector3.Scale(ref mat.Row1, in scale);
        }
        #endregion

        #region Matrix2x3 Operators
        public static Matrix2x3 operator +(Matrix2x3 left, Matrix2x3 right) {
            Add(ref left, in right);
            return left;
        }
        public static Matrix2x3 operator -(Matrix2x3 left, Matrix2x3 right) {
            Subtract(ref left, in right);
            return left;
        }

        public static Matrix2x4 operator *(Matrix2x3 left, Matrix3x4 right) {
            Multiply(in left, in right, out Matrix2x4 prod);
            return prod;
        }
        public static Matrix2x3 operator *(Matrix2x3 left, Matrix3 right) {
            Multiply(in left, in right, out Matrix2x3 prod);
            return prod;
        }
        public static Matrix2 operator *(Matrix2x3 left, Matrix3x2 right) {
            Multiply(in left, in right, out Matrix2 prod);
            return prod;
        }
        public static Vector2 operator *(Matrix2x3 left, Vector3 right) {
            Multiply(in left, in right, out Vector2 prod);
            return prod;
        }
        public static Matrix2x3 operator *(float scale, Matrix2x3 mat) {
            Scale(ref mat, in scale);
            return mat;
        }
        public static Matrix2x3 operator *(Matrix2x3 mat, float scale) {
            Scale(ref mat, in scale);
            return mat;
        }

        public static bool operator ==(Matrix2x3 left, Matrix2x3 right) {
            return Equals(in left, in right);
        }
        public static bool operator !=(Matrix2x3 left, Matrix2x3 right) {
            return !Equals(in left, in right);
        }
        #endregion

        #region Matrix2x3 Overrides
        public override bool Equals(object o) => o is Matrix2x3 m && Equals(in this, in m);
        public bool Equals(Matrix2x3 other) => Equals(in this, in other);
        public static bool Equals(in Matrix2x3 left, in Matrix2x3 right) {
            return Vector3.Equals(in left.Row0, in right.Row0) &&
                   Vector3.Equals(in left.Row1, in right.Row1);
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
