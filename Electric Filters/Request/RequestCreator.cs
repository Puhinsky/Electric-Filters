using System;
using System.Collections.Generic;

namespace Electric_Filters.Request
{
    public class RequestCreator
    {
        public static void ListRequest(List<IRequestable> inputRequests, string context)
        {
            string requestTitle = "";
            foreach (var request in inputRequests)
            {
                requestTitle += string.Format("\t{0}\n", VariableParser.LongContextParse(request));
            }
            Console.WriteLine(string.Format("Значения:\n{0}\t({1})", requestTitle, context));

            string[] requestResults = Console.ReadLine().Split(" ");

            for (int i = 0; i < inputRequests.Count; i++)
            {
                inputRequests[i].SetInput(Convert.ToDecimal(requestResults[i]));
            }
            Console.WriteLine();
        }

        public static void SingleRequest(IRequestable inputRequest, string context)
        {
            Console.WriteLine(string.Format("{0} ({1})", VariableParser.ShortContextParse(inputRequest), context));
            inputRequest.SetInput(Convert.ToDecimal(Console.ReadLine()));
            Console.WriteLine();
        }

        public static void UnknownListRequest(List<IRequestable> inputRequest, IVariable example, string context)
        {
            Console.WriteLine(context);
            string[] requestResults = Console.ReadLine().Split(" ");

            for (int i = 0; i < requestResults.Length; i++)
            {
                var variable = new ConstantVariable(Convert.ToDecimal(requestResults[i]), example.Symbol, example.Dimension, example.Name);
                inputRequest.Add(variable);
            }
            Console.WriteLine();
        }
    }
}
