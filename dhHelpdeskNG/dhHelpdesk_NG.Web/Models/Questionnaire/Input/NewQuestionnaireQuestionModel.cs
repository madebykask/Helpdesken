namespace DH.Helpdesk.Web.Models.Questionnaire.Input
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class NewQuestionnaireQuestionModel
    {
        public int Id { get; set; }

        [LocalizedDisplay("QuestionnaireId")]
        public int QuestionnaireId { get; set; }

        [StringLength(10)]
        [LocalizedDisplay("QuestionNumber")]
        public string QuestionNumber { get; set; }

        [StringLength(1000)]
        [LocalizedDisplay("Question")]
        public string Question { get; set; }
        
        [LocalizedDisplay("ShowNote")]        
        public int ShowNote { get; set; }

        [StringLength(1000)]
        [LocalizedDisplay("NoteText")]        
        public string NoteText { get; set; }
    
        [LocalizedDisplay("CreateDate")]
        public DateTime CreateDate { get; set; }

        public int LanguageId { get; set; }

    }
}