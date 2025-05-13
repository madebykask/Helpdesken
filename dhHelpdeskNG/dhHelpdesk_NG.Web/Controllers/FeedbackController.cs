using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
using DH.Helpdesk.BusinessData.Models.Grid;
using DH.Helpdesk.BusinessData.Models.Paging;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Input;
using DH.Helpdesk.BusinessData.Models.Questionnaire.Write;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Grid;
using DH.Helpdesk.Web.Enums;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure.Grid;
using DH.Helpdesk.Web.Infrastructure.ModelFactories.CaseLockMappers;
using DH.Helpdesk.Web.Infrastructure.UrlHelpers.Mvc;
using DH.Helpdesk.Web.Models.Case;
using DH.Helpdesk.Web.Models.CaseLock;
using DH.Helpdesk.Web.Models.Feedback;
using DH.Helpdesk.Web.Models.Questionnaire.Input;

namespace DH.Helpdesk.Web.Controllers
{
    public class FeedbackController : UserInteractionController
    {
        const string DefaultQuestionnaireNumber = "1";

        private readonly IQestionnaireQuestionOptionService _questionnaireQuestionOptionService;
        private readonly IQestionnaireQuestionService _questionnaireQuestionService;
        private readonly IFeedbackService _feedbackService;
        private readonly ICircularService _circularService;
        private readonly GridSettingsService _gridSettingsService;
        private readonly ICaseLockService _caseLockService;
        private readonly ISettingService _settingService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly ICaseSearchService _caseSearchService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseSettingsService _caseSettingService;
        private readonly IDepartmentService _departmentService;
        private readonly ICustomerUserService _customerUserService;
        private readonly ICaseTypeService _caseTypeService;

        public FeedbackController(IMasterDataService masterDataService, IQestionnaireQuestionOptionService questionnaireQuestionOptionService,
            IQestionnaireQuestionService questionnaireQuestionService, IFeedbackService feedbackService, ICircularService circularService,
            GridSettingsService gridSettingsService, ICaseLockService caseLockService, ISettingService settingService,
            IGlobalSettingService globalSettingService, ICaseSearchService caseSearchService, ICaseFieldSettingService caseFieldSettingService,
            ICaseSettingsService caseSettingService,
            IDepartmentService departmentService,
            ICustomerUserService customerUserService, ICaseTypeService caseTypeService) 
            : base(masterDataService)
        {
            _customerUserService = customerUserService;
            _caseTypeService = caseTypeService;
            _questionnaireQuestionOptionService = questionnaireQuestionOptionService;
            _questionnaireQuestionService = questionnaireQuestionService;
            _feedbackService = feedbackService;
            _circularService = circularService;
            _gridSettingsService = gridSettingsService;
            _caseLockService = caseLockService;
            _settingService = settingService;
            _globalSettingService = globalSettingService;
            _caseSearchService = caseSearchService;
            _caseFieldSettingService = caseFieldSettingService;
            _caseSettingService = caseSettingService;
            _departmentService = departmentService;
        }

        public ActionResult NewFeedback(EditFeedbackParams parameters)
        {
            return EditFeedback(parameters);
        }

