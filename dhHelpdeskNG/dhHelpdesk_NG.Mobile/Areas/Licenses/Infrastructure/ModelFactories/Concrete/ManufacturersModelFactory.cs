namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Manufacturers;

    public sealed class ManufacturersModelFactory : IManufacturersModelFactory
    {
        public ManufacturersIndexModel GetIndexModel(ManufacturersFilterModel filter)
        {
            return new ManufacturersIndexModel();
        }

        public ManufacturersContentModel GetContentModel(ManufacturerOverview[] manufacturers)
        {
            return new ManufacturersContentModel(manufacturers);
        }
    }
}