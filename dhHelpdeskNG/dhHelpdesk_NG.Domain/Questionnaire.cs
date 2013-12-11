using System;

namespace dhHelpdesk_NG.Domain
{
    public class Questionnaire : Entity
    {
        public int Customer_Id { get; set; }
        public string QuestionnaireDescription { get; set; }
        public string QuestionnaireName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
