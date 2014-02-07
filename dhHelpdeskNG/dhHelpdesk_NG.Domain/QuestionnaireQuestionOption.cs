namespace DH.Helpdesk.Domain
{
    using global::System;

    public class QuestionnaireQuestionOption : Entity
    {
        public int OptionValue { get; set; }
        public int QuestionnaireQuestion_Id { get; set; }
        public int QuestionnaireQuestionOptionPos { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual QuestionnaireQuestion QuestionnaireQuestion { get; set; }
    }
}
