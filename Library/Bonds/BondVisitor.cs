namespace Library
{
    public abstract class BondVisitor
    {
        // Default constructor
        public BondVisitor()
        {
        }

        // Copy constructor
        public BondVisitor(BondVisitor source)
        {
        }

        // Visit Vasicek.
        public abstract void Visit(VasicekModel model);

        public abstract void Visit(CIRModel model);
    }
}
