namespace DH.Helpdesk.Dal.Repositories.Computers
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Dal.Dal;

    public interface IComputerRepository : INewRepository
    {
        void Add(ComputerForInsert businessModel);
        void Update(ComputerForUpdate businessModel);
        void UpdateInfo(int id, string info);
        void DeleteById(int id);
        ComputerFile GetFile(int contractId);
        void SaveFile(int id, string fileName, byte[] data);
        void DeleteFile(int id);
        ComputerForRead FindById(int id);
        List<ComputerResults> Search(int customerId, string searchFor);
        List<ComputerOverview> FindOverviews(int customerId,
            int? domainId,
            int? departmentId,
            int? regionId,
            int? ouId,
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
            int recordsOnPage,
            SortField sortOptions,
            int? recordsCount,
            bool isComputerDepartmentSource);
        ComputerShortOverview FindShortOverview(int id);
        void RemoveReferenceOnNic(int id);
        void RemoveReferenceOnRam(int id);
        void RemoveReferenceOnProcessor(int id);
        void RemoveReferenceOnOs(int id);
        void RemoveReferenceOnComputerType(int id);
        void RemoveReferenceOnComputerModel(int id);
        int GetComputerCount(int customerId, int? departmentId);
        List<ReportModel> FindConnectedToComputerLocationOverviews(int customerId, int? departmentId, string searchFor);
        int GetIdByName(string computerName, int customerId);
        List<ComputerOverview> GetRelatedOverviews(int customerId, string userId);
        int? GetComputerTypeById(int id);
        bool IsMacAddressUnique(int exceptId, string macAddress);
        bool IsTheftMarkUnique(int exceptId, string theftMark);
        bool IsComputerNameUnique(int exceptId, string computerName);
        bool IsIpAddressUnique(int exceptId, string ipAddress);
    }
}