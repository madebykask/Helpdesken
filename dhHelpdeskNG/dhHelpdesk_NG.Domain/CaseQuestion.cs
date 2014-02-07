namespace DH.Helpdesk.Domain
{
    using global::System;

    public class CaseQuestion : Entity
    {
        public decimal Weight { get; set; }
        public int Answer { get; set; }
        public int CaseQuestionCategory_Id { get; set; }
        public string Note { get; set; }
        public string Question { get; set; }
        public string QuestionHelp { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual CaseQuestionCategory CaseQuestionCategory { get; set; }
    }
}
