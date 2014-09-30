namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.Web.Areas.Licenses.Models.Vendors;

    public sealed class VendorsModelFactory : IVendorsModelFactory
    {
        public VendorsIndexModel GetIndexModel()
        {
            return new VendorsIndexModel();
        }
    }
}