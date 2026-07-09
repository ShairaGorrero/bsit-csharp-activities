using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sci_cal
{
    public static class CalculatorFunctions
    {
        public static double Square(double value)
        {
            return Math.Pow(value, 2);
        }

        public static double Sqrt(double value)
        {
            if (value < 0)
                throw new Exception("Cannot calculate the square root of a negative number.");

            return Math.Sqrt(value);
        }

        public static double Sin(double value)
        {
            double result = Math.Sin(value * Math.PI / 180);

            if (Math.Abs(result) < 1E-10)
                result = 0;

            return result;
        }

        public static double Cos(double value)
        {
            double result = Math.Cos(value * Math.PI / 180);

            if (Math.Abs(result) < 1E-10)
                result = 0;

            return result;
        }

        public static double Tan(double value)
        {
            double result = Math.Tan(value * Math.PI / 180);

            if (Math.Abs(result) < 1E-10)
                result = 0;

            return result;
        }

        public static double Power(double value, double exponent)
        {
            return Math.Pow(value, exponent);
        }

        public static double Log(double value)
        {
            if (value <= 0)
                throw new Exception("Log is only defined for positive numbers.");

            return Math.Log10(value);
        }

        public static double Ln(double value)
        {
            if (value <= 0)
                throw new Exception("Natural log is only defined for positive numbers.");

            return Math.Log(value);
        }
    }
}
