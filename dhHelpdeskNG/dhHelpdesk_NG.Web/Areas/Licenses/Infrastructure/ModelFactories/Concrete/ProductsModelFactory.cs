namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Products;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Products;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class ProductsModelFactory : IProductsModelFactory
    {
        public ProductsIndexModel GetIndexModel(ProductsFilterData data, ProductsFilterModel filter)
        {
            var regions = WebMvcHelper.CreateMultiSelectField(data.Regions, filter.RegionIds);
            var departments = WebMvcHelper.CreateMultiSelectField(data.Departments, filter.DepartmentIds);

            return new ProductsIndexModel(
                                    regions, 
                                    departments, 
                                    filter.RegionIds, 
                                    filter.DepartmentIds);
        }

        public ProductsContentModel GetContentModel(ProductOverview[] products)
        {
            return new ProductsContentModel(products);
        }
    }
}