        public ActionResult EditFeedback(EditFeedbackParams parameters)
        {
            FillEditLists();

            var model = new EditFeedbackModel
            {
                QuestionnaireId = parameters.FeedbackId.HasValue ? parameters.FeedbackId.Value : 0,
                LanguageId = parameters.LanguageId
            };

            if (parameters.FeedbackId.HasValue)
            {
                //Get feedback
                var feedback = _feedbackService.GetFeedback(parameters.FeedbackId.Value, parameters.LanguageId);
                model.Name = feedback.Name;
                model.Description = feedback.Description;
                model.Identifier = feedback.Identifier;
                model.ExcludeAdministrators = feedback.ExcludeAdministrators;
                model.UseBase64Images = feedback.UseBase64Images;

                //Get Feeddback question
                var feedbackQuestions =
                    _questionnaireQuestionService.FindQuestionnaireQuestionsOverviews(feedback.Id, feedback.LanguageId);
                if (feedbackQuestions != null && feedbackQuestions.Any())
                {
                    var question = feedbackQuestions.Single();
                    model.QuestionId = question.Id;
                    model.Question = question.Question;
                    model.NoteText = question.NoteText;

                    if (parameters.LanguageId != LanguageIds.Swedish)
                    {
                        var questionnaireQuestionSwedish =
                            _questionnaireQuestionService.GetQuestionnaireQuestionById(
                                question.Id,
                                LanguageIds.Swedish);
                        model.ShowNote = questionnaireQuestionSwedish.ShowNote;
                    }
                    else
                    {
                        model.ShowNote = question.ShowNote;
                    }

                    //Get Feedback question options
                    var feedbackQuestionOptions =
                        _questionnaireQuestionOptionService.FindQuestionnaireQuestionOptions(
                            question.Id,
                            feedback.LanguageId);
                    if (feedbackQuestionOptions != null && feedbackQuestionOptions.Any())
                    {
                        model.Options = feedbackQuestionOptions.Select(
                        q =>
                        new QuestionnaireQuesOptionModel
                        {
                            Id = q.Id,
                            LanguageId = q.LanguageId,
                            QuestionId = q.QuestionId,
                            ChangedDate = q.ChangedDate,
                            Option = q.Option,
                            OptionPos = q.OptionPos,
                            OptionValue = q.OptionValue,
                            IconId = q.IconId,
                            IconSrc = q.IconSrc != null ? FeedBack.ImgId + Convert.ToBase64String(q.IconSrc) : string.Empty
                        }).OrderBy(qq => qq.OptionPos).ToList();
                    }

                    //Get Feedback circular
                    var dbCircular = _circularService.GetSingleOrDefaultByQuestionnaireId(model.QuestionnaireId);
                    if (dbCircular != null)
                    {
                        model.CircularId = dbCircular.Id;
                        model.SelectedPercent = dbCircular.CaseFilter.SelectedProcent;
                        model.IsSent = dbCircular.Status == CircularStates.Sent;
                        model.CaseFilter.SelectedCaseTypes = dbCircular.CaseFilter.SelectedCaseTypes;
                    }
                }
            }

            return View("EditFeedback", model);
        }

        [HttpPost]
        public ActionResult EditFeedback(EditFeedbackModel model)
        {
            if (model.IsNew)
            {
                return CreateFeedback(model);
            }
            return UpdateFeedback(model);
        }

        private ActionResult CreateFeedback(EditFeedbackModel model)
        {
            Validate(model);
            if (!ModelState.IsValid)
            {
                FillEditLists();
                return View("EditFeedback", model);
            }

            //Create feedback
            var newFeedback = new NewQuestionnaire(
                model.Name,
                model.Description,
                SessionFacade.CurrentCustomer.Id,
                DateTime.Now);
            newFeedback.Identifier = model.Identifier;
            newFeedback.ExcludeAdministrators = model.ExcludeAdministrators;

            var feedbackId = _feedbackService.AddFeedback(newFeedback);

            //Create feedback question
            var newFeedbackQuestion = new NewQuestionnaireQuestion(
                feedbackId,
                DefaultQuestionnaireNumber,
                model.Question,
                model.ShowNote,
                model.NoteText,
                DateTime.Now);

            _questionnaireQuestionService.AddQuestionnaireQuestion(newFeedbackQuestion);

            CreateCircular(model.SelectedPercent, model.CaseFilter, feedbackId);

            return RedirectToAction(MvcUrlName.Feedback.Edit, new EditFeedbackParams
            {
                FeedbackId = feedbackId,
                LanguageId = model.LanguageId
            });
        }


