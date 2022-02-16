using System.Collections.Generic;

namespace Electric_Filters.Request
{
    public class RequestCache
    {
        private static Dictionary<string, IVariable> _varaibles = new Dictionary<string, IVariable>();

        public static void AddVariableToCache(IVariable variable)
        {
            if (_varaibles.ContainsKey(variable.Name))
                _varaibles[variable.Name] = variable;
            else
                _varaibles.Add(variable.Name, variable);

        }

        public static IVariable FindVariable(string key)
        {
            return _varaibles[key];
        }
    }
}
