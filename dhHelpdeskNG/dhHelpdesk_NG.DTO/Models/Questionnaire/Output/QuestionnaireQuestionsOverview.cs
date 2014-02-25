using DH.Helpdesk.Domain;

namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class QuestionnaireQuestionsOverview
    {
        #region Constructors and Destructors

        public QuestionnaireQuestionsOverview(int id, string questionNumber, string question, int showNote, string noteText, int languageId)
        {
            this.Id = id;
            this.QuestionNumber = questionNumber;
            this.Question = question;
            this.ShowNote = showNote;
            this.NoteText = noteText;
            this.LanguageId = languageId;           
        }

        #endregion

        #region Properties

        [IsId]
        public int Id { get; private set; }

        public string QuestionNumber { get; private set; }

        public string Question { get; private set; }

        public int ShowNote { get; private set; }

        public string NoteText { get; private set; }

        public int LanguageId { get; private set; }


        #endregion
    }
}