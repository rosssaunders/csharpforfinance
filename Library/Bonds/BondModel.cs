namespace Library
{
    public abstract class BondModel : IBondModel
    {
        // SDE data; in general this would be an SDE object
        public double kappa;    // Speed of mean reversion
        public double theta;    // Long-term level
        public double vol;      // Volatility of short rate
        public double r;        // Flat term structure             

        // Interface IBondModel
        public abstract double P(double t, double s);

        public abstract double R(double t, double s);

        public abstract double YieldVolatility(double t, double s);

        public BondModel(double kappa, double theta, double vol, double r)
        {
            this.kappa = kappa;
            this.theta = theta;
            this.vol = vol;
            this.r = r;
        }

        // Accept visitor.
        public abstract void Accept(BondVisitor visitor);
    }
}
