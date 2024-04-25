using System;
using System.Collections;
using System.Collections.Generic;

namespace Matics {
    public static class Random {
        public enum Coin : byte {
            None = 0,
            Heads = 1,
            Tails = 2,
        }

        private static readonly System.Random _Random = new System.Random(DateTime.Now.Millisecond);

        public static int Int() => _Random.Next();
        public static int Int(int max) => _Random.Next(max);
        public static int Int(int min, int max) => _Random.Next(min, max);

        public static float Float() => (float)_Random.NextDouble();
        public static float Float(float max) => Float() * max;
        public static float Float(float min, float max) => (Float() * (max - min)) + min;

        public static bool Bool() => Int(2) == 0;
        public static bool Bool(float bias) => Float() <= bias;

        public static int DieRoll() => Int(1, 7);
        public static Coin CoinFlip() => (Coin)Int(1, 3);

        public static IList<T> Shuffle<T>(IList<T> a, int passes = 1) {
            int n = a.Count;
            passes = Math.Max(1, passes);
            for (int p = 0; p < passes; ++p) {
                for (int i = 0; i < n; ++i) {
                    int r = Int(n);
                    T t = a[i];
                    a[i] = a[r];
                    a[r] = t;
                }
            }
            return a;
        }
    }
}
