using System;

namespace DH.Helpdesk.Domain.MetaData
{
    public class EntityRelationship: Entity
    {
        public Guid ParentEntity_Guid { get; set; }

        public Guid ChildEntity_Guid { get; set; }

        public Guid ParentItem_Guid { get; set; }

        public Guid ChildItem_Guid { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }       
        
    }
}
