namespace CSharpForFinancialMarkets.Orders
{
    public interface IPricing
    {
        double Price { get; set; }

        double Discount { get; set; }
    }
}
