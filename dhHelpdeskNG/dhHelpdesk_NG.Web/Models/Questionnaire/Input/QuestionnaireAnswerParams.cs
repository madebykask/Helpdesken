using System;

namespace DH.Helpdesk.Web.Models.Questionnaire.Input
{
    public class QuestionnaireAnswerParams
    {
        public Guid Guid { get; set; }
        public int LanguageId { get; set; }
        public int CustomerId { get; set; }
        public int OptionId { get; set; }
    }
}