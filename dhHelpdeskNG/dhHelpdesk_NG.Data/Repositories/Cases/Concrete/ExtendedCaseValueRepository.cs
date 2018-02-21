using System.Collections.Generic;
using System.Linq;
using System;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    using BusinessData.Models.Case;    
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;
    

    public sealed class ExtendedCaseValueRepository : RepositoryBase<ExtendedCaseValueEntity>, IExtendedCaseValueRepository
    {        
        private readonly IEntityToBusinessModelMapper<ExtendedCaseValueEntity, ExtendedCaseValueModel> _ExtendedCaseValueToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<ExtendedCaseValueModel, ExtendedCaseValueEntity> _ExtendedCaseValueToEntityMapper;

        public ExtendedCaseValueRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<ExtendedCaseValueEntity, ExtendedCaseValueModel> ExtendedCaseValueToBusinessModelMapper,
            IBusinessModelToEntityMapper<ExtendedCaseValueModel, ExtendedCaseValueEntity> ExtendedCaseValueToEntityMapper)
            : base(databaseFactory)
        {
            _ExtendedCaseValueToBusinessModelMapper = ExtendedCaseValueToBusinessModelMapper;
            _ExtendedCaseValueToEntityMapper = ExtendedCaseValueToEntityMapper;
        }

        public ExtendedCaseValueEntity GetExtendedCaseValue(int extendedCaseDataId, string fieldId)
        {
            return this.Table
                  .Where(c => c.ExtendedCaseDataId == extendedCaseDataId && c.FieldId.ToLower() == fieldId.ToLower()) 
                  .Distinct()
                  .FirstOrDefault();
        }
    }
}