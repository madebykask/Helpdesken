namespace DH.Helpdesk.Dal.Repositories.Computers
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.Dal.Dal;

    public interface IComputerRepository : INewRepository
    {
        void Add(Computer businessModel);

        void Update(Computer businessModel);

        Computer FindById(int id);

        List<ComputerOverview> FindOverviews(
            int customerId,
            int? departmentId,
            int? computerTypeId,
            int? contractStatusId,
            DateTime? contractStartDateFrom,
            DateTime? contractStartDateTo,
            DateTime? contractEndDateFrom,
            DateTime? contractEndDateTo,
            DateTime? scanDateFrom,
            DateTime? scanDateTo,
            DateTime? scrapDateFrom,
            DateTime? scrapDateTo,
            string searchFor);

        List<ComputerResults> Search(int customerId, string searchFor);
    }
}