namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Survey;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.User;

    public class SurveyService : ISurveyService
    {
        #region Fields

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        #endregion

        #region Constructors and Destructors

        public SurveyService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        #endregion

        #region Public Methods and Operators

        public Survey GetByCaseId(int caseId)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.Create())
            {
                IRepository<Survey> repository = uow.GetRepository<Survey>();
                return repository.GetAll().Where(item => item.CaseId == caseId).FirstOrDefault();
            }
        }

        public Tuple<int, int, int, int> GetSurveyStat(
            int customerId,
            DateTime finishingDateFrom,
            DateTime finishingDateTo,
            int[] selectedCaseTypes,
            int? selectedProductArea,
            int[] selectedWorkingGroups)
        {
            Tuple<int, int, int, int> res;
            int[] selectedProducts = null;
            if (selectedProductArea.HasValue)
            {
                selectedProducts = new[] { selectedProductArea.Value };
            }

            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var caseRepository = uof.GetRepository<Case>();
                var userRepository = uof.GetRepository<User>();
                var surveryRepository = uof.GetRepository<Survey>();
                IQueryable<Case> cases = caseRepository.GetAll()
                    .GetAvaliableCustomerCases(customerId)
                    .GetCasesFromFinishingDate(finishingDateFrom)
                    .GetCasesToFinishingDate(finishingDateTo)
                    .GetCaseTypesCases(selectedCaseTypes)
                    .GetProductAreasCases(selectedProducts);
                if (selectedWorkingGroups != null && selectedWorkingGroups.Any())
                {
                    IQueryable<int> userIds =
                        userRepository.GetAll()
                            .GetWorkingGroupsUsers(selectedWorkingGroups)
                            .Select(u => u.Id);
                    cases = cases.GetUserCases(userIds);
                }
                IQueryable<int> casesIds = cases.Select(it => it.Id);
                IQueryable<Survey> surveys = surveryRepository.GetAll().WhereCases(casesIds);
                res = new Tuple<int, int, int, int>(
                    surveys.Where(it => it.VoteResult == SurveyVoteResult.GOOD).Count(),
                    surveys.Where(it => it.VoteResult == SurveyVoteResult.NORMAL).Count(),
                    surveys.Where(it => it.VoteResult == SurveyVoteResult.BAD).Count(),
                    surveys.Count());
            }

            return res;
        }

        public void SaveSurvey(Survey item)
        {
            using (IUnitOfWork uow = this.unitOfWorkFactory.Create())
            {
                IRepository<Survey> repository = uow.GetRepository<Survey>();
                repository.Add(item);
                uow.Save();
            }
        }

        #endregion
    }
}