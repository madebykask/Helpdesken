namespace DH.Helpdesk.Dal.Repositories.Computers
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.Dal.Dal;

    public interface IComputerRepository : INewRepository
    {
        void Add(Computer businessModel);

        void Update(Computer businessModel);

        Computer FindById(int id);

        List<ComputerResults> Search(int customerId, string searchFor);

        List<ComputerOverview> FindOverviews(
            int customerId,
            int? departmentId,
            int? regionId,
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
            string searchFor,
            bool isShowScrapped,
            int recordsOnPage);

        void RemoveReferenceOnNic(int id);

        void RemoveReferenceOnRam(int id);

        void RemoveReferenceOnProcessor(int id);

        void RemoveReferenceOnOs(int id);

        void RemoveReferenceOnComputerType(int id);

        void RemoveReferenceOnComputerModel(int id);

        int GetComputerCount(int customerId, int? departmentId);

        List<ReportModel> FindConnectedToComputerLocationOverviews(int customerId, int? departmentId, string searchFor);
    }
}