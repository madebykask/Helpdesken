namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;    
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;
    using DH.Helpdesk.BusinessData.Models.Case;
    using System;

    public interface IExtendedCaseDataRepository : IRepository<ExtendedCaseDataEntity>
    {
      void AddEcd(ExtendedCaseDataEntity e);
       ExtendedCaseDataEntity GetExtendedCaseData(Guid extendedCaseGuid);
    }
}