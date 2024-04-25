using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Matics {
    [StructLayout(LayoutKind.Sequential, Size = 8)]
    public struct Complex : IEquatable<Complex> {
        #region Static Readonly Complex Variables
        public static readonly Complex I = new Complex(1);
        public static readonly Complex R = new Complex(1, 0);
        public static readonly Complex Zero = new Complex(0, 0);
        public static readonly Complex One = new Complex(1, 1);
        private static readonly Complex LN_I = new Complex(Maths.HalfPI);
        private const float InvHalfPI = 1 / Maths.HalfPI;
        #endregion

        public float Re, Im;

        #region Complex Properties & Indexers
        public float this[int index] {
            get {
                return index switch {
                    0 => Re,
                    1 => Im,
                    _ => throw new IndexOutOfRangeException(),
                };
            }
            set {
                switch (index) {
                    case 0: Re = value; break;
                    case 1: Im = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }
        
        public float Magnitude => MathF.Sqrt(MagnitudeSquared);
        public float MagnitudeSquared => (Re * Re) + (Im * Im);
        
        public float Angle => MathF.Atan2(Im, Re);

        public unsafe float* Raw {
            get {
                fixed (float* p = &Re) {
                    return p;
                }
            }
        }
        #endregion

        #region Complex Constructors
        public Complex(float re, float im) {
            Re = re;
            Im = im;
        }
        public Complex(float im) {
            Re = 0;
            Im = im;
        }
        public Complex(Vector2 ri) {
            Re = ri.X; Im = ri.Y;
        }
        public unsafe Complex(float* start) {
            Re = *start++;
            Im = *start;
        }
        #endregion

        #region Complex Methods
        [Pure]
        public static Complex Sqrt(float n) {
            return (n < 0)
                ? new Complex(0, MathF.Sqrt(-n))
                : new Complex(MathF.Sqrt(n), 0);
        }
        [Pure]
        public static Complex Sqrt(Complex n) {
            if (n.Im == 0f) {
                return Sqrt(n.Re);
            }
            float m = n.Magnitude;
            return new Complex(
                re: MathF.Sqrt((n.Re + m) * 0.5f),
                im: MathF.Sqrt((-n.Re + m) * 0.5f));
        }

        [Pure]
        public static Complex Exp(Complex n) {
            float x = MathF.Exp(n.Re);
            return new Complex(x * MathF.Cos(n.Im), x * MathF.Sin(n.Im));
        }
        [Pure]
        public static Complex Pow(float n, Complex e) {
            float b = e.Im * MathF.Log(n);
            float a = MathF.Pow(n, e.Re);
            return new Complex(a * MathF.Cos(b), a * MathF.Sin(b));
        }
        [Pure]
        public static Complex Pow(Complex n, Complex e) {
            Complex a = Log(n);
            Scale(in a, in e.Re, out Complex b);
            b = Exp(b);
            a = new Complex(-a.Im * e.Im, a.Re * e.Im);
            a = Exp(a);
            Multiply(ref a, in b);
            return a;
        }
        [Pure]
        public static Complex Pow(Complex n, int e) {
            if (e <= 1) {
                if (e < 0) {
                    Complex partial = Pow(n, -e);
                    Divide(1, in partial, out Complex result);
                    return result;
                }
                return (e == 0) ? R : n;
            } else {
                Complex partial = Pow(n, e >> 1);
                Multiply(in partial, in partial, out Complex result);
                if ((e & 1) == 1) {
                    Multiply(ref result, in n);
                }
                return result;
            }
        }
        [Pure]
        public static Complex Pow(Complex n, float e) {
            Complex r = Log(n);
            Scale(ref r, in e);
            return Exp(r);
        }


        [Pure]
        public static Complex Log(Complex a) {
            return new Complex(
                re: MathF.Log(a.MagnitudeSquared) * 0.5f,
                im: a.Angle);
        }
        [Pure]
        public static Complex Log(float a) {
            return (a < 0)
                ? new Complex(MathF.Log(-a), Maths.PI)
                : new Complex(MathF.Log(a), 0);
        }
        [Pure]
        public static Complex Log(Complex @base, Complex a) {
            @base = Log(@base); a = Log(a);
            Divide(in a, in @base, out Complex x);
            return x;
        }
        [Pure]
        public static Complex Log(float @base, Complex a) {
            a = Log(a);
            if (@base < 0) {
                Complex b = new Complex(MathF.Log(-@base), Maths.PI);
                Divide(in a, in b, out Complex x);
                return x;
            } else {
                @base = MathF.Log(@base);
                Divide(ref a, in @base);
                return a;
            }
        }
        [Pure]
        public static Complex Log(Complex @base, float a) {
            a = MathF.Log(a);
            @base = Log(@base);
            Divide(in a, in @base, out Complex x);
            return x;
        }
        [Pure]
        public static Complex Log(float @base, float a) {
            Complex b = Log(@base);
            Complex c = Log(a);
            Divide(in c, in b, out Complex x);
            return x;
        }
        [Pure]
        public static Complex LogI(Complex a) {
            a = Log(a);
            float re = a.Re;
            a.Re = a.Im * InvHalfPI;
            a.Im = -re * InvHalfPI;
            return a;
        }
        [Pure]
        public static Complex LogI(float a) {
            if (a < 0) {
                // TODO: Clean up
                Complex c = Log((Complex)a);
                Divide(in c, in LN_I, out Complex x);
                return x;
            }
            return new Complex(-MathF.Log(a) * InvHalfPI);
        }
        [Pure]
        public static Complex Log10(Complex a) {
            a = Log(a);
            Divide(ref a, Maths.LN10);
            return a;
        }
        [Pure]
        public static Complex Log10(float a) {
            const float PI_LN10 = Maths.PI / Maths.LN10;
            return a < 0
                ? new Complex(MathF.Log(-a) / Maths.LN10, PI_LN10)
                : new Complex(MathF.Log(a) / Maths.LN10, 0);
        }
        [Pure]
        public static Complex Log2(Complex a) {
            a = Log(a);
            Divide(ref a, Maths.LN2);
            return a;
        }
        [Pure]
        public static Complex Log2(float a) {
            const float PI_LN2 = Maths.PI / Maths.LN2;
            return a < 0
                ? new Complex(MathF.Log(-a) / Maths.LN2, PI_LN2)
                : new Complex(MathF.Log(a) / Maths.LN2, 0);
        }

        [Pure]
        public static Complex Sin(Complex c) {
            return new Complex(
                re: MathF.Sin(c.Re) * MathF.Cosh(c.Im),
                im: MathF.Cos(c.Re) * MathF.Sinh(c.Im));
        }
        [Pure]
        public static Complex Cos(Complex c) {
            return new Complex(
                re: MathF.Cos(c.Re) * MathF.Cosh(c.Im),
                im: -MathF.Sin(c.Re) * MathF.Sinh(c.Im));
        }
        [Pure]
        public static Complex Tan(Complex c) {
            float d = MathF.Cos(2 * c.Re) + MathF.Cosh(2 * c.Im);
            return new Complex(
                re: MathF.Sin(2 * c.Re) / d,
                im: MathF.Sinh(2 * c.Im) / d);
        }

        public static void Add(in Complex left, in Complex right, out Complex sum) {
            sum.Re = left.Re + right.Re;
            sum.Im = left.Im + right.Im;
        }
        public static void Add(ref Complex complex, in Complex other) {
            complex.Re += other.Re;
            complex.Im += other.Im;
        }
        public static void Add(in Complex left, in float right, out Complex sum) {
            sum.Re = left.Re + right;
            sum.Im = left.Im;
        }
        public static void Add(ref Complex complex, in float other) {
            complex.Re += other;
        }

        public static void Subtract(in Complex left, in Complex right, out Complex diff) {
            diff.Re = left.Re - right.Re;
            diff.Im = left.Im - right.Im;
        }
        public static void Subtract(ref Complex complex, in Complex other) {
            complex.Re -= other.Re;
            complex.Im -= other.Im;
        }
        public static void Subtract(ref Complex complex, in float other) {
            complex.Re -= other;
        }

        public static void Multiply(in Complex left, in Complex right, out Complex prod) {
            prod.Re = (left.Re * right.Re) - (left.Im * right.Im);
            prod.Im = (left.Re * right.Im) + (left.Im * right.Re);
        }
        public static void Multiply(ref Complex complex, in Complex other) {
            float re = complex.Re;
            complex.Re = (re * other.Re) - (complex.Im * other.Im);
            complex.Im = (re * other.Im) + (complex.Im * other.Re);
        }
        public static void Scale(in Complex complex, in float scale, out Complex prod) {
            prod.Re = complex.Re * scale;
            prod.Im = complex.Im * scale;
        }
        public static void Scale(ref Complex complex, in float scale) {
            complex.Re *= scale;
            complex.Im *= scale;
        }

        public static void Divide(in Complex left, in Complex right, out Complex quot) {
            float m = right.MagnitudeSquared;
            quot.Re = ((left.Re * right.Re) + (left.Im * right.Im)) / m;
            quot.Im = ((left.Im * right.Re) - (left.Re * right.Im)) / m;
        }
        public static void Divide(in Complex left, in float right, out Complex quot) {
            quot.Re = left.Re / right;
            quot.Im = left.Im / right;
        }
        public static void Divide(in float left, in Complex right, out Complex quot) {
            if (right.Im != 0f) {
                float m = right.MagnitudeSquared;
                quot.Re = left * right.Re / m;
                quot.Im = -(left * right.Im) / m;
            } else {
                quot.Re = left / right.Re;
                quot.Im = 0f;
            }
        }
        public static void Divide(ref Complex complex, in float by) {
            complex.Re /= by;
            complex.Im /= by;
        }

        public static void Increment(ref Complex complex) {
            ++complex.Re; ++complex.Im;
        }
        public static void Decrement(ref Complex complex) {
            --complex.Re; --complex.Im;
        }

        public static void Negate(ref Complex complex) {
            complex.Re = -complex.Re;
            complex.Im = -complex.Im;
        }
        #endregion

        #region Complex Operators
        public static Complex operator +(Complex left, Complex right) {
            Add(ref left, in right);
            return left;
        }
        public static Complex operator +(Complex left, float right) {
            Add(ref left, in right);
            return left;
        }
        public static Complex operator +(float left, Complex right) {
            Add(ref right, in left);
            return right;
        }
        public static Complex operator ++(Complex complex) {
            Increment(ref complex);
            return complex;
        }

        public static Complex operator -(Complex left, Complex right) {
            Subtract(ref left, in right);
            return left;
        }
        public static Complex operator -(Complex left, float right) {
            Subtract(ref left, in right);
            return left;
        }
        public static Complex operator -(float left, Complex right) {
            right.Re = left - right.Re;
            right.Im = -right.Im;
            return right;
        }
        public static Complex operator -(Complex complex) {
            Negate(ref complex);
            return complex;
        }
        public static Complex operator --(Complex complex) {
            Decrement(ref complex);
            return complex;
        }

        public static Complex operator *(Complex left, Complex right) {
            Multiply(ref left, in right);
            return left;
        }
        public static Complex operator *(Complex left, float right) {
            Scale(ref left, in right);
            return left;
        }
        public static Complex operator *(float left, Complex right) {
            Scale(ref right, in left);
            return right;
        }

        public static Complex operator /(Complex left, Complex right) {
            Divide(in left, in right, out Complex quot);
            return quot;
        }
        public static Complex operator /(Complex left, float right) {
            Divide(ref left, in right);
            return left;
        }
        public static Complex operator /(float left, Complex right) {
            Divide(in left, in right, out Complex quot);
            return quot;
        }

        public static bool operator ==(Complex left, Complex right) {
            return left.Im == right.Im && left.Re == right.Re;
        }
        public static bool operator !=(Complex left, Complex right) {
            return left.Im != right.Im || left.Re != right.Re;
        }


        public static explicit operator double(Complex c) => c.Re;
        public static explicit operator Complex(double re) => new Complex((float)re, 0);

        public static explicit operator float(Complex c) => c.Re;
        public static implicit operator Complex(float re) => new Complex(re, 0);

        public static explicit operator long(Complex c) => (long)c.Re;
        public static implicit operator Complex(long re) => new Complex(re, 0);

        public static explicit operator int(Complex c) => (int)c.Re;
        public static implicit operator Complex(int re) => new Complex(re, 0);
        
        public static implicit operator System.Numerics.Complex(Complex c) {
            return new System.Numerics.Complex(c.Re, c.Im);
        }
        public static implicit operator Complex(System.Numerics.Complex c) {
            return new Complex((float)c.Real, (float)c.Imaginary);
        }

        public static explicit operator Matrix2(Complex c) {
            return new Matrix2(
                c.Re, -c.Im,
                c.Im, c.Re);
        }
        
        public static explicit operator Vector2(Complex c) => new Vector2(c.Re, c.Im);
        public static explicit operator Complex(Vector2 v) => new Complex(v.X, v.Y);
        #endregion

        #region Complex Overrides
        [Pure]
        public override bool Equals(object o) {
            return o is Complex c && Equals(c);
        }
        [Pure]
        public bool Equals(Complex other) {
            return Im == other.Im && Re == other.Re;
        }
        public static bool Equals(in Complex left, in Complex right) {
            return left.Re == right.Re && left.Im == right.Im;
        }
        public override int GetHashCode() {
            return HashCode.Combine(Re, Im);
        }
        public override string ToString() {
            return $"{Re}{Im:+#0.########;-#0.########;+0}i";
        }
        #endregion
    }
}
