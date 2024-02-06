namespace DH.Helpdesk.Domain
{
    using global::System;

    public class Status : Entity
    {
        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public int IsDefault { get; set; }
        public int? WorkingGroup_Id { get; set; }
        public int? StateSecondary_Id { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid StatusGUID { get; set; }
        public bool SplitOnSave { get; set; }
        public bool SplitOnNext { get; set; }


        public virtual WorkingGroupEntity WorkingGroup { get; set; }
        public virtual StateSecondary StateSecondary { get; set; }
    }
}
