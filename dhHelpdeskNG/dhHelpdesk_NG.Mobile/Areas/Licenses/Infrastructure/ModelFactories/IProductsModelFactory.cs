namespace DH.Helpdesk.Mobile.Areas.Licenses.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.BusinessData.Models.Licenses.Products;
    using DH.Helpdesk.Mobile.Areas.Licenses.Models.Products;

    public interface IProductsModelFactory
    {
        ProductsIndexModel GetIndexModel(ProductsFilterData data, ProductsFilterModel filter);

        ProductsContentModel GetContentModel(ProductOverview[] products);
    }
}