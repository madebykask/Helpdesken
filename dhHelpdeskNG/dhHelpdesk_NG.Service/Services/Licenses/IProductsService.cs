namespace DH.Helpdesk.Services.Services.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses;

    public interface IProductsService
    {
        ProductOverview[] GetProducts(
                                int customerId,
                                int[] regions,
                                int[] departments);
    }
}