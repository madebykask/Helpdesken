namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Computers;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Computers;

    public interface IComputersModelFactory
    {
        ComputersContentModel GetContentModel(ComputerOverview[] computers);
    }
}