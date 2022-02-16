using Electric_Filters.Extensions;
using Electric_Filters.Filters;
using Electric_Filters.Request;
using Electric_Filters.Variables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Electric_Filters.Calculators
{
    public class AirClearDegreeCalculator
    {
        public static void Calculate(int accuracy)
        {
            var airQuantity = new ConstantVariable(0m, "V", "м^3/ч", "количество газа");
            var maxAirVelocity = new ConstantVariable(0m, "v_г", "м/с", "максимальная скорость газа");
            RequestCreator.ListRequest(new List<IRequestable>() { maxAirVelocity, airQuantity }, "через пробел");

            var minActiveArea = new MinActiveArea(airQuantity, maxAirVelocity);
            var filter = new FilterCatalog().GetOptimalFilter(minActiveArea);

            var activeArea = filter.ActiveSectionArea;
            var sedimentationArea = filter.SedimentationArea;
            var airVelocity = new AirVelocity(airQuantity, activeArea);
            var geometryKoefficient = new GeometryKoefficient(activeArea, sedimentationArea, airVelocity);

            var currentTemperature = RequestCache.FindVariable("температура газа");

            var airContentParts = new List<IRequestable>();
            var currentViscosities = new List<IDescriptable>();
            var molecularMasses = new List<IVariable>();

            var airsNames = new List<string>() { "CO2", "O2", "H2O", "N2" };

            foreach (var airName in airsNames)
            {
                var airContentPart = new ConstantVariable(0m, airName, "%", string.Format("Процент {0} от газовой смеси", airName));
                airContentParts.Add(airContentPart);

                var currentViscosity = new AirViscosity(airName, Constants.NominalViscosities[airName], Constants.SutherlandConstants[airName], currentTemperature);
                currentViscosities.Add(currentViscosity);

                molecularMasses.Add(Constants.MolecularMasses[airName]);
            }
            RequestCreator.ListRequest(airContentParts, "через пробел");

            var relativeMolecularMass = new RelativaMolecularMass(molecularMasses, airContentParts.Cast<IVariable>().ToList());
            var viscosityMassRatio = new ViscosityMassRatio(molecularMasses, airContentParts.Cast<IVariable>().ToList(), currentViscosities.Cast<IVariable>().ToList());
            var mixAirViscosity = new MixAirViscosity(relativeMolecularMass, viscosityMassRatio);

            var dustSizeExample = new ConstantVariable(0m, "r", "мкм", "средний радиус частицы");
            var dustSizes = new List<IRequestable>();
            var dustContentParts = new List<IRequestable>();
            var driftVelocities = new List<IDescriptable>();
            var airClearDegrees = new List<IDescriptable>();

            RequestCreator.UnknownListRequest(dustSizes, dustSizeExample, string.Format("Список значений:\n\t{0}\n\t(через пробел)", VariableParser.LongContextParse(dustSizeExample)));

            var electricIntensity = RequestCache.FindVariable("напряженность электрического поля");

            foreach (var dustSize in dustSizes)
            {
                var dustContentPart = new ConstantVariable(0m, "c", "%", string.Format("содеражние масс частиц радиуса {0}", dustSize.Value));
                dustContentParts.Add(dustContentPart);

                var driftVelocity = new DriftVelocity(electricIntensity, mixAirViscosity, dustSize, Constants.KoefficientA, Constants.MolecularDistance);
                driftVelocities.Add(driftVelocity);

                var airClearDegree = new AirClearDegree(geometryKoefficient, driftVelocity);
                airClearDegrees.Add(airClearDegree);
            }

            var generalAirClearDegree = new GeneralAirClearDegree(airClearDegrees.Cast<IVariable>().ToList(), dustContentParts.Cast<IVariable>().ToList());

            RequestCreator.ListRequest(dustContentParts, "через пробел");

            Console.WriteLine(currentViscosities.ListDescriptionParse(accuracy));
            Console.WriteLine();
            Console.WriteLine(viscosityMassRatio.ExpressionParseWithName(accuracy));
            Console.WriteLine();
            Console.WriteLine(mixAirViscosity.ExpressionParseWithName(accuracy));
            Console.WriteLine();
            Console.WriteLine(driftVelocities.ListDescriptionParse(accuracy));
            Console.WriteLine();
            Console.WriteLine(geometryKoefficient.DescriptionParse(accuracy));
            Console.WriteLine();
            Console.WriteLine(airClearDegrees.ListDescriptionParse(accuracy));
            Console.WriteLine();
            Console.WriteLine(generalAirClearDegree.ExpressionParseWithName(accuracy));
        }
    }
}