using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Models.CaseLock;

namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.CaseLockMappers
{  
    public static class CaseLockMapper
    {
        public static CaseLockModel MapToViewModel(this ICaseLockOverview businessModel, 
                                                   bool isLocked,
                                                   int extendValue, 
                                                   int timerInterval,
                                                   string activeTab = "")
        {
            var caseLockUser = businessModel.User;

            var user = new User
            {
                Id = caseLockUser.Id,
                UserID = caseLockUser.UserId,
                FirstName = caseLockUser.FirstName,
                SurName = caseLockUser.LastName
            };

            return new CaseLockModel(
                                      isLocked,
                                      businessModel.CaseId,
                                      businessModel.UserId,
                                      (isLocked ? Guid.NewGuid().ToString() : businessModel.LockGUID.ToString()),
                                      businessModel.BrowserSession,
                                      businessModel.CreatedTime,
                                      businessModel.ExtendedTime,
                                      extendValue,
                                      timerInterval,
                                      user,
                                      activeTab
                                    );
        }

        public static CaseLockModel MapToViewModel(this CaseLock businessModel, bool isLocked, 
                                                   int extendValue, int timerInterval, string activeTab = "")
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
                                      timerInterval,
                                      businessModel.User,
                                      businessModel.ActiveTab
                                    );
        }
    }
    
}