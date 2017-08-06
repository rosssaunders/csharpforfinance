namespace Library
{
    public interface ITwoFactorPayoff
    { 
        // The unambiguous specification of two-factor payoff contracts
        // This interfaces can be specialised to various underlyings
        double Payoff(double factorI1, double factorII);
    }
}