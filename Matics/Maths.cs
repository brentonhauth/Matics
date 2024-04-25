using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Matics {
    public static class Maths {
        public const float PI = 3.1415926535897932384626f;
        public const float HalfPI = PI / 2f;
        public const float E = 2.7182818284590452353603f;        
        public const float LN2 = .6931471805599453f;
        public const float LN10 = 2.302585092994046f;
        private const float PIOver180 = PI / 180f;
        private const float PIUnder180 = 180f / PI;

        [Pure]
        public static float Sin(float a) => (float)Math.Sin(a);
        [Pure]
        public static float Sinh(float a) => (float)Math.Sinh(a);
        [Pure]
        public static float Asin(float a) => (float)Math.Asin(a);
        [Pure]
        public static float Csc(float a) => 1f / (float)Math.Sin(a);
        [Pure]
        public static float Sinc(float a) => a != 0f ? Sin(a) / a : 1f;


        [Pure]
        public static float Cos(float a) => (float)Math.Cos(a);
        [Pure]
        public static float Cosh(float a) => (float)Math.Cosh(a);
        [Pure]
        public static float Acos(float a) => (float)Math.Acos(a);
        [Pure]
        public static float Sec(float a) => 1f / (float)Math.Cos(a);

        [Pure]
        public static float Tan(float a) => (float)Math.Tan(a);
        [Pure]
        public static float Tanh(float a) => (float)Math.Tanh(a);
        [Pure]
        public static float Atan(float a) => (float)Math.Atan(a);
        [Pure]
        public static float Atan2(float y, float x) => (float)Math.Atan2(y, x);
        [Pure]
        public static float Cot(float a) => 1f / (float)Math.Tan(a);


        [Pure]
        public static float Sqrt(float a) => (float)Math.Sqrt(a);
        [Pure]
        public static float Cbrt(float a) => (float)Math.Cbrt(a);
        [Pure]
        public static Complex ISqrt(float a) {
            Complex c = Complex.Zero;
            if (a < 0) {
                c.Im = Sqrt(-a);
            } else {
                c.Re = Sqrt(a);
            }
            return c;
        }
        [Pure]
        public static Complex ISqrt(Complex c) {
            if (c.Im == 0f) {
                return ISqrt(c.Re);
            }
            float m = Sqrt((c.Re * c.Re) + (c.Im * c.Im)),
                  g = Sqrt((c.Re + m) / 2),
                  d = Sqrt((-c.Re + m) / 2);
            if (c.Im < 0) d = -d;
            return new Complex(g, d);
        }

        [Pure]
        public static float Deg2Rad(float d) => d * PIOver180;
        [Pure]
        public static float Rad2Deg(float r) => r * PIUnder180;

        [Pure]
        public static float Log2(float a) => Log(a) / LN2;
        [Pure]
        public static float Log10(float a) => Log(a) / LN10;
        [Pure]
        public static float Log(float a) => (float)Math.Log(a);
        [Pure]
        public static float Log(float @base, float a) => (float)Math.Log(a, @base);

        [Pure]
        public static float Pow(float b, float x) => (float)Math.Pow(b, x);
        [Pure]
        public static float Exp(float x) => (float)Math.Exp(x);

        [Pure]
        public static T Taylor<T>(T start, T identity, uint n, Func<T, T, T> mult, Func<T, T, T> add, Func<T, float, T> div) where T : unmanaged {
            ulong fact = 1;
            T pow = start;
            T sum = identity;
            if (n >= 1) {
                sum = add(sum, pow);
            }
            for (uint i = 2; i <= n; ++i) {
                pow = mult(pow, start);
                fact *= i;
                sum = add(sum, div(pow, (float)fact));
            }
            return sum;
        } 

        [Pure]
        public static float Abs(float a) => a < 0 ? -a : a;
        [Pure]
        public static float Clamp(float n, float min, float max) => Math.Max(min, Math.Min(n, max));
        [Pure]
        public static float Clamp01(float n) => Math.Max(0f, Math.Min(n, 1f));

        public static void QuadraticRoots(float a, float b, float c, out float x0, out float x1) {
            if (a == 0) {
                throw new Exception("Not quadratic");
            }
            c = (b * b) - (4 * a * c);
            if (c < 0) {
                x0 = x1 = float.NaN;
            } else {
                c = Sqrt(c);
                b = -b;
                a *= 2;
                x0 = (b + c) / a;
                x1 = (b - c) / a;
            }
        }

        public static void Qsort(int[] a) {
            const int STACK_LIMIT = 32;
            static void Qsorti(Span<int> s) {
                // Console.WriteLine($"{new string('\x20', l)}[{l}] {string.Join(',', s.ToArray())}");
                if (s.Length <= 1) {
                    return;
                } else if (s.Length == 2) {
                    if (s[0] > s[1]) {
                        int t = s[1];
                        s[1] = s[0];
                        s[0] = t;
                        /*s[0] = s[0] ^ s[1];
                        s[1] = s[0] ^ s[1];
                        s[0] = s[0] ^ s[1];*/
                    }
                    return;
                }
                int p = s[0], li = 0, ri = s.Length;
                Span<int> b = s.Length <= STACK_LIMIT ? stackalloc int[s.Length] : new int[s.Length];
                for (int i = 1; i < s.Length; ++i) {
                    if (s[i] <= p) {
                        b[li++] = s[i];
                    } else {
                        b[--ri] = s[i];
                    }
                }
                Qsorti(b[0..li]);
                Qsorti(b[ri..s.Length]);
                b[li] = p;
                b.CopyTo(s);
            }
            Qsorti(a.AsSpan());
        }


        public static float Mean(float[] a) {
            float sum = 0;
            for (int i = 0; i < a.Length; ++i) {
                sum += a[i];
            }
            return a.Length > 1 ? sum / a.Length : sum;
        }

        public static float Var(float[] a) {
            float sum = 0, mu = Mean(a);
            for (int i = 0; i < a.Length; ++i) {
                float x = a[i] - mu;
                sum += x * x;
            }
            return a.Length > 2 ? sum / a.Length : sum;
        }

        public static float Stddev(float[] a) {
            return Sqrt(Var(a));
        }
    }
}
