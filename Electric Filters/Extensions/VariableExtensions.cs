using Electric_Filters.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electric_Filters.Extensions
{
    public static class VariableExtensions
    {

        public static IEnumerable<T> Cast<T>(this List<IVariable> variables) where T : IVariable
        {
            return variables as IEnumerable<T>;
        }

        public static string ExpressionParse(this IVariable variable, int accuracy)
        {
            return string.Format("\t{0} = {1} {2}", variable.Symbol, variable.Value.RoundToSignificantDigits(accuracy), variable.Dimension);
        }

        public static string ExpressionParseWithName(this IVariable variable, int accuracy)
        {
            return string.Format("{0}:\n\t{1} = {2} {3}", variable.Name, variable.Symbol, variable.Value.RoundToSignificantDigits(accuracy), variable.Dimension);
        }

        public static string ListExpressionParse(this List<IVariable> variables, int accuracy)
        {
            var result = "";

            foreach (var variable in variables)
            {
                result += string.Format("{0}\n", variable.ExpressionParseWithName(accuracy));
            }

            return result;
        }

        public static string ListDescriptionParse(this List<IDescriptable> descriptables, int accuracy)
        {
            var result = "";

            foreach (var descriptable in descriptables)
            {
                result += string.Format("{0}\n",descriptable.DescriptionParse(accuracy));
            }

            return result;
        }

        public static string DescriptionParse(this IDescriptable descriptable, int accuracy)
        {
            var result = string.Format("{0}, где:\n", descriptable.ExpressionParseWithName(accuracy));
            foreach (var variable in descriptable.Description)
            {
                result += string.Format("\t{0}\n", variable.ExpressionParse(accuracy));
            }
            return result;
        }
    }
}
