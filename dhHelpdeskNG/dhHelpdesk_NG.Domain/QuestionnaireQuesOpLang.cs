namespace DH.Helpdesk.Domain
{
    using global::System;

    public class QuestionnaireQuesOpLang : Entity
    {
        public int Language_Id { get; set; }
        public int QuestionnaireQuestionOption_Id { get; set; }
        public string QuestionnaireQuestionOption { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
