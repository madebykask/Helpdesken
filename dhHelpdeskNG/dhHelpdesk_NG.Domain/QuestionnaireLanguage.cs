using System;

namespace dhHelpdesk_NG.Domain
{
    public class QuestionnaireLanguage
    {
        public int Language_Id { get; set; }
        public int Questionnaire_Id { get; set; }
        public string QuestionnaireDescription { get; set; }
        public string QuestionnaireName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
