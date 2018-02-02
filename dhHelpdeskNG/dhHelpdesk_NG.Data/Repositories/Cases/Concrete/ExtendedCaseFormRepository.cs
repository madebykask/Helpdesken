using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    using BusinessData.Models.Case;    
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;
    using DH.Helpdesk.Common.Enums;

	using Common.Enums.Cases;

	public sealed class ExtendedCaseFormRepository : RepositoryBase<ExtendedCaseFormEntity>, IExtendedCaseFormRepository
    {
        private readonly IEntityToBusinessModelMapper<ExtendedCaseFormEntity, ExtendedCaseFormModel> _extendedCaseFormToBusinessModelMapper;
        private readonly IBusinessModelToEntityMapper<ExtendedCaseFormModel, ExtendedCaseFormEntity> _extendedCaseFormToEntityMapper;
        private readonly ExtendedCaseDataRepository _extendedCaseDataRepository;

        public ExtendedCaseFormRepository(
            IDatabaseFactory databaseFactory,
            ExtendedCaseDataRepository extendedCaseDataRepository,
            IEntityToBusinessModelMapper<ExtendedCaseFormEntity, ExtendedCaseFormModel> extendedCaseFormToBusinessModelMapper,
            IBusinessModelToEntityMapper<ExtendedCaseFormModel, ExtendedCaseFormEntity> extendedCaseFormToEntityMapper)
            : base(databaseFactory)
        {
            _extendedCaseFormToBusinessModelMapper = extendedCaseFormToBusinessModelMapper;
            _extendedCaseFormToEntityMapper = extendedCaseFormToEntityMapper;
            _extendedCaseDataRepository = extendedCaseDataRepository;
        }

        private Guid CreateExtendedCaseData(int formId, string userGuid)
        {
            var newGuid = Guid.NewGuid();

            var extendedCaseData = new Helpdesk.Domain.ExtendedCaseEntity.ExtendedCaseDataEntity
            {
                ExtendedCaseGuid = newGuid,
                ExtendedCaseFormId = formId,
                //TODO: THIS is temp
                CreatedBy = userGuid,
                CreatedOn = System.DateTime.Now
            };

            _extendedCaseDataRepository.AddEcd(extendedCaseData);

            return newGuid;
        }

        public IList<ExtendedCaseFormModel> GetExtendedCaseForm(int caseSolutionId, int customerId,
                                                                int caseId, int userLanguageId, string userGuid,
                                                                int caseStateSecondaryId, int caseWorkingGroupId,
                                                                string extendedCasePath, int? userId, string userName,
                                                                ApplicationType applicationType, int userWorkingGroupId)
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

            if (caseSolutionId > 0)
            {
                //fallback to casesolution.StateSecondaryId if case is new
                if (caseStateSecondaryId == 0 && caseSolution.StateSecondary_Id != null && caseSolution.StateSecondary_Id != 0)
                {
                    caseStateSecondaryId = this.DataContext.StateSecondaries.Where(x => x.Id == caseSolution.StateSecondary_Id).FirstOrDefault().StateSecondaryId;
                }

                ////fallback to casesolution.StateSecondaryId if case is new
                //if (caseWorkingGroupId == 0 && caseSolution.CaseWorkingGroup_Id != null && caseSolution.CaseWorkingGroup_Id != 0)
                //{
                //    caseWorkingGroupId = this.DataContext.WorkingGroups.Where(x => x.Id == caseSolution.CaseWorkingGroup_Id).FirstOrDefault().WorkingGroupId;
                //}
            }



            extendedCasePath = extendedCasePath
            .Replace("&languageId=[LanguageId]", "") //sent in by js function
            .Replace("&extendedCaseGuid=[extendedCaseGuid]", "")//sent in by js function
            .Replace("[CaseStateSecondaryId]", caseStateSecondaryId.ToString())
            .Replace("[CaseWorkingGroupId]", userWorkingGroupId.ToString()) //NOTE, this is from now on userWorkingGroupId. 
            .Replace("[UserGuid]", userGuid)
            .Replace("[CustomerId]", customerId.ToString());

            if (caseId == 0)
            {
                extendedForm = caseSolution.ExtendedCaseForms.Select(x => new ExtendedCaseFormModel
                {
                    CaseId = caseId,
                    Id = x.Id,
                    ExtendedCaseGuid = CreateExtendedCaseData(x.Id, userGuid),
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
                extendedForm = this.DataContext.Cases.Where(c => c.Customer_Id == customerId && c.Id == caseId).FirstOrDefault().CaseExtendedCaseDatas.Select(x => new ExtendedCaseFormModel
                {
                    CaseId = caseId,
                    Id = x.ExtendedCaseData.ExtendedCaseForm.Id,
                    ExtendedCaseGuid = x.ExtendedCaseData.ExtendedCaseGuid,
                    Path = extendedCasePath,
                    LanguageId = userLanguageId,
                    //CaseStatus = caseStateSecondaryId,/Sent by querystring at the moment
                    //UserRole = caseWorkingGroupId,/Sent by querystring at the moment
                    Name = (x.ExtendedCaseData.ExtendedCaseForm?.Name != null ? x.ExtendedCaseData.ExtendedCaseForm.Name : caseSolution.Name),
                    //UserGuid
                }).Take(1).ToList();
            }

            return extendedForm;
        }

		public ExtendedCaseFormModel GetExtendedCaseSectionForm(int caseSolutionId, int customerId,
															   int caseId, int caseSectionType, int userLanguageId, string userGuid,
															   int caseStateSecondaryId, int caseWorkingGroupId,
															   string extendedCasePath, int userWorkingGroupId)
		{
			////TODO: 
			///Cache this!
			////REFACTOR! No logic in repository
			ExtendedCaseFormModel extendedForm;

			if (caseSolutionId == 0)
				return null;

			//if no path is specified in GlobalSetting
			if (string.IsNullOrEmpty(extendedCasePath))
				return null;

			var caseSolution = this.DataContext.CaseSolutions.Include(o => o.CaseSectionsExtendedCaseForm.Select(p => p.CaseSection))
				.Include(o => o.CaseSectionsExtendedCaseForm.Select(p => p.ExtendedCaseForm))
				.Where(c => c.Customer_Id == customerId && c.Id == caseSolutionId)
				.FirstOrDefault();

			if (caseSolution == null)
				return null;

			if (caseSolutionId > 0)
			{
				//fallback to casesolution.StateSecondaryId if case is new
				if (caseStateSecondaryId == 0 && caseSolution.StateSecondary_Id != null && caseSolution.StateSecondary_Id != 0)
				{
					caseStateSecondaryId = this.DataContext.StateSecondaries.Where(x => x.Id == caseSolution.StateSecondary_Id).FirstOrDefault().StateSecondaryId;
				}
			}

			extendedCasePath = extendedCasePath
				.Replace("&languageId=[LanguageId]", "") //sent in by js function
				.Replace("&extendedCaseGuid=[extendedCaseGuid]", "")//sent in by js function
				.Replace("[CaseStateSecondaryId]", caseStateSecondaryId.ToString())
				.Replace("[CaseWorkingGroupId]", userWorkingGroupId.ToString()) //NOTE, this is from now on userWorkingGroupId. 
				.Replace("[UserGuid]", userGuid)
				.Replace("[CustomerId]", customerId.ToString());

			if (caseId == 0)
			{
				var ext = caseSolution.CaseSectionsExtendedCaseForm.FirstOrDefault(o => o.CaseSection.SectionType == caseSectionType).ExtendedCaseForm;

				extendedCasePath = extendedCasePath.Replace("[ExtendedCaseFormId]", ext.Id.ToString());

				extendedCasePath = extendedCasePath + "&autoLoad=true";

				extendedForm = new ExtendedCaseFormModel
				{
					CaseId = caseId,
					Id = ext.Id,
					ExtendedCaseGuid = CreateExtendedCaseData(ext.Id, userGuid),
					Path = extendedCasePath,
					Name = (ext.Name != null ? ext.Name : caseSolution.Name),
					LanguageId = userLanguageId,
					//CaseStatus = caseStateSecondaryId, /Sent by querystring at the moment
					//UserRole = caseWorkingGroupId/Sent by querystring at the moment
					//UserGuid
				};
			}
			else
			{
				extendedForm = this.DataContext.Cases.Where(c => c.Customer_Id == customerId && c.Id == caseId).FirstOrDefault().CaseSectionExtendedCaseDatas.Select(x =>
							new ExtendedCaseFormModel
							{
								CaseId = caseId,
								Id = x.ExtendedCaseData.ExtendedCaseForm.Id,
								ExtendedCaseGuid = x.ExtendedCaseData.ExtendedCaseGuid,
								Path = extendedCasePath,
								LanguageId = userLanguageId,
								//CaseStatus = caseStateSecondaryId,/Sent by querystring at the moment
								//UserRole = caseWorkingGroupId,/Sent by querystring at the moment
								Name = (x.ExtendedCaseData.ExtendedCaseForm?.Name != null ? x.ExtendedCaseData.ExtendedCaseForm.Name : caseSolution.Name),
								//UserGuid
							}
					).Single();
			}

			return extendedForm;
		}


		public ExtendedCaseFormModel GetExtendedCaseSectionForm(int caseID, int customerID, CaseSectionType caseSection, int userLanguageId, string userGuid, int caseStateSecondaryId, int caseWorkingGroupId, int userWorkingGroupId, string extendedCasePath)
		{
			var caseX = this.DataContext.Cases
				.Include(o => o.CaseSectionExtendedCaseDatas.Select(p => p.CaseSection))
				.Include(o => o.CaseSectionExtendedCaseDatas.Select(p => p.ExtendedCaseData.ExtendedCaseForm))
				.Where(c => c.Customer_Id == customerID && c.Id == caseID)
				.FirstOrDefault();

			extendedCasePath = extendedCasePath
				.Replace("&languageId=[LanguageId]", "") //sent in by js function
				.Replace("&extendedCaseGuid=[extendedCaseGuid]", "")//sent in by js function
				.Replace("[CaseStateSecondaryId]", caseStateSecondaryId.ToString())
				.Replace("[CaseWorkingGroupId]", userWorkingGroupId.ToString()) //NOTE, this is from now on userWorkingGroupId. 
				.Replace("[UserGuid]", userGuid)
				.Replace("[CustomerId]", customerID.ToString());

			var extendedForm = caseX.CaseSectionExtendedCaseDatas
				.Where(o => o.CaseSection.SectionType == (int)caseSection)
				.Select(x =>
					new ExtendedCaseFormModel
					{
						CaseId = caseID,
						Id = x.ExtendedCaseData.ExtendedCaseForm.Id,
						ExtendedCaseGuid = x.ExtendedCaseData.ExtendedCaseGuid,
						Path = extendedCasePath,
						LanguageId = userLanguageId,
						//CaseStatus = caseStateSecondaryId,/Sent by querystring at the moment
						//UserRole = caseWorkingGroupId,/Sent by querystring at the moment
						Name = (x.ExtendedCaseData.ExtendedCaseForm?.Name != null ? x.ExtendedCaseData.ExtendedCaseForm.Name : ""),
						//UserGuid
					}).SingleOrDefault();

			if (extendedForm != null)
			{
				extendedForm.Path = extendedForm.Path.Replace("[ExtendedCaseFormId]", extendedForm.Id.ToString());
				extendedForm.Path += "&autoLoad=true";
			}
			return extendedForm;
		}


		public ExtendedCaseFormModel GetExtendedCaseFormForCaseSolution(int caseSolutionId)
        {
            var caseSolutionEntity = DataContext.CaseSolutions.Where(cs => cs.Id == caseSolutionId).FirstOrDefault();
            if (caseSolutionEntity == null || !caseSolutionEntity.ExtendedCaseForms.Any())
                return null;

            return _extendedCaseFormToBusinessModelMapper.Map(caseSolutionEntity.ExtendedCaseForms.FirstOrDefault());
        }
    }
}
