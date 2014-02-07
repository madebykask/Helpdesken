namespace DH.Helpdesk.Domain
{
    using global::System;

    public class Country : Entity
    {
        public Country()
        {
            this.IsActive = 1;
        }

        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public int IsDefault { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SyncChangedDate { get; set; }
    }
}
