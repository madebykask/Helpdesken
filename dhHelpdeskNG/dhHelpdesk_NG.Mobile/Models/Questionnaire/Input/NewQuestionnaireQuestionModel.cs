using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.Mobile.Models.Questionnaire.Input
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public class NewQuestionnaireQuestionModel
    {
        [IsId]
        public int Id { get; set; }

        [LocalizedDisplay("QuestionnaireId")]
        public int QuestionnaireId { get; set; }

        [Required]
        [StringLength(10)]
        [LocalizedDisplay("QuestionNumber")]
        public string QuestionNumber { get; set; }

        [Required]
        [StringLength(1000)]
        [LocalizedDisplay("Question")]
        public string Question { get; set; }
        
        [LocalizedDisplay("ShowNote")]        
        public int ShowNote { get; set; }

        [Required]
        [StringLength(1000)]
        [LocalizedDisplay("NoteText")]        
        public string NoteText { get; set; }
    
        [LocalizedDisplay("CreateDate")]
        public DateTime CreateDate { get; set; }

        public int LanguageId { get; set; }

    }
}