using System;

namespace Electric_Filters.Extensions
{
    public static class MathDecimalExtensions
    {
        public static decimal Pow(this decimal value, decimal power)
        {
            return (decimal)Math.Pow((double)value, (double)power);
        }

        public static decimal Sqrt(this decimal value)
        {
            return (decimal)Math.Sqrt((double)value);
        }

        public static decimal Log(this decimal value)
        {
            return (decimal)Math.Log((double)value);
        }

        public static decimal Log10(this decimal value)
        {
            return (decimal)Math.Log10((double)value);
        }

        public static decimal Exp(this decimal value)
        {
            return (decimal)Math.Exp((double)value);
        }

        public static decimal RoundToSignificantDigits(this decimal value, int digits)
        {
            if (value == 0)
                return 0;

            decimal scale = 10m.Pow(Math.Floor(Math.Abs(value).Log10()) + 1 - digits);
            return scale * Math.Truncate(value / scale);
        }
    }
}
