namespace Electric_Filters.Filters
{
    public class Filter
    {
        private string _name;
        private IVariable _activeSectionArea;
        private IVariable _sedimentationArea;

        public string Name => _name;
        public IVariable ActiveSectionArea => _activeSectionArea;
        public IVariable SedimentationArea => _sedimentationArea;

        public Filter(string name, IVariable activeSectionArea, IVariable sedimentationArea)
        {
            _name = name;
            _activeSectionArea = activeSectionArea;
            _sedimentationArea = sedimentationArea;
        }
    }
}
