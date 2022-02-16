namespace Electric_Filters
{
    public interface IVariable
    {
        public decimal Value { get; }
        public string Symbol { get; }
        public string Dimension { get; }
        public string Name { get; }
    }
}
