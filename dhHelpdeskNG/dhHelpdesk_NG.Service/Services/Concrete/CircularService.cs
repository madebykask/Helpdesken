namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Read;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Write;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.User;

    public class CircularService : ICircularService
    {
        #region Fields

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        #endregion

        #region Constructors and Destructors

        public CircularService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        #endregion

        #region Public Methods and Operators

        public CircularForEdit GetById(int circularId)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var questionnaireRepository = uof.GetRepository<QuestionnaireCircularEntity>();

                CircularForEdit entity = questionnaireRepository.GetAll().MapToEditModelById(circularId);
                return entity;
            }
        }

        public List<CircularOverview> GetCircularOverviewsByQuestionnaireId(int questionnaireId)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var questionnaireRepository = uof.GetRepository<QuestionnaireCircularEntity>();
                List<CircularOverview> overviews =
                    questionnaireRepository.GetAll().MapToOverviewsByQuestionnaireId(questionnaireId);

                return overviews;
            }
        }

        public void AddCircular(CircularForInsert businessModel)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularRepository = uof.GetRepository<QuestionnaireCircularEntity>();

                var entity = new QuestionnaireCircularEntity();

                Map(businessModel, entity);
                entity.Id = businessModel.Id;
                entity.Questionnaire_Id = businessModel.QuestionnaireId;
                entity.Status = businessModel.Status;
                entity.ChangedDate = businessModel.CreatedDate;
                entity.CreatedDate = businessModel.CreatedDate;
                entity.CircularName = businessModel.CircularName;

                foreach (var id in businessModel.RelatedCaseIds)
                {
                    entity.QuestionnaireCircularPartEntities.Add(
                        new QuestionnaireCircularPartEntity { CreatedDate = businessModel.CreatedDate, Case_Id = id });
                }

                circularRepository.Add(entity);

                uof.Save();
            }
        }

        public void UpdateCircular(CircularForUpdate businessModel)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularRepository = uof.GetRepository<QuestionnaireCircularEntity>();

                var entity = circularRepository.GetById(businessModel.Id);

                Map(businessModel, entity);
                entity.ChangedDate = businessModel.ChangedDate;

                circularRepository.Update(entity);

                uof.Save();
            }
        }

        public void DeleteById(int id)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var questionnaireRepository = uof.GetRepository<QuestionnaireCircularEntity>();
                questionnaireRepository.DeleteById(id);
                uof.Save();
            }
        }

        public List<CircularPart> GetCases(
            int customerId,
            int questionnaireId,
            int[] selectedDepartments,
            int[] selectedCaseTypes,
            int[] selectedProductArea,
            int[] selectedWorkingGroups,
            int procent,
            DateTime? finishingDateFrom,
            DateTime? finishingDateTo,
            bool isUniqueEmail)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var caseRepository = uof.GetRepository<Case>();
                var userRepository = uof.GetRepository<User>();
                var questionnaireCircularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();

                IQueryable<Case> query =
                    caseRepository.GetAll()
                        .GetAvaliableCustomerCases(customerId)
                        .GetDepartmentsCases(selectedDepartments)
                        .GetCaseTypesCases(selectedCaseTypes)
                        .GetProductAreasCases(selectedProductArea)
                        .GetCasesFromFinishingDate(finishingDateFrom)
                        .GetCasesToFinishingDate(finishingDateTo);

                if (selectedWorkingGroups != null && selectedWorkingGroups.Any())
                {
                    IQueryable<int> userIds =
                        userRepository.GetAll()
                            .GetByCustomer(customerId)
                            .GetWorkingGroupsUsers(selectedWorkingGroups)
                            .Select(u => u.Id);

                    query = query.GetUserCases(userIds);
                }

                int count = caseRepository.GetAll().GetAvaliableCustomerCases(customerId).Count();
                int percentageOfCases = (count * procent) / 100;

                query = query.Take(percentageOfCases);

                var questionnaireCirculars =
                    questionnaireCircularPartRepository.GetAll()
                        .GetByQuestionnaireId(questionnaireId)
                        .Select(x => new { x.Case_Id, x.SendDate })
                        .GroupBy(x => x.Case_Id)
                        .Select(x => new { Case_Id = x.Key, SendDate = x.Max(c => c.SendDate) });

                var anonymus = from @case in query
                               join questionnaireCircular in questionnaireCirculars on @case.Id equals
                                   questionnaireCircular.Case_Id into splits
                               from split in splits.DefaultIfEmpty()
                               select
                                   new { @case.Id, @case.CaseNumber, @case.Caption, @case.PersonsEmail, split.SendDate };

                if (isUniqueEmail)
                {
                    anonymus = from element in anonymus
                               group element by element.PersonsEmail
                               into groups
                               select groups.OrderBy(p => p.CaseNumber).FirstOrDefault();
                }

                List<CircularPart> businessModels =
                    anonymus.ToList()
                        .Select(
                            c =>
                            new CircularPart(c.Id, (int)c.CaseNumber, c.Caption, c.PersonsEmail, c.SendDate != null))
                        .ToList();

                return businessModels;
            }
        }

        #endregion

        #region PRIVATE

        private static void Map(Circular businessModel, QuestionnaireCircularEntity entity)
        {
            entity.CircularName = businessModel.CircularName;
        }

        #endregion
    }
}