namespace DH.Helpdesk.BusinessData.Models.Licenses.Products
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProductData
    {
        public ProductData(ProductModel product)
        {
            this.Product = product;
        }

        [NotNull]
        public ProductModel Product { get; private set; }
    }
}