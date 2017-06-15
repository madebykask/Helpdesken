namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;    
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;
    using DH.Helpdesk.BusinessData.Models.Case;
    using System;

    public interface IExtendedCaseFormRepository : IRepository<ExtendedCaseFormEntity>
    {
        IList<ExtendedCaseFormModel> GetExtendedCaseForm(int caseSolutionId, int customerId, int caseId, int userLanguageId, Guid userGuid, int caseStateSecondaryId, int caseWorkingGroupId, string extendedCasePath);
    }
}