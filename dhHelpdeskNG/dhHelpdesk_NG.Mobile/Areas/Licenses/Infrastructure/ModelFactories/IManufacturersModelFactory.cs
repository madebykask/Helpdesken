namespace DH.Helpdesk.Mobile.Areas.Licenses.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.BusinessData.Models.Licenses.Manufacturers;
    using DH.Helpdesk.Mobile.Areas.Licenses.Models.Manufacturers;

    public interface IManufacturersModelFactory
    {
        ManufacturersIndexModel GetIndexModel(ManufacturersFilterModel filter);

        ManufacturersContentModel GetContentModel(ManufacturerOverview[] manufacturers);
    }
}