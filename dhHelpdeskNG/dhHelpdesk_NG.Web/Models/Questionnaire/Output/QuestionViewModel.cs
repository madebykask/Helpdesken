namespace DH.Helpdesk.Web.Models.Questionnaire.Output
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class AnswersViewModel
    {
        public Guid Guid { get; set; }

        public bool IsAnonym { get; set; }

        [NotNull]
        public List<QuestionInputModel> Questions { get; set; }

        public int CustomerId { get; set; }

        public int LanguageId { get; set; }
    }
}