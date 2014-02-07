namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class Document : Entity
    {
        public byte[] File { get; set; }
        public int? ChangedByUser_Id { get; set; }
        public int? CreatedByUser_Id { get; set; }
        public int Customer_Id { get; set; }
        public int? DocumentCategory_Id { get; set; }
        public int Size { get; set; }
        public string ContentType { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual DocumentCategory DocumentCategory { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User ChangedByUser { get; set; }
        public virtual ICollection<User> Us { get; set; }     
        public virtual ICollection<WorkingGroupEntity> WGs { get; set; }
    }
}
