namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Computers;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Computers;

    public sealed class ComputersModelFactory : IComputersModelFactory
    {
        public ComputersContentModel GetContentModel(ComputerOverview[] computers)
        {
            return new ComputersContentModel(computers);
        }
    }
}