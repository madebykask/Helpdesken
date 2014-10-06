namespace DH.Helpdesk.Mobile.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.BusinessData.Models.Licenses.Products;
    using DH.Helpdesk.Mobile.Areas.Licenses.Models.Products;
    using DH.Helpdesk.Mobile.Infrastructure.Tools;

    public sealed class ProductsModelFactory : IProductsModelFactory
    {
        public ProductsIndexModel GetIndexModel(ProductsFilterData data, ProductsFilterModel filter)
        {
            var regions = WebMvcHelper.CreateMultiSelectField(data.Regions, filter.RegionIds);
            var departments = WebMvcHelper.CreateMultiSelectField(data.Departments, filter.RegionIds);

            return new ProductsIndexModel(regions, departments);
        }

        public ProductsContentModel GetContentModel(ProductOverview[] products)
        {
            return new ProductsContentModel(products);
        }
    }
}