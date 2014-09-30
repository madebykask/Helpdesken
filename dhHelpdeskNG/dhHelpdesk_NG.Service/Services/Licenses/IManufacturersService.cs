namespace DH.Helpdesk.Services.Services.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses;

    public interface IManufacturersService
    {
        ManufacturerOverview[] GetManufacturers(int customerId);
    }
}