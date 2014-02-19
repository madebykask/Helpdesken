namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using System.Collections.Generic;

    public sealed class QuestionnaireQuestionsOverview
    {
        #region Constructors and Destructors

        public QuestionnaireQuestionsOverview(int id, string questionNumber, string question, int languageId)
        {
            this.Id = id;
            this.QuestionNumber = questionNumber;
            this.Question = question;
            this.LanguageId = languageId;
        }

        #endregion

        #region Properties

        [IsId]
        public int Id { get; private set; }

        public string QuestionNumber { get; private set; }

        public string Question { get; private set; }

        public int LanguageId { get; set; }

        #endregion
    }
}