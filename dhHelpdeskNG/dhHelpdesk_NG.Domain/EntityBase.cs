using System;

namespace DH.Helpdesk.Domain
{
    /// <summary>
    /// The entity.
    /// </summary>
    public abstract class EntityBase: Entity
    {
        public int Status { get; set; }
        public int SortOrder { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public int? CreatedByUser_Id { get; set; }
        public DateTime ChangedDate { get; set; }
        public int? ChangedByUser_Id { get; set; }
    }

}
