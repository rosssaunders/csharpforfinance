using System.Collections.Generic;
using System.Text;

namespace Library
{
    public interface IBondModel
    {
        /// <summary>
        /// Price at time t of a PDB of maturity s
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        double P(double t, double s);

        /// <summary>
        /// Yield == spot rate at time t of PDB of maturity s
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        double R(double t, double s);

        /// <summary>
        /// Volatility of yield R(t,s)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        double YieldVolatility(double t, double s);
    }
}
