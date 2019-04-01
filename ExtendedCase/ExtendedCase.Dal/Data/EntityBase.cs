using System;

namespace ExtendedCase.Dal.Data
{
    public abstract class EntityBase
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}