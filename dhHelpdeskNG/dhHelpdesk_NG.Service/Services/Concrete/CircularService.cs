namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Output;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Questionnaire;

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
            DateTime? finishingDateTo)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var caseRepository = uof.GetRepository<Case>();
                var userRepository = uof.GetRepository<User>();
                var questionnaireCircularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();

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
                            .GetByCustomer(customerId)
                            .Where(
                                u =>
                                u.Default_WorkingGroup_Id != null
                                && selectedWorkingGroups.ToList().Contains(u.Default_WorkingGroup_Id.Value))
                            .Select(u => u.Id);

                    query = query.Where(c => userIds.Contains(c.Performer_User_Id));
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

                var questionnaireCirculars =
                    questionnaireCircularPartRepository.GetAll().GetByQuestionnaireId(questionnaireId);

                var anonymus = (from @case in query
                                join questionnaireCircular in questionnaireCirculars on @case.Id equals
                                    questionnaireCircular.Case_Id into splits
                                from split in splits.DefaultIfEmpty()
                                select
                                    new
                                        {
                                            @case.Id,
                                            @case.CaseNumber,
                                            @case.Caption,
                                            @case.PersonsEmail,
                                            split.SendDate
                                        }).ToList();

                List<CircularPart> businessModels =
                    anonymus.Select(
                        c => new CircularPart(c.Id, (int)c.CaseNumber, c.Caption, c.PersonsEmail, c.SendDate != null))
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