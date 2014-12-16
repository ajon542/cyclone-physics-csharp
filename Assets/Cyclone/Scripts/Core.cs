using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclone
{
    public class Core
    {
        public static readonly double SleepEpsilon = 0.3;

        public static readonly double Epsilon = 0.001;

        public static bool Equals(double lhs, double rhs)
        {
            if (System.Math.Abs(lhs - rhs) < Epsilon)
            {
                return true;
            }
            return false;
        }
    }
}
