using System;

namespace DH.Helpdesk.Domain.MetaDataEntity
{
    public class EntityInfo: Entity
    {        
        public Guid EntityGuid { get; set; }
        
        public string EntityName { get; set; }
         
        public string EntityType { get; set; }

        public string EntityDescription { get; set; }

        public bool Translate { get; set; }

        public int? Source { get; set; }

    }
}
