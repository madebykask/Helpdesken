using DH.Helpdesk.BusinessData.Models.Shared.Input;
using DH.Helpdesk.Common.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Checklists.Output
{
    public sealed class CheckListActionBM : INewBusinessModel
    {

        public CheckListActionBM()
        { }

        public CheckListActionBM(
                int id,
                int serviceId,                   
                int isActive,
                string actionName,
                DateTime changedDate,
                DateTime createdDate
               )
        {
            this.Id = id;
            this.Service_Id = serviceId;     
            this.IsActive = isActive;
            this.ActionName = actionName;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
        }

        public int Id { get; set; }
        public int Service_Id { get; private set; }
        public int IsActive { get; private set; }
        public string ActionName { get; private set; }
        public DateTime ChangedDate { get; private set; }
        public DateTime CreatedDate { get; private set; }

    }
}
