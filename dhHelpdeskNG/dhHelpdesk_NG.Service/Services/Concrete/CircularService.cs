namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.MailTemplates;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.BusinessData.Models.Questionnaire;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Read;
    using DH.Helpdesk.BusinessData.Models.Questionnaire.Write;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.MailTemplates;
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
                var circularRepository = uof.GetRepository<QuestionnaireCircularEntity>();

                CircularForEdit entity = circularRepository.GetAll().MapToEditModelById(circularId);
                return entity;
            }
        }

        public List<CircularOverview> GetCircularOverviews(int questionnaireId, int state)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularRepository = uof.GetRepository<QuestionnaireCircularEntity>();
                var query = circularRepository.GetAll().GetByQuestionnaireId(questionnaireId);
                if (state != CircularStateId.All)
                {
                    query = query.GetByState(state);
                }

                List<CircularOverview> overviews = query.MapToOverviews();

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
                var circularRepository = uof.GetRepository<QuestionnaireCircularEntity>();
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();

                circularPartRepository.DeleteWhere(x => x.QuestionnaireCircular_Id == id);
                circularRepository.DeleteById(id);

                uof.Save();
            }
        }

        public void DeleteConnectedCase(int cirularId, int caseId)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();

                circularPartRepository.DeleteWhere(x => x.QuestionnaireCircular_Id == cirularId && x.Case_Id == caseId);

                uof.Save();
            }
        }

        public List<AvailableCase> GetAvailableCases(
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
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();

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
                    circularPartRepository.GetAll()
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

                List<AvailableCase> businessModels =
                    anonymus.ToList()
                        .Select(
                            c =>
                            new AvailableCase(c.Id, (int)c.CaseNumber, c.Caption, c.PersonsEmail, c.SendDate != null))
                        .ToList();

                return businessModels;
            }
        }

        public List<ConnectedCase> GetConnectedCases(int circularId)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();
                var caseRepository = uof.GetRepository<Case>();

                IQueryable<QuestionnaireCircularPartEntity> query =
                    circularPartRepository.GetAll().GetCircularCases(circularId);

                IQueryable<Case> cases = caseRepository.GetAll();

                var anonymus = from connectedCase in query
                               join @case in cases on connectedCase.Case_Id equals @case.Id into splits
                               from split in splits.DefaultIfEmpty()
                               select
                                   new
                                       {
                                           split.Id,
                                           split.CaseNumber,
                                           split.Caption,
                                           split.PersonsEmail,
                                           connectedCase.Guid,
                                           connectedCase.SendDate
                                       };

                List<ConnectedCase> businessModels =
                    anonymus.ToList()
                        .Select(
                            c =>
                            new ConnectedCase(c.Id, (int)c.CaseNumber, c.Caption, c.PersonsEmail, c.Guid, c.SendDate != null))
                        .ToList();

                return businessModels;
            }
        }

        public void SendQuestionnaire(
            string actionAbsolutePath,
            int circularId,
            OperationContext operationContext)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();
                var templateLangaugeRepository = uof.GetRepository<MailTemplateLanguageEntity>();
                var templateRepository = uof.GetRepository<MailTemplateEntity>();

                var cases =
                    circularPartRepository.GetAll()
                        .Where(x => x.QuestionnaireCircular_Id == circularId)
                        .Select(
                            x =>
                            new
                                {
                                    x.Guid, x.Case.CaseNumber, x.Case.Description, x.Case.Caption, x.Case.PersonsEmail
                                })
                        .ToList();

                var query =
                    from l in
                        templateLangaugeRepository.GetAll().Where(x => x.Language_Id == operationContext.CustomerId)
                    join t in
                        templateRepository.GetAll()
                        .Where(
                            c =>
                            c.MailID == (int)QuestionnaireTemplates.Questionnaire
                            && c.Customer_Id == operationContext.CustomerId) on l.MailTemplate_Id equals t.Id
                    select new { l.Body, l.Subject };

                var anonymus = query.Single(); 
                var mailTemplate = new MailTemplate(anonymus.Subject, anonymus.Body);
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