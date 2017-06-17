using System;
using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    using BusinessData.Models.Case;    
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;

    public sealed class ExtendedCaseFormRepository : RepositoryBase<ExtendedCaseFormEntity>, IExtendedCaseFormRepository
    {        
        private readonly IEntityToBusinessModelMapper<ExtendedCaseFormEntity, ExtendedCaseFormModel> _ExtendedCaseFormToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<ExtendedCaseFormModel, ExtendedCaseFormEntity> _ExtendedCaseFormToEntityMapper;
        private readonly ExtendedCaseDataRepository _extendedCaseDataRepository;

        public ExtendedCaseFormRepository(
            IDatabaseFactory databaseFactory,
            ExtendedCaseDataRepository extendedCaseDataRepository,
            IEntityToBusinessModelMapper<ExtendedCaseFormEntity, ExtendedCaseFormModel> ExtendedCaseFormToBusinessModelMapper,
            IBusinessModelToEntityMapper<ExtendedCaseFormModel, ExtendedCaseFormEntity> ExtendedCaseFormToEntityMapper)
            : base(databaseFactory)
        {
            _ExtendedCaseFormToBusinessModelMapper = ExtendedCaseFormToBusinessModelMapper;
            _ExtendedCaseFormToEntityMapper = ExtendedCaseFormToEntityMapper;
            _extendedCaseDataRepository = extendedCaseDataRepository;
    }

        private Guid CreateExtendedCaseData(int formId)
        {
            var newGuid = Guid.NewGuid();

            var extendedCaseData = new Helpdesk.Domain.ExtendedCaseEntity.ExtendedCaseDataEntity
            {
                ExtendedCaseGuid = newGuid,
                ExtendedCaseFormId = formId,
                //TODO:
                CreatedBy = "??",
                CreatedOn = System.DateTime.Now
            };
            
            _extendedCaseDataRepository.AddEcd(extendedCaseData);

            return newGuid;
        }

        public IList<ExtendedCaseFormModel> GetExtendedCaseForm(int caseSolutionId, int customerId, int caseId, int userLanguageId, string userGuid, int caseStateSecondaryId, int caseWorkingGroupId, string extendedCasePath)
        {
            ////TODO: 
            ///Cache this!
            ////REFACTOR! No logic in repository
            IList<ExtendedCaseFormModel> extendedForm;

            if (caseSolutionId == 0)
                return null;

            //if no path is specified in GlobalSetting
            if (string.IsNullOrEmpty(extendedCasePath))
                return null;

            var caseSolution = this.DataContext.CaseSolutions.Where(c => c.Customer_Id == customerId && c.Id == caseSolutionId).FirstOrDefault();

            if (caseSolution == null)
                return null;

            extendedCasePath = extendedCasePath
            .Replace("&languageId=[LanguageId]", "") //sent in by js function
            .Replace("&extendedCaseGuid=[extendedCaseGuid]", "")//sent in by js function
            .Replace("[CaseStateSecondaryId]", caseStateSecondaryId.ToString())
            .Replace("[CaseWorkingGroupId]", caseWorkingGroupId.ToString())
            .Replace("[UserGuid]", userGuid)
            .Replace("[CustomerId]", customerId.ToString());

            if (caseId == 0)
            {
                extendedForm = caseSolution.ExtendedCaseForms.Select(x => new ExtendedCaseFormModel
                {
                    CaseId = caseId,
                    Id = x.Id,
                    ExtendedCaseGuid = CreateExtendedCaseData(x.Id),
                    Path = extendedCasePath,
                    Name = (x.Name != null ? x.Name : caseSolution.Name),
                    LanguageId = userLanguageId,
                    //CaseStatus = caseStateSecondaryId, /Sent by querystring at the moment
                    //UserRole = caseWorkingGroupId/Sent by querystring at the moment
                    //UserGuid
                }).Take(1).ToList();
            }
            else
            {
                extendedForm = this.DataContext.Cases.Where(c => c.Customer_Id == customerId && c.Id == caseId).FirstOrDefault().ExtendedCaseDatas.Select(x => new ExtendedCaseFormModel
                {
                    CaseId = caseId,
                    Id = x.ExtendedCaseForm.Id,
                    ExtendedCaseGuid = x.ExtendedCaseGuid,
                    Path = extendedCasePath,
                    LanguageId = userLanguageId,
                    //CaseStatus = caseStateSecondaryId,/Sent by querystring at the moment
                    //UserRole = caseWorkingGroupId,/Sent by querystring at the moment
                    Name = (x.ExtendedCaseForm.Name != null ? x.ExtendedCaseForm.Name : caseSolution.Name),
                    //UserGuid
                }).Take(1).ToList();
            }

            return extendedForm;
        }
    }
}
