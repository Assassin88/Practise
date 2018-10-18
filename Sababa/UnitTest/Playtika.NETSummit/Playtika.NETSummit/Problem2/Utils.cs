using System;

namespace Playtika.NETSummit.Problem2
{
    public static class Utils
    {
        public static int Foo1(int n)
        {
            var result = 1;
            for (var i = 1; i <= n; ++i)
            {
                result *= i;
            }

            return result;
        }

        public static bool Foo2(int a, int b, int c)
        {
            var result = true;

            result &= a < b + c;
            result &= b < a + c;
            result &= c < a + b;

            return result;
        }

        public static bool Foo3(int n)
        {
            var flag = true;
            var root = (int) Math.Sqrt(n);
            for (var i = 2; i < root + 1; ++i)
            {
                if (n % i == 0)
                {
                    flag = false;
                }
            }

            return flag;
        }

        public static int Foo4(int n)
        {
            if (n == 0)
            {
                return 1;
            }
            else
            {
                var number = (int)Math.Floor(Math.Log10(Math.Abs(n)) + 1);
                return number;
            }
        }
    }
}