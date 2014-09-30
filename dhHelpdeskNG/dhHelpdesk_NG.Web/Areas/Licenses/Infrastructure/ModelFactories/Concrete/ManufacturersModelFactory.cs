namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.Web.Areas.Licenses.Models.Manufacturers;

    public sealed class ManufacturersModelFactory : IManufacturersModelFactory
    {
        public ManufacturersIndexModel GetIndexModel()
        {
            return new ManufacturersIndexModel();
        }
    }
}