using System.Runtime.CompilerServices;
using static System.Math;
using static CSharpForFinancialMarkets.SpecialFunctions;

namespace CSharpForFinancialMarkets
{

    public class Option
    {
        public double r { get; set; }       // Interest rate
        public double sig { get; set; }     // Volatility
        public double K { get; set; }       // Strike price
        public double T { get; set; }       // Expiry date
        public double b { get; set; }      // Cost of carry

        public OptionType Type { get; set; }    // Option name (call, put)

        /////////////////////////////////////////////////////////////////////////////////////
        public Option(OptionType optionType, double expiry, double strike, double costOfCarry, double interest, double volatility)
        {
            // Create option instance
            Type = optionType;
            T = expiry;
            K = strike;
            b = costOfCarry;
            r = interest;
            sig = volatility;
        }

        // Functions that calculate option price and sensitivities
        public virtual double Price(double U)
        {
            if (Type == OptionType.Call)
                return CallPrice(U);
            else
                return PutPrice(U);
        }

        public virtual double Delta(double U)
        {
            if (Type == OptionType.Call)
                return CallDelta(U);
            else
                return PutDelta(U);
        }

        public virtual double Gamma(double U)
        {
            if (Type == OptionType.Call)
                return CallGamma(U);
            else
                return PutGamma(U);
        }

        public virtual double Vega(double U)
        {
            if (Type == OptionType.Call)
                return CallVega(U);
            else
                return PutVega(U);
        }

        public virtual double Theta(double U)
        {
            if (Type == OptionType.Call)
                return CallTheta(U);
            else
                return PutTheta(U);
        }

        public virtual double Rho(double U)
        {
            if (Type == OptionType.Call)
                return CallRho(U);
            else
                return PutRho(U);
        }

        /// <summary>
        /// Cost of carry
        /// </summary>
        /// <param name="U"></param>
        /// <returns></returns>
        public virtual double Coc(double U)
        {
            if (Type == OptionType.Call)
                return CallCoc(U);
            else
                return PutCoc(U);
        }

        /// <summary>
        /// Charm
        /// </summary>
        /// <param name="U"></param>
        /// <returns></returns>
        public virtual double Charm(double U)
        {
            if (Type == OptionType.Call)
                return CallCharm(U);
            else
                return PutCharm(U);
        }

        /// <summary>
        /// Charm
        /// </summary>
        /// <param name="U"></param>
        /// <returns></returns>
        public double Vomma(double U)
        {
            if (Type == OptionType.Call)
                return CallVomma(U);
            else
                return PutVomma(U);
        }

