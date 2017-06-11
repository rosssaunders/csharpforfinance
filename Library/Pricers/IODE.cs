// Interfaces for *Continuous* problem

namespace Library
{
    public interface IODE<U> // V = underlying numeric type
    { 
        // u = u(t); du/dt + a(t)u = f(t), u(0) = A

        U a(U t);       // Coefficient of free term

        U f(U t);       // Inhomogeneous term
    }
}
