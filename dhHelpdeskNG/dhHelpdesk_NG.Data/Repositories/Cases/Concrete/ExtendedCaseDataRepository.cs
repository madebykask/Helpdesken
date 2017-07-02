using System.Linq;
using System;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    using BusinessData.Models.Case;    
    using Infrastructure;
    using Mappers;
    using Domain.ExtendedCaseEntity;
    

    public sealed class ExtendedCaseDataRepository : RepositoryBase<ExtendedCaseDataEntity>, IExtendedCaseDataRepository
    {        
        private readonly IEntityToBusinessModelMapper<ExtendedCaseDataEntity, ExtendedCaseDataModel> _extendedCaseDataToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<ExtendedCaseDataModel, ExtendedCaseDataEntity> _extendedCaseDataToEntityMapper;

        public ExtendedCaseDataRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<ExtendedCaseDataEntity, ExtendedCaseDataModel> extendedCaseDataToBusinessModelMapper,
            IBusinessModelToEntityMapper<ExtendedCaseDataModel, ExtendedCaseDataEntity> extendedCaseDataToEntityMapper)
            : base(databaseFactory)
        {
            _extendedCaseDataToBusinessModelMapper = extendedCaseDataToBusinessModelMapper;
            _extendedCaseDataToEntityMapper = extendedCaseDataToEntityMapper;
        }

        public ExtendedCaseDataModel CreateTemporaryExtendedCaseData(int formId, string creator)
        {
            var newGuid = Guid.NewGuid();

            var extendedCaseDataEntity = new ExtendedCaseDataEntity
            {
                ExtendedCaseGuid = newGuid,
                ExtendedCaseFormId = formId,                
                CreatedBy = creator,
                CreatedOn = DateTime.UtcNow
            };

            Add(extendedCaseDataEntity);
            Commit();

            return _extendedCaseDataToBusinessModelMapper.Map(extendedCaseDataEntity);
        }

        public void AddEcd(ExtendedCaseDataEntity e)
        {
            Add(e);
            Commit();
        }

        public ExtendedCaseDataEntity GetExtendedCaseData(Guid extendedCaseGuid)
        {
            return Table
                  .Where(c => c.ExtendedCaseGuid == extendedCaseGuid)
                  .Distinct()
                  .FirstOrDefault();
        }

        public ExtendedCaseDataModel GetExtendedCaseDataByCaseId(int caseId)
        {
            var case_ExtendedCaseEntity = DataContext.Case_ExtendedCases.Where(ce => ce.Case_Id == caseId).FirstOrDefault();
            if (case_ExtendedCaseEntity == null)
                return null;

            return _extendedCaseDataToBusinessModelMapper.Map(case_ExtendedCaseEntity.ExtendedCaseData);
        }
    }
}