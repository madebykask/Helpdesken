using System;
using System.ComponentModel.DataAnnotations;
using DH.Helpdesk.BusinessData.Models.Common.Input;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Input
{
    public sealed class CircularPart : INewBusinessModel
    {        
        public CircularPart(int caseId, int caseNumber, string caption, string email)
        {
            this.CaseId = caseId;
            this.CaseNumber = caseNumber;
            this.Caption = caption;
            this.Email = email;
        }

        public int Id { get; set; }

        public int CaseId { get; set; }

        public int CaseNumber { get; set; }

        public string Caption { get; set; }

        public string Email { get; set; }
    } 

    public sealed class NewCircular:INewBusinessModel
    {
        public NewCircular(int questionnaireId, 
                           string circularName,
                           int status,
                           DateTime changedDate)            
        {
            this.QuestionnaireId = questionnaireId;
            this.CircularName = circularName;
            this.Status = status;
            this.ChangedDate = changedDate;
        }

        [IsId]
        public int Id { get; set; }

        public int QuestionnaireId { get; private set; }

        [Required]
        [StringLength(50)]        
        public string CircularName { get; private set; }
                
        public int Status { get; private set; }
                
        public DateTime ChangedDate { get; private set; }

    }
}
