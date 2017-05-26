using DH.Helpdesk.Domain.Interfaces;

namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Accounts;

    using global::System;
    using global::System.Collections.Generic;

    public class Document : Entity, IWorkingGroupEntity, IUserEntity, ICustomerEntity, IDatedEntity, IStartPageEntity
    {
        public Document()
        {
            this.Us = new List<User>();
            this.WGs = new List<WorkingGroupEntity>();
            this.AccountActivities = new List<AccountActivity>();
            this.OrderTypes = new List<OrderType>();
            this.Links = new List<Link>();
        }

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
        public int ShowOnStartPage { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual DocumentCategory DocumentCategory { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User ChangedByUser { get; set; }
        public virtual ICollection<User> Us { get; set; }     
        public virtual ICollection<WorkingGroupEntity> WGs { get; set; }

        public virtual ICollection<AccountActivity> AccountActivities { get; set; } 

        public virtual ICollection<OrderType> OrderTypes { get; set; } 

        public virtual ICollection<Link> Links { get; set; }
    }
}
