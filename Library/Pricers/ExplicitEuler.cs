
using System;

namespace Library
{
    public class ExplicitEuler : OneStepFDM
    {
        public ExplicitEuler(int NSteps, ODE ode) : base(NSteps, ode) { }

        public override void CalculateOneStep(int n)
        { 
            // One-step method

            // Create temp vars for readility
            double aVar = ode.a(mesh[n]);
            double fVar = ode.f(mesh[n]);
            
            vNew = (1.0 - delta_T * aVar) * vOld + delta_T * fVar;
            Console.WriteLine("old, new: [{0}, {1}]", vOld, vNew);
        }
    }
}