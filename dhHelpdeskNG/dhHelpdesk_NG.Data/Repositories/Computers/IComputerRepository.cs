namespace DH.Helpdesk.Dal.Repositories.Computers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Dal;

    public interface IComputerRepository : INewRepository
    {
        ComputerResults GetComputerInventory(string computername, bool join);

        List<ComputerResults> Search(int customerId, string searchFor);
    }
}