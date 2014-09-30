namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories.Questionnaire;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;

    public class CircularService : ICircularService
    {
        #region Fields

        private readonly ICircularRepository circularRepository;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        #endregion

        #region Constructors and Destructors

        public CircularService(ICircularRepository circularRepository, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.circularRepository = circularRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        #endregion

        #region Public Methods and Operators

        public EditCircular GetCircularById(int circularId)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var questionnaireRepository = uof.GetRepository<QuestionnaireCircularEntity>();

                EditCircular entity = questionnaireRepository.GetAll().MapToEditModelById(circularId);
                return entity;
            }
        }

        public List<CircularOverview> FindCircularOverviews(int questionnaireId)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var questionnaireRepository = uof.GetRepository<QuestionnaireCircularEntity>();
                List<CircularOverview> overviews =
                    questionnaireRepository.GetAll().MapToOverviewsByQuestionnaireId(questionnaireId);

                return overviews;
            }
        }

        public void AddCircular(NewCircular newCircular)
        {
            this.circularRepository.AddCircular(newCircular);
            this.circularRepository.Commit();
        }

        public void UpdateCircular(EditCircular editedCircular)
        {
            this.circularRepository.UpdateCircular(editedCircular);
            this.circularRepository.Commit();
        }

        public void DeleteCircularById(int deletedCircularId)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var questionnaireRepository = uof.GetRepository<QuestionnaireCircularEntity>();
                questionnaireRepository.DeleteById(deletedCircularId);
                uof.Save();
            }
        }

        public List<CircularPart> GetCases(
            int customerId,
            int[] selectedDepartments,
            int[] selectedCaseTypes,
            int[] selectedProductArea,
            int[] selectedWorkingGroups,
            int procent,
            DateTime? finishingDateFrom,
            DateTime? finishingDateTo)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var caseRepository = uof.GetRepository<Case>();
                var userRepository = uof.GetRepository<User>();

                IQueryable<Case> query = caseRepository.GetAll().GetAvaliableCustomerCases(customerId);

                if (selectedDepartments != null && selectedDepartments.Any())
                {
                    query =
                        query.Where(
                            c => c.Department_Id != null && selectedDepartments.ToList().Contains(c.Department_Id.Value));
                }

                if (selectedCaseTypes != null && selectedCaseTypes.Any())
                {
                    query = query.Where(c => selectedCaseTypes.ToList().Contains(c.CaseType_Id));
                }

                if (selectedProductArea != null && selectedProductArea.Any())
                {
                    query =
                        query.Where(
                            c =>
                            c.ProductArea_Id != null && selectedProductArea.ToList().Contains(c.ProductArea_Id.Value));
                }

                if (selectedWorkingGroups != null && selectedWorkingGroups.Any())
                {
                    IQueryable<int> userIds =
                        userRepository.GetAll()
                            .GetUsersByCustomer(customerId)
                            .Where(
                                u =>
                                u.Default_WorkingGroup_Id != null
                                && selectedWorkingGroups.ToList().Contains(u.Default_WorkingGroup_Id.Value))
                            .Select(u => u.Id);

                    query = query.Where(c => userIds.ToList().Contains(c.Performer_User_Id));
                }

                if (finishingDateFrom.HasValue)
                {
                    query = query.Where(x => x.FinishingDate >= finishingDateFrom);
                }

                if (finishingDateTo.HasValue)
                {
                    query = query.Where(x => x.FinishingDate <= finishingDateTo);
                }

                int count = caseRepository.GetAll().GetAvaliableCustomerCases(customerId).Count();
                int percentageOfCases = (count * procent) / 100;

                query = query.Take(percentageOfCases);

                var anonymus = query.Select(x => new { x.Id, x.CaseNumber, x.Caption, x.PersonsEmail }).ToList();

                List<CircularPart> businessModels =
                    anonymus.Select(c => new CircularPart(c.Id, (int)c.CaseNumber, c.Caption, c.PersonsEmail)).ToList();

                return businessModels;
            }
        }

        #endregion
    }
}