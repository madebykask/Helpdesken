using System.Linq;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
using DH.Helpdesk.Web.Areas.Admin.Models;

namespace DH.Helpdesk.Web.Areas.Admin.Infrastructure.Mappers
{  
    public static class LockedCaseMapper
    {
        public static LockedCasesModel MapToViewModel(this LockedCaseOverview businessModel)
        {
            return new LockedCasesModel(   
                                      businessModel.User.Id,                   
                                      businessModel.User.UserID,
                                      businessModel.User.FirstName,
                                      businessModel.User.SurName,
                                      businessModel.LockInfo.Select(l=> new LockedCaseInfoModel(l.CaseId, l.CaseNumber, l.CustomerId, 
                                                                                                l.CustomerName, l.CreatedTime)).ToList()
                                    );
        }
    }
}