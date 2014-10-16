namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using System.Linq;

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
                                    departments);
        }

        public ProductsContentModel GetContentModel(ProductOverview[] products)
        {
            return new ProductsContentModel(products);
        }

        public ProductEditModel GetEditModel(ProductData data)
        {
            return new ProductEditModel(
                            data.Product.Id,
                            data.Product.CustomerId,
                            data.Product.ProductName,
                            WebMvcHelper.GetEmptyList(),
                            WebMvcHelper.GetListItems(data.Applications));
        }

        public ProductModel GetBusinessModel(ProductEditModel editModel)
        {
            var model = new ProductModel(
                            editModel.Id,
                            editModel.ProductName,
                            null,
                            editModel.CustomerId,
                            WebMvcHelper.GetOverviews(editModel.SelectedApplications.ToArray()));

            return model;
        }
    }
}