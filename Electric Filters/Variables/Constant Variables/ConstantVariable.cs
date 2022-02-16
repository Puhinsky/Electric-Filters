namespace Electric_Filters
{
    public class ConstantVariable : IRequestable
    {
        private decimal _value;
        private string _symbol;
        private string _dimension;
        private string _name; 

        public ConstantVariable(decimal value, string symbol, string dimension, string name)
        {
            _value = value;
            _symbol = symbol;
            _dimension = dimension;
            _name = name;
        }

        public virtual decimal Value => _value;

        public string Symbol => _symbol;

        public string Dimension => _dimension;

        public string Name => _name;

        public void SetInput(decimal value)
        {
            _value = value;
        }
    }
}