        /// <summary>
        /// Elasticity
        /// </summary>
        /// <param name="percentageMovement"></param>
        /// <param name="U"></param>
        /// <returns></returns>
        public virtual double Elasticity(double percentageMovement, double U)
        {
            if (Type == OptionType.Call)
                return CallElasticity(percentageMovement, U);
            else
                return PutElasticity(percentageMovement, U);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected (double d1, double d2, double tmp) D(double U)
        {
            var tmp = sig * Sqrt(T);
            var d1 = (Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
            var d2 = d1 - tmp;

            return (d1, d2, tmp);
        }

        // Kernel Functions (Haug)
        protected virtual double CallPrice(double U)
        {
            var (d1, d2, tmp) = D(U);

            return (U * Exp((b - r) * T) * N(d1)) - (K * Exp(-r * T) * N(d2));
        }

        protected virtual double PutPrice(double U)
        {
            var (d1, d2, tmp) = D(U);

            return (K * Exp(-r * T) * N(-d2)) - (U * Exp((b - r) * T) * N(-d1));
        }

        private double CallDelta(double U)
        {
            var (d1, d2, tmp) = D(U);
            return Exp((b - r) * T) * N(d1);
        }

        private double PutDelta(double U)
        {
            var (d1, d2, tmp) = D(U);
            return Exp((b - r) * T) * (N(d1) - 1.0);
        }

        private double CallGamma(double U)
        {
            var (d1, d2, tmp) = D(U);
            return N(d1) * Exp((b - r) * T) / (U * tmp);
        }

        private double PutGamma(double U)
        {
            return CallGamma(U);
        }

        private double CallVega(double U)
        {
            var (d1, d2, tmp) = D(U);
            return (U * Exp((b - r) * T) * n(d1) * Sqrt(T));
        }

        private double PutVega(double U)
        {
            return CallVega(U);
        }

        private double CallRho(double U)
        {
            var (d1, d2, tmp) = D(U);

            if (b != 0.0)
                return T * K * Exp(-r * T) * N(d2);
            else
                return -T * CallPrice(U);
        }

        private double PutRho(double U)
        {
            var (d1, d2, tmp) = D(U);

            if (b != 0.0)
                return -T * K * Exp(-r * T) * N(-d2);
            else
                return -T * PutPrice(U);
        }

        private double CallTheta(double U)
        {
            var (d1, d2, tmp) = D(U);

            var t1 = (U * Exp((b - r) * T) * n(d1) * sig * 0.5) / Sqrt(T); // AG: here SpecialFunctions.n vs SpecialFunctions.N
            var t2 = (b - r) * (U * Exp((b - r) * T) * N(d1));
            var t3 = r * K * Exp(-r * T) * N(d2);

            return -(t1 + t2 + t3);
        }

        private double PutTheta(double U)
        {
            var (d1, d2, tmp) = D(U);

            var t1 = (U * Exp((b - r) * T) * n(d1) * sig * 0.5) / Sqrt(T); // AG: here SpecialFunctions.n vs SpecialFunctions.N
            var t2 = (b - r) * (U * Exp((b - r) * T) * N(-d1));
            var t3 = r * K * Exp(-r * T) * N(-d2);

            return t2 + t3 - t1;
        }

        private double CallCoc(double U)
        {
            var (d1, d2, tmp) = D(U);

            return T * U * Exp((b - r) * T) * N(d1);
        }

        private double PutCoc(double U)
        {
            var (d1, d2, tmp) = D(U);

            return -T * U * Exp((b - r) * T) * N(-d1);
        }

        private double CallElasticity(double percentageMovement, double U)
        {
            return (CallDelta(U) * U) / percentageMovement;
        }

        private double PutElasticity(double percentageMovement, double U)
        {
            return (PutDelta(U) * U) / percentageMovement;
        }

        /// <summary>
        /// Calculates the Call Charm
        /// </summary>
        /// <remarks>
        /// From Ch.3 Homework
        /// </remarks>
        /// <param name="U">Price of the underlying instrument</param>
        /// <returns>The value of charm</returns>
        private double CallCharm(double U)
        {
            var (d1, d2, tmp) = D(U);

            //From Haug - Complete guide to Option Pricing formulas
            var x = -Exp((b - r) * T);
            var y = n(d1) * ((b / (sig * Sqrt(T))) - (d2 / (2 * T)));
            var z = (b - r) * N(d1);
            
            var charm = x * (y + z);
            
            return charm;
        }

        private double PutCharm(double U)
        {
            var (d1, d2, tmp) = D(U);

            //From Haug - Complete guide to Option Pricing formulas
            var x = -Exp((b - r) * T);
            var y = n(d1) * ((b / (sig * Sqrt(T))) - (d2 / (2 * T)));
            var z = (b - r) * N(-d1);

            var charm = x * (y - z);

            return charm;
        }

        /// <summary>
        /// Calculates the Call Vomma
        /// </summary>
        /// <remarks>
        /// From Ch.3 Homework
        /// </remarks>
        /// <param name="U">Price of the underlying instrument</param>
        /// <returns>The value of Vomma</returns>
        private double CallVomma(double U)
        {
            var (d1, d2, _) = D(U);

            return CallVega(d1 * d2 / sig);
        }

        /// <summary>
        /// Calculates the Call Vomma
        /// </summary>
        /// <remarks>
        /// From Ch.3 Homework
        /// </remarks>
        /// <param name="U">Price of the underlying instrument</param>
        /// <returns>The value of Vomma</returns>
        private double PutVomma(double U)
        {
            var (d1, d2, tmp) = D(U);

            return PutVega(d1 * d2 / sig);
        }
    }
}
