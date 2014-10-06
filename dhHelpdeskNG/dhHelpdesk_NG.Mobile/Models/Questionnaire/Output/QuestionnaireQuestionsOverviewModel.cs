namespace DH.Helpdesk.Mobile.Models.Questionnaire.Output
{
   
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public sealed class QuestionnaireQuestionsOverviewModel
    {

        #region Constructors and Destructors

        public QuestionnaireQuestionsOverviewModel(int id, string questionNumber, string question, int languageId)
        {
            this.Id = id;
            this.QuestionNumber = questionNumber;
            this.Question = question;
            this.LanguageId = languageId;
        }

        #endregion

        #region Public Properties

        [IsId]
        public int Id { get; set; }

        [LocalizedDisplay("QuestionNumber")]
        public string QuestionNumber { get; set; }

        [LocalizedDisplay("Question")]
        public string Question { get; set; }

        public int LanguageId { get; set; }

        #endregion
    }

}