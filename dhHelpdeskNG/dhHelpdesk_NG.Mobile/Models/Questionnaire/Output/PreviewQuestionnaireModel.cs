using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;

namespace DH.Helpdesk.Web.Models.Questionnaire.Output
{
   
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class SubOptions
    {
        public SubOptions(int optionPos, string optionText, int optionValue)
        {
            this.OptionPos = optionPos;
            this.OptionText = optionText;
            this.OptionValue = optionValue;
        }

        [LocalizedDisplay("OptionPos")]
        public int OptionPos { get; set; }

        [LocalizedDisplay("OptionPos")]
        public string OptionText { get; set; }

        [LocalizedDisplay("OptionValue")]
        public int OptionValue { get; set; }
    }


    public sealed class SubQuestions
    {
        public SubQuestions(string questionNumber, string questionText, bool showNote, string noteText, List<SubOptions> options)
        {
            this.QuestionNumber = questionNumber;
            this.QuestionText = questionText;
            this.ShowNote = showNote;
            this.NoteText = noteText;
            this.Options = options;
        }

        [LocalizedDisplay("QuestionNumber")]
        public string QuestionNumber { get;set; }     

        [LocalizedDisplay("QuestionText")]
        public string QuestionText { get; set; }

        [LocalizedDisplay("ShowNote")]
        public bool ShowNote { get; set; }

        [LocalizedDisplay("NoteText")]
        public string NoteText { get; set; }
        
        [LocalizedDisplay("Options")]
        public List<SubOptions> Options { get; set; }
    }


    public sealed class PreviewQuestionnaireModel
    {

        #region Constructors and Destructors        

        public PreviewQuestionnaireModel(int id, int languageId, string name, string description, List<SubQuestions> questions, SelectList languages)
        {
            this.Id = id;
            this.LanguageId = languageId;
            this.Name = name;
            this.Description = description;
            this.Questions = questions;
            this.Languages = languages;
        }

        #endregion

        #region Public Properties

        [IsId]
        public int Id { get; set; }

        [LocalizedDisplay("LanguageId")]
        public int LanguageId { get; set; }
        
        [LocalizedDisplay("Name")]
        public string Name { get; set; }

        [LocalizedDisplay("Description")]
        public string Description { get; set; }

        [LocalizedDisplay("Questions")]
        public List<SubQuestions> Questions { get; set; }

        public SelectList Languages { get; set; }

        #endregion
    }

}