using DH.Helpdesk.Domain.Interfaces;

namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.ComponentModel.DataAnnotations;

    public class BulletinBoard : Entity, IWorkingGroupEntity, IDatedEntity, ICustomerEntity, IStartPageEntity
    {
        public BulletinBoard()
        {
            this.WGs = new List<WorkingGroupEntity>();
        }

        public int Customer_Id { get; set; }
        public int PublicInformation { get; set; }
        public int ShowOnStartPage { get; set; }
        [Required] 
        public string Text { get; set; }
        [DataType(DataType.Date)]
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required] 
        [DataType(DataType.Date)]
        public DateTime? ShowDate { get; set; }
        [Required] 
        [DataType(DataType.Date)]
        public DateTime? ShowUntilDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<WorkingGroupEntity> WGs { get; set; }
    }
}
