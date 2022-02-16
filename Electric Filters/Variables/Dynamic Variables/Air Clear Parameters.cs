using Electric_Filters.Extensions;
using Electric_Filters.Variables;
using System;
using System.Collections.Generic;

namespace Electric_Filters
{
    public class MinActiveArea : IVariable
    {
        private IVariable _airQuantity;
        private IVariable _airSpeed;

        public MinActiveArea(IVariable airQuantity, IVariable airSpeed)
        {
            _airQuantity = airQuantity;
            _airSpeed = airSpeed;
        }

        public decimal Value => _airQuantity.Value / (3600 * _airSpeed.Value);

        public string Symbol => "Fa_мин";

        public string Dimension => "м^2";

        public string Name => "минимальная площадь поперчного сечения";
    }

    public class AirVelocity : IVariable
    {
        private IVariable _airQuantiry;
        private IVariable _activeArea;

        public AirVelocity(IVariable airQuantity, IVariable activeArea)
        {
            _airQuantiry = airQuantity;
            _activeArea = activeArea;
        }

        public decimal Value => _airQuantiry.Value / (3600 * _activeArea.Value);

        public string Symbol => "v_г";

        public string Dimension => "м/с";

        public string Name => "скорость газа";
    }

    public class AirClearDegree : IDescriptable
    {
        private IVariable _geometryKoefficient;
        private IVariable _driftSpeed;

        public AirClearDegree(IVariable geometryKoefficient, IVariable driftSpeed)
        {
            _geometryKoefficient = geometryKoefficient;
            _driftSpeed = driftSpeed;
        }

        public decimal Value => (1m - (-_driftSpeed.Value * _geometryKoefficient.Value / 2).Exp()) * 100m;

        public string Symbol => "η_i";

        public string Dimension => "%";

        public string Name => "степень очистки газа";

        public List<IVariable> Description => new List<IVariable>() { _geometryKoefficient, _driftSpeed };
    }

    public class GeometryKoefficient : IDescriptable
    {
        private IVariable _activeArea;
        private IVariable _sedimentationArea;
        private IVariable _airVelocity;

        public GeometryKoefficient(IVariable activeArea, IVariable sedimentationArea, IVariable airVelocity)
        {
            _activeArea = activeArea;
            _sedimentationArea = sedimentationArea;
            _airVelocity = airVelocity;
        }

        public decimal Value => _sedimentationArea.Value / (_activeArea.Value * _airVelocity.Value);

        public string Symbol => "f";

        public string Dimension => "б/р";

        public string Name => "коэффициент, определяемый геометрией ЭФ";

        public List<IVariable> Description => new List<IVariable>() { _activeArea, _sedimentationArea, _airVelocity };
    }

    public class DriftVelocity : IDescriptable
    {
        private IVariable _electricIntensity;
        private IVariable _airViscosity;
        private IVariable _ionRadius;
        private IVariable _koefficientA;
        private IVariable _molecularDistance;

        public DriftVelocity(IVariable electricIntensity, IVariable airViscosity, IVariable ionRadius, IVariable koefficientA, IVariable molecularDistance)
        {
            _electricIntensity = electricIntensity;
            _airViscosity = airViscosity;
            _ionRadius = ionRadius;
            _koefficientA = koefficientA;
            _molecularDistance = molecularDistance;
        }

        public decimal Value
        {
            get
            {
                if (_ionRadius.Value * 2 > 2m)
                    return GetDriftVelocity();
                else
                    return GetDriftVelocity() * (1m + _koefficientA.Value * _molecularDistance.Value / (_ionRadius.Value * 1E-6m));
            }
        }

        public string Symbol => "v_д " + _ionRadius.Value.ToString();

        public string Dimension => "м/с";

        public string Name => "скорость дрейфа частиц радиуса " + VariableParser.ShortParse(_ionRadius);

        public List<IVariable> Description => new List<IVariable>() { _electricIntensity, _airViscosity, _ionRadius, _koefficientA, _molecularDistance };

        private decimal GetDriftVelocity()
        {
            return 1.18E-12m * Constants.Gravity.Value * _electricIntensity.Value.Pow(2m) * _ionRadius.Value * 1E-6m / _airViscosity.Value;
        }
    }

    public class GeneralAirClearDegree : IVariable
    {
        private List<IVariable> _airClearDegrees;
        private List<IVariable> _fractions;

        public GeneralAirClearDegree(List<IVariable> airClearDegrees, List<IVariable> fractions)
        {
            _airClearDegrees = airClearDegrees;
            _fractions = fractions;
        }

        public decimal Value
        {
            get
            {
                var result = 0m;
                for (int i = 0; i < _airClearDegrees.Count; i++)
                {
                    result += _airClearDegrees[i].Value * _fractions[i].Value;
                }
                return result / 100m;
            }
        }

        public string Symbol => "η";

        public string Dimension => "%";

        public string Name => "общая степень очистки газа";
    }
}