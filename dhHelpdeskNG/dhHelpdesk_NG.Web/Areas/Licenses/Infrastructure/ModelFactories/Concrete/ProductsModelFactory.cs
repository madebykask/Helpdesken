namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.Web.Areas.Licenses.Models.Products;

    public sealed class ProductsModelFactory : IProductsModelFactory
    {
        public ProductsIndexModel GetIndexModel()
        {
            return new ProductsIndexModel();
        }
    }
}