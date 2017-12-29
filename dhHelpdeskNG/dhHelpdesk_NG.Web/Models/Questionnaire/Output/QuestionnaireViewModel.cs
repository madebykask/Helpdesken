namespace DH.Helpdesk.Web.Models.Questionnaire.Output
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class QuestionnaireViewModel
    {
        public QuestionnaireViewModel(QuestionnaireModel questionnaireModel, bool isAnonym, Guid guid, int customerId, int languageId)
        {
            this.QuestionnaireModel = questionnaireModel;
            this.IsAnonym = isAnonym;
            this.Guid = guid;
            CustomerId = customerId;
            LanguageId = languageId;
        }

        [NotNull]
        public QuestionnaireModel QuestionnaireModel { get; set; }

        public bool IsAnonym { get; set; }

        public Guid Guid { get; set; }

        public int CustomerId { get; set; }

        public int LanguageId { get; set; }
    }
}