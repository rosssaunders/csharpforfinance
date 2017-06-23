using System;

namespace CSharpForFinancialMarkets.Orders
{
    internal class NoNameException : Exception
    {
        public NoNameException()
        {
        }

        public NoNameException(string message) : base(message)
        {
        }

        public NoNameException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}