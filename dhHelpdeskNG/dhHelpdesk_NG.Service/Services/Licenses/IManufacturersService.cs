namespace DH.Helpdesk.Services.Services.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.BusinessData.Models.Licenses.Manufacturers;

    public interface IManufacturersService
    {
        ManufacturerOverview[] GetManufacturers(int customerId);
    }
}