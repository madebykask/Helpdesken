namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewQuestionnaireQuestion : INewBusinessModel
    {
        #region Constructors and Destructors

        public NewQuestionnaireQuestion(int questionnaireId, string questionNumber, string question, int showNote, string noteText, DateTime createDate)
        {
            this.QuestionnaireId = questionnaireId;
            this.QuestionNumber = questionNumber;
            this.Question = question;
            this.ShowNote = showNote;
            this.NoteText = noteText;
            this.CreatedDate = createDate;
        }

        #endregion

        #region Public Properties

        [IsId]
        public int Id { get; set; }
        
        public int QuestionnaireId { get; private set; }

        [NotNullAndEmpty]
        public string QuestionNumber { get; private set; }

        [NotNullAndEmpty]
        public string Question { get; private set; }

        public int ShowNote { get; private set; }

        [NotNullAndEmpty]
        public string NoteText { get; private set; }

        public DateTime CreatedDate { get; private set; }
        
       
        #endregion
    }
}