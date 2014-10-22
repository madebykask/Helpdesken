using DH.Helpdesk.Domain.Interfaces;

namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.ComponentModel.DataAnnotations;

    public class Calendar : Entity, IWorkingGroupEntity, ICustomerEntity, IStartPageEntity, IDatedEntity
    {
        public int ChangedByUser_Id { get; set; }
        public int Customer_Id { get; set; }
        public int PublicInformation { get; set; }
        public int ShowOnStartPage { get; set; }
        public string Caption { get; set; }
        public string Text { get; set; }
        [DataType(DataType.Date)]
        public DateTime CalendarDate { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ShowUntilDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual User ChangedByUser { get; set; }
        public virtual ICollection<WorkingGroupEntity> WGs { get; set; }
    }
}
