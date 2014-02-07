namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class CaseQuestionCategory : Entity
    {
        public int CaseQuestionHeader_Id { get; set; }
        public int? QuestionGroup_Id { get; set; }
        public int Weight { get; set; }
        public string Name { get; set; }
        public string Pos { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CaseQuestionCategoryGUID { get; set; }

        public virtual CaseQuestionHeader CaseQuestionHeader { get; set; }
        public virtual QuestionGroup QuestionGroup { get; set; }
        public virtual ICollection<CaseQuestion> CaseQuestions { get; set; }
    }
}
