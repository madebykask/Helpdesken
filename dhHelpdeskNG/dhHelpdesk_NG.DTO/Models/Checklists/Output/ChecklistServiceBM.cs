using DH.Helpdesk.BusinessData.Models.Shared.Input;
using DH.Helpdesk.Common.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.Checklists.Output
{
    public sealed class CheckListServiceBM : INewBusinessModel
    {
       
        public CheckListServiceBM()
        {}

        public CheckListServiceBM(
                int customerId,
                int checkListId,  
                int id, 
                int isActive,
                string Name,
                DateTime changedDate,
                DateTime createdDate
               )
        {
            this.CustomerId = customerId;
            this.CheckListId = checkListId;
            this.Id = id;
            this.IsActive = isActive;
            this.Name = Name;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
        }

        public int CustomerId { get; private set; }
        public int CheckListId { get; private set; }

        [IsId]
        public int Id { get; set; }
        public int IsActive { get; set; }
        public string Name { get; private set; }
        public DateTime ChangedDate { get; private set; }
        public DateTime CreatedDate { get; private set; }
       
        

    }
}
