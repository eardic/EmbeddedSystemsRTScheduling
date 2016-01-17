using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESScheduling.Jobs;

namespace ESScheduling.Schedulers
{
    static class MathHelpers
    {
        // Sum(Ci/Pi) i = 0 to Jobs.Count
        public static double U(IEnumerable<Job> Jobs)
        {
            return Jobs.Sum(j => j.Period != 0 ? ((double)j.ComputationTime) / j.Period : double.PositiveInfinity);
        }

        // n * (2^1/n - 1)
        public static double M(int n)
        {
            return n != 0 ? n * (Math.Pow(2, 1.0 / n) - 1) : double.PositiveInfinity;
        }

        public static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static int LCD(int a, int b)
        {
            return a * b / GCD(a, b);
        }

    }
}
