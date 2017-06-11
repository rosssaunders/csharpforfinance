
namespace Library
{
    public interface IIVP<U> : IODE<U>
    { 
        // Specify initial condition and range [0,T] of integation

        U InitialCondition
        {
            get;
            set;
        }

        U Expiry
        {
            get;
            set;
        }
    }
}

