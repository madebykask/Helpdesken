namespace DH.Helpdesk.Services.Services.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Manufacturers;

    public interface IManufacturersService
    {
        ManufacturerOverview[] GetManufacturers(int customerId);

        ManufacturerData GetManufacturerData(int customerId, int? manufacturerId);

        ManufacturerModel GetById(int id);

        int AddOrUpdate(ManufacturerModel manufacturer);

        void Delete(int id);
    }
}