        private ActionResult UpdateFeedback(EditFeedbackModel model)
        {
            Validate(model);
            if (!ModelState.IsValid)
            {
                FillEditLists();
                return View("EditFeedback", model);
            }

            var editFeedback = new EditQuestionnaire(
                model.QuestionnaireId,
                model.Name,
                model.Description,
                model.LanguageId,
                DateTime.Now);
            editFeedback.Identifier = model.Identifier;
            editFeedback.ExcludeAdministrators = model.ExcludeAdministrators;
            editFeedback.UseBase64Images = model.UseBase64Images;

            _feedbackService.UpdateFeedback(editFeedback);

            var editFeedbackQuestion = new EditQuestionnaireQuestion(
                model.QuestionId,
                model.QuestionnaireId,
                model.LanguageId,
                DefaultQuestionnaireNumber,
                model.Question,
                model.ShowNote,
                model.NoteText,
                DateTime.Now);

            _questionnaireQuestionService.UpdateQuestionnaireQuestion(editFeedbackQuestion);

            if (model.Options != null)
            {
                var now = DateTime.Now;
                foreach (var option in model.Options)
                {
                    var questionOption = new QuestionnaireQuesOption(
                        option.Id,
                        option.QuestionId,
                        option.OptionPos,
                        option.Option,
                        option.OptionValue,
                        model.LanguageId,
                        now);
                    questionOption.IconId = option.IconId;
                    questionOption.IconSrc = string.IsNullOrEmpty(option.IconSrc) || option.IconId != FeedBack.IconId ? null : Convert.FromBase64String(option.IconSrc.Split(',')[1]);
                    _questionnaireQuestionOptionService.UpdateQuestionnaireQuestionOption(questionOption);
                }
            }

            if (model.CircularId.HasValue)
            {
                var dbCircular = _circularService.GetById(model.CircularId.Value);
                if (dbCircular != null)
                {
                    dbCircular.CaseFilter.SelectedProcent = model.SelectedPercent;
                    dbCircular.CaseFilter.SelectedCaseTypes = model.CaseFilter.SelectedCaseTypes;

                    var circular = new CircularForUpdate(dbCircular.Id, dbCircular.CircularName, DateTime.Now,
                        dbCircular.CaseFilter);
                    _circularService.UpdateCircular(circular);
                }
            }
            else
            {
                CreateCircular(model.SelectedPercent, model.CaseFilter, model.QuestionnaireId);
            }

            return RedirectToAction(MvcUrlName.Feedback.Edit, new EditFeedbackParams
            {
                FeedbackId = model.QuestionnaireId,
                LanguageId = model.LanguageId
            });
        }

        [HttpPost]
        public ActionResult DeleteFeedback(DeleteFeedbackParams parameters)
        {
            _circularService.DeleteById(parameters.CircularId);
            _feedbackService.DeleteFeedbackById(parameters.FeedbackId);
            return RedirectToAction(MvcUrlName.Questionnaire.Index, MvcUrlName.Questionnaire.Controller, new { tab = "feedback"});
        }

        [HttpPost]
        public ActionResult DeleteQuestionOption(DeleteQuestionOptionParams parameters)
        {
            _questionnaireQuestionOptionService.DeleteQuestionnaireQuestionOptionById(parameters.OptionId, parameters.LanguageId);

            return RedirectToAction(MvcUrlName.Feedback.Edit, new EditFeedbackParams
            {
                FeedbackId = parameters.FeedbackId,
                LanguageId = parameters.LanguageId
            });
        }

        [HttpPost]
        public ActionResult AddQuestionOption(AddQuestionOptionParams parameters)
        {
            var newOption = new QuestionnaireQuesOption(
                1,
                parameters.QuestionId,
                parameters.OptionPos,
                parameters.OptionText,
                parameters.OptionValue,
                parameters.LanguageId,
                DateTime.Now);
            newOption.IconId = parameters.OptionIcon;
            newOption.IconSrc = string.IsNullOrEmpty(parameters.IconSrc) ? null : Convert.FromBase64String(parameters.IconSrc);

            _questionnaireQuestionOptionService.AddQuestionnaireQuestionOption(newOption);

            return Json(new
            {
                success = true,
                FeedbackId = parameters.FeedbackId,
                LanguageId = parameters.LanguageId
            });
        }

        [HttpGet]
        public ViewResult Statistics(int feedbackId)
        {
            var circularId = this._circularService.GetCircularIdByQuestionnaireId(feedbackId);
            if (circularId < 0)
            {
                throw new NullReferenceException("Missing Circular for Feedback. Feedback should contain Circular");
            }
            var results = this._circularService.GetResult(circularId);
            var feedbackOverview = this._circularService.GetQuestionnaire(feedbackId, OperationContext);
            var jsonCaseIndexViewModel = GetJsonCaseIndexViewModel();

            var departments = _departmentService.GetDepartments(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.DepartmentName).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.DepartmentName
            }).ToList();

            var viewModel = new FeedbackStatisticsViewModel(feedbackId, feedbackOverview, results, new StatisticsFilter(), jsonCaseIndexViewModel);
            viewModel.Departments = departments;

