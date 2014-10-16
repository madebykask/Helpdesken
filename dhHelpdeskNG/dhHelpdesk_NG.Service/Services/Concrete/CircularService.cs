namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.MailTemplates;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Email;
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
                IQueryable<QuestionnaireCircularEntity> query =
                    circularRepository.GetAll().GetByQuestionnaireId(questionnaireId);
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

                QuestionnaireCircularEntity entity = circularRepository.GetById(businessModel.Id);

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
                List<ConnectedCase> businessModels =
                    circularPartRepository.GetAll().GetCircularCases(circularId).MapToConnectedCases();

                return businessModels;
            }
        }

        public void SendQuestionnaire(string actionAbsolutePath, int circularId, OperationContext operationContext)
        {
            this.SetStatus(circularId, CircularStates.Sent);

            List<QuestionnaireMailItem> mails = this.GetMails(actionAbsolutePath, circularId, operationContext);
            this.SendMails(mails, operationContext.DateAndTime);
        }

        public void GetQuestionnaire(Guid guid, OperationContext operationContext)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var questionnaireRepository = uof.GetRepository<QuestionnaireEntity>();
                var questionnaireLanguageRepository = uof.GetRepository<QuestionnaireLanguageEntity>();
                var questionnaireQuestionRepository = uof.GetRepository<QuestionnaireQuestionEntity>();
                var questionnaireQuestionLanguageRepository = uof.GetRepository<QuestionnaireQuesLangEntity>();
                var questionnaireQuestionOptionRepository = uof.GetRepository<QuestionnaireQuestionOptionEntity>();
                var questionnaireQuestionOptionLanguageRepository = uof.GetRepository<QuestionnaireQuesOpLangEntity>();
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();

                if (operationContext.LanguageId == LanguageId.Swedish)
                {
                    var query = from circularPart in circularPartRepository.GetAll().GetByGuid(guid)
                                join questionnaire in
                                    questionnaireRepository.GetAll().GetByCustomer(operationContext.CustomerId) on
                                    circularPart.QuestionnaireCircular.Questionnaire_Id equals questionnaire.Id
                                join questionnaireQuestion in questionnaireQuestionRepository.GetAll() on
                                    questionnaire.Id equals questionnaireQuestion.Questionnaire_Id
                                join questionnaireQuestionOption in questionnaireQuestionOptionRepository.GetAll() on
                                    questionnaireQuestion.Id equals questionnaireQuestionOption.QuestionnaireQuestion_Id
                                select
                                    new
                                        {
                                            questionnaireId = questionnaire.Id,
                                            questionnaire.QuestionnaireName,
                                            questionnaire.QuestionnaireDescription,
                                            questionnaireQuestionId = questionnaireQuestion.Id,
                                            questionnaireQuestion.QuestionnaireQuestion,
                                            questionnaireQuestion.QuestionnaireQuestionNumber,
                                            questionnaireQuestion.ShowNote,
                                            questionnaireQuestion.NoteText,
                                            questionnaireQuestionOptionId = questionnaireQuestionOption.Id,
                                            questionnaireQuestionOption.OptionValue,
                                            questionnaireQuestionOption.QuestionnaireQuestionOption,
                                            questionnaireQuestionOption.QuestionnaireQuestionOptionPos,
                                        };

                    var anonymus = query.ToList();
                }
            }
        }

        #endregion

        #region PRIVATE STATIC

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
            path = string.Format("<a href=\"{0}\">{0}</a>", path);

            var markValues = new EmailMarkValues();
            markValues.Add("[#1]", caseNumber.ToString());
            markValues.Add("[#4]", caseCaption);
            markValues.Add("[#5]", caseDescription);
            markValues.Add("[#99]", path);

            return markValues;
        }

        #endregion

        #region PRIVATE

        private List<QuestionnaireMailItem> GetMails(
            string actionAbsolutePath,
            int circularId,
            OperationContext operationContext)
        {
            var mails = new List<QuestionnaireMailItem>();

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

                mails.AddRange(
                    from connectedCase in connectedCases
                    let markValues =
                        CreateMarkValues(
                            actionAbsolutePath,
                            connectedCase.Guid,
                            connectedCase.CaseNumber,
                            connectedCase.Caption,
                            connectedCase.Description)
                    let mail = this.mailTemplateFormatter.Format(mailTemplate, markValues)
                    select new QuestionnaireMailItem(connectedCase.Guid, mailFrom, connectedCase.PersonsEmail, mail));
            }

            return mails;
        }

        private void SetStatus(int circularId, CircularStates circularState)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularRepository = uof.GetRepository<QuestionnaireCircularEntity>();

                QuestionnaireCircularEntity circular = circularRepository.GetById(circularId);
                circular.Status = (int)circularState;

                uof.Save();
            }
        }

        private void SendMails(List<QuestionnaireMailItem> mailItems, DateTime operationDate)
        {
            foreach (QuestionnaireMailItem mailItem in mailItems)
            {
                this.SendMail(mailItem, operationDate);
            }
        }

        private void SendMail(QuestionnaireMailItem mailItem, DateTime operationDate)
        {
            this.emailService.SendEmail(mailItem.MailItem);

            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();

                QuestionnaireCircularPartEntity circularPart =
                    circularPartRepository.GetAll().Where(x => x.Guid == mailItem.Guid).SingleOrDefault();

                if (circularPart != null)
                {
                    circularPart.SendDate = operationDate;
                }

                uof.Save();
            }
        }

        #endregion
    }
}