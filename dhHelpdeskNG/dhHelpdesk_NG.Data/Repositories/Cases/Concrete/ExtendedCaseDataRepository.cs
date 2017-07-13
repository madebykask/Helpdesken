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

        public ExtendedCaseDataModel GetExtendedCaseDataByCaseId(int caseId)
        {
            var case_ExtendedCaseEntity = DataContext.Case_ExtendedCases.Where(ce => ce.Case_Id == caseId).FirstOrDefault();
            if (case_ExtendedCaseEntity == null)
                return null;

            return _extendedCaseDataToBusinessModelMapper.Map(case_ExtendedCaseEntity.ExtendedCaseData);
        }

        public ExtendedCaseDataModel CopyExtendedCaseToCase(int extendedCaseDataID, int caseID, int userID)
        {
            var copyFromExtendedCaseData = DataContext.ExtendedCaseDatas.Where(o => o.Id == extendedCaseDataID).Single();

			var now = DateTime.UtcNow;

			var user = _userRepository.GetById(userID);

			var newExtendedCaseData = new ExtendedCaseDataEntity
			{
				ExtendedCaseGuid = Guid.NewGuid(),
				ExtendedCaseFormId = copyFromExtendedCaseData.ExtendedCaseFormId,
				CreatedOn = now,
				CreatedBy = user.UserID
			};

			var newCaseExtendedCase = new Case_ExtendedCaseEntity
			{
				Case_Id = caseID
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

			DataContext.SaveChanges();

			var model = _extendedCaseDataToBusinessModelMapper.Map(newExtendedCaseData);

			return model;
        }
    }
}