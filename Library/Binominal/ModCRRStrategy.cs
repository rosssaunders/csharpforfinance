using System;

namespace Library.Binominal
{
    public class ModCRRStrategy : BinomialLatticeStrategy
    {
        public ModCRRStrategy(double vol, double interest, double delta, double S, double K, int N)
            : base(vol, interest, delta)
        {
            // s == volatility, k = step size in time
            var KN = Math.Log(K / S) / N;
            var VN = s * Math.Sqrt(k);

            u = Math.Exp(KN + VN);
            d = Math.Exp(KN - VN);

            p = (Math.Exp(r * k) - d) / (u - d);
        }
    }
}