namespace DH.Helpdesk.Services.Services.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Products;

    public interface IProductsService
    {
        ProductOverview[] GetProducts(
                                int customerId,
                                int[] regions,
                                int[] departments);

        ProductsFilterData GetProductsFilterData(int customerId);

        ProductData GetProductData(int? productId);
    }
}