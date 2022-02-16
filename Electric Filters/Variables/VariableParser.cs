using System;
using Electric_Filters.Extensions;

namespace Electric_Filters
{
    public class VariableParser
    {
        public static string Parse(IVariable variable)
        {
            return string.Format("{0}:\n{1} = {2} {3}", variable.Name, variable.Symbol, variable.Value, variable.Dimension);
        }

        public static string ShortParse(IVariable variable)
        {
            return string.Format("{0} = {1} {2}", variable.Symbol, variable.Value, variable.Dimension);
        }

        public static string Parse(IVariable variable, int accuracy)
        {
            return string.Format("{0}:\n{1} = {2} {3}", variable.Name, variable.Symbol, GetRoundValue(variable.Value, accuracy), variable.Dimension);
        }

        public static string ShortParse(IVariable variable, int accuracy)
        {
            return string.Format("{0} = {1} {2}", variable.Symbol, GetRoundValue(variable.Value, accuracy), variable.Dimension);
        }

        public static string ShortContextParse(IVariable variable)
        {
            return string.Format("{0}, {1}", variable.Symbol, variable.Dimension);
        }

        public static string LongContextParse(IVariable variable)
        {
            return string.Format("{0} [{1}], {2}", variable.Name, variable.Symbol, variable.Dimension);
        }

        private static decimal GetRoundValue(decimal value, int accuracy)
        {
            return value.RoundToSignificantDigits(accuracy);
        }
    }
}
