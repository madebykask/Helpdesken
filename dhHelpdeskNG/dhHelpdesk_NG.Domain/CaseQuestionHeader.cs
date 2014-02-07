namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class CaseQuestionHeader : Entity
    {
        public int Alternative { get; set; }
        public int Case_Id { get; set; }
        public int? ChangedByUser_Id { get; set; }
        public int SelectedAlternative { get; set; }
        public int VerificationAlternative { get; set; }
        public int Version { get; set; }
        public string AlternativeDescription { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime FinishingDate { get; set; }
        public Guid CaseQuestionHeaderGUID { get; set; }

        public virtual Case Case { get; set; }
        public virtual User ChangedByUser { get; set; }
        public virtual ICollection<CaseQuestionCategory> CaseQuestionCategories { get; set; }
    }
}
