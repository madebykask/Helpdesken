namespace DH.Helpdesk.Domain
{
    using global::System;

    public class QuestionnaireQuesLang: Entity
    {
        public int Language_Id { get; set; }
        public int QuestionnaireQuestion_Id { get; set; }
        public string NoteText { get; set; }
        public string QuestionnaireQuestion { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
