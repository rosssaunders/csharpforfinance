// Interfaces for the *Discrete* problem

namespace Library
{
    public interface IFDM<U> // V = underlying numeric type
    { // Computation fron a given time level n to next n+1

        void CalculateOneStep(int m);   // One-step method
    }
}
