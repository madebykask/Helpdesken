using System.Collections.Generic;
using System.Web.Mvc;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.Web.Models.Questionnaire.Input
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class EditQuestionnaireQuestionModel
    {
        public EditQuestionnaireQuestionModel()
        {

        }

        public EditQuestionnaireQuestionModel(int id,int questionnaireId, int languageId, string questionNumber, 
                                              string question, int showNote, string noteText,  DateTime createDate, 
                                              SelectList languages, List<QuestionnaireQuesOptionModel> options )
        {
            this.Id = id;
            this.QuestionnaireId = questionnaireId;
            this.LanguageId = languageId;
            this.QuestionNumber = questionNumber;
            this.Question = question;
            this.ShowNote = showNote;
            this.NoteText = noteText;
            this.CreateDate = createDate;
            this.Languages = languages;
            this.Options = options;
        }

        [IsId]
        public int Id { get; set; }
        
        [LocalizedDisplay("LanguageId")]
        public int LanguageId { get; set; }

        [LocalizedDisplay("QuestionnaireId")]
        public int QuestionnaireId { get; set; }

        [LocalizedRequired]
        [StringLength(10)]
        [LocalizedDisplay("QuestionNumber")]
        public string QuestionNumber { get; set; }

        [LocalizedRequired]
        [StringLength(1000)]
        [LocalizedDisplay("Question")]
        public string Question { get; set; }
        
        [LocalizedDisplay("ShowNote")]        
        public int ShowNote { get; set; }

        [StringLength(1000)]
        [LocalizedDisplay("NoteText")]        
        public string NoteText { get; set; }

        public SelectList Languages { get; set; }

        [LocalizedDisplay("CreateDate")]
        public DateTime CreateDate { get; set; }

        public List<QuestionnaireQuesOptionModel> Options { get; set; } 
    }
}