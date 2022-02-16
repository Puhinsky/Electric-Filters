using System;
using System.Collections.Generic;

namespace Electric_Filters
{
    public class Constants
    {
        public static IVariable AbsoluteTemperature = new ConstantVariable(273m, "T", "C", "Абсолютная температура");

        public static IVariable DielectricConstant = new ConstantVariable(8.85E-12m, "e_0", "Ф/м", "Диэлектрическая прницаемость вакуума");

        public static IVariable NominalPressure = new ConstantVariable(1.013E5m, "p_бар", "Па", "Барометрическое давление");

        public static IVariable NominalTemperature = new ConstantVariable(20m, "t_норм", "C", "Нормальная температура");

        public static IVariable PI = new ConstantVariable((decimal)Math.PI, "pi", "б/р", "Число пи");

        public static IVariable Gravity = new ConstantVariable(9.81m, "g", "м/с^2", "Ускорение свободного падения");

        public static IVariable KoefficientA = new ConstantVariable(1m, "A", "б/р", "Коэффициент");

        public static IVariable MolecularDistance = new ConstantVariable(1E-7m, "λ", "м", "средняя длина пробега молекул");

        public static Dictionary<string, IVariable> NominalViscosities = new Dictionary<string, IVariable>
        {
            {"N2", new ConstantVariable(1776E-8m, "μ_N2_0", "Па*с", "динамическая вязкость N2 при нормальных условиях") },
            {"CO2", new ConstantVariable(1463E-8m, "μ_C02_0", "Па*с", "динамическая вязкость CO2 при нормальных условиях") },
            {"H2O", new ConstantVariable(970E-8m, "μ_H2O_0", "Па*с", "динамическая вязкость H2O при нормальных условиях") },
            {"O2", new ConstantVariable(2026E-8m, "μ_O2_0", "Па*с", "динамическая вязкость O2 при нормальных условиях") },
        };

        public static Dictionary<string, IVariable> SutherlandConstants = new Dictionary<string, IVariable>()
        {
            {"N2", new ConstantVariable(114m, "C_N2", "б/р", "Постоянная Сазерленда") },
            {"CO2", new ConstantVariable(254m, "C_C02", "б/р", "Постоянная Сазерленда") },
            {"H2O", new ConstantVariable(961m, "C_H2O", "б/р", "Постоянная Сазерленда") },
            {"O2", new ConstantVariable(131m, "C_O2", "б/р", "Постоянная Сазерленда") },
        };

        public static Dictionary<string, IVariable> MolecularMasses = new Dictionary<string, IVariable>
        {
            { "N2", new ConstantVariable(28m, "M_N2", "кг/кмоль", "Молярная масса газа")},
            { "CO2", new ConstantVariable(44m, "M_N2", "кг/кмоль", "Молярная масса газа")},
            { "H2O", new ConstantVariable(18m, "M_N2", "кг/кмоль", "Молярная масса газа")},
            { "O2", new ConstantVariable(32m, "M_N2", "кг/кмоль", "Молярная масса газа")}
        };
    }
}
