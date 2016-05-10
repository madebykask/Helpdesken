using DH.Helpdesk.Domain.Interfaces;

namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.ComponentModel.DataAnnotations;

    public class OperationLog : Entity, IWorkingGroupEntity, IUserEntity, IStartPageEntity, ICustomerEntity, IDatedEntity
    {
        public OperationLog()
        {
            this.Us = new List<User>();
            this.WGs = new List<WorkingGroupEntity>();
            this.EmailLogs = new List<OperationLogEMailLog>();            
        }

        public int Customer_Id { get; set; }
        public int InformUsers { get; set; }
        public int? OperationLogCategory_Id { get; set; }
        public int OperationObject_Id { get; set; }
        public int Popup { get; set; }
        public int PublicInformation { get; set; }
        public int ShowOnStartPage { get; set; }
        public int User_Id { get; set; }
        public int WorkingTime { get; set; }
        public string LogAction { get; set; }
        public string LogText { get; set; }
        public string LogTextExternal { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ShowDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ShowUntilDate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual User Admin { get; set; }
        public virtual ICollection<User> Us { get; set; }
        public virtual ICollection<WorkingGroupEntity> WGs { get; set; }
        public virtual OperationLogCategory Category { get; set; }        
        public virtual ICollection<OperationLogEMailLog> EmailLogs { get; set; }

        public virtual OperationObject Object { get; set; }
    }
}
