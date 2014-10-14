namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;

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
    using DH.Helpdesk.Services.BusinessLogic.MailTools;
    using DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.EmailTemplate;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.User;

    public class CircularService : ICircularService
    {
        #region Fields

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IMailTemplateFormatterNew mailTemplateFormatter;

        private readonly IEmailService emailService;

        #endregion

        #region Constructors and Destructors

        public CircularService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IMailTemplateFormatterNew mailTemplateFormatter,
            IEmailService emailService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.mailTemplateFormatter = mailTemplateFormatter;
            this.emailService = emailService;
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

                IQueryable<QuestionnaireCircularPartEntity> questionnaireCirculars =
                    circularPartRepository.GetAll().GetQuestionnaireCases(questionnaireId);

                List<AvailableCase> businessModels = questionnaireCirculars.MapToAvilableCases(query, isUniqueEmail);

                return businessModels;
            }
        }

        public List<ConnectedCase> GetConnectedCases(int circularId)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();
                var businessModels = circularPartRepository.GetAll().GetCircularCases(circularId).MapToConnectedCases();

                return businessModels;
            }
        }

        public void SendQuestionnaire(string actionAbsolutePath, int circularId, OperationContext operationContext)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();
                var templateLangaugeRepository = uof.GetRepository<MailTemplateLanguageEntity>();
                var templateRepository = uof.GetRepository<MailTemplateEntity>();
                var customerRepository = uof.GetRepository<Customer>();

                var connectedCases =
                    circularPartRepository.GetAll()
                        .GetCircularCases(circularId)
                        .Select(
                            x =>
                            new { x.Guid, x.Case.CaseNumber, x.Case.Description, x.Case.Caption, x.Case.PersonsEmail })
                        .ToList();

                MailTemplate mailTemplate =
                    templateLangaugeRepository.GetAll()
                        .ExtractMailTemplate(
                            templateRepository.GetAll(),
                            operationContext.CustomerId,
                            operationContext.LanguageId,
                            (int)QuestionnaireTemplates.Questionnaire);

                string mailFrom =
                    customerRepository.GetAll()
                        .GetById(operationContext.CustomerId)
                        .Select(x => x.HelpdeskEmail)
                        .Single();

                foreach (var connectedCase in connectedCases)
                {
                    EmailMarkValues markValues = CreateMarkValues(
                        actionAbsolutePath,
                        connectedCase.Guid,
                        connectedCase.CaseNumber,
                        connectedCase.Caption,
                        connectedCase.Description);

                    Mail mail = this.mailTemplateFormatter.Format(mailTemplate, markValues);
                    this.emailService.SendEmail(
                        new MailAddress(mailFrom),
                        new MailAddress(connectedCase.PersonsEmail),
                        mail);
                }
            }
        }

        #endregion

        #region PRIVATE

        private static void Map(Circular businessModel, QuestionnaireCircularEntity entity)
        {
            entity.CircularName = businessModel.CircularName;
        }

        private static EmailMarkValues CreateMarkValues(
            string actionAbsolutePath,
            Guid guid,
            decimal caseNumber,
            string caseCaption,
            string caseDescription)
        {
            string path = string.Format("{0}{1}", actionAbsolutePath, guid);

            var markValues = new EmailMarkValues();
            markValues.Add("[#1]", caseNumber.ToString());
            markValues.Add("[#4]", caseCaption);
            markValues.Add("[#5]", caseDescription);
            markValues.Add("[#99]", path);

            return markValues;
        }

        #endregion
    }
}