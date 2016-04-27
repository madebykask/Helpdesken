namespace DH.Helpdesk.Services.Services.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Products;

    public interface IProductsService
    {
        ProductOverview[] GetProducts(
                                int customerId,
                                int[] regions,
                                int[] departments,
                                int[] products);

        ProductsFilterData GetProductsFilterData(int customerId);

        ProductData GetProductData(int customerId, int? productId);

        ProductModel GetById(int id);

        int AddOrUpdate(ProductModel product);

        void Delete(int id);
    }
}