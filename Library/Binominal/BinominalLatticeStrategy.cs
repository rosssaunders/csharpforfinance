namespace Library.Binominal
{
    public abstract class BinomialLatticeStrategy
    {
        public BinomialType BinomialType { get; set; }

        protected double d;
        protected double p;
        protected double u;

        //Not used by all strategies. Shouldn't be here
        protected double k;        
        protected double r;
        protected double s;
        
        protected BinomialLatticeStrategy(double vol, double interest, double delta)
        {
            s = vol;
            r = interest;
            k = delta;
            BinomialType = BinomialType.Multiplicative;
        }

        // Useful function
        public void UpdateLattice(Lattice<double> source, double rootValue)
        {
            // Find the depth of the lattice; this a Template Method Pattern
            var si = source.MinIndex;
            source[si, si] = rootValue;

            // Loop from the min index to the end index
            for (var n = source.MinIndex + 1; n <= source.MaxIndex; n++)
            for (var i = 0; i < source.NumberColumns(n); i++)
            {
                source[n, i] = d * source[n - 1, i];
                source[n, i + 1] = u * source[n - 1, i];
            }
        }

        public double DownValue => d;

        public double UpValue => u;

        public double ProbValue => p;
    }
}