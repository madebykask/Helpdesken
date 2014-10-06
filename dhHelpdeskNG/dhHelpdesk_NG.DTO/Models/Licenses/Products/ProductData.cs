namespace DH.Helpdesk.BusinessData.Models.Licenses.Products
{
    public sealed class ProductData
    {
        public ProductData(ProductModel product)
        {
            this.Product = product;
        }

        public ProductModel Product { get; private set; }
    }
}