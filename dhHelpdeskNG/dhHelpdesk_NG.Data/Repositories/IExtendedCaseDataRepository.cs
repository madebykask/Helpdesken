namespace DH.Helpdesk.Dal.Repositories.Cases
{    
    using System;
    using Infrastructure;
    using Domain.ExtendedCaseEntity;
    using BusinessData.Models.Case;
    using System.Collections.Generic;

    public interface IExtendedCaseDataRepository : IRepository<ExtendedCaseDataEntity>
    {
        ExtendedCaseDataModel CreateTemporaryExtendedCaseData(int formId, string creator);

        void AddEcd(ExtendedCaseDataEntity e);        

        ExtendedCaseDataEntity GetExtendedCaseData(Guid extendedCaseGuid);

        ExtendedCaseDataModel CopyExtendedCaseToCase(int extendedCaseDataID, int caseID, string userID, int? extendedCaseFormId = null);

        ExtendedCaseDataModel GetExtendedCaseDataByCaseId(int caseID);

        int GetCaseIdByExtendedCaseGuid(Guid uniqueId);
    }
}