namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Products;

    public interface IProductsModelFactory
    {
        ProductsIndexModel GetIndexModel(ProductsFilterData data, ProductsFilterModel filter);

        ProductsContentModel GetContentModel(ProductOverview[] products);
    }
}