using System;
using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    using BusinessData.Models.Case;    
    using Infrastructure;
    using Domain.ExtendedCaseEntity;
    
    //NOTE: This is performance optimised class - pls do not use mappers!
    public sealed class ExtendedCaseFormRepository : RepositoryBase<ExtendedCaseFormEntity>, IExtendedCaseFormRepository
    {
        private readonly IExtendedCaseDataRepository _extendedCaseDataRepository;

        #region ctor()

        public ExtendedCaseFormRepository(
            IDatabaseFactory databaseFactory,
            IExtendedCaseDataRepository extendedCaseDataRepository)
            : base(databaseFactory)
        {
            _extendedCaseDataRepository = extendedCaseDataRepository;
        }

        #endregion

        public Guid CreateExtendedCaseData(int formId, string userGuid)
        {
            var newGuid = Guid.NewGuid();

            var extendedCaseData = new ExtendedCaseDataEntity
            {
                ExtendedCaseGuid = newGuid,
                ExtendedCaseFormId = formId,
                //TODO: THIS is temp
                CreatedBy = userGuid,
                CreatedOn = DateTime.Now
            };

            _extendedCaseDataRepository.AddEcd(extendedCaseData);

            return newGuid;
        }

        public ExtendedCaseDataOverview GetExtendedCaseFormForSolution(int caseSolutionId, int customerId)
        {
            var extendedFormData =
                (from cs in DataContext.CaseSolutions
                    from exCaseForm in cs.ExtendedCaseForms
                    where cs.Customer_Id == customerId &&
                          cs.Id == caseSolutionId
                    select new ExtendedCaseDataOverview
                    {
                        ExtendedCaseFormId = exCaseForm.Id,
                        ExtendedCaseFormName = exCaseForm.Name,
                        Version = exCaseForm.Version
                    })
                .FirstOrDefault();

            return extendedFormData;
        }

        public ExtendedCaseDataOverview GetCaseSectionExtendedCaseFormForSolution(int caseSolutionId, int customerId, int sectionType)
        {
            var extendedFormData =
                   (from cs in DataContext.CaseSolutions
                    from exCaseSec in cs.CaseSectionsExtendedCaseForm
                    where cs.Customer_Id == customerId &&
                          cs.Id == caseSolutionId &&
                          exCaseSec.CaseSection.SectionType == sectionType
                    let extendedCaseForm = exCaseSec.ExtendedCaseForm
                    select new ExtendedCaseDataOverview
                    {
                        CaseId = 0,
                        SectionType = sectionType,
                        ExtendedCaseFormId = exCaseSec.ExtendedCaseFormID,
                        ExtendedCaseFormName = extendedCaseForm.Name
                    })
                .FirstOrDefault();

            return extendedFormData;
        }

        public ExtendedCaseDataOverview GetExtendedCaseFormForCase(int caseId, int customerId)
        {
            var extendedFormData =
                   (from _case in DataContext.Cases
                    from sec in _case.CaseExtendedCaseDatas
                    where _case.Customer_Id == customerId &&
                          _case.Id == caseId
                    let extendedCaseForm = sec.ExtendedCaseData.ExtendedCaseForm
                    select new ExtendedCaseDataOverview
                    {
                        CaseId = caseId,
                        ExtendedCaseFormId = sec.ExtendedCaseData.ExtendedCaseForm.Id,
                        ExtendedCaseFormName = extendedCaseForm.Name,
                        ExtendedCaseGuid = sec.ExtendedCaseData.ExtendedCaseGuid
                    })
                .SingleOrDefault();

            return extendedFormData;
        }

        public ExtendedCaseDataOverview GetCaseSectionExtendedCaseFormForCase(int caseId, int customerId)
        {
           var extendedFormData =
                   (from _case in DataContext.Cases
                    from sec in _case.CaseSectionExtendedCaseDatas
                    where _case.Customer_Id == customerId &&
                          _case.Id == caseId
                    let extendedCaseForm = sec.ExtendedCaseData.ExtendedCaseForm
                    select new ExtendedCaseDataOverview
                    {
                        CaseId = caseId,
                        ExtendedCaseFormId = sec.ExtendedCaseData.ExtendedCaseForm.Id,
                        ExtendedCaseFormName = extendedCaseForm.Name,
                        ExtendedCaseGuid = sec.ExtendedCaseData.ExtendedCaseGuid,
                        SectionType = sec.CaseSection.SectionType
                    })
                .Single();

            return extendedFormData;
        }

        public List<ExtendedCaseDataOverview> GetExtendedCaseFormsForSections(int caseId, int customerId)
        {
            var extendedForm = 
                (from _case in DataContext.Cases
                 from sec in _case.CaseSectionExtendedCaseDatas
                 where _case.Customer_Id == customerId && _case.Id == caseId 
                 let extendedCaseForm = sec.ExtendedCaseData.ExtendedCaseForm
                 select new ExtendedCaseDataOverview
                 {
                    CaseId = caseId,
                    ExtendedCaseFormId = sec.ExtendedCaseData.ExtendedCaseForm.Id,
                    SectionType = sec.CaseSection.SectionType,
                    ExtendedCaseGuid = sec.ExtendedCaseData.ExtendedCaseGuid,
                    ExtendedCaseFormName = extendedCaseForm.Name
                 }).ToList();
            
            return extendedForm;
        }
    }
}
