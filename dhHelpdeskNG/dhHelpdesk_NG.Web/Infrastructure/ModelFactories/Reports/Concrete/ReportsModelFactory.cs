namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Reports.Concrete
{
    using DH.Helpdesk.Web.Models.Reports;

    internal sealed class ReportsModelFactory : IReportsModelFactory
    {
        public IndexModel CreateIndexModel()
        {
            var instance = new IndexModel();
            return instance;
        }
    }
}