namespace DH.Helpdesk.Web.Models.Questionnaire.Input
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class EditCircularModel
    {
        public EditCircularModel()
        {
        }

        public EditCircularModel(
            int id,
            int questionnaireId,
            string circularName,
            DateTime changedDate,
            List<ConnectedToCircularOverview> connectedCases)
        {
            this.Id = id;
            this.ConnectedCases = connectedCases;
            this.QuestionnaireId = questionnaireId;
            this.CircularName = circularName;
            this.ChangedDate = changedDate;
        }

        [IsId]
        public int Id { get; set; }

        [LocalizedDisplay("QuestionnaireId")]
        public int QuestionnaireId { get; set; }

        [Required]
        [StringLength(100)]
        [LocalizedDisplay("CircularName")]
        public string CircularName { get; set; }

        [LocalizedDisplay("State")]
        public int State { get; set; }

        [LocalizedDisplay("StateText")]
        public string StateText { get; set; }

        [LocalizedDisplay("ChangedDate")]
        public DateTime ChangedDate { get; set; }
        
        [NotNull]
        public List<ConnectedToCircularOverview> ConnectedCases { get; set; }
    }
}