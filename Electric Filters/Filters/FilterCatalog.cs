using System;
using System.Collections.Generic;
using System.Linq;

namespace Electric_Filters.Filters
{
    public class FilterCatalog
    {
        private List<Filter> _filters;

        public FilterCatalog()
        {
            var activeAreaName = "Активная площадь";
            var sedimentationAreaName = "Площадь осаждения";

            _filters = new List<Filter>()
            {
                new Filter("ЭГА1-10-6-4-2", new ConstantVariable(16.5m, "Fа", "м^2", activeAreaName), new ConstantVariable(630m, "Fос", "м^2", sedimentationAreaName)),
                new Filter("ЭГА1-10-6-4-3", new ConstantVariable(16.5m, "Fа", "м^2", activeAreaName), new ConstantVariable(950m, "Fос", "м^2", sedimentationAreaName)),
                new Filter("ЭГА1-10-6-6-2", new ConstantVariable(16.5m, "Fа", "м^2", activeAreaName), new ConstantVariable(950m, "Fос", "м^2", sedimentationAreaName)),
                new Filter("ЭГА1-10-6-6-3", new ConstantVariable(16.5m, "Fа", "м^2", activeAreaName), new ConstantVariable(1430m, "Fос", "м^2", sedimentationAreaName)),
                new Filter("ЭГА1-14-7,5-4-3", new ConstantVariable(28.7m, "Fа", "м^2", activeAreaName), new ConstantVariable(1660m, "Fос", "м^2", sedimentationAreaName)),
                new Filter("ЭГА1-14-7,5-4-4", new ConstantVariable(28.7m, "Fа", "м^2", activeAreaName), new ConstantVariable(2210m, "Fос", "м^2", sedimentationAreaName)),
                new Filter("ЭГА1-14-7,5-6-2", new ConstantVariable(28.7m, "Fа", "м^2", activeAreaName), new ConstantVariable(1660m, "Fос", "м^2", sedimentationAreaName)),
                new Filter("ЭГА1-88-12-6-4", new ConstantVariable(285.6m, "Fа", "м^2", activeAreaName), new ConstantVariable(32976m, "Fос", "м^2", sedimentationAreaName))
            };
            _filters = _filters.OrderBy(x => x.ActiveSectionArea.Value).ToList();
        }

        public Filter GetOptimalFilter(IVariable activeArea)
        {
            var allAppropriateFilters = _filters.FindAll(x => x.ActiveSectionArea.Value > activeArea.Value);

            if (allAppropriateFilters.Count == 0)
            {
                throw new ArgumentException("Нет подходящих фильтров");
            }

            return allAppropriateFilters.First();
        }
    }
}
