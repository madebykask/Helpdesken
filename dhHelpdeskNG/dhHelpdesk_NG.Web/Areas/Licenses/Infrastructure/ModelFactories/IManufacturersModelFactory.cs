namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Manufacturers;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Manufacturers;

    public interface IManufacturersModelFactory
    {
        ManufacturersIndexModel GetIndexModel(ManufacturersFilterModel filter);

        ManufacturersContentModel GetContentModel(ManufacturerOverview[] manufacturers);

        ManufacturerEditModel GetEditModel(ManufacturerData data);

        ManufacturerModel GetBusinessModel(ManufacturerEditModel editModel);
    }
}