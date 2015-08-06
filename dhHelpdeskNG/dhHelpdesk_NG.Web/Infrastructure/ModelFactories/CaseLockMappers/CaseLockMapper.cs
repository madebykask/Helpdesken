using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
using DH.Helpdesk.Web.Models.CaseLock;

namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.CaseLockMappers
{  
    public static class CaseLockMapper
    {
        public static CaseLockModel MapToViewModel(this CaseLock businessModel, bool isLocked, int extendValue)
        {   
            return new CaseLockModel(
                                      isLocked, 
                                      businessModel.CaseId,
                                      businessModel.UserId,
                                      (isLocked? Guid.NewGuid().ToString() : businessModel.LockGUID.ToString()),
                                      businessModel.BrowserSession,
                                      businessModel.CreatedTime,
                                      businessModel.ExtendedTime,
                                      extendValue,
                                      businessModel.User
                                    );
        }
    }
    
}