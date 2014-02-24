namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class EditQuestionnaireQuestion : INewBusinessModel
    {
        #region Constructors and Destructors

        public EditQuestionnaireQuestion(int id,int questionnaireId, int languageId, string questionNumber, string question, int showNote, string noteText, DateTime changeDate)
        {
            this.Id = id;
            this.QuestionnaireId = questionnaireId;    
            this.LanguageId = languageId;            
            this.QuestionNumber = questionNumber;
            this.Question = question;
            this.ShowNote = showNote;
            this.NoteText = noteText;
            this.ChangeDate = changeDate;
        }

        #endregion

        #region Public Properties

        [IsId]
        public int Id { get; set; }
        
        public int QuestionnaireId { get; private set; }

        public int LanguageId { get; private set; }

        [NotNullAndEmpty]
        public string QuestionNumber { get; private set; }

        [NotNullAndEmpty]
        public string Question { get; private set; }
        
        public int ShowNote { get; private set; }

        [NotNullAndEmpty]
        public string NoteText { get; private set; }

        public DateTime ChangeDate { get; private set; }
        
       
        #endregion
    }
}