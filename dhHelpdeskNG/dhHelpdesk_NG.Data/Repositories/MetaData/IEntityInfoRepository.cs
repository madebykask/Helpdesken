using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.MetaData;
using System;

namespace DH.Helpdesk.Dal.Repositories.MetaData
{
    public interface IEntityInfoRepository: IRepository<EntityInfo>
    {
        Guid? GetEntityInfoByName(string entityInfoName);
    }
}
