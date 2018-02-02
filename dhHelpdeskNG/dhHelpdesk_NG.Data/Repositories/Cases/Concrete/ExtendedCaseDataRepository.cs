using System.Linq;
using System;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    using BusinessData.Models.Case;
    using Infrastructure;
    using Mappers;
    using Domain.ExtendedCaseEntity;    
    using System.Collections.ObjectModel;

    public sealed class ExtendedCaseDataRepository : RepositoryBase<ExtendedCaseDataEntity>, IExtendedCaseDataRepository
    {        
        private readonly IEntityToBusinessModelMapper<ExtendedCaseDataEntity, ExtendedCaseDataModel> _extendedCaseDataToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<ExtendedCaseDataModel, ExtendedCaseDataEntity> _extendedCaseDataToEntityMapper;
		private readonly IUserRepository _userRepository;
		private readonly IExtendedCaseValueRepository _extendedCaseValueRepository;

		public ExtendedCaseDataRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<ExtendedCaseDataEntity, ExtendedCaseDataModel> extendedCaseDataToBusinessModelMapper,
            IBusinessModelToEntityMapper<ExtendedCaseDataModel, ExtendedCaseDataEntity> extendedCaseDataToEntityMapper,
			IUserRepository userRepository,
			IExtendedCaseValueRepository extendedCaseValueRepository)
            : base(databaseFactory)
        {
            _extendedCaseDataToBusinessModelMapper = extendedCaseDataToBusinessModelMapper;
            _extendedCaseDataToEntityMapper = extendedCaseDataToEntityMapper;
			_userRepository = userRepository;
			_extendedCaseValueRepository = extendedCaseValueRepository;
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

        public int GetCaseIdByExtendedCaseGuid(Guid uniqueId)
        {
            var query = from c_cd in DataContext.Case_ExtendedCases
                where c_cd.ExtendedCaseData.ExtendedCaseGuid == uniqueId
                select c_cd.Case_Id;

            var caseId = query.FirstOrDefault();
            return caseId;
        }

        public ExtendedCaseDataModel GetExtendedCaseDataByCaseId(int caseId)
        {
            var case_ExtendedCaseEntity = DataContext.Case_ExtendedCases.Where(ce => ce.Case_Id == caseId).FirstOrDefault();
            if (case_ExtendedCaseEntity == null)
                return null;

            return _extendedCaseDataToBusinessModelMapper.Map(case_ExtendedCaseEntity.ExtendedCaseData);
        }

        public ExtendedCaseDataModel CopyExtendedCaseToCase(int extendedCaseDataID, int caseID, string userID, int? extendedCaseFormId = null)
        {
            var copyFromExtendedCaseData = DataContext.ExtendedCaseDatas.Where(o => o.Id == extendedCaseDataID).FirstOrDefault();

            if (copyFromExtendedCaseData == null)
                return null;

            var now = DateTime.UtcNow;
            var newExtendedCaseData = new ExtendedCaseDataEntity
            {
                ExtendedCaseGuid = Guid.NewGuid(),
                ExtendedCaseFormId = (extendedCaseFormId.HasValue ? extendedCaseFormId.Value : copyFromExtendedCaseData.ExtendedCaseFormId),
                CreatedOn = now,
                CreatedBy = userID,
                ExtendedCaseValues = new Collection<ExtendedCaseValueEntity>()
            };

            DataContext.ExtendedCaseDatas.Add(newExtendedCaseData);
            var copyFromExtendedCaseValues = DataContext.ExtendedCaseValues.Where(o => o.ExtendedCaseDataId == extendedCaseDataID);

            foreach (var copyValue in copyFromExtendedCaseValues)
            {
                var newValue = new ExtendedCaseValueEntity
                {
                    FieldId = copyValue.FieldId,
                    Value = copyValue.Value,
                    SecondaryValue = copyValue.SecondaryValue
                };

                newExtendedCaseData.ExtendedCaseValues.Add(newValue);
            }

            var newCaseExtendedCase = new Case_ExtendedCaseEntity
            {
                Case_Id = caseID,
                ExtendedCaseData_Id = newExtendedCaseData.Id,
                ExtendedCaseForm_Id = extendedCaseFormId
            };

            DataContext.Case_ExtendedCases.Add(newCaseExtendedCase);
            DataContext.SaveChanges();

            var model = _extendedCaseDataToBusinessModelMapper.Map(newExtendedCaseData);
            return model;
        }

		public ExtendedCaseDataEntity GetExtendedCaseFromCase(int caseID)
		{
			var extendedCaseDataID = DataContext.Case_ExtendedCases.SingleOrDefault(o => o.Case_Id == caseID)?.ExtendedCaseData_Id;

			ExtendedCaseDataEntity dataEntity;
			if (extendedCaseDataID.HasValue)
			{
				dataEntity = DataContext.ExtendedCaseDatas.Single(o => o.Id == extendedCaseDataID.Value);
			}
			else
			{
				dataEntity = null;
			}

			return dataEntity;
		}
	}
}