﻿namespace DH.Helpdesk.Services.Services.Concrete
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
    using DH.Helpdesk.Domain.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.MailTools;
    using DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Questionnaire;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.User;
    using DH.Helpdesk.Services.Response.Questionnaire;
    using Infrastructure;

    public class CircularService : ICircularService
    {
        #region Fields

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IMailTemplateFormatterNew mailTemplateFormatter;

        private readonly IEmailService emailService;

        private readonly IMailTemplateServiceNew mailTemplateService;

        private readonly ISettingService settingService;

        private readonly IEmailSendingSettingsProvider _emailSendingSettingsProvider;

        #endregion

        #region Constructors and Destructors

        public CircularService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IMailTemplateFormatterNew mailTemplateFormatter,
            IEmailService emailService,
            IMailTemplateServiceNew mailTemplateService,
            ISettingService settingService,
            IEmailSendingSettingsProvider emailSendingSettingsProvider)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.mailTemplateFormatter = mailTemplateFormatter;
            this.emailService = emailService;
            this.mailTemplateService = mailTemplateService;
            this.settingService = settingService;
            _emailSendingSettingsProvider = emailSendingSettingsProvider;
        }

        #endregion

        #region Public Methods and Operators

        public CircularForEdit GetById(int circularId)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularRepository = uof.GetRepository<QuestionnaireCircularEntity>();

                CircularForEdit entity = circularRepository.GetAll(
                        x => x.QuestionnaireCircularDepartmentEntities
                        , x => x.QuestionnaireCircularCaseTypeEntities
                        , x => x.QuestionnaireCircularProductAreaEntities
                        , x => x.QuestionnaireCircularWorkingGroupEntities
                    ).MapToEditModelById(circularId);
                return entity;
            }
        }

        public CircularForEdit GetSingleOrDefaultByQuestionnaireId(int id)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularRepository = uof.GetRepository<QuestionnaireCircularEntity>();

                CircularForEdit entity = circularRepository.GetAll(
                    x => x.QuestionnaireCircularDepartmentEntities
                    , x => x.QuestionnaireCircularCaseTypeEntities
                    , x => x.QuestionnaireCircularProductAreaEntities
                    , x => x.QuestionnaireCircularWorkingGroupEntities
                    ).MapToEditModelByQuestionnaireId(id);
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
                entity.ChangedDate = businessModel.CreatedDate;
                entity.CircularName = businessModel.CircularName;

                entity.Questionnaire_Id = businessModel.QuestionnaireId;
                entity.Status = businessModel.Status;
                entity.CreatedDate = businessModel.CreatedDate;

                entity.IsUniqueEmail = businessModel.CaseFilter.IsUniqueEmail;
                entity.FinishingDateFrom = businessModel.CaseFilter.FinishingDateFrom;
                entity.FinishingDateTo = businessModel.CaseFilter.FinishingDateTo;
                entity.SelectedProcent = businessModel.CaseFilter.SelectedProcent;

                foreach (var id in businessModel.RelatedCaseIds)
                {
                    entity.QuestionnaireCircularPartEntities.Add(
                        new QuestionnaireCircularPartEntity { CreatedDate = businessModel.CreatedDate, Case_Id = id });
                }

                foreach (var id in businessModel.CaseFilter.SelectedDepartments)
                {
                    entity.QuestionnaireCircularDepartmentEntities.Add(
                        new QuestionnaireCircularDepartmentEntity { DepartmentId = id });
                }

                foreach (var id in businessModel.CaseFilter.SelectedCaseTypes)
                {
                    entity.QuestionnaireCircularCaseTypeEntities.Add(
                        new QuestionnaireCircularCaseTypeEntity { CaseTypeId = id });
                }

                foreach (var id in businessModel.CaseFilter.SelectedProductAreas)
                {
                    entity.QuestionnaireCircularProductAreaEntities.Add(
                        new QuestionnaireCircularProductAreaEntity { ProductAreaId = id });
                }

                foreach (var id in businessModel.CaseFilter.SelectedWorkingGroups)
                {
                    entity.QuestionnaireCircularWorkingGroupEntities.Add(
                        new QuestionnaireCircularWorkingGroupEntity { WorkingGroupId = id });
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

                var entity = new QuestionnaireCircularEntity();

                Map(businessModel, entity);
                entity.Id = businessModel.Id;
                entity.ChangedDate = businessModel.ChangedDate;
                entity.CircularName = businessModel.CircularName;

                entity.IsUniqueEmail = businessModel.CaseFilter.IsUniqueEmail;
                entity.FinishingDateFrom = businessModel.CaseFilter.FinishingDateFrom;
                entity.FinishingDateTo = businessModel.CaseFilter.FinishingDateTo;
                entity.SelectedProcent = businessModel.CaseFilter.SelectedProcent;


                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();
                var current =
                    circularPartRepository.GetAll().Where(x => x.QuestionnaireCircular_Id == businessModel.Id).ToList();
                foreach (
                    var toDel in current.Where(x => !businessModel.RelatedCaseIds.Exists(y => y == x.Case_Id)).ToList())
                {
                    circularPartRepository.Delete(toDel);
                }
                foreach (
                    var toIns in businessModel.RelatedCaseIds.Where(x => !current.Exists(y => y.Case_Id == x)).ToList())
                {
                    circularPartRepository.Add(new QuestionnaireCircularPartEntity
                    {
                        QuestionnaireCircular_Id = businessModel.Id,
                        CreatedDate = businessModel.ChangedDate,
                        Case_Id = toIns
                    });
                }


                var circularDepartmentRepository = uof.GetRepository<QuestionnaireCircularDepartmentEntity>();
                circularDepartmentRepository.MergeList(x => x.QuestionnaireCircularId == businessModel.Id
                    , businessModel.CaseFilter.SelectedDepartments.Select(y => new QuestionnaireCircularDepartmentEntity
                    {
                        DepartmentId = y,
                        QuestionnaireCircularId = businessModel.Id
                    }).ToList()
                    , (a, b) => a.DepartmentId == b.DepartmentId);

                var circularCaseTypeRepository = uof.GetRepository<QuestionnaireCircularCaseTypeEntity>();
                circularCaseTypeRepository.MergeList(x => x.QuestionnaireCircularId == businessModel.Id
                    , businessModel.CaseFilter.SelectedCaseTypes.Select(y => new QuestionnaireCircularCaseTypeEntity
                    {
                        CaseTypeId = y,
                        QuestionnaireCircularId = businessModel.Id
                    }).ToList()
                    , (a, b) => a.CaseTypeId == b.CaseTypeId);

                var circularProductAreaRepository = uof.GetRepository<QuestionnaireCircularProductAreaEntity>();
                circularProductAreaRepository.MergeList(x => x.QuestionnaireCircularId == businessModel.Id
                    , businessModel.CaseFilter.SelectedProductAreas.Select(y => new QuestionnaireCircularProductAreaEntity
                    {
                        ProductAreaId = y,
                        QuestionnaireCircularId = businessModel.Id
                    }).ToList()
                    , (a, b) => a.ProductAreaId == b.ProductAreaId);

                var circularWorkingGroupRepository = uof.GetRepository<QuestionnaireCircularWorkingGroupEntity>();
                circularWorkingGroupRepository.MergeList(x => x.QuestionnaireCircularId == businessModel.Id
                    , businessModel.CaseFilter.SelectedWorkingGroups.Select(y => new QuestionnaireCircularWorkingGroupEntity
                    {
                        WorkingGroupId = y,
                        QuestionnaireCircularId = businessModel.Id
                    }).ToList()
                    , (a, b) => a.WorkingGroupId == b.WorkingGroupId);

                circularRepository.Update(
                entity,
                x => x.CreatedDate,
                x => x.Questionnaire_Id,
                x => x.Status);

                uof.Save();
            }
        }

        public void DeleteById(int id)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularRepository = uof.GetRepository<QuestionnaireCircularEntity>();
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();
                var circularDepartmentRepository = uof.GetRepository<QuestionnaireCircularDepartmentEntity>();
                var circularCaseTypeRepository = uof.GetRepository<QuestionnaireCircularCaseTypeEntity>();
                var circularProductAreaRepository = uof.GetRepository<QuestionnaireCircularProductAreaEntity>();
                var circularWorkingGroupRepository = uof.GetRepository<QuestionnaireCircularWorkingGroupEntity>();

                circularPartRepository.DeleteWhere(x => x.QuestionnaireCircular_Id == id);
                circularDepartmentRepository.DeleteWhere(x => x.QuestionnaireCircularId == id);
                circularCaseTypeRepository.DeleteWhere(x => x.QuestionnaireCircularId == id);
                circularProductAreaRepository.DeleteWhere(x => x.QuestionnaireCircularId == id);
                circularWorkingGroupRepository.DeleteWhere(x => x.QuestionnaireCircularId == id);
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
            IList<int> selectedDepartments,
            IList<int> selectedCaseTypes,
            IList<int> selectedProductArea,
            IList<int> selectedWorkingGroups,
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
                        .GetDepartmentsCases(selectedDepartments.ToArray())
                        .GetCaseTypesCases(selectedCaseTypes.ToArray())
                        .GetProductAreasCases(selectedProductArea.ToArray())
                        .GetCasesFromFinishingDate(finishingDateFrom)
                        .GetCasesToFinishingDate(finishingDateTo);

                if (selectedWorkingGroups != null && selectedWorkingGroups.Any())
                {
                    IQueryable<int> userIds =
                        userRepository.GetAll()
                            .GetByCustomer(customerId)
                            .GetWorkingGroupsUsers(selectedWorkingGroups.ToArray())
                            .Select(u => u.Id);

                    query = query.GetUserCases(userIds);
                }

                var count = query.Count();
                int percentageOfCases = (count * procent) / 100;

                query = query.OrderBy(x => x.CaseGUID).Take(percentageOfCases);

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

            MailTemplate mailTemplate = this.mailTemplateService.GetTemplate(
                (int)QuestionnaireTemplates.Questionnaire,
                operationContext);

            List<BusinessLogic.MapperData.Participant> participants = this.GetAllParticipants(circularId);

            List<QuestionnaireMailItem> mails = this.GetMails(
                actionAbsolutePath,
                mailTemplate,
                participants,
                operationContext.CustomerId);

            this.SendMails(mails, operationContext.DateAndTime, operationContext.CustomerId);
        }

        public void Remind(string actionAbsolutePath, int circularId, OperationContext operationContext)
        {
            MailTemplate mailTemplate = this.mailTemplateService.GetTemplate(
                (int)QuestionnaireTemplates.Reminder,
                operationContext);

            List<BusinessLogic.MapperData.Participant> participants = this.GetNotAnsweredParticipants(circularId);

            List<QuestionnaireMailItem> mails = this.GetMails(
                actionAbsolutePath,
                mailTemplate,
                participants,
                operationContext.CustomerId);

            this.SendMails(mails, operationContext.DateAndTime, operationContext.CustomerId);
        }

        public QuestionnaireDetailedOverview GetQuestionnaire(Guid guid, int languageId)
        {
            var id = 0;
            var caseId = 0;
            var caption = "";
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();

                // todo ef include with join doesn't work
                var circular =
                    circularPartRepository.GetAll()
                        .GetByGuid(guid)
                        .Select(x => new { id = x.QuestionnaireCircular.Questionnaire_Id, caseId = x.Case_Id, caption = x.Case.Caption })
                        .SingleOrDefault();
                if (circular != null)
                {
                    id = circular.id;
                    caseId = circular.caseId;
                    caption = circular.caption;
                }
            }

            QuestionnaireOverview overview = GetQuestionnaireEntity(id, languageId);
            return new QuestionnaireDetailedOverview { Questionnaire = overview, CaseId = caseId, Caption = caption };
        }

        public QuestionnaireOverview GetQuestionnaire(int id, OperationContext operationContext)
        {
            QuestionnaireOverview overview = this.GetQuestionnaireEntity(id, operationContext.LanguageId);
            return overview;
        }

        public List<OptionResult> GetResult(int circularId)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var questionnaireQuestionResultRepository = uof.GetRepository<QuestionnaireQuestionResultEntity>();

                List<OptionResult> overviews =
                    questionnaireQuestionResultRepository.GetAll()
                        .GetCircularQuestionnaireQuestionResultEntities(circularId)
                        .MapToOptionResults();

                return overviews;
            }
        }

        public List<OptionResult> GetResults(List<int> circularIds, DateTime? from, DateTime? to)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var questionnaireQuestionResultRepository = uof.GetRepository<QuestionnaireQuestionResultEntity>();

                List<OptionResult> overviews =
                    questionnaireQuestionResultRepository.GetAll()
                        .GetCircularsQuestionnaireQuestionResultEntities(circularIds)
                        .GetCircularDateFromQuestionnaireQuestionResultEntities(from)
                        .GetCircularDateToQuestionnaireQuestionResultEntities(to)
                        .MapToOptionResults()
                        .ToList();

                return overviews;
            }
        }

        public List<OptionResult> GetResults(int circularId, DateTime? from, DateTime? to)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var questionnaireQuestionResultRepository = uof.GetRepository<QuestionnaireQuestionResultEntity>();
                List<OptionResult> overviews =
                    questionnaireQuestionResultRepository.GetAll()
                        .GetCircularQuestionnaireQuestionResultEntities(circularId)
                        .GetQuestionResultDateFrom(from)
                        .GetQuestionResultDateTo(to)
                        .MapToOptionResults();

                return overviews;
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

        public List<BusinessLogic.MapperData.Participant> GetNotAnsweredParticipants(int circularId)
        {
            List<BusinessLogic.MapperData.Participant> connectedCases;

            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();
                var questionnaireResultRepository = uof.GetRepository<QuestionnaireResultEntity>();

                connectedCases = (from circularPart in circularPartRepository.GetAll().GetCircularCases(circularId)
                                  join participant in questionnaireResultRepository.GetAll() on circularPart.Id equals
                                      participant.QuestionnaireCircularPartic_Id into group1
                                  from g1 in group1.DefaultIfEmpty()
                                  where g1 == null
                                  select circularPart).MapToParticipants();
            }

            return connectedCases;
        }

        public List<int> GetAllCircularCasesIds(int circularId)
        {
            List<int> connectedCases;

            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();

                connectedCases =
                    circularPartRepository.GetAll().GetCircularCases(circularId).Select(c => c.Case_Id).Distinct().ToList();
            }

            return connectedCases;
        }

        public void SetStatus(int circularId, CircularStates circularState)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularRepository = uof.GetRepository<QuestionnaireCircularEntity>();

                QuestionnaireCircularEntity circular = circularRepository.GetById(circularId);
                circular.Status = (int)circularState;

                uof.Save();
            }
        }

        public void UpdateParticipantSendDate(Guid participantGuid, DateTime operationDate)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();

                QuestionnaireCircularPartEntity circularPart =
                    circularPartRepository.GetAll().GetByGuid(participantGuid).SingleOrDefault();

                if (circularPart != null)
                {
                    circularPart.SendDate = operationDate;
                }

                uof.Save();
            }
        }

		/// <summary>
		/// Returns circularId, if entity not found returns -1
		/// </summary>
		/// <param name="questionnaireId"></param>
		/// <returns></returns>
		public int GetCircularIdByQuestionnaireId(int questionnaireId)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularRepository = uof.GetRepository<QuestionnaireCircularEntity>();

                var entity = circularRepository.GetAll().SingleOrDefault(x => x.Questionnaire_Id == questionnaireId);
                return entity != null ? entity.Id : -1;
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

        private
           QuestionnaireOverview MapToQuestionnaireOverview(int languageId, QuestionnaireEntity anonymus)
        {
            var questions = (from question in anonymus.QuestionnaireQuestionEntities
                             let options = (from option in question.QuestionnaireQuestionOptionEntities
                                            let translatedOption =
                                                option.QuestionnaireQuesOpLangEntities.Where(
                                                    x => x.Language_Id == languageId).FirstOrDefault()
                                            let translatedOptionText =
                                                translatedOption != null
                                                && !string.IsNullOrWhiteSpace(
                                                    translatedOption.QuestionnaireQuestionOption)
                                                    ? translatedOption.QuestionnaireQuestionOption
                                                    : option.QuestionnaireQuestionOption
                                            select
                                                new QuestionnaireQuestionOptionOverview(
                                                option.Id,
                                                translatedOptionText,
                                                option.OptionValue,
                                                option.QuestionnaireQuestionOptionPos)).ToList()
                             let translatedQuestion =
                                 question.QuestionnaireQuesLangEntities.Where(x => x.Language_Id == languageId)
                                 .FirstOrDefault()
                             let translatedQuestionText =
                                 translatedQuestion != null
                                 && !string.IsNullOrWhiteSpace(translatedQuestion.QuestionnaireQuestion)
                                     ? translatedQuestion.QuestionnaireQuestion
                                     : question.QuestionnaireQuestion
                             let translatedNoteText =
                                 translatedQuestion != null && !string.IsNullOrWhiteSpace(translatedQuestion.NoteText)
                                     ? translatedQuestion.NoteText
                                     : question.NoteText
                             select
                                 new QuestionnaireQuestionOverview(
                                 question.Id,
                                 translatedQuestionText,
                                 question.QuestionnaireQuestionNumber,
                                 question.ShowNote.ToBool(),
                                 translatedNoteText,
                                 options)).ToList();

            QuestionnaireLanguageEntity translatedQuestionnaire =
                anonymus.QuestionnaireLanguageEntities.Where(x => x.Language_Id == languageId).FirstOrDefault();

            string translatedName = translatedQuestionnaire != null
                                    && !string.IsNullOrWhiteSpace(translatedQuestionnaire.QuestionnaireName)
                                        ? translatedQuestionnaire.QuestionnaireName
                                        : anonymus.QuestionnaireName;

            string translatedDescription = translatedQuestionnaire != null
                                           && !string.IsNullOrWhiteSpace(
                                               translatedQuestionnaire.QuestionnaireDescription)
                                               ? translatedQuestionnaire.QuestionnaireDescription
                                               : anonymus.QuestionnaireDescription;

            var questionnarie = new QuestionnaireOverview(anonymus.Id, translatedName, translatedDescription, questions);

            return questionnarie;
        }

        private QuestionnaireOverview GetQuestionnaireEntity(int id, int languageId)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var questionnaireRepository = uof.GetRepository<QuestionnaireEntity>();

                QuestionnaireEntity anonymus =
                    questionnaireRepository.GetAll(
                        x => x.QuestionnaireQuestionEntities,
                        x => x.QuestionnaireQuestionEntities.Select(y => y.QuestionnaireQuestionOptionEntities),
                        x => x.QuestionnaireLanguageEntities,
                        x => x.QuestionnaireQuestionEntities.Select(y => y.QuestionnaireQuesLangEntities),
                        x =>
                        x.QuestionnaireQuestionEntities.Select(
                            y => y.QuestionnaireQuestionOptionEntities.Select(z => z.QuestionnaireQuesOpLangEntities)))
                        .GetById(id)
                        .SingleOrDefault();

                return anonymus == null
                           ? QuestionnaireOverview.GetDefault()
                           : this.MapToQuestionnaireOverview(languageId, anonymus);
            }
        }

        private List<BusinessLogic.MapperData.Participant> GetAllParticipants(int circularId)
        {
            List<BusinessLogic.MapperData.Participant> connectedCases;

            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var circularPartRepository = uof.GetRepository<QuestionnaireCircularPartEntity>();

                connectedCases = circularPartRepository.GetAll().GetCircularCases(circularId).MapToParticipants();
            }

            return connectedCases;
        }

        private List<QuestionnaireMailItem> GetMails(
            string actionAbsolutePath,
            MailTemplate mailTemplate,
            List<BusinessLogic.MapperData.Participant> participants,
            int customerId)
        {
            var mails = new List<QuestionnaireMailItem>();

            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var customerRepository = uof.GetRepository<Customer>();

                string mailFrom = customerRepository.GetAll().GetById(customerId).Select(x => x.HelpdeskEmail).Single();

                mails.AddRange(
                    from connectedCase in participants
                    let markValues =
                        CreateMarkValues(
                            actionAbsolutePath,
                            connectedCase.Guid,
                            connectedCase.CaseNumber,
                            connectedCase.Caption,
                            connectedCase.CaseDescription)
                    let mail = this.mailTemplateFormatter.Format(mailTemplate, markValues)
                    select new QuestionnaireMailItem(connectedCase.Guid, mailFrom, connectedCase.Email, mail));
            }

            return mails;
        }

        private void SendMails(List<QuestionnaireMailItem> mailItems, DateTime operationDate, int customerId)
        {
            foreach (QuestionnaireMailItem mailItem in mailItems)
            {
                this.SendMail(mailItem, operationDate, customerId);
            }
        }

        private void SendMail(QuestionnaireMailItem mailItem, DateTime operationDate, int customerId)
        {
            var customerSetting = settingService.GetCustomerSetting(customerId);
            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }
            var mailResponse = EmailResponse.GetEmptyEmailResponse();
            var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);

            this.emailService.SendEmail(mailItem.MailItem, mailSetting);

            UpdateParticipantSendDate(mailItem.Guid, operationDate);
        }

        #endregion
    }
}