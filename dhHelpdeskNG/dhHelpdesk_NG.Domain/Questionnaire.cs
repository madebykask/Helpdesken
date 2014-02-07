namespace DH.Helpdesk.Domain
{
    using global::System;

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
