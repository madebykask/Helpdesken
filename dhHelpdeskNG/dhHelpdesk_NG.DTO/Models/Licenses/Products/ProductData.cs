namespace DH.Helpdesk.BusinessData.Models.Licenses.Products
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProductData
    {
        public ProductData(
                ProductModel product, 
                ItemOverview[] applications)
        {
            this.Applications = applications;
            this.Product = product;
        }

        [NotNull]
        public ProductModel Product { get; private set; }

        [NotNull]
        public ItemOverview[] Applications { get; private set; }
    }
}