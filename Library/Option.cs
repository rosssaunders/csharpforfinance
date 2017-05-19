using static System.Math;
using static UnitTestProject1.SpecialFunctions;

namespace UnitTestProject1
{
    public class Option
    {
        public double r { get; set; }       // Interest rate
        public double sig { get; set; }     // Volatility
        public double K { get; set; }       // Strike price
        public double T { get; set; }       // Expiry date
        public double b { get; set; }      // Cost of carry

        public OptionType Type { get; set; }    // Option name (call, put)

        // Kernel Functions (Haug)
        private double CallPrice(double U)
        {
            double tmp = sig * Sqrt(T);

            double d1 = (Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
            double d2 = d1 - tmp;

            return (U * Exp((b - r) * T) * N(d1)) - (K * Exp(-r * T) * N(d2));
        }

        private double PutPrice(double U)
        {
            double tmp = sig * Sqrt(T);

            double d1 = (Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
            double d2 = d1 - tmp;
            return (K * Exp(-r * T) * N(-d2)) - (U * Exp((b - r) * T) * N(-d1));
        }

        /////////////////////////////////////////////////////////////////////////////////////
        public Option(OptionType optionType)
        {   
            // Create option instance of given type and default values
            Type = optionType;
        }

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
        public double Price(double U)
        {
            if (Type == OptionType.Call)
                return CallPrice(U);
            else
                return PutPrice(U);
        }

        private double CallDelta(double U)
        {
            double tmp = sig * Sqrt(T);
            double d1 = (Log(U/K) + (b + (sig*sig) * 0.5) * T) / tmp;
            return Exp((b - r) * T) * N(d1);
        }

        private double CallGamma(double U)
        {
            double tmp = sig * Sqrt(T);
            double d1 = Log(U / K) + (b + (sig * sig) * 0.5 * T) / tmp;
            double d2 = d1 - tmp;

            return N(d1) * Exp((b-r) * T) / (U * tmp);
        }

        private double CallVega(double U)
        {
            double tmp = sig * Sqrt(T);
            double d1 = (Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
            return (U * Exp((b - r) * T) * n(d1) * Sqrt(T));
        }

        private double CallRho(double U)
        {
            double tmp = sig * Sqrt(T);

            double d1 = (Log(U / K) + (b + (sig * sig) * 0.5) * T) / tmp;
            double d2 = d1 - tmp;

            if (b != 0.0)
                return T * K * Exp(-r * T) * N(d2);
            else
                return -T * CallPrice(U);
        }

        private double CallCharm(double U)
        {
            double tmp = sig * Sqrt(T);
            double d1 = Log(U / K) + (b + (sig * sig) * 0.5 * T) / tmp;
            double d2 = d1 - tmp;

            //Is this correct?
            //http://www.wilmottwiki.com/wiki/index.php?title=Call_option
            double charm = (b * Exp(-b * (T)) * N(d1) + Exp(-b * T) * N(d1) * (d2 / (2 * T) - (r - b) / (sig * Sqrt(T))));

            return charm;
        }

        private double CallVomma(double U)
        {
            double tmp = sig * Sqrt(T);
            double d1 = Log(U / K) + (b + (sig * sig) * 0.5 * T) / tmp;
            double d2 = d1 - tmp;

            //Is this correct?
            //http://www.wilmottwiki.com/wiki/index.php?title=Call_option
            double vomma = U * Sqrt(T) * Exp(-b*T) * N(d1) * (d1*d2) / sig;

            return vomma;
        }
    }
}
