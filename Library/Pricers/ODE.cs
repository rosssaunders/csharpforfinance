namespace Library
{
    public class ODE : IIVP<double>
    {
        private double A;   // Initial condition
        private double T;  // Solve in interval [0, End]

        public ODE(double InitConditon, double Expiry)
        {
            A = InitConditon;
            T = Expiry;
        }

        public double a(double t)
        { 
            // Coefficient of free term

            return 1.0;
        }

        public double f(double t)
        { 
            // Inhomogeneous term
            return 2.0 + t;
        }

        public double InitialCondition
        {
            get
            {
                return A;
            }
            set
            {
                A = value;
            }
        }

        public double Expiry
        {
            get
            {
                return T;
            }
            set
            {
                T = value;
            }
        }
    }
}
