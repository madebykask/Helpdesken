using System.Collections.Generic;
using System.Linq;
using System;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    using BusinessData.Models.Case;    
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;
    

    public sealed class ExtendedCaseDataRepository : RepositoryBase<ExtendedCaseDataEntity>, IExtendedCaseDataRepository
    {        
        private readonly IEntityToBusinessModelMapper<ExtendedCaseDataEntity, ExtendedCaseDataModel> _ExtendedCaseDataToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<ExtendedCaseDataModel, ExtendedCaseDataEntity> _ExtendedCaseDataToEntityMapper;

        public ExtendedCaseDataRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<ExtendedCaseDataEntity, ExtendedCaseDataModel> ExtendedCaseDataToBusinessModelMapper,
            IBusinessModelToEntityMapper<ExtendedCaseDataModel, ExtendedCaseDataEntity> ExtendedCaseDataToEntityMapper)
            : base(databaseFactory)
        {
            _ExtendedCaseDataToBusinessModelMapper = ExtendedCaseDataToBusinessModelMapper;
            _ExtendedCaseDataToEntityMapper = ExtendedCaseDataToEntityMapper;
        }

        public void AddEcd(ExtendedCaseDataEntity e)
        {
            this.Add(e);
            this.Commit();
        }

        public ExtendedCaseDataEntity GetExtendedCaseData(Guid extendedCaseGuid)
        {

            return this.Table
                  .Where(c => c.ExtendedCaseGuid == extendedCaseGuid)
                  .Distinct()
                  .FirstOrDefault();
        }
    }
}