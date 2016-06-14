namespace DH.Helpdesk.Domain
{
    using global::System;

    public class FinishingCauseCategory : Entity
    {
        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? FinishingCauseCategoryGUID { get; set; }
        
    }
}
