namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class FAQ : Entity
    {
        public int? Customer_Id { get; set; }
        public int FAQCategory_Id { get; set; }
        public int? WorkingGroup_Id { get; set; }
        public string Answer { get; set; }
        public string Answer_Internal { get; set; }
        public string FAQQuery { get; set; }
        public string URL1 { get; set; }
        public string URL2 { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual FAQCategory FAQCategory { get; set; }
        public virtual WorkingGroupEntity WorkingGroup { get; set; }
        public virtual ICollection<FAQFile> FAQFiles { get; set; }

        public int ShowOnStartPage { get; set; }

        public int InformationIsAvailableForNotifiers { get; set; }
    }
}
