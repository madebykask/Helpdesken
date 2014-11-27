namespace DH.Helpdesk.Web.Areas.Licenses.Models.Computers
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Computers;

    public sealed class ComputersContentModel
    {
        public ComputersContentModel(ComputerOverview[] computers)
        {
            this.Computers = computers;
        }

        public ComputerOverview[] Computers { get; private set; }
    }
}