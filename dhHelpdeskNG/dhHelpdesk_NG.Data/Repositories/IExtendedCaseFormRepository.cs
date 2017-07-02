namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;    
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Enums;

    public interface IExtendedCaseFormRepository : IRepository<ExtendedCaseFormEntity>
    {
        ExtendedCaseFormModel GetExtendedCaseFormForCaseSolution(int caseSolutionId);

        IList<ExtendedCaseFormModel> GetExtendedCaseForm(int caseSolutionId, int customerId, int caseId, int userLanguageId, string userGuid, int caseStateSecondaryId, int caseWorkingGroupId, string extendedCasePath, int? userId, string userName, ApplicationType applicationType);
    }
}