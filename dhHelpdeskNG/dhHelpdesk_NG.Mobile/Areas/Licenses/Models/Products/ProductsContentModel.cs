namespace DH.Helpdesk.Web.Areas.Licenses.Models.Products
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.BusinessData.Models.Licenses.Products;

    public sealed class ProductsContentModel
    {
        public ProductsContentModel(ProductOverview[] products)
        {
            this.Products = products;
        }

        public ProductOverview[] Products { get; private set; }
    }
}