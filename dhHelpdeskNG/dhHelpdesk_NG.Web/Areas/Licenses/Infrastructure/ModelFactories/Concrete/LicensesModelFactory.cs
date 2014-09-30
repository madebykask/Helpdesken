namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.Web.Areas.Licenses.Models.Licenses;

    public sealed class LicensesModelFactory : ILicensesModelFactory
    {
        public LicensesIndexModel GetIndexModel()
        {
            return new LicensesIndexModel();
        }
    }
}