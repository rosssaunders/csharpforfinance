
using System;
namespace Library
{
    public abstract class OneStepFDM : IFDM<double>
    {
        protected int NSteps;   // Number of time steps
        protected ODE ode;      // The reference to ODE

        protected double vOld, vNew;   // Values at levels n, n+1
        protected double[] mesh;
        protected double delta_T;      // Step length

        public OneStepFDM(int NSteps, ODE ode)
        {
            this.NSteps = NSteps;
            this.ode = ode;

            vOld = ode.InitialCondition;
            vNew = vOld;

            mesh = new double[NSteps + 1];
            mesh[0] = 0.0;
            mesh[NSteps] = ode.Expiry;
            delta_T = (mesh[NSteps] - mesh[0]) / NSteps;

            for (int n = 1; n < NSteps; n++)
            {
                mesh[n] = mesh[n - 1] + delta_T;
                Console.Write(", {0}", mesh[n]);
            }
        }

        public abstract void CalculateOneStep(int m);   // One-step method

        // The full algorithm computed at the expity t = T
        public double Calculate()
        {
            for (int m = 0; m <= NSteps; m++)
            {
                CalculateOneStep(m);
                vOld = vNew;

            }

            return vNew;
        }

        public double Value
        { // Computed value

            get
            {
                return vNew;
            }
            private set { }
        }
    }
}
