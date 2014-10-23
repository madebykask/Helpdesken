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
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Common.Extensions.Integer;
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

    using LinqLib.Operators;
    using LinqLib.Sequence;

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

        public QuestionnaireOverview GetQuestionnaire(Guid guid, OperationContext operationContext)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();
                var questionnaireRepository = uof.GetRepository<QuestionnaireEntity>();
                var questionnaireQuestionRepository = uof.GetRepository<QuestionnaireQuestionEntity>();
                var questionnaireQuestionOptionRepository = uof.GetRepository<QuestionnaireQuestionOptionEntity>();
                var questionnaireLanguageRepository = uof.GetRepository<QuestionnaireLanguageEntity>();
                var questionnaireQuestionLanguageRepository = uof.GetRepository<QuestionnaireQuesLangEntity>();
                var questionnaireQuestionOptionLanguageRepository = uof.GetRepository<QuestionnaireQuesOpLangEntity>();

                var questionnairies =
                    from questionnaire in questionnaireRepository.GetAll().GetByCustomer(operationContext.CustomerId)
                    join questionnaireLanguage in
                        questionnaireLanguageRepository.GetAll().GetByLanguage(operationContext.LanguageId) on
                        questionnaire.Id equals questionnaireLanguage.Questionnaire_Id into group1
                    from g1 in group1.DefaultIfEmpty()
                    select
                        new
                            {
                                QuestionnaireId = questionnaire.Id,
                                QuestionnaireName = g1.QuestionnaireName ?? questionnaire.QuestionnaireName,
                                QuestionnaireDescription =
                        g1.QuestionnaireDescription ?? questionnaire.QuestionnaireDescription
                            };

                var questionnairieQuestions = from questionnaireQuestion in questionnaireQuestionRepository.GetAll()
                                              join questionnairieLanguageQuestion in
                                                  questionnaireQuestionLanguageRepository.GetAll()
                                                  .GetByLanguage(operationContext.LanguageId) on
                                                  questionnaireQuestion.Id equals
                                                  questionnairieLanguageQuestion.QuestionnaireQuestion_Id into group1
                                              from g1 in group1.DefaultIfEmpty()
                                              select
                                                  new
                                                      {
                                                          QuestionnaireId = questionnaireQuestion.Questionnaire_Id,
                                                          QuestionnaireQuestionId = questionnaireQuestion.Id,
                                                          questionnaireQuestion.QuestionnaireQuestionNumber,
                                                          QuestionnaireQuestionNoteText = questionnaireQuestion.NoteText,
                                                          QuestionnaireQuestionShowNote = questionnaireQuestion.ShowNote,
                                                          QuestionnaireQuestion =
                                                  g1.QuestionnaireQuestion
                                                  ?? questionnaireQuestion.QuestionnaireQuestion
                                                      };

                var questionnairieQuestionOptions =
                    from questionnaireQuestionOption in questionnaireQuestionOptionRepository.GetAll()
                    join questionnaireLanguageQuestionOption in
                        questionnaireQuestionOptionLanguageRepository.GetAll()
                        .GetByLanguage(operationContext.LanguageId) on questionnaireQuestionOption.Id equals
                        questionnaireLanguageQuestionOption.QuestionnaireQuestionOption_Id into group1
                    from g1 in group1.DefaultIfEmpty()
                    select
                        new
                            {
                                QuestionnaireQuestionId = questionnaireQuestionOption.QuestionnaireQuestion_Id,
                                QuestionnaireQuestionOptionId = questionnaireQuestionOption.Id,
                                QuestionnaireQuestionOptionPosition =
                        questionnaireQuestionOption.QuestionnaireQuestionOptionPos,
                                QuestionnaireQuestionOptionValue = questionnaireQuestionOption.OptionValue,
                                QuestionnaireQuestionOption =
                        g1.QuestionnaireQuestionOption ?? questionnaireQuestionOption.QuestionnaireQuestionOption
                            };

                var query = from circularPart in circularPartRepository.GetAll().GetByGuid(guid)
                            join questionnaire in questionnairies on circularPart.QuestionnaireCircular.Questionnaire_Id
                                equals questionnaire.QuestionnaireId
                            join questionnaireQuestion in questionnairieQuestions on questionnaire.QuestionnaireId
                                equals questionnaireQuestion.QuestionnaireId into group1
                            from g1 in group1.DefaultIfEmpty()
                            join questionnaireQuestionOption in questionnairieQuestionOptions on
                                g1.QuestionnaireQuestionId equals questionnaireQuestionOption.QuestionnaireQuestionId
                                into group2
                            from g2 in group2.DefaultIfEmpty()
                            select
                                new
                                    {
                                        QuestionnaireId = (int?)questionnaire.QuestionnaireId,
                                        questionnaire.QuestionnaireName,
                                        questionnaire.QuestionnaireDescription,
                                        QuestionnaireQuestionId = (int?)g1.QuestionnaireQuestionId,
                                        g1.QuestionnaireQuestion,
                                        g1.QuestionnaireQuestionNumber,
                                        ShowNote = (int?)g1.QuestionnaireQuestionShowNote,
                                        NoteText = g1.QuestionnaireQuestionNoteText,
                                        QuestionnaireQuestionOptionId = (int?)g2.QuestionnaireQuestionOptionId,
                                        OptionValue = (int?)g2.QuestionnaireQuestionOptionValue,
                                        g2.QuestionnaireQuestionOption,
                                        questionnaireQuestionOptionPos = (int?)g2.QuestionnaireQuestionOptionPosition,
                                    };

                var flatData = query.ToList();

                var anonymus =
                    flatData.GroupBy(
                        flat =>
                        new
                            {
                                QuestionnaireId = (int)flat.QuestionnaireId,
                                flat.QuestionnaireName,
                                flat.QuestionnaireDescription
                            })
                        .Select(
                            g1 =>
                            new
                                {
                                    g1.Key.QuestionnaireId,
                                    g1.Key.QuestionnaireName,
                                    g1.Key.QuestionnaireDescription,
                                    Questions = (from i in g1
                                                 group i by
                                                     new
                                                         {
                                                             i.QuestionnaireQuestionId,
                                                             i.QuestionnaireQuestion,
                                                             i.QuestionnaireQuestionNumber,
                                                             i.NoteText,
                                                             i.ShowNote
                                                         }
                                                     into g2
                                                     select
                                                         new
                                                             {
                                                                 g2.Key.QuestionnaireQuestionId,
                                                                 g2.Key.QuestionnaireQuestion,
                                                                 g2.Key.QuestionnaireQuestionNumber,
                                                                 g2.Key.NoteText,
                                                                 g2.Key.ShowNote,
                                                                 Options = (from j in g2
                                                                            group j by
                                                                                new
                                                                                    {
                                                                                        j.QuestionnaireQuestionOptionId,
                                                                                        j.OptionValue,
                                                                                        j.QuestionnaireQuestionOption,
                                                                                        j.questionnaireQuestionOptionPos
                                                                                    }
                                                                                into g3
                                                                                select
                                                                                    new
                                                                                        {
                                                                                            g3.Key.QuestionnaireQuestionOptionId,
                                                                                            g3.Key.OptionValue,
                                                                                            g3.Key.QuestionnaireQuestionOption,
                                                                                            g3.Key.questionnaireQuestionOptionPos
                                                                                        })
                                                         .SkipWhile(x => x.QuestionnaireQuestionOptionId == null).ToList()
                                                             })
                                .SkipWhile(x => x.QuestionnaireQuestionId == null).ToList()
                                }).SingleOrDefault();

                if (anonymus == null)
                {
                    return QuestionnaireOverview.GetDefault();
                }

                var questions = (from question in anonymus.Questions
                                 let options =
                                     question.Options.Select(
                                         option =>
                                         new QuestionnaireQuestionOptionOverview(
                                             (int)option.QuestionnaireQuestionOptionId,
                                             option.QuestionnaireQuestionOption,
                                             (int)option.OptionValue,
                                             (int)option.questionnaireQuestionOptionPos)).ToList()
                                 select
                                     new QuestionnaireQuestionOverview(
                                     (int)question.QuestionnaireQuestionId,
                                     question.QuestionnaireQuestion,
                                     question.QuestionnaireQuestionNumber,
                                     question.ShowNote.ToBool(),
                                     question.NoteText,
                                     options)).ToList();

                var questionnarie = new QuestionnaireOverview(
                    anonymus.QuestionnaireId,
                    anonymus.QuestionnaireName,
                    anonymus.QuestionnaireDescription,
                    questions);

                return questionnarie;
            }
        }

        public void SaveAnswers(ParticipantForInsert businessModel)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var questionnaireResultRepository = uof.GetRepository<QuestionnaireResultEntity>();
                var questionnaireQuestionResultRepository = uof.GetRepository<QuestionnaireQuestionResultEntity>();

                QuestionnaireResultEntity entity =
                    questionnaireResultRepository.Find(
                        x => x.QuestionnaireCircularPart.Guid == businessModel.Guid,
                        x => x.QuestionnaireQuestionResultEntities).SingleOrDefault();

                if (entity != null)
                {
                    entity.QuestionnaireQuestionResultEntities.ToList()
                        .ForEach(x => questionnaireQuestionResultRepository.DeleteById(x.Id));
                }
                else
                {
                    var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();

                    int id = circularPartRepository.GetAll().GetByGuid(businessModel.Guid).Select(x => x.Id).Single();
                    entity = new QuestionnaireResultEntity
                                 {
                                     CreatedDate = businessModel.CreatedDate,
                                     QuestionnaireCircularPartic_Id = id
                                 };
                    questionnaireResultRepository.Add(entity);
                }

                entity.Anonymous = businessModel.IsAnonym.ToInt();

                foreach (Answer answer in businessModel.Answers)
                {
                    entity.QuestionnaireQuestionResultEntities.Add(
                        new QuestionnaireQuestionResultEntity
                            {
                                QuestionnaireQuestionNote =
                                    string.IsNullOrWhiteSpace(answer.Note)
                                        ? string.Empty
                                        : answer.Note,
                                QuestionnaireQuestionOption_Id = answer.OptionId
                            });
                }

                uof.Save();
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