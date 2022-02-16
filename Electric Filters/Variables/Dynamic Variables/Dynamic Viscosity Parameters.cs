using Electric_Filters.Extensions;
using Electric_Filters.Variables;
using System.Collections.Generic;

namespace Electric_Filters
{
    public class AirViscosity : IDescriptable
    {
        private string _symbolIndex;

        private IVariable _nominalViscosity;
        private IVariable _sutherlandConstant;
        private IVariable _currentTemperature;

        public AirViscosity(string symbolIndex, IVariable nominalViscosity, IVariable sutherlandConstant, IVariable currentTemperature)
        {
            _symbolIndex = symbolIndex;

            _nominalViscosity = nominalViscosity;
            _sutherlandConstant = sutherlandConstant;
            _currentTemperature = currentTemperature;
        }

        public decimal Value => _nominalViscosity.Value * (Constants.AbsoluteTemperature.Value + _sutherlandConstant.Value) / (GetAbsoluteTemperature() + _sutherlandConstant.Value) * (GetAbsoluteTemperature() / Constants.AbsoluteTemperature.Value).Pow(1.5m);

        public string Symbol => "µ_" + _symbolIndex;

        public string Dimension => "Н*с/м^2";

        public string Name => "динамическая вязкость газа";

        public List<IVariable> Description => new List<IVariable>() { _nominalViscosity, _sutherlandConstant, _currentTemperature };

        private decimal GetAbsoluteTemperature()
        {
            return _currentTemperature.Value + Constants.AbsoluteTemperature.Value;
        }
    }

    public class RelativaMolecularMass : IVariable
    {
        private List<IVariable> _molecularMasses;
        private List<IVariable> _contentParts;

        public RelativaMolecularMass(List<IVariable> molecularMasses, List<IVariable> contentParts)
        {
            _molecularMasses = molecularMasses;
            _contentParts = contentParts;
        }

        public decimal Value
        {
            get
            {
                var result = 0m;
                for (int i = 0; i < _molecularMasses.Count; i++)
                {
                    result += _molecularMasses[i].Value * _contentParts[i].Value / 100m;
                }
                return result;
            }
        }

        public string Symbol => "M_см";

        public string Dimension => "кг/кмоль";

        public string Name => "Относительная молярная масса";
    }

    public class ViscosityMassRatio : IVariable
    {
        private List<IVariable> _relativeMolecularMasses;
        private List<IVariable> _contentParts;
        private List<IVariable> _viscosities;

        public ViscosityMassRatio(List<IVariable> relativeMolecularMasses, List<IVariable> contentParts, List<IVariable> viscosities)
        {
            _relativeMolecularMasses = relativeMolecularMasses;
            _contentParts = contentParts;
            _viscosities = viscosities;
        }

        public decimal Value
        {
            get
            {
                var result = 0m;
                for (int i = 0; i < _viscosities.Count; i++)
                {
                    result += _relativeMolecularMasses[i].Value * _contentParts[i].Value / (_viscosities[i].Value * 100m);
                }
                return result;
            }
        }

        public string Symbol => "D";

        public string Dimension => "б/р";

        public string Name => "отношение величин молекулярной массы смеси М_см и динамической вязкости газовой смеси µ_см";
    }

    public class MixAirViscosity : IVariable
    {
        private IVariable _relativeMolecularMass;
        private IVariable _viscosityMassRatio;

        public MixAirViscosity(IVariable relativeMolecularMass, IVariable viscosityMassRatio)
        {
            _relativeMolecularMass = relativeMolecularMass;
            _viscosityMassRatio = viscosityMassRatio;
        }

        public decimal Value => _relativeMolecularMass.Value / _viscosityMassRatio.Value;

        public string Symbol => "µ_см";

        public string Dimension => "Па*с";

        public string Name => "динамическая вязкость газовой смеси";
    }
}