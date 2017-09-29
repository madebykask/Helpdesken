using System;
using DH.Helpdesk.Dal.Infrastructure;
using System.Linq;
using DH.Helpdesk.Domain.MetaData;

namespace DH.Helpdesk.Dal.Repositories.MetaData.Concrete
{
    public static class EntityInfoType
    {
        public static string LineManager_Coworkers = "LineManager_Coworkers_Relationship";
    } 

    public class EntityInfoRepository : RepositoryBase<EntityInfo>, IEntityInfoRepository
    {
        
        public EntityInfoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public Guid? GetEntityInfoByName(string entityInfoName)
        {
            var entity = GetMany(ei => ei.EntityName.Equals(entityInfoName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (entity == null)
                return null;
            
            return entity.EntityGuid;
        }
    }
}