            return this.View("Statistics", viewModel);
        }

        [HttpPost]
        public PartialViewResult Statistics(int questionnaireId, StatisticsFilter statisticsFilter)
        {
            var circularId = this._circularService.GetCircularIdByQuestionnaireId(questionnaireId);
            if (circularId < 0)
            {
                throw new NullReferenceException("Missing Circular for Feedback. Feedback should contain Circular");
            }
            var questionnaire = this._circularService.GetQuestionnaire(
                questionnaireId,
                OperationContext);
            var results = this._circularService.GetResults(
                circularId,
                statisticsFilter.CircularCreatedDate.DateFrom,
                statisticsFilter.CircularCreatedDate.DateTo.GetEndOfDay(),
                statisticsFilter.Departments);
            var jsonCaseIndexViewModel = GetJsonCaseIndexViewModel();
            var viewModel = new FeedbackStatisticsViewModel(questionnaireId, questionnaire, results, new StatisticsFilter(), jsonCaseIndexViewModel);
            var random = new Random();
            viewModel.Emails = results.SelectMany(x => x.Emails).OrderBy(i => random.Next()).Take(statisticsFilter.EmailsCount).ToList();

            return this.PartialView("~/Views/Questionnaire/FeedBack/FeedbackStatisticsGrid.cshtml", viewModel);
        }

        [ValidateInput(false)]
        public ActionResult GetCases(FormCollection frm)
        {
            var customerSettings = _settingService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(SessionFacade.CurrentUser.TimeZoneId);

            var f = new CaseSearchFilter
            {
                CustomerId = SessionFacade.CurrentCustomer.Id,
                UserId = SessionFacade.CurrentUser.Id
            };
            f.CaseProgress = "9";
        
            var caseIds = frm.ReturnFormValue("caseIds").Split(',').Select(int.Parse).ToArray();
            var gridSettings = _gridSettingsService.GetForCustomerUserGrid(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentUser.UserGroupId,
                SessionFacade.CurrentUser.Id,
                GridSettingsService.CASE_CONNECTPARENT_GRID_ID);

            var sortBy = frm.ReturnFormValue(CaseFilterFields.OrderColumnNum);
            var sort = frm.ReturnFormValue(CaseFilterFields.OrderColumnDir);
            var sortDir = !string.IsNullOrEmpty(sort)
                ? GridSortOptions.SortDirectionFromString(sort)
                : SortingDirection.Asc;
            gridSettings.sortOptions.sortBy = sortBy;
            gridSettings.sortOptions.sortDir = sortDir;

            int recPerPage;
            int pageStart;
            if (int.TryParse(frm.ReturnFormValue(CaseFilterFields.PageSize), out recPerPage) &&
                int.TryParse(frm.ReturnFormValue(CaseFilterFields.PageStart), out pageStart))
            {
                f.PageInfo = new PageInfo
                {
                    PageSize = recPerPage,
                    PageNumber = recPerPage != 0 ? pageStart/recPerPage : 0
                };
                gridSettings.pageOptions.recPerPage = recPerPage;
            }
            var search = new Search
            {
                SortBy = gridSettings.sortOptions.sortBy,
                Ascending = gridSettings.sortOptions.sortDir == SortingDirection.Asc
            };

            var caseSettings = 
                _caseSettingService.GetCaseSettingsWithUser(f.CustomerId, SessionFacade.CurrentUser.Id, SessionFacade.CurrentUser.UserGroupId);

            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(f.CustomerId).ToArray();
            CaseRemainingTimeData remainingTimeData;
            CaseAggregateData aggregateData;

            var currentUserId = SessionFacade.CurrentUser.Id;
            var customerId = SessionFacade.CurrentCustomer.Id;
            var customerUserSettings = _customerUserService.GetCustomerUserSettings(customerId, currentUserId);

            var searchResult = _caseSearchService.Search(
                f,
                caseSettings,
                caseFieldSettings,
                SessionFacade.CurrentUser.Id,
                SessionFacade.CurrentUser.UserId,
                SessionFacade.CurrentUser.ShowNotAssignedWorkingGroups,
                SessionFacade.CurrentUser.UserGroupId,
                customerUserSettings.RestrictedCasePermission,
                search,
                SessionFacade.CurrentCustomer.WorkingDayStart,
                SessionFacade.CurrentCustomer.WorkingDayEnd,
                userTimeZone,
                ApplicationTypes.Helpdesk,
                false,
                out remainingTimeData,
                out aggregateData,
                null, null, caseIds);
            var outputFormatter = new OutputFormatter(customerSettings.IsUserFirstLastNameRepresentation == 1,
                userTimeZone);
            var data = BuildSearchResultData(searchResult.Items, gridSettings, outputFormatter);
            return Json(new
            {
                result = "success",
                data = data,
                recordsTotal = searchResult.Count,
                recordsFiltered = searchResult.Count,
            });
        }

        [HttpPost]
        public ActionResult UpdateOptionIcon(UpdateQuestionOptionIconParams data)
        {
            var imageSource = Convert.FromBase64String(data.Src);
            _questionnaireQuestionOptionService.UpdateQuestionnaireQuestionOptionIcon(data.OptionId, imageSource);
            return Json(new
            {
                success = true,
                data = Convert.ToBase64String(imageSource)
            });
        }

        #region Private

        private JsonCaseIndexViewModel GetJsonCaseIndexViewModel()
        {
            var gridSettings = _gridSettingsService.GetForCustomerUserGrid(
                            SessionFacade.CurrentCustomer.Id,
                            SessionFacade.CurrentUser.UserGroupId,
                            SessionFacade.CurrentUser.Id,
                            GridSettingsService.CASE_CONNECTPARENT_GRID_ID);

            var connectToParentGrid = gridSettings;
            var userSelectedGrid = _gridSettingsService.GetForCustomerUserGrid(
                            SessionFacade.CurrentCustomer.Id,
                            SessionFacade.CurrentUser.UserGroupId,
                            SessionFacade.CurrentUser.Id,
                            GridSettingsService.CASE_OVERVIEW_GRID_ID);
           
            var existingColumns = connectToParentGrid.columnDefs.Where(a => userSelectedGrid.columnDefs.Any(b => b.name == a.name)).ToList();
            var otherColumns = userSelectedGrid.columnDefs.Where(a => !existingColumns.Any(b => b.name == a.name)).ToList();

            var maxColumns = 7;
            if (existingColumns.Count < maxColumns)
            {
                var missingAmount = Math.Abs(maxColumns - existingColumns.Count);
                existingColumns.AddRange(otherColumns.Take(missingAmount));
                connectToParentGrid.columnDefs = existingColumns;
            }

            gridSettings = connectToParentGrid;

            var m = new JsonCaseIndexViewModel
            {
                PageSettings = new PageSettingsModel
                {
                    gridSettings = JsonGridSettingsMapper.ToJsonGridSettingsModel(
                            gridSettings,
                            SessionFacade.CurrentCustomer.Id,
                            7,
                            CaseColumnsSettingsModel.PageSizesModal.Select(x => x.Value).ToArray()),
                    refreshContent = 0,
                    messages = new Dictionary<string, string>()
                    {
                        {"information", Translation.GetCoreTextTranslation("Information")},
                        {
                            "records_limited_msg",
                            Translation.GetCoreTextTranslation("Antal ärende som visas är begränsade till 500.")
                        },
                    }
                }
            };
            return m;
        }

        private IList<Dictionary<string, object>> BuildSearchResultData(IList<CaseSearchResult> caseSearchResults, GridSettingsModel gridSettings, OutputFormatter outputFormatter)
        {
            var data = new List<Dictionary<string, object>>();
            var ids = caseSearchResults.Select(o => o.Id).ToArray();

            var casesLocks = _caseLockService.GetCasesLocks(ids);
            var globalSettings = _globalSettingService.GetGlobalSettings().FirstOrDefault();

            foreach (var searchRow in caseSearchResults)
            {
                var caseId = searchRow.Id;

                var jsRow = new Dictionary<string, object>
                {
                    {"case_id", searchRow.Id},
                    {"caseIconTitle", searchRow.CaseIcon.CaseIconTitle()},
                    {"caseIconUrl", $"/Content/icons/{searchRow.CaseIcon.CaseIconSrc()}"},
                    {"isUnread", searchRow.IsUnread},
                    {"isUrgent", searchRow.IsUrgent},
                    {"isClosed", searchRow.IsUrgent}
                };

                var caseLock = casesLocks.ContainsKey(caseId) ? casesLocks[caseId] : null;

                var caseLockModel = GetCaseLockModel(caseLock, searchRow.Id, SessionFacade.CurrentUser.Id, globalSettings, false);
                if (caseLockModel.IsLocked)
                {
                    jsRow.Add("isCaseLocked", caseLockModel.IsLocked);
                    jsRow.Add("caseLockedIconTitle", $"{caseLockModel.User.FirstName} {caseLockModel.User.SurName} ({caseLockModel.User.UserID})");
                    jsRow.Add("caseLockedIconUrl", $"/Content/icons/{GlobalEnums.CaseIcon.Locked.CaseIconSrc()}");
                }

                foreach (var col in gridSettings.columnDefs)
                {
                    var searchCol = searchRow.Columns.FirstOrDefault(it => it.Key == col.name);
                    jsRow.Add(col.name, searchCol != null ? outputFormatter.FormatField(searchCol) : string.Empty);
                }

                data.Add(jsRow);
            }

            return data;
        }

        private CaseLockModel GetCaseLockModel(ICaseLockOverview caseLock, int caseId, int userId, GlobalSetting globalSettings, bool isNeedLock = true)
        {
            CaseLockModel caseLockModel;

            var caseIsLocked = true;
            var extendedSec = (globalSettings != null && globalSettings.CaseLockExtendTime > 0 ? globalSettings.CaseLockExtendTime : 30);
            var timerInterval = (globalSettings != null ? globalSettings.CaseLockTimer : 0);
            var bufferTime = (globalSettings != null && globalSettings.CaseLockBufferTime > 0 ? globalSettings.CaseLockBufferTime : 60);
            var nowTime = DateTime.Now;

            if (caseLock == null)
            {
                // Case is not locked 
                caseIsLocked = false;
            }
            else
            {
                if ((caseLock.ExtendedTime.AddSeconds(bufferTime) < nowTime) ||
                    (caseLock.ExtendedTime.AddSeconds(bufferTime) >= nowTime &&
                     caseLock.UserId == userId &&
                     caseLock.BrowserSession == Session.SessionID))
                {
                    // Unlock case because user has leaved the Case in anormal way (Close browser/reset computer)
                    // Unlock case because current user was opened this case last time and recently
                    this._caseLockService.UnlockCaseByCaseId(caseId);
                    caseIsLocked = false;
                }
            }

            if (!caseIsLocked)
            {
                // Lock Case if it's not locked
                var now = DateTime.Now;
                var extendedLockTime = now.AddSeconds(extendedSec);
                var newLockGuid = Guid.NewGuid();

                var newCaseLock = new CaseLock(caseId, userId, newLockGuid, Session.SessionID, now, extendedLockTime);
                if (isNeedLock)
                    this._caseLockService.LockCase(newCaseLock);

                caseLockModel = newCaseLock.MapToViewModel(caseIsLocked, extendedSec, timerInterval);
            }
            else
            {
                caseLockModel = caseLock.MapToViewModel(caseIsLocked, extendedSec, timerInterval);
            }

            return caseLockModel;
        }

        private List<SelectListItem> GetPercents()
        {
            var lst = new List<SelectListItem>();

            for (var i = 5; i <= 100; i += 5)
            {
                lst.Add(new SelectListItem { Text = i.ToString(), Value =i.ToString() });
            }
            return lst;
        }


        private List<ItemOverview> GetLocalizedLanguages()
        {
            var languageOverviewsOrginal = _questionnaireQuestionService.FindActiveLanguageOverivews();
            if (languageOverviewsOrginal.Any())
            {
                return languageOverviewsOrginal.Select(
                        o =>
                            new ItemOverview(
                                Translation.GetCoreTextTranslation(o.Name),
                                o.Value.ToString())).ToList();

            }
            
            return new List<ItemOverview>();
        }


        private void CreateCircular(int selectedPercent, CircularCaseFilter caseFilter, int feedbackId)
        {
            var newCaseFilter = new BusinessData.Models.Questionnaire.CircularCaseFilter
            {
                SelectedProcent = selectedPercent,
                SelectedCaseTypes = caseFilter.SelectedCaseTypes
            };

            var circular = new CircularForInsert("Feedback", feedbackId,
                CircularStateId.ReadyToSend, DateTime.Now, new List<int>(), newCaseFilter);

            _circularService.AddCircular(circular);
        }

        private List<SelectListItem> GetIcons()
        {
            var lst = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "ic_sentiment_very_satisfied_black_24dp_1x.png",
                    Text = "Very Satisfied"
                },
                new SelectListItem
                {
                    Value = "ic_sentiment_satisfied_black_24dp_1x.png",
                    Text = "Satisfied"
                },
                new SelectListItem
                {
                    Value = "ic_sentiment_neutral_black_24dp_1x.png",
                    Text = "Neutral"
                },
                new SelectListItem
                {
                    Value = "ic_sentiment_dissatisfied_black_24dp_1x.png",
                    Text = "Dissatisfied"
                },
                new SelectListItem
                {
                    Value = "ic_sentiment_very_dissatisfied_black_24dp_1x.png",
                    Text = "Very Dissatisfied"
                },
                new SelectListItem
                {
                    Value = "ic_sentiment_shocked_black_24dp_1x.png",
                    Text = "Shocked"
                },
                new SelectListItem
                {
                    Value = "ic_sentiment_tongue_black_24dp_1x.png",
                    Text = "Tongue"
                },
                new SelectListItem
                {
                    Value = "ic_sentiment_silent_black_24dp_1x.png",
                    Text = "Tongue"
                },
                new SelectListItem
                {
                    Value = "ic_thumbs_up_black_24dp_1x.png",
                    Text = "Thumbs up"
                },
                new SelectListItem
                {
                    Value = "ic_thumbs_down_black_24dp_1x.png",
                    Text = "Thumbs down"
                },
                new SelectListItem
                {
                    Value = "ic_0_black_24dp_1x.png",
                    Text = "0"
                },
                new SelectListItem
                {
                    Value = "ic_1_black_24dp_1x.png",
                    Text = "1"
                },
                new SelectListItem
                {
                    Value = "ic_2_black_24dp_1x.png",
                    Text = "2"
                },
                new SelectListItem
                {
                    Value = "ic_3_black_24dp_1x.png",
                    Text = "3"
                },
                new SelectListItem
                {
                    Value = "ic_4_black_24dp_1x.png",
                    Text = "4"
                },
                new SelectListItem
                {
                    Value = "ic_5_black_24dp_1x.png",
                    Text = "5"
                },
                new SelectListItem
                {
                    Value = "ic_6_black_24dp_1x.png",
                    Text = "6"
                },
                new SelectListItem
                {
                    Value = "ic_7_black_24dp_1x.png",
                    Text = "7"
                },
                new SelectListItem
                {
                    Value = "ic_8_black_24dp_1x.png",
                    Text = "8"
                },
                new SelectListItem
                {
                    Value = "ic_9_black_24dp_1x.png",
                    Text = "9"
                },
                new SelectListItem
                {
                    Text = Translation.GetCoreTextTranslation("Lägg till"),
                    Value = FeedBack.IconId
                }
            };
            return lst;
        }

        private void Validate(EditFeedbackModel model)
        {
            //Check for unique identifier
            var feedbacks = _feedbackService.FindFeedbackOverviews(SessionFacade.CurrentCustomer.Id);
            if (feedbacks.Any(f => f.Id != model.QuestionnaireId && 
                f.Identifier.Equals(model.Identifier, StringComparison.InvariantCultureIgnoreCase)))
            {
                ModelState.AddModelError(model.NameOf(m => m.Identifier),
                    Translation.GetCoreTextTranslation("Unik identifierare krävs"));
            }
        }

        private void FillEditLists()
        {
            var languages = GetLocalizedLanguages();
            ViewBag.Languages = new SelectList(languages, "Value", "Name");
            ViewBag.Percents = GetPercents();
            ViewBag.IconsList = GetIcons();
            var caseTypes = _caseTypeService.GetAllCaseTypes(SessionFacade.CurrentCustomer.Id, true, true).ToList();
            ViewBag.CaseTypes = _caseTypeService.GetChildrenInRow(caseTypes, true)
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                .ToList();
        }

        #endregion

    }
}