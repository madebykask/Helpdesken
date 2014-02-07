namespace DH.Helpdesk.Domain
{
    using global::System;

    public class QuestionCategory : Entity
    {
        public int Customer_Id { get; set; }
        public int Parent_QuestionCategory_Id { get; set; }
        public int QuestionGroup_Id { get; set; }
        public int Weight { get; set; }
        public string Name { get; set; }
        public string Pos { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual QuestionGroup QuestionGroup { get; set; }
    }
}
