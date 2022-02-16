using Electric_Filters.Extensions;
using Electric_Filters.Request;
using System;
using System.Collections.Generic;

namespace Electric_Filters.Calculators
{
    public class ElectricParametersCalculator
    {
        public static void Calculate(int accuracy)
        {
            var currentTemperature = new ConstantVariable(150m, "t", "C", "температура газа");
            var deltaPressure = new ConstantVariable(-2E3m, "p", "Па", "разрежение в системе");
            RequestCreator.ListRequest(new List<IRequestable>() { currentTemperature, deltaPressure }, "через пробел");

            RequestCache.AddVariableToCache(currentTemperature);

            var electrodRadius = new ConstantVariable(0, "r1", "м", "радиус коронирующего электрода");
            var electrodDistance = new ConstantVariable(0, "H", "м", "расстояние между коронирующими и осадительными электродами");
            var electrodStep = new ConstantVariable(0, "d", "м", "расстояние между соседними коронирующими электродами в ряду");
            RequestCreator.ListRequest(new List<IRequestable>() { electrodRadius, electrodDistance, electrodStep }, "через пробел");

            var relativeDensity = new RelativeDensity(Constants.NominalPressure, deltaPressure, Constants.NominalTemperature, currentTemperature);
            var criticalIntensity = new CriticalIntensity(relativeDensity, electrodRadius);
            var criticalVoltage = new CriticalVoltage(criticalIntensity, electrodRadius, electrodDistance, electrodStep);

            var voltageKoefficient = new ConstantVariable(0m, "k_U", "б/р", "коэффициент напряжения");
            RequestCreator.SingleRequest(voltageKoefficient, "диапазон значений: 1,5 - 2,5");

            var electrodVoltage = new ElectrodVoltage(criticalVoltage, voltageKoefficient);
            var electrodRatio = new ElectrodRatio(electrodDistance, electrodStep);
            var electrodKoefficient = new ElectrodKoefficient(electrodRatio);
            var ionMobility = new ConstantVariable(2.1E-4m, "k", "м^2/(В*с)", "подвижность ионов");
            var linearCurrentDensity = new LinearCurrentDensity(ionMobility, electrodKoefficient, electrodRadius, electrodDistance, electrodStep, criticalVoltage, electrodVoltage);

            var electrodIntensity = new ElectrodIntensity(linearCurrentDensity, electrodDistance, electrodStep, Constants.DielectricConstant, ionMobility);
            RequestCache.AddVariableToCache(electrodIntensity);

            Console.WriteLine(relativeDensity.DescriptionParse(accuracy));
            Console.WriteLine();
            Console.WriteLine(criticalIntensity.DescriptionParse(accuracy));
            Console.WriteLine();
            Console.WriteLine(criticalVoltage.DescriptionParse(accuracy));
            Console.WriteLine();
            Console.WriteLine(electrodVoltage.DescriptionParse(accuracy));
            Console.WriteLine();
            Console.WriteLine(electrodKoefficient.DescriptionParse(accuracy));
            Console.WriteLine();
            Console.WriteLine(linearCurrentDensity.DescriptionParse(accuracy));
            Console.WriteLine();
            Console.WriteLine(electrodIntensity.DescriptionParse(accuracy));
            Console.WriteLine();
        }

        private static string Format(string input)
        {
            return input + "\n";
        }
    }
}
