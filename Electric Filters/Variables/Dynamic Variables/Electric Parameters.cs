using Electric_Filters.Extensions;
using Electric_Filters.Variables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Electric_Filters
{
    public class RelativeDensity : IDescriptable
    {
        private IVariable _nominalPressure;
        private IVariable _deltaPressure;
        private IVariable _nominalTemperature;
        private IVariable _currentTemperature;

        public RelativeDensity(IVariable nominalPressure, IVariable deltaPressure, IVariable nominalTemperature, IVariable currentTemperature)
        {
            _nominalPressure = nominalPressure;
            _deltaPressure = deltaPressure;
            _nominalTemperature = nominalTemperature;
            _currentTemperature = currentTemperature;
        }

        public decimal Value => (_nominalPressure.Value + _deltaPressure.Value) / 1.013E5m * (Constants.AbsoluteTemperature.Value + _nominalTemperature.Value) / (Constants.AbsoluteTemperature.Value + _currentTemperature.Value);

        public string Symbol => "b";

        public string Dimension => "б/р";

        public string Name => "Относительная плотность газового потока";

        public List<IVariable> Description =>new List<IVariable>() { _nominalPressure, _deltaPressure, _nominalTemperature, _currentTemperature };
    }

    public class CriticalIntensity : IDescriptable
    {
        private IVariable _relativeDensity;
        private IVariable _electrodRadius;

        public CriticalIntensity(IVariable relativeDensity, IVariable electrodRadius)
        {
            _relativeDensity = relativeDensity;
            _electrodRadius = electrodRadius;
        }

        public decimal Value => 3.04m * (_relativeDensity.Value + 0.0311m * (_relativeDensity.Value / _electrodRadius.Value).Sqrt()) * 1E6m;

        public string Symbol => "E_0";

        public string Dimension => "В/м";

        public string Name => "Критическая напряженность";

        public List<IVariable> Description => new List<IVariable>() { _relativeDensity, _electrodRadius };
    }

    public class CriticalVoltage : IDescriptable
    {
        private IVariable _criticalIntensity;
        private IVariable _electrodRadius;
        private IVariable _electrodDistance;
        private IVariable _electrodStep;

        public CriticalVoltage(IVariable criticalIntensity, IVariable electrodRadius, IVariable electrodDistance, IVariable electrodStep)
        {
            _criticalIntensity = criticalIntensity;
            _electrodRadius = electrodRadius;
            _electrodDistance = electrodDistance;
            _electrodStep = electrodStep;
        }

        public decimal Value => _criticalIntensity.Value * _electrodRadius.Value * (Constants.PI.Value * _electrodDistance.Value / _electrodStep.Value - (2m * Constants.PI.Value * _electrodRadius.Value / _electrodStep.Value).Log());

        public string Symbol => "U_0";

        public string Dimension => "В";

        public string Name => "Критическое напряжение";

        public List<IVariable> Description => new List<IVariable>() { _criticalIntensity, _electrodRadius, _electrodDistance, _electrodStep };
    }

    public class ElectrodVoltage : IDescriptable
    {
        private IVariable _criticalVoltage;
        private IVariable _voltageKoefficient;

        public ElectrodVoltage(IVariable criticalVoltage, IVariable voltageKoefficient)
        {
            _criticalVoltage = criticalVoltage;
            _voltageKoefficient = voltageKoefficient;
        }

        public decimal Value => _criticalVoltage.Value * _voltageKoefficient.Value;

        public string Symbol => "U";

        public string Dimension => "В";

        public string Name => "Напряжение на электродах";

        public List<IVariable> Description => new List<IVariable>() { _criticalVoltage, _voltageKoefficient };
    }

    public class ElectrodRatio : IVariable
    {
        private IVariable _electrodDistance;
        private IVariable _electrodStep;

        public ElectrodRatio(IVariable electrodDistance, IVariable electrodStep)
        {
            _electrodDistance = electrodDistance;
            _electrodStep = electrodStep;
        }

        public decimal Value => _electrodDistance.Value / _electrodStep.Value;

        public string Symbol => "H/d";

        public string Dimension => "б/р";

        public string Name => "Соотношение";
    }

    public class ElectrodKoefficient : IDescriptable
    {
        private IVariable _ratio;

        private List<Point> _function;

        public ElectrodKoefficient(IVariable ratio)
        {
            _ratio = ratio;
            _function = new List<Point>
            {
                new Point(0.6m, 0.08m),
                new Point(0.7m, 0.068m),
                new Point(0.8m, 0.046m),
                new Point(0.9m, 0.035m),
                new Point(1m, 0.027m),
                new Point(1.1m, 0.022m),
                new Point(1.2m, 0.0175m),
                new Point(1.3m, 0.015m),
                new Point(1.4m, 0.013m),
                new Point(1.5m, 0.0115m)
            };
        }

        public decimal Value 
        {
            get
            {
                var value = Math.Clamp(_ratio.Value, _function.Min(x => x.X), _function.Max(x => x.X));
                var points = FindClostestPoints(_function.GetRange(0, _function.Count), value, 3);
                return GetQuadraticInterpolation(value, points[0], points[1], points[2]);
            }
        }

        public string Symbol => "σ";

        public string Dimension => "б/р";

        public string Name => "коэффициент, зависящий от расположения электродов";

        public List<IVariable> Description => new List<IVariable>() { _ratio };

        public struct Point
        {
            public decimal X;
            public decimal Y;

            public Point(decimal x, decimal y)
            {
                X = x;
                Y = y;
            }
        }

        private List<Point> FindClostestPoints(List<Point> source, decimal value, int count)
        {
            if (_function.Count < count)
                throw new ArgumentException("Недостаточно точек интерполяции");

            var closestPoints = new List<Point>();

            for (int i = 0; i < 3; i++)
            {
                var closest = source.OrderBy(x => Math.Abs(x.X - value)).First();
                closestPoints.Add(closest);
                source.Remove(closest);
            }

            return closestPoints;
        }

        private decimal GetQuadraticInterpolation(decimal x, Point point1, Point point2, Point point3)
        {
            return point1.Y * (x - point2.X) * (x - point3.X) / ((point1.X - point2.X) * (point1.X - point3.X))
                + point2.Y * (x - point1.X) * (x - point3.X) / ((point2.X - point1.X) * (point2.X - point3.X))
                + point3.Y * (x - point1.X) * (x - point2.X) / ((point3.X - point1.X) * (point3.X - point2.X));
        }
    }

    public class LinearCurrentDensity : IDescriptable
    {
        private IVariable _ionMobility;
        private IVariable _electrodKoefficient;
        private IVariable _electrodRadius;
        private IVariable _electrodDistance;
        private IVariable _electrodStep;
        private IVariable _criticalVoltage;
        private IVariable _electrodVoltage;

        public LinearCurrentDensity(IVariable ionMobility, IVariable electrodKoefficient, IVariable electrodRadius, IVariable electrodDistance, IVariable electrodStep, IVariable criticalVoltage, IVariable electrodVoltage)
        {
            _ionMobility = ionMobility;
            _electrodKoefficient = electrodKoefficient;
            _electrodRadius = electrodRadius;
            _electrodDistance = electrodDistance;
            _electrodStep = electrodStep;
            _criticalVoltage = criticalVoltage;
            _electrodVoltage = electrodVoltage;
        }

        public decimal Value => 4m * Constants.PI.Value.Pow(2) * _ionMobility.Value * _electrodKoefficient.Value / (9E9m * _electrodStep.Value.Pow(2) * (Constants.PI.Value * _electrodDistance.Value / _electrodStep.Value - (2m *Constants.PI.Value * _electrodRadius.Value / _electrodStep.Value).Log())) * _electrodVoltage.Value * (_electrodVoltage.Value - _criticalVoltage.Value);

        public string Symbol => "i_0";

        public string Dimension => "А/м";

        public string Name => "линейная плотность тока";

        public List<IVariable> Description => new List<IVariable>() { _ionMobility, _electrodKoefficient, _electrodRadius, _electrodDistance, _electrodStep, _criticalVoltage, _electrodVoltage };
    }

    public class ElectrodIntensity : IDescriptable
    {
        private IVariable _linearCurrentDensity;
        private IVariable _electrodDistance;
        private IVariable _electrodStep;
        private IVariable _dielectricConstant;
        private IVariable _ionMobility;

        public ElectrodIntensity(IVariable linearCurrentDensity, IVariable electrodDistance, IVariable electrodStep, IVariable dielectricConstant, IVariable ionMobility)
        {
            _linearCurrentDensity = linearCurrentDensity;
            _electrodDistance = electrodDistance;
            _electrodStep = electrodStep;
            _dielectricConstant = dielectricConstant;
            _ionMobility = ionMobility;
        }

        public decimal Value => (8m * _linearCurrentDensity.Value * _electrodDistance.Value / (4m * Constants.PI.Value * _dielectricConstant.Value * _ionMobility.Value * _electrodStep.Value)).Sqrt();
        public string Symbol => "E";
        public string Dimension => "В/м";
        public string Name => "напряженность электрического поля";

        public List<IVariable> Description => new List<IVariable>() { _linearCurrentDensity, _electrodDistance, _electrodStep, _electrodStep, _dielectricConstant, _ionMobility };
    }
}
