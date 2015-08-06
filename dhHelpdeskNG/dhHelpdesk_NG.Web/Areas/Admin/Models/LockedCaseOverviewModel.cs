using System;
using System.Collections.Generic;
using System.Web.Mvc;    

namespace DH.Helpdesk.Web.Areas.Admin.Models
{   
    public sealed class LockedCaseOverviewModel
    {
        public LockedCaseOverviewModel()
        {            
        }

        public LockedCaseOverviewModel(
            IList<SelectListItem> customers,
            int? filterCustomerId,
            decimal filterCaseNumber,
            string filterText,
            List<LockedCasesModel> lockedCasesModels
            )
        {
            this.Customers = customers;
            this.FilterCustomerId = filterCustomerId;
            this.FilterCaseNumber = (filterCaseNumber > 0 ? filterCaseNumber.ToString() : "");
            this.FilterText = filterText;            
            this.LockedCasesModels = lockedCasesModels;
        }

        public IList<SelectListItem> Customers { get; private set; }

        public int? FilterCustomerId { get; private set; }

        public string FilterCaseNumber { get; private set; }

        public string FilterText { get; private set; }

        public List<LockedCasesModel> LockedCasesModels { get; private set; }

    }
 
    public sealed class LockedCasesModel
    {
        public LockedCasesModel()
        {            
        }

        public LockedCasesModel(
            int user_Id,
            string userId,
            string userFirstName,
            string userSureName,           
            List<LockedCaseInfoModel> lockedInfo
            )
        {
            this.User_Id = user_Id;
            this.UserId = userId;
            this.UserFirstName = userFirstName;
            this.UserSureName = userSureName;            
            this.LockedInfo = lockedInfo;
        }

        public int User_Id { get; private set; }

        public string UserId { get; private set; }

        public string UserFirstName{ get; private set; }

        public string UserSureName{ get; private set; }
        
        public List<LockedCaseInfoModel> LockedInfo{ get; private set; }

    }

     public sealed class LockedCaseInfoModel
     {
        public LockedCaseInfoModel()
        {            
        }

        public LockedCaseInfoModel(int caseId, decimal caseNumber,
                                   int customerId, string customerName,
                                   DateTime createdTime)
        {
            this.CaseId = caseId;
            this.CaseNumber = caseNumber;
            this.CustomerId = customerId;
            this.CustomerName = customerName;
            this.CreatedTime = createdTime;
        }

        public int CaseId { get; private set;}

        public decimal CaseNumber { get; private set;}

        public int CustomerId { get; private set; }

        public string CustomerName { get; private set; }

        public DateTime CreatedTime { get; private set; }

     }

}