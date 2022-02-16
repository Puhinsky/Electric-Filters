using Electric_Filters.Calculators;
using System;
using System.Text;

namespace Electric_Filters
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            Console.WriteLine("Точность округления:");
            int accuracy = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            ElectricParametersCalculator.Calculate(accuracy);
            AirClearDegreeCalculator.Calculate(accuracy);
        }
    }
}
