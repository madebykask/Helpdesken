using System;
using System.ComponentModel.DataAnnotations;
using DH.Helpdesk.BusinessData.Models.Common.Input;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Input
{
    public sealed class EditCircular:INewBusinessModel
    {
        public EditCircular(int id, 
                            string circularName,                            
                            int status,
                            DateTime changedDate)
        {
            this.Id = id;
            this.CircularName = circularName;
            this.Status = status;
            this.ChangedDate = changedDate;
        }

        [IsId]
        public int Id { get; set; }        

        [Required]
        [StringLength(50)]        
        public string CircularName { get; private set; }
                
        public int Status { get; private set; }
                
        public DateTime ChangedDate { get; private set; }

    }
}
