﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.BusinessData.Models.CaseSolution;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.CaseSolution;
using DH.Helpdesk.Common.Extensions.DateTime;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Dal.NewInfrastructure;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.Cases;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services
{
    using IUnitOfWork = Dal.Infrastructure.IUnitOfWork;


    public interface ICaseSolutionService : IBaseCaseSolutionService
    {
        IList<CaseSolution> GetCaseSolutions(int customerId);
        IList<CaseTemplateData> GetSelfServiceCaseTemplates(int customerId);
        IList<CaseSolutionOverview> GetCustomerCaseSolutionsOverview(int customerId, int? userId = null);

        IList<ApplicationTypeEntity> GetApplicationTypes(int customerId);
        IList<CaseSolutionCategory> GetCaseSolutionCategories(int customerId);
        IList<CaseSolution> SearchAndGenerateCaseSolutions(int customerId, ICaseSolutionSearch SearchCaseSolutions, bool isFirstNamePresentation);

        void ApplyCaseSolution(ICaseEntity model, CaseSolution caseSolution);

        //int GetAntal(int customerId, int userid);
        CaseSolutionSchedule GetCaseSolutionSchedule(int id);

        DeleteMessage DeleteCaseSolution(int id, int customerId);
        DeleteMessage DeleteCaseSolutionCategory(int id);

        List<CaseTemplateCategoryNode> GetCaseSolutionCategoryTree(int customerId, int userId, CaseSolutionLocationShow location);
        void SaveCaseSolution(CaseSolution caseSolution, CaseSolutionSchedule caseSolutionSchedule, IList<CaseFieldSetting> CaseFieldSetting, out IDictionary<string, string> errors);
        void SaveCaseSolutionCategory(CaseSolutionCategory caseSolutionCategory, out IDictionary<string, string> errors);
        void SaveEmptyForm(Guid formGuid, int caseId);
        
        IList<WorkflowStepModel> GetWorkflowSteps(int customerId, Case case_, IList<int> workFlowCaseSolutionIds, bool isRelatedCase, UserOverview user, ApplicationType applicationType, int? templateId);

        IList<CaseSolution> GetCaseSolutions();
        IList<int> GetWorkflowCaseSolutionIds(int customerId);

        IList<CaseSolution_SplitToCaseSolutionEntity> GetSplitToCaseSolutionDescendants(CaseSolution self, int[] descendantIds);
        bool CheckIfExtendedFormExistForSolutionsInCategories(int customerId, List<int> list);
    }

    public class CaseSolutionService : BaseCaseSolutionService, ICaseSolutionService
    {
        private readonly ICaseSolutionCategoryRepository _caseSolutionCategoryRepository;
        private readonly ICaseSolutionScheduleRepository _caseSolutionScheduleRepository;

        private readonly ICaseSolutionSettingRepository _caseSolutionSettingRepository;
        private readonly IFormRepository _formRepository;
        private readonly ILinkService _linkService;
        private readonly ILinkRepository _linkRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheProvider _cache;

        private readonly ICaseSolutionConditionRepository _caseSolutionConditionRepository;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ICaseSolutionConditionService _caseSolutionConditionService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private readonly Dictionary<string, IList<WorkingGroupEntity>> _adminGroups =
            new Dictionary<string, IList<WorkingGroupEntity>>(StringComparer.OrdinalIgnoreCase);

        private readonly IComputerUserCategoryRepository _computerUserCategoryRepository;

        public CaseSolutionService(
            ICaseSolutionRepository caseSolutionRepository,
            ICaseSolutionCategoryRepository caseSolutionCategoryRepository,
            ICaseSolutionScheduleRepository caseSolutionScheduleRepository,
            ICaseSolutionSettingRepository caseSolutionSettingRepository,
            IFormRepository formRepository,
            ILinkRepository linkRepository,
            ILinkService linkService,
            IUnitOfWork unitOfWork,
            ICaseSolutionConditionRepository caseSolutionConditionRepository,
            ICacheProvider cache,
            IWorkingGroupService workingGroupService,
            ICaseSolutionConditionService caseSolutionCondtionService,
            IStateSecondaryService stateSecondaryService,
            IComputerUserCategoryRepository computerUserCategoryRepository,
            IUnitOfWorkFactory unitOfWorkFactory) 
            : base(caseSolutionRepository, caseSolutionCategoryRepository) 
        {
            _caseSolutionCategoryRepository = caseSolutionCategoryRepository;
            _caseSolutionScheduleRepository = caseSolutionScheduleRepository;
            _caseSolutionSettingRepository = caseSolutionSettingRepository;
            _linkRepository = linkRepository;
            _linkService = linkService;
            _formRepository = formRepository;
            _unitOfWork = unitOfWork;
            _cache = cache;
            _workingGroupService = workingGroupService;
            _caseSolutionConditionService = caseSolutionCondtionService;
            _caseSolutionConditionRepository = caseSolutionConditionRepository;
            _stateSecondaryService = stateSecondaryService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _computerUserCategoryRepository = computerUserCategoryRepository;
        }
        
        public IList<CaseSolutionOverview> GetCustomerCaseSolutionsOverview(int customerId, int? userId = null)
        {
            var customerCaseSolutions = GetCustomerCaseSolutionsCached(customerId);

            if (userId.HasValue && userId.Value > 0)
            {
                // restrict case solutions by user working groups
                var userWorkingGroups =
                    _workingGroupService.ListWorkingGroupsForUser(userId.Value);
                
                customerCaseSolutions =
                    customerCaseSolutions.Where(cs => cs.WorkingGroupId == null || userWorkingGroups.Contains(cs.WorkingGroupId.Value)).ToList();
            }

            return customerCaseSolutions;
        }

        //todo : check if case solutions can be passed as an arguement
        public List<CaseTemplateCategoryNode> GetCaseSolutionCategoryTree(int customerId, int userId, CaseSolutionLocationShow location)
        {
            var customerCaseSolutions = 
                GetCustomerCaseSolutionsOverview(customerId, userId) //uses cached version
                    .Where(cs => cs.ConnectedButton != 0 &&
                                 (
                                     location == CaseSolutionLocationShow.BothCaseOverviewAndInsideCase ||
                                     (location == CaseSolutionLocationShow.OnCaseOverview && cs.ShowOnCaseOverview != 0) ||
                                     (location == CaseSolutionLocationShow.InsideTheCase && cs.ShowInsideCase != 0)
                                 )).OrderBy(cs => cs.Name).ToList();

            #region Solutions Without Category

            var solutionsWithoutCategory =
                customerCaseSolutions.Where(cs => cs.CategoryId == null)
                    .Select(cs => CaseTemplateCategoryNode.CreateRoot(cs.CaseSolutionId, cs.Name))
                    .ToList();

            #endregion

            #region Solutions With Category

            var solutionsWithCategory = new List<CaseTemplateCategoryNode>();

            var allCategories = 
                _caseSolutionCategoryRepository.GetMany(c => c.Customer_Id == customerId).OrderBy(c => c.Name);

            foreach (var category in allCategories)
            {
                var categoryCaseSolutions =
                    customerCaseSolutions.Where(s => s.CategoryId == category.Id)
                        .OrderBy(cs => cs.Name)
                        .Select(cs => new CaseTemplateNode
                        {
                            CaseTemplateId = cs.CaseSolutionId,
                            CaseTemplateName = cs.Name ?? string.Empty,
                            WorkingGroup = cs.WorkingGroupName ?? string.Empty
                        }).ToList();

                var curCategory = new CaseTemplateCategoryNode
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name,
                    CaseTemplates = categoryCaseSolutions
                };

                solutionsWithCategory.Add(curCategory);
            }

            #endregion
            
            if (solutionsWithoutCategory.Any() && solutionsWithCategory.Any())
            {
                var maxLen = solutionsWithoutCategory.Concat(solutionsWithCategory).Max(x => x.CategoryName?.Length ?? 0);
                var separateLine = new CaseTemplateCategoryNode { CategoryName = new string('_', maxLen + 5) };
                solutionsWithoutCategory.Add(separateLine);
            }

            return solutionsWithCategory.Any()
                ? solutionsWithoutCategory.Concat(solutionsWithCategory).ToList()
                : solutionsWithoutCategory;
        }

        //TODO: review. not performance optimised
        [Obsolete("Use GetCustomerCaseSolutionsOverview instead")]
        public IList<CaseSolution> GetCaseSolutions(int customerId)
        {
            return CaseSolutionRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<CaseTemplateData> GetSelfServiceCaseTemplates(int customerId)
        {
            // x.ConnectedButton != 0 - exclude workflow steps templates
            return
                CaseSolutionRepository.GetMany(
                        x => x.Customer_Id == customerId &&
                             x.ShowInSelfService &&
                             x.Status > 0 &&
                             x.ConnectedButton != 0).AsQueryable()
                    .OrderBy(x => x.Name)
                    .Select(t => new CaseTemplateData()
                    {
                        Id = t.Id,
                        Name = t.Name,
                        ShortDescription = t.ShortDescription,
                        TemplatePath = t.TemplatePath,
                        CaseSolutionCategory_Id = t.CaseSolutionCategory_Id,
                        CaseSolutionCategoryName = t.CaseSolutionCategory != null ? t.CaseSolutionCategory.Name : null,
                        OrderNum = t.OrderNum,
                        ContainsExtendedForm = t.ExtendedCaseForms.Any()
                    })
                    .ToList();

        }

        //todo: review. not performance optimised
        public IList<CaseSolution> GetCaseSolutions()
        {
            return this.CaseSolutionRepository.GetMany(x => x.Status >= 0).OrderBy(x => x.Customer.Name).ThenBy(x => x.Name).ToList();
        }

        public IList<int> GetWorkflowCaseSolutionIds(int customerId)
        {
            //ConnectedButton == 0 - worfklow type
            var query = 
                CaseSolutionRepository.GetMany(x => x.Customer_Id == customerId && x.ConnectedButton == 0 && x.Status > 0)
                .AsQueryable();

            return query.Select(x => x.Id).ToList();
        }

        public IList<CaseSolution_SplitToCaseSolutionEntity> GetSplitToCaseSolutionDescendants(CaseSolution self, int[] descendantIds)
        {
            return 
                CaseSolutionRepository.GetMany(x => descendantIds.Contains(x.Id)).AsQueryable()
                    .Select(cs => new CaseSolution_SplitToCaseSolutionEntity
                    {
                        CaseSolution = self,
                        CaseSolution_Id  = self.Id,
                        SplitToCaseSolutionDescendant = cs,
                        SplitToCaseSolution_Id = cs.Id,

                    }).ToList();
        }

        public IList<ApplicationTypeEntity> GetApplicationTypes(int customerId)
        {
            List<ApplicationTypeEntity> items;
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                items = uow.GetRepository<ApplicationTypeEntity>().GetAll().OrderBy(x => x.Type).ToList();
            }
            return items;
        }

        public void ApplyCaseSolution(ICaseEntity model, CaseSolution caseTemplate)
        {
            model.ReportedBy = caseTemplate.ReportedBy.IfNullThenElse(model.ReportedBy);
            model.PersonsName = caseTemplate.PersonsName.IfNullThenElse(model.PersonsName);
            model.PersonsEmail = caseTemplate.PersonsEmail.IfNullThenElse(model.PersonsEmail);
            model.PersonsPhone = caseTemplate.PersonsPhone.IfNullThenElse(model.PersonsPhone);
            model.PersonsCellphone = caseTemplate.PersonsCellPhone.IfNullThenElse(model.PersonsCellphone);
            model.Region_Id = caseTemplate.Region_Id.IfNullThenElse(model.Region_Id);
            model.Department_Id = caseTemplate.Department_Id.IfNullThenElse(model.Department_Id);
            model.OU_Id = caseTemplate.OU_Id.IfNullThenElse(model.OU_Id);
            model.Place = caseTemplate.Place.IfNullThenElse(model.Place);
            model.UserCode = caseTemplate.UserCode.IfNullThenElse(model.UserCode);
            model.CostCentre = caseTemplate.CostCentre.IfNullThenElse(model.CostCentre);

            model.InventoryNumber = caseTemplate.InventoryNumber.IfNullThenElse(model.InventoryNumber);
            model.InventoryType = caseTemplate.InventoryType.IfNullThenElse(model.InventoryType);
            model.InventoryLocation = caseTemplate.InventoryLocation.IfNullThenElse(model.InventoryLocation);
            
            if (caseTemplate.CaseType_Id != null)
                model.CaseType_Id = caseTemplate.CaseType_Id.Value;

            model.RegistrationSourceCustomer_Id = caseTemplate.RegistrationSource.IfNullThenElse(model.RegistrationSourceCustomer_Id);
            model.ProductArea_Id = caseTemplate.ProductArea_Id.IfNullThenElse(model.ProductArea_Id);
            model.System_Id = caseTemplate.System_Id.IfNullThenElse(model.System_Id);
            model.Caption = caseTemplate.Caption.IfNullThenElse(model.Caption);
            model.Description = caseTemplate.Description.IfNullThenElse(model.Description);
            model.Priority_Id = caseTemplate.Priority_Id.IfNullThenElse(model.Priority_Id);
            model.Project_Id = caseTemplate.Project_Id.IfNullThenElse(model.Project_Id);
            model.Urgency_Id = caseTemplate.Urgency_Id.IfNullThenElse(model.Urgency_Id);
            model.Impact_Id = caseTemplate.Impact_Id.IfNullThenElse(model.Impact_Id);
            model.Category_Id = caseTemplate.Category_Id.IfNullThenElse(model.Category_Id);
            model.Supplier_Id = caseTemplate.Supplier_Id.IfNullThenElse(model.Supplier_Id);

            model.InvoiceNumber = caseTemplate.InvoiceNumber.IfNullThenElse(model.InvoiceNumber);
            model.ReferenceNumber = caseTemplate.ReferenceNumber.IfNullThenElse(model.ReferenceNumber);
            model.Miscellaneous = caseTemplate.Miscellaneous.IfNullThenElse(model.Miscellaneous);
            model.ContactBeforeAction = caseTemplate.ContactBeforeAction;
            model.SMS = caseTemplate.SMS;
            model.AgreedDate = caseTemplate.AgreedDate.IfNullThenElse(model.AgreedDate);
            model.Available = caseTemplate.Available.IfNullThenElse(model.Available);
            model.Cost = caseTemplate.Cost;
            model.OtherCost = caseTemplate.OtherCost;
            model.Currency = caseTemplate.Currency.IfNullThenElse(model.Currency);

            model.Performer_User_Id = caseTemplate.PerformerUser_Id.IfNullThenElse(model.Performer_User_Id);
            model.CausingPartId = caseTemplate.CausingPartId.IfNullThenElse(model.CausingPartId);
            model.WorkingGroup_Id = caseTemplate.CaseWorkingGroup_Id.IfNullThenElse(model.WorkingGroup_Id);
            model.Problem_Id = caseTemplate.Problem_Id.IfNullThenElse(model.Problem_Id);
            model.Change_Id = caseTemplate.Change_Id.IfNullThenElse(model.Change_Id);
            model.PlanDate = caseTemplate.PlanDate.IfNullThenElse(model.PlanDate);
            model.WatchDate = caseTemplate.WatchDate.IfNullThenElse(model.WatchDate);

            model.IsAbout_ReportedBy = caseTemplate.IsAbout_ReportedBy.IfNullThenElse(model.IsAbout_ReportedBy);
            model.IsAbout_PersonsName = caseTemplate.IsAbout_PersonsName.IfNullThenElse(model.IsAbout_PersonsName);
            model.IsAbout_PersonsEmail = caseTemplate.IsAbout_PersonsEmail.IfNullThenElse(model.IsAbout_PersonsEmail);
            model.IsAbout_PersonsPhone = caseTemplate.IsAbout_PersonsPhone.IfNullThenElse(model.IsAbout_PersonsPhone);
            model.IsAbout_PersonsCellPhone = caseTemplate.IsAbout_PersonsCellPhone.IfNullThenElse(model.IsAbout_PersonsCellPhone);
            model.IsAbout_Region_Id = caseTemplate.IsAbout_Region_Id.IfNullThenElse(model.IsAbout_Region_Id);
            model.IsAbout_Department_Id = caseTemplate.IsAbout_Department_Id.IfNullThenElse(model.IsAbout_Department_Id);
            model.IsAbout_OU_Id = caseTemplate.IsAbout_OU_Id.IfNullThenElse(model.IsAbout_OU_Id);
            model.IsAbout_CostCentre = caseTemplate.IsAbout_CostCentre.IfNullThenElse(model.IsAbout_CostCentre);
            model.IsAbout_Place = caseTemplate.IsAbout_Place.IfNullThenElse(model.IsAbout_Place);
            model.IsAbout_UserCode = caseTemplate.UserCode.IfNullThenElse(model.IsAbout_UserCode);

            model.Status_Id = caseTemplate.Status_Id.IfNullThenElse(model.Status_Id);
            model.StateSecondary_Id = caseTemplate.StateSecondary_Id.IfNullThenElse(model.StateSecondary_Id);
            model.Verified = caseTemplate.Verified;
            model.VerifiedDescription = caseTemplate.VerifiedDescription.IfNullThenElse(model.VerifiedDescription);
            model.SolutionRate = caseTemplate.SolutionRate.IfNullThenElse(model.SolutionRate);
            model.FinishingDescription = caseTemplate.FinishingDescription.IfNullThenElse(model.FinishingDescription);
        }

        public IList<WorkflowStepModel> GetWorkflowSteps(int customerId, Case caseEntity, IList<int> caseSolutionsIds, bool isRelatedCase, UserOverview user, ApplicationType applicationType, int? templateId)
        {
            var modelList = new List<WorkflowStepModel>();
            var workflowStepsContext = new WorkflowConditionsContext
            {
                CustomerId = customerId,
                Case = caseEntity,
                User = user,
                ApplicationType = applicationType,
                TemplateId = templateId,
                IsRelatedCase = isRelatedCase
            };

            var caseSolutionsWithConditions = CaseSolutionRepository.GetCaseSolutionsWithConditions(caseSolutionsIds);

            foreach (var cs in caseSolutionsWithConditions)
            {
                // load conditions for solutions from the loaded list (perf optimisation)
                var solutionConditions = caseSolutionsWithConditions.FirstOrDefault(x => x.CaseSolutionId == cs.CaseSolutionId);

                //set conditions to process
                workflowStepsContext.Conditions = solutionConditions?.Conditions;

                var res = ShowWorkflowStep(workflowStepsContext);
                if (res)
                {
                    var workFlowStepModel = new WorkflowStepModel
                    {
                        CaseTemplateId = cs.CaseSolutionId,
                        Name = cs.Name,
                        //If value exist in NextStepState - use it. Otherwise check if caseSolution.StateSecondary_id have value, otherwise return 0;
                        NextStep = cs.NextStepState ?? (cs.StateSecondary?.StateSecondaryId ?? 0) 
                    };

                    modelList.Add(workFlowStepModel);
                }
            }

            return modelList.ToList();
        }

        //todo: poor spaghetti-code implementation! Need to refactor to create separate extendable and maintainable class(es) aligned with SOLID and OOP principles
        // CaseDocumentService.ShowCaseDocument and CaseDocumentService.CheckCaseDocumentTextConditions similar method where same approach can be shared! 
        private bool ShowWorkflowStep(WorkflowConditionsContext ctx)
        {
            var _case = ctx.Case;
            var conditions = ctx.Conditions ?? new List<CaseSolutionConditionOverview>();
            var user = ctx.User; 

            //ALL conditions must be met
            var showWorkflowStep = false;

            //If no conditions are set in (CaseSolutionCondition) for the template, do not show step in list
            if (!conditions.Any())
                return false;

            foreach (var condition in conditions)
            {
                var conditionValue = condition.Values.Tidy().ToLower();
                var conditionKey = condition.Property.Tidy();

                if (conditionValue == "null")
                {
                    return true;
                }

                try
                {
                    var value = "";

                    //Check if case is related (either child or parent);
                    if (conditionKey.ToLower() == "case_relation")
                    {
                        if (ctx.IsRelatedCase)
                        {
                            value = "1";
                        }
                        else
                        {
                            value = "0";
                        }
                    }

                    //GET FROM APPLICATION
                    else if (conditionKey.ToLower() == "application_type")
                    {
                        value = ((int)ctx.ApplicationType).ToString();
                    }

                    //GET FROM USER
                    else if (conditionKey.ToLower().StartsWith("user_WorkingGroup.WorkingGroupGUID".ToLower()))
                    {
                        //Get working groups connected to "UserRole.Admin"

                        if (ctx.WorkingGroups == null)
                        {
                            //store int context for next methods calls
                            ctx.WorkingGroups = _workingGroupService.GetWorkingGroupsAdmin(ctx.CustomerId, user.Id);
                        }

                        var wgShowWorkflowStep = false;
                        var conditionValues = conditionValue.Split(',').Select(sValue => sValue.Trim()).ToArray();

                        for (int i = 0; i < conditionValues.Length; i++)
                        {
                            var val = conditionValues[i];
                            if (ctx.WorkingGroups.Where(x => x.WorkingGroupGuid.ToString().ToLower() == val).Count() > 0)
                            {
                                wgShowWorkflowStep = true;
                                //it is enough with one hit
                                break;
                            }
                        }

                        //no match
                        if (wgShowWorkflowStep == false)
                            return false;

                        //leave foreach loop
                        continue;
                    }
                    //GET FROM USER
                    else if (conditionKey.ToLower().StartsWith("user_"))
                    {
                        conditionKey = conditionKey.Replace("user_", "");
                        //Get value from Model by casting to dictionary and look for property name
                        value = user.GetType().GetProperty(conditionKey)?.GetValue(user, null).ToString();
                    }
                    //Get the specific property of Case in "Property_Name"
                    else if (_case != null && _case.Id != 0 && conditionKey.ToLower().StartsWith("case_"))
                    {
                        conditionKey = conditionKey.Replace("case_", "");

                        if (!conditionKey.Contains("."))
                        {
                            //Get value from Case Model by casting to dictionary and look for property name
                            value = _case.GetType().GetProperty(conditionKey)?.GetValue(_case, null).ToString();
                        }
                        else
                        {
                            var parentClassName = string.Concat(conditionKey.TakeWhile((c) => c != '.'));
                            var propertyName = conditionKey.Substring(conditionKey.LastIndexOf('.') + 1);

                            var parentClass = _case.GetType().GetProperty(parentClassName)?.GetValue(_case, null);
                            value = parentClass?.GetType().GetProperty(propertyName)?.GetValue(parentClass, null).ToString();
                        }
                    }

                    //if [Null] = we do not have any caseID yet.Only valid for Case Condition
                    else if (conditionKey.ToLower().StartsWith("case_"))
                    {
                        //Case is new, or value is not set for the specific property
                        value = "new";
                    }
                    //THIS IS USED ON NEW CASE, get templateId from the template that is opening the case.
                    else if (conditionKey.ToLower().StartsWith("casesolution_"))
                    {
                        conditionKey = conditionKey.Replace("casesolution_", "");

                        var templateId = ctx.TemplateId ?? 0;
                        if (templateId > 0)
                        {
                            if (ctx.Template == null)
                            {
                                //store int context for next methods calls
                                ctx.Template = GetCaseSolution(templateId);
                            }

                            var caseTemplate = ctx.Template;

                            if (!conditionKey.Contains("."))
                            {
                                //Get value from Case Solution Model by casting to dictionary and look for property name
                                value = caseTemplate.GetType().GetProperty(conditionKey)?.GetValue(caseTemplate, null).ToString();
                            }
                            else
                            {
                                var parentClassName = string.Concat(conditionKey.TakeWhile((c) => c != '.'));
                                var propertyName = conditionKey.Substring(conditionKey.LastIndexOf('.') + 1);

                                var parentClass = caseTemplate.GetType().GetProperty(parentClassName)?.GetValue(caseTemplate, null);
                                value = parentClass?.GetType().GetProperty(propertyName)?.GetValue(parentClass, null).ToString();
                            }
                        }
                    }

                    // Check conditions
                    if (!string.IsNullOrEmpty(value) && conditionValue.IndexOf(value.ToLower()) > -1)
                    {
                        showWorkflowStep = true;
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    //throw;
                    //TODO?
                    return false;
                }
            }

            //To be true all conditions needs to be fulfilled
            return showWorkflowStep;
        }

        // todo: refactor. code is a total mess!
        public IList<CaseSolution> SearchAndGenerateCaseSolutions(int customerId, ICaseSolutionSearch searchCaseSolutions, bool isFirstNamePresentation)
        {
            var query = (from cs in this.CaseSolutionRepository.GetMany(x => x.Customer_Id == customerId)
                         select cs);

            #region Search

            if (!string.IsNullOrEmpty(searchCaseSolutions.SearchCss))
            {
                searchCaseSolutions.SearchCss = searchCaseSolutions.SearchCss.ToLower();

                query = query.Where(x => (!string.IsNullOrEmpty(x.Caption) && x.Caption.ToLower().Contains(searchCaseSolutions.SearchCss))
                                      || (!string.IsNullOrEmpty(x.Description) && x.Description.ToLower().Contains(searchCaseSolutions.SearchCss))
                                      || (!string.IsNullOrEmpty(x.Miscellaneous) && x.Miscellaneous.ToLower().Contains(searchCaseSolutions.SearchCss))
                                      || (!string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(searchCaseSolutions.SearchCss))
                                      || (!string.IsNullOrEmpty(x.Text_External) && x.Text_External.ToLower().Contains(searchCaseSolutions.SearchCss))
                                      || (!string.IsNullOrEmpty(x.Text_Internal) && x.Text_Internal.ToLower().Contains(searchCaseSolutions.SearchCss))
                                   );
            }

            if (searchCaseSolutions.CategoryIds != null && searchCaseSolutions.CategoryIds.Any())
            {
                query = query.Where(x => x.CaseSolutionCategory_Id.HasValue && searchCaseSolutions.CategoryIds.Contains(x.CaseSolutionCategory_Id.Value));
            }


            if (searchCaseSolutions.OnlyActive == true)
            {
                query = query.Where(x => x.Status == 1);

            }
            else
            {
                query = query.Where(x => x.Status == 1 | x.Status == 0);
            }

            DataTable table = new DataTable();
            if (query.Count() > 0)
            {
                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("Name", typeof(string));
                table.Columns.Add("CategoryName", typeof(string));
                table.Columns.Add("CaseSolutionDescription", typeof(string));
                table.Columns.Add("ConnectedToButton", typeof(int));
                table.Columns.Add("CaseCaption", typeof(string));

                table.Columns.Add("PerformerUserName", typeof(string));
                table.Columns.Add("PriorityName", typeof(string));
                table.Columns.Add("IsActive", typeof(int));
                table.Columns.Add("SortOrder", typeof(int));

                foreach (var category in query)
                {
                    int id = 0;
                    if (category.Id != null)
                    {
                        id = category.Id;
                    }


                    string name = string.Empty;
                    if (category.Name != null)
                    {
                        name = category.Name;
                    }

                    string catname = string.Empty;
                    if (category.Category != null)
                    {
                        if (category.Category.Name != null)
                        {
                            catname = category.Category.Name;
                        }
                    }

                    string casedesc = string.Empty;
                    if (category.CaseSolutionDescription != null)
                    {
                        casedesc = category.CaseSolutionDescription;
                    }

                    int conbut = 0;
                    if (category.ConnectedButton != null)
                    {
                        conbut = (int)category.ConnectedButton;
                    }
                    else
                    {
                        conbut = 0;
                    }

                    string cap = string.Empty;
                    if (category.Caption != null)
                    {
                        cap = category.Caption;
                    }

                    string perf = string.Empty;
                    if (category.PerformerUser != null)
                    {
                        if (category.PerformerUser.FirstName != null && category.PerformerUser.SurName != null)
                        {
                            perf = category.PerformerUser.FirstName + " " + category.PerformerUser.SurName;
                        }
                    }

                    string prio = string.Empty;
                    if (category.Priority != null)
                    {
                        if (category.Priority.Name != null)
                        {
                            prio = category.Priority.Name;
                        }
                    }

                    int stat = 0;
                    if (category.Status != null)
                    {
                        stat = category.Status;
                    }

                    int sort = 0;
                    if (category.SortOrder != null)
                    {
                        sort = category.SortOrder;
                    }

                    table.Rows.Add(id, name, catname, casedesc, conbut, cap, perf, prio, stat, sort);
                }
            }

            var conditions = _caseSolutionConditionRepository.GetAll()
                        .OrderBy(z => z.CaseSolution_Id)
                        .ThenBy(z => z.Property_Name)
                        .ThenBy(z => z.Values)
                        .ToList();

            DataTable tableCond = new DataTable();
            if (conditions.Any())
            {
                tableCond.Columns.Add("Id_Cond", typeof(int));
                tableCond.Columns.Add("CaseSolution_Id", typeof(int));
                tableCond.Columns.Add("Property_Name", typeof(string));
                tableCond.Columns.Add("Values", typeof(string));

                foreach (var condition in conditions)
                {
                    int id = condition.Id; 
                    
                    int casedsolid = 0;
                    if (condition.CaseSolution_Id != null)
                    {
                        casedsolid = condition.CaseSolution_Id;
                    }

                    string pname = string.Empty;
                    if (condition.Property_Name != null)
                    {
                        pname = condition.Property_Name;
                    }
                    string values = string.Empty;
                    if (condition.Values != null)
                    {
                        values = condition.Values;
                    }
                    char[] delimiterChars = { ',' };
                    string[] words = values.Split(delimiterChars);

                    foreach (string s in words)
                    {
                        tableCond.Rows.Add(id, casedsolid, pname, s);
                    }
                }
            }

            var cresList = new List<CaseSolution>();
            var cmoList = new List<CaseSolutionConditionModel>();

            if (searchCaseSolutions.ApplicationIds != null | searchCaseSolutions.PriorityIds != null | searchCaseSolutions.ProductAreaIds != null | searchCaseSolutions.StatusIds != null | searchCaseSolutions.SubStatusIds != null | searchCaseSolutions.TemplateProductAreaIds != null | searchCaseSolutions.UserWGroupIds != null | searchCaseSolutions.WgroupIds != null)
            {
                if (searchCaseSolutions.ApplicationIds.Count > 0 | searchCaseSolutions.PriorityIds.Count > 0 | searchCaseSolutions.ProductAreaIds.Count > 0 | searchCaseSolutions.StatusIds.Count > 0 | searchCaseSolutions.SubStatusIds.Count > 0 | searchCaseSolutions.TemplateProductAreaIds.Count > 0 | searchCaseSolutions.UserWGroupIds.Count > 0 | searchCaseSolutions.WgroupIds.Count > 0)
                {
                    var results = from table1 in table.AsEnumerable()
                                  join table2 in tableCond.AsEnumerable() on (int)table1["Id"] equals (int)table2["CaseSolution_Id"]
                                  select new
                                  {
                                      Id = (int)table1["Id"],
                                      Name = (string)table1["Name"],
                                      CategoryName = (string)table1["CategoryName"],
                                      CaseSolutionDescription = (string)table1["CaseSolutionDescription"],
                                      ConnectedToButton = (int)table1["ConnectedToButton"],
                                      CaseCaption = (string)table1["CaseCaption"],
                                      PerformerUserName = (string)table1["PerformerUserName"],
                                      PriorityName = (string)table1["PriorityName"],
                                      IsActive = (int)table1["IsActive"],
                                      SortOrder = (int)table1["SortOrder"],
                                      Id_Cond = (int)table2["Id_Cond"],
                                      CaseSolution_Id = (int)table2["CaseSolution_Id"],
                                      Property_Name = (string)table2["Property_Name"],
                                      Values = (string)table2["Values"]
                                  };



                    if (searchCaseSolutions.SubStatusIds != null && searchCaseSolutions.SubStatusIds.Any())
                    {
                        foreach (CaseSolution ca in cresList)
                        {
                            IEnumerable<CaseSolutionConditionModel> cco = _caseSolutionConditionRepository.GetCaseSolutionConditions(ca.Id);

                            foreach (CaseSolutionConditionModel cm in cco)
                            {
                                cmoList.Add(cm);
                            }
                        }

                        if (cmoList != null && cmoList.Count() > 0)
                        {
                            var results1 = from d in cmoList
                                           where searchCaseSolutions.SubStatusIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                        else
                        {
                            var results1 = from c in results
                                           where searchCaseSolutions.SubStatusIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }


                    }

                    //Working group
                    if (searchCaseSolutions.WgroupIds != null && searchCaseSolutions.WgroupIds.Any())
                    {
                        foreach (CaseSolution ca in cresList)
                        {
                            IEnumerable<CaseSolutionConditionModel> cco = _caseSolutionConditionRepository.GetCaseSolutionConditions(ca.Id);

                            foreach (CaseSolutionConditionModel cm in cco)
                            {
                                cmoList.Add(cm);
                            }
                        }

                        if (cmoList != null && cmoList.Count() > 0)
                        {
                            var results1 = from d in cmoList
                                           where searchCaseSolutions.WgroupIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                        else
                        {
                            var results1 = from c in results
                                           where searchCaseSolutions.WgroupIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                    }

                    //Priority
                    if (searchCaseSolutions.PriorityIds != null && searchCaseSolutions.PriorityIds.Any())
                    {
                        foreach (CaseSolution ca in cresList)
                        {
                            IEnumerable<CaseSolutionConditionModel> cco = _caseSolutionConditionRepository.GetCaseSolutionConditions(ca.Id);

                            foreach (CaseSolutionConditionModel cm in cco)
                            {
                                cmoList.Add(cm);
                            }
                        }

                        if (cmoList != null && cmoList.Count() > 0)
                        {
                            var results1 = from d in cmoList
                                           where searchCaseSolutions.PriorityIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                        else
                        {
                            var results1 = from c in results
                                           where searchCaseSolutions.PriorityIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                    }

                    //Status
                    if (searchCaseSolutions.StatusIds != null && searchCaseSolutions.StatusIds.Any())
                    {
                        foreach (var cs in cresList)
                        {
                            var caseSolutionConditions = _caseSolutionConditionRepository.GetCaseSolutionConditions(cs.Id);
                            foreach (CaseSolutionConditionModel cm in caseSolutionConditions)
                            {
                                cmoList.Add(cm);
                            }
                        }

                        if (cmoList != null && cmoList.Count() > 0)
                        {
                            var results1 = from d in cmoList
                                           where searchCaseSolutions.StatusIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        var cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                        else
                        {
                            var results1 = from c in results
                                           where searchCaseSolutions.StatusIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                    }

                    //ProductArea
                    if (searchCaseSolutions.ProductAreaIds != null && searchCaseSolutions.ProductAreaIds.Any())
                    {
                        foreach (CaseSolution ca in cresList)
                        {
                            IEnumerable<CaseSolutionConditionModel> cco = _caseSolutionConditionRepository.GetCaseSolutionConditions(ca.Id);

                            foreach (CaseSolutionConditionModel cm in cco)
                            {
                                cmoList.Add(cm);
                            }
                        }

                        if (cmoList != null && cmoList.Count() > 0)
                        {
                            var results1 = from d in cmoList
                                           where searchCaseSolutions.ProductAreaIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                        else
                        {
                            var results1 = from c in results
                                           where searchCaseSolutions.ProductAreaIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                    }

                    //UserWGroup, ????????????
                    if (searchCaseSolutions.UserWGroupIds != null && searchCaseSolutions.UserWGroupIds.Any())
                    {
                        foreach (CaseSolution ca in cresList)
                        {
                            IEnumerable<CaseSolutionConditionModel> cco = _caseSolutionConditionRepository.GetCaseSolutionConditions(ca.Id);

                            foreach (CaseSolutionConditionModel cm in cco)
                            {
                                cmoList.Add(cm);
                            }
                        }

                        if (cmoList != null && cmoList.Count() > 0)
                        {
                            var results1 = from d in cmoList
                                           where searchCaseSolutions.UserWGroupIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                        else
                        {
                            var results1 = from c in results
                                           where searchCaseSolutions.UserWGroupIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                    }

                    //TemplateProduct, ????????????
                    if (searchCaseSolutions.TemplateProductAreaIds != null && searchCaseSolutions.TemplateProductAreaIds.Any())
                    {
                        foreach (CaseSolution ca in cresList)
                        {
                            IEnumerable<CaseSolutionConditionModel> cco = _caseSolutionConditionRepository.GetCaseSolutionConditions(ca.Id);

                            foreach (CaseSolutionConditionModel cm in cco)
                            {
                                cmoList.Add(cm);
                            }
                        }

                        if (cmoList != null && cmoList.Count() > 0)
                        {
                            var results1 = from d in cmoList
                                           where searchCaseSolutions.TemplateProductAreaIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                        else
                        {
                            var results1 = from c in results
                                           where searchCaseSolutions.TemplateProductAreaIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                    }

                    //Application, ????????????
                    if (searchCaseSolutions.ApplicationIds != null && searchCaseSolutions.ApplicationIds.Any())
                    {
                        foreach (CaseSolution ca in cresList)
                        {
                            IEnumerable<CaseSolutionConditionModel> cco = _caseSolutionConditionRepository.GetCaseSolutionConditions(ca.Id);

                            foreach (CaseSolutionConditionModel cm in cco)
                            {
                                cmoList.Add(cm);
                            }
                        }

                        if (cmoList != null && cmoList.Count() > 0)
                        {
                            var results1 = from d in cmoList
                                           where searchCaseSolutions.ApplicationIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                        else
                        {
                            var results1 = from c in results
                                           where searchCaseSolutions.ApplicationIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = CaseSolutionRepository.GetById(r.CaseSolution_Id);
                                        cresList.Add(cres);
                                    }
                                }
                                else
                                {
                                    cresList = new List<CaseSolution>();
                                }
                            }
                            else
                            {
                                cresList = new List<CaseSolution>();
                            }
                        }
                    }

                    if (searchCaseSolutions.ApplicationIds == null && searchCaseSolutions.PriorityIds == null && searchCaseSolutions.ProductAreaIds == null && searchCaseSolutions.StatusIds == null && searchCaseSolutions.SubStatusIds == null && searchCaseSolutions.TemplateProductAreaIds == null && searchCaseSolutions.UserWGroupIds == null && searchCaseSolutions.WgroupIds == null)
                    {
                        if (cresList.Count == 0 | cresList == null)
                        {
                            if (results != null)
                            {
                                foreach (var r in results)
                                {

                                    CaseSolution cres = CaseSolutionRepository.GetById(r.Id);

                                    cresList.Add(cres);
                                }
                            }
                        }
                    }

                    if (searchCaseSolutions.ApplicationIds != null && searchCaseSolutions.PriorityIds != null && searchCaseSolutions.ProductAreaIds != null && searchCaseSolutions.StatusIds != null && searchCaseSolutions.SubStatusIds != null && searchCaseSolutions.TemplateProductAreaIds != null && searchCaseSolutions.UserWGroupIds != null && searchCaseSolutions.WgroupIds != null)
                    {
                        if (searchCaseSolutions.ApplicationIds.Count == 0 && searchCaseSolutions.PriorityIds.Count == 0 && searchCaseSolutions.ProductAreaIds.Count == 0 && searchCaseSolutions.StatusIds.Count == 0 && searchCaseSolutions.SubStatusIds.Count == 0 && searchCaseSolutions.TemplateProductAreaIds.Count == 0 && searchCaseSolutions.UserWGroupIds.Count == 0 && searchCaseSolutions.WgroupIds.Count == 0)
                        {

                            if (cresList.Count == 0 | cresList == null)
                            {
                                if (results != null)
                                {
                                    foreach (var r in results)
                                    {

                                        CaseSolution cres = CaseSolutionRepository.GetById(r.Id);

                                        cresList.Add(cres);
                                    }
                                }
                            }
                        }
                    }

                }
                else //HÄR
                {

                    var results = from table1 in table.AsEnumerable()
                                  select new
                                  {
                                      Id = (int)table1["Id"],
                                      Name = (string)table1["Name"],
                                      CategoryName = (string)table1["CategoryName"],
                                      CaseSolutionDescription = (string)table1["CaseSolutionDescription"],
                                      ConnectedToButton = (int)table1["ConnectedToButton"],
                                      CaseCaption = (string)table1["CaseCaption"],
                                      PerformerUserName = (string)table1["PerformerUserName"],
                                      PriorityName = (string)table1["PriorityName"],
                                      IsActive = (int)table1["IsActive"],
                                      SortOrder = (int)table1["SortOrder"],
                                      Id_Cond = 0,
                                      CaseSolution_Id = 0,
                                      Property_Name = "",
                                      Values = ""
                                  };



                    if (searchCaseSolutions.ApplicationIds == null && searchCaseSolutions.PriorityIds == null && searchCaseSolutions.ProductAreaIds == null && searchCaseSolutions.StatusIds == null && searchCaseSolutions.SubStatusIds == null && searchCaseSolutions.TemplateProductAreaIds == null && searchCaseSolutions.UserWGroupIds == null && searchCaseSolutions.WgroupIds == null)
                    {
                        if (cresList.Count == 0 | cresList == null)
                        {
                            if (results != null)
                            {
                                foreach (var r in results)
                                {

                                    CaseSolution cres = CaseSolutionRepository.GetById(r.Id);

                                    cresList.Add(cres);
                                }
                            }
                        }
                    }

                    if (searchCaseSolutions.ApplicationIds != null && searchCaseSolutions.PriorityIds != null && searchCaseSolutions.ProductAreaIds != null && searchCaseSolutions.StatusIds != null && searchCaseSolutions.SubStatusIds != null && searchCaseSolutions.TemplateProductAreaIds != null && searchCaseSolutions.UserWGroupIds != null && searchCaseSolutions.WgroupIds != null)
                    {
                        if (searchCaseSolutions.ApplicationIds.Count == 0 && searchCaseSolutions.PriorityIds.Count == 0 && searchCaseSolutions.ProductAreaIds.Count == 0 && searchCaseSolutions.StatusIds.Count == 0 && searchCaseSolutions.SubStatusIds.Count == 0 && searchCaseSolutions.TemplateProductAreaIds.Count == 0 && searchCaseSolutions.UserWGroupIds.Count == 0 && searchCaseSolutions.WgroupIds.Count == 0)
                        {

                            if (cresList.Count == 0 | cresList == null)
                            {
                                if (results != null)
                                {
                                    foreach (var r in results)
                                    {

                                        CaseSolution cres = CaseSolutionRepository.GetById(r.Id);

                                        cresList.Add(cres);
                                    }
                                }
                            }
                        }
                    }
                }


            }
            else
            {
                var results = from table1 in table.AsEnumerable()
                              select new
                              {
                                  Id = (int)table1["Id"],
                                  Name = (string)table1["Name"],
                                  CategoryName = (string)table1["CategoryName"],
                                  CaseSolutionDescription = (string)table1["CaseSolutionDescription"],
                                  ConnectedToButton = (int)table1["ConnectedToButton"],
                                  CaseCaption = (string)table1["CaseCaption"],
                                  PerformerUserName = (string)table1["PerformerUserName"],
                                  PriorityName = (string)table1["PriorityName"],
                                  IsActive = (int)table1["IsActive"],
                                  SortOrder = (int)table1["SortOrder"],
                                  Id_Cond = 0,
                                  CaseSolution_Id = 0,
                                  Property_Name = "",
                                  Values = ""
                              };



                if (searchCaseSolutions.ApplicationIds == null && searchCaseSolutions.PriorityIds == null && searchCaseSolutions.ProductAreaIds == null && searchCaseSolutions.StatusIds == null && searchCaseSolutions.SubStatusIds == null && searchCaseSolutions.TemplateProductAreaIds == null && searchCaseSolutions.UserWGroupIds == null && searchCaseSolutions.WgroupIds == null)
                {
                    if (cresList.Count == 0 | cresList == null)
                    {
                        if (results != null)
                        {
                            foreach (var r in results)
                            {

                                CaseSolution cres = CaseSolutionRepository.GetById(r.Id);

                                cresList.Add(cres);
                            }
                        }
                    }
                }

                if (searchCaseSolutions.ApplicationIds != null && searchCaseSolutions.PriorityIds != null && searchCaseSolutions.ProductAreaIds != null && searchCaseSolutions.StatusIds != null && searchCaseSolutions.SubStatusIds != null && searchCaseSolutions.TemplateProductAreaIds != null && searchCaseSolutions.UserWGroupIds != null && searchCaseSolutions.WgroupIds != null)
                {
                    if (searchCaseSolutions.ApplicationIds.Count == 0 && searchCaseSolutions.PriorityIds.Count == 0 && searchCaseSolutions.ProductAreaIds.Count == 0 && searchCaseSolutions.StatusIds.Count == 0 && searchCaseSolutions.SubStatusIds.Count == 0 && searchCaseSolutions.TemplateProductAreaIds.Count == 0 && searchCaseSolutions.UserWGroupIds.Count == 0 && searchCaseSolutions.WgroupIds.Count == 0)
                    {

                        if (cresList.Count == 0 | cresList == null)
                        {
                            if (results != null)
                            {
                                foreach (var r in results)
                                {

                                    CaseSolution cres = CaseSolutionRepository.GetById(r.Id);

                                    cresList.Add(cres);
                                }
                            }
                        }
                    }
                }

            }

            //Sub status



            #endregion

            #region Sort



            cresList = cresList.GroupBy(test => test.Id)
               .Select(grp => grp.First())
               .ToList();

            if (!string.IsNullOrEmpty(searchCaseSolutions.SortBy) && (searchCaseSolutions.SortBy != "undefined"))
            {
                switch (searchCaseSolutions.SortBy)
                {

                    case CaseSolutionIndexColumns.Name:


                        var query1 = (searchCaseSolutions.Ascending) ?
                                cresList.OrderBy(l => (l.Name != null ? l.Name : string.Empty)) :
                                cresList.OrderByDescending(l => (l.Name != null ? l.Name : string.Empty));

                        return query1.ToList();

                        break;

                    case CaseSolutionIndexColumns.Category:
                        query1 = (searchCaseSolutions.Ascending) ?
                                cresList.OrderBy(l => (l.CaseSolutionCategory != null ? l.CaseSolutionCategory.Name : string.Empty)) :
                                cresList.OrderByDescending(l => (l.CaseSolutionCategory != null ? l.CaseSolutionCategory.Name : string.Empty));

                        return query1.ToList();
                        break;

                    case CaseSolutionIndexColumns.Caption:
                        query1 = (searchCaseSolutions.Ascending) ?
                                cresList.OrderBy(l => (l.Caption != null ? l.Caption : string.Empty)) :
                                cresList.OrderByDescending(l => (l.Caption != null ? l.Caption : string.Empty));

                        return query1.ToList();
                        break;

                    case CaseSolutionIndexColumns.Administrator:
                        if (searchCaseSolutions.Ascending)
                            query1 = isFirstNamePresentation ?
                                        cresList.OrderBy(l => (l.PerformerUser != null ? l.PerformerUser.FirstName : string.Empty))
                                            .ThenBy(l => (l.PerformerUser != null ? l.PerformerUser.SurName : string.Empty)) :

                                        cresList.OrderBy(l => (l.PerformerUser != null ? l.PerformerUser.SurName : string.Empty))
                                        .ThenBy(l => (l.PerformerUser != null ? l.PerformerUser.FirstName : string.Empty));
                        else
                            query1 = isFirstNamePresentation ?
                                       cresList.OrderByDescending(l => (l.PerformerUser != null ? l.PerformerUser.FirstName : string.Empty))
                                           .ThenByDescending(l => (l.PerformerUser != null ? l.PerformerUser.SurName : string.Empty)) :

                                       cresList.OrderByDescending(l => (l.PerformerUser != null ? l.PerformerUser.SurName : string.Empty))
                                       .ThenByDescending(l => (l.PerformerUser != null ? l.PerformerUser.FirstName : string.Empty));

                        return query1.ToList();
                        break;

                    case CaseSolutionIndexColumns.Priority:
                        query1 = (searchCaseSolutions.Ascending) ?
                                    cresList.OrderBy(l => (l.Priority != null ? l.Priority.Name : string.Empty)) :
                                    cresList.OrderByDescending(l => (l.Priority != null ? l.Priority.Name : string.Empty));

                        return query1.ToList();
                        break;


                    case CaseSolutionIndexColumns.Status:
                        query1 = (searchCaseSolutions.Ascending) ?
                                cresList.OrderBy(l => l.Status) :
                                cresList.OrderByDescending(l => l.Status);

                        return query1.ToList();
                        break;

                    case CaseSolutionIndexColumns.ConnectedToButton:
                        query1 = (searchCaseSolutions.Ascending) ?
                                cresList.OrderBy(l => l.ConnectedButton) :
                                cresList.OrderByDescending(l => l.ConnectedButton);

                        return query1.ToList();
                        break;
                    case CaseSolutionIndexColumns.SortOrder:
                        query1 = (searchCaseSolutions.Ascending) ?
                                cresList.OrderBy(l => l.SortOrder) :
                                cresList.OrderByDescending(l => l.SortOrder);

                        return query1.ToList();
                        break;
                    default:
                        query1 = (searchCaseSolutions.Ascending) ?
                                cresList.OrderBy(l => (l.Name != null ? l.Name : string.Empty)) :
                                cresList.OrderByDescending(l => (l.Name != null ? l.Name : string.Empty));

                        return query1.ToList();
                        break;


                }


            }
            else
            {
                var query1 = cresList;
                return query1.ToList();
            }
            //if (!string.IsNullOrEmpty(SearchCaseSolutions.SortBy) && (SearchCaseSolutions.SortBy != "undefined"))
            //{
            //    switch (SearchCaseSolutions.SortBy)
            //    {

            //        case CaseSolutionIndexColumns.Name:
            //            query = (SearchCaseSolutions.Ascending) ?
            //                    query.OrderBy(l => (l.Name != null ? l.Name : string.Empty)) :
            //                    query.OrderByDescending(l => (l.Name != null ? l.Name : string.Empty));
            //            break;

            //        case CaseSolutionIndexColumns.Category:
            //            query = (SearchCaseSolutions.Ascending) ?
            //                    query.OrderBy(l => (l.CaseSolutionCategory != null ? l.CaseSolutionCategory.Name : string.Empty)) :
            //                    query.OrderByDescending(l => (l.CaseSolutionCategory != null ? l.CaseSolutionCategory.Name : string.Empty));
            //            break;

            //        case CaseSolutionIndexColumns.Caption:
            //            query = (SearchCaseSolutions.Ascending) ?
            //                    query.OrderBy(l => (l.Caption != null ? l.Caption : string.Empty)) :
            //                    query.OrderByDescending(l => (l.Caption != null ? l.Caption : string.Empty));
            //            break;

            //        case CaseSolutionIndexColumns.Administrator:
            //            if (SearchCaseSolutions.Ascending)
            //                query = isFirstNamePresentation ?
            //                            query.OrderBy(l => (l.PerformerUser != null ? l.PerformerUser.FirstName : string.Empty))
            //                                .ThenBy(l => (l.PerformerUser != null ? l.PerformerUser.SurName : string.Empty)) :

            //                            query.OrderBy(l => (l.PerformerUser != null ? l.PerformerUser.SurName : string.Empty))
            //                            .ThenBy(l => (l.PerformerUser != null ? l.PerformerUser.FirstName : string.Empty));
            //            else
            //                query = isFirstNamePresentation ?
            //                           query.OrderByDescending(l => (l.PerformerUser != null ? l.PerformerUser.FirstName : string.Empty))
            //                               .ThenByDescending(l => (l.PerformerUser != null ? l.PerformerUser.SurName : string.Empty)) :

            //                           query.OrderByDescending(l => (l.PerformerUser != null ? l.PerformerUser.SurName : string.Empty))
            //                           .ThenByDescending(l => (l.PerformerUser != null ? l.PerformerUser.FirstName : string.Empty));
            //            break;

            //        case CaseSolutionIndexColumns.Priority:
            //            query = (SearchCaseSolutions.Ascending) ?
            //                        query.OrderBy(l => (l.Priority != null ? l.Priority.Name : string.Empty)) :
            //                        query.OrderByDescending(l => (l.Priority != null ? l.Priority.Name : string.Empty));
            //            break;


            //        case CaseSolutionIndexColumns.Status:
            //            query = (SearchCaseSolutions.Ascending) ?
            //                    query.OrderBy(l => l.Status) :
            //                    query.OrderByDescending(l => l.Status);
            //            break;

            //        case CaseSolutionIndexColumns.ConnectedToButton:
            //            query = (SearchCaseSolutions.Ascending) ?
            //                    query.OrderBy(l => l.ConnectedButton) :
            //                    query.OrderByDescending(l => l.ConnectedButton);
            //            break;
            //        case CaseSolutionIndexColumns.SortOrder:
            //            query = (SearchCaseSolutions.Ascending) ?
            //                    query.OrderBy(l => l.SortOrder) :
            //                    query.OrderByDescending(l => l.SortOrder);
            //            break;
            //        default:
            //            query = (SearchCaseSolutions.Ascending) ?
            //                    query.OrderBy(l => (l.Name != null ? l.Name : string.Empty)) :
            //                    query.OrderByDescending(l => (l.Name != null ? l.Name : string.Empty));
            //            break;
            //    }
            //}

            #endregion


            //return query.ToList();
        }

        //public  IEnumerable<DataRow> AsEnumerable(this DataTable table)
        //{
        //    return table.Rows.Cast<DataRow>();
        //}

        public IList<CaseSolutionCategory> GetCaseSolutionCategories(int customerId)
        {
            return this._caseSolutionCategoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public CaseSolution GetCaseSolution(int id)
        {
            return this.CaseSolutionRepository.GetById(id);
        }

        public CaseSolutionCategory GetCaseSolutionCategory(int id)
        {
            return this._caseSolutionCategoryRepository.GetById(id);
        }

        public CaseSolutionSchedule GetCaseSolutionSchedule(int id)
        {
            return this._caseSolutionScheduleRepository.GetById(id);
        }

        public DeleteMessage DeleteCaseSolution(int id, int customerId)
        {
            var caseSolution = this.CaseSolutionRepository.GetById(id);

            if (caseSolution != null)
            {
                try
                {
                    _caseSolutionConditionRepository.DeleteByCaseSolutionId(id);
                    //this.caseSolutionConditionRepository.Commit();

                    this._caseSolutionSettingRepository.DeleteByCaseSolutionId(id);
                    this._caseSolutionSettingRepository.Commit();

                    var caseSolutionSchedule = this._caseSolutionScheduleRepository.GetById(id);

                    if (caseSolutionSchedule != null)
                        this._caseSolutionScheduleRepository.Delete(caseSolutionSchedule);

                    var caseSolutionLinks = this._linkService.GetLinksBySolutionIdAndCustomer(id, customerId);
                    if (caseSolutionLinks.Count > 0)
                        foreach (var link in caseSolutionLinks)
                            this._linkRepository.Delete(x => x.Id == link.Id);

                    this.CaseSolutionRepository.Delete(caseSolution);

                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch (Exception ex)
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public DeleteMessage DeleteCaseSolutionCategory(int id)
        {
            var caseSolutionCategory = this._caseSolutionCategoryRepository.GetById(id);

            if (caseSolutionCategory != null)
            {
                try
                {
                    this._caseSolutionCategoryRepository.Delete(caseSolutionCategory);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SaveCaseSolution(CaseSolution caseSolution, CaseSolutionSchedule caseSolutionSchedule, IList<CaseFieldSetting> CaseFieldSetting, out IDictionary<string, string> errors)
        {
            if (caseSolution == null)
                throw new ArgumentNullException("casesolution");

            errors = new Dictionary<string, string>();

            caseSolution.Caption = caseSolution.Caption ?? string.Empty;
            caseSolution.Description = caseSolution.Description ?? string.Empty;
            caseSolution.Miscellaneous = caseSolution.Miscellaneous ?? string.Empty;
            caseSolution.ReportedBy = caseSolution.ReportedBy ?? string.Empty;
            caseSolution.Text_External = caseSolution.Text_External ?? string.Empty;
            caseSolution.Text_Internal = caseSolution.Text_Internal ?? string.Empty;
            caseSolution.PersonsName = caseSolution.PersonsName ?? string.Empty;
            caseSolution.PersonsPhone = caseSolution.PersonsPhone ?? string.Empty;
            caseSolution.PersonsEmail = caseSolution.PersonsEmail ?? string.Empty;
            caseSolution.Place = caseSolution.Place ?? string.Empty;
            caseSolution.UserCode = caseSolution.UserCode ?? string.Empty;
            caseSolution.InvoiceNumber = caseSolution.InvoiceNumber ?? string.Empty;
            caseSolution.ReferenceNumber = caseSolution.ReferenceNumber ?? string.Empty;
            caseSolution.VerifiedDescription = caseSolution.VerifiedDescription ?? string.Empty;
            caseSolution.SolutionRate = caseSolution.SolutionRate ?? string.Empty;
            caseSolution.InventoryNumber = caseSolution.InventoryNumber ?? string.Empty;
            caseSolution.InventoryType = caseSolution.InventoryType ?? string.Empty;
            caseSolution.InventoryLocation = caseSolution.InventoryLocation ?? string.Empty;
            caseSolution.Available = caseSolution.Available ?? string.Empty;
            caseSolution.Currency = caseSolution.Currency ?? string.Empty;

            if (caseSolution.Text_External != null && caseSolution.Text_External.Length > 3000)
                caseSolution.Text_External = caseSolution.Text_External.Substring(0, 3000);

            if (caseSolution.Text_Internal != null && caseSolution.Text_Internal.Length > 3000)
                caseSolution.Text_Internal = caseSolution.Text_Internal.Substring(0, 3000);

            if (caseSolution.Id == 0)
                this.CaseSolutionRepository.Add(caseSolution);
            else
            {
                this.CaseSolutionRepository.Update(caseSolution);
                this._caseSolutionScheduleRepository.Delete(x => x.CaseSolution_Id == caseSolution.Id);
            }

            if (caseSolutionSchedule != null)
            {
                this._caseSolutionScheduleRepository.Add(caseSolutionSchedule);
            }

            if (errors.Count == 0)
            { 
                this.Commit();

                DeleteSplitToCaseSolutionDescendants(caseSolution.Id);

                if (caseSolution.SplitToCaseSolutionDescendants != null)
                {
                    foreach (var item in caseSolution.SplitToCaseSolutionDescendants)
                    {
                        UpdateSplitToCaseSolutionDescendants(caseSolution.Id, item.SplitToCaseSolution_Id);
                    }
                }
            }
        }

        private void UpdateSplitToCaseSolutionDescendants(int caseSolutionId, int splitToCaseSolutionId)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var rep = uow.GetRepository<CaseSolution_SplitToCaseSolutionEntity>();
                var relation = rep.Find(it => it.CaseSolution_Id == caseSolutionId && it.SplitToCaseSolution_Id == splitToCaseSolutionId).FirstOrDefault();
                if (relation == null)
                {
                    rep.Add(new CaseSolution_SplitToCaseSolutionEntity() { CaseSolution_Id = caseSolutionId, SplitToCaseSolution_Id = splitToCaseSolutionId });
                    uow.Save();
                }
            }
        }

        private void DeleteSplitToCaseSolutionDescendants(int caseSolutionId)
        {
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var rep = uow.GetRepository<CaseSolution_SplitToCaseSolutionEntity>();

                rep.DeleteWhere(x => x.CaseSolution_Id == caseSolutionId);
                uow.Save();
            }
        }

        private void CheckRequiredFields(CaseSolution caseSolution, IList<CaseFieldSetting> MandatoryFields, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();  // Dictionary <FieldName,TranslationCaseFields>
            if (string.IsNullOrEmpty(caseSolution.Name))
                errors.Add("Name", "Du måste ange en ärendemall");

            if (MandatoryFields != null) // This Section dosen't use now
            {
                if (string.IsNullOrEmpty(caseSolution.ReportedBy) && Convert.ToBoolean((MandatoryFields.Where(i => i.Name == "ReportedBy").First().Required)))
                    errors.Add("ReportedBy", "ReportedBy");

                if (string.IsNullOrEmpty(caseSolution.Department_Id.ToString()) && Convert.ToBoolean((MandatoryFields.Where(i => i.Name == "Department_Id").First().Required)))
                    errors.Add("Department", "Department_Id");

                if (string.IsNullOrEmpty(caseSolution.CaseType_Id.ToString()) && Convert.ToBoolean((MandatoryFields.Where(i => i.Name == "CaseType_Id").First().Required)))
                    errors.Add("CaseType", "CaseType_Id");

                if (string.IsNullOrEmpty(caseSolution.ProductArea_Id.ToString()) && Convert.ToBoolean((MandatoryFields.Where(i => i.Name == "ProductArea_Id").First().Required)))
                    errors.Add("ProductArea", "ProductArea_Id");

                if (string.IsNullOrEmpty(caseSolution.Category_Id.ToString()) && Convert.ToBoolean((MandatoryFields.Where(i => i.Name == "Category_Id").First().Required)))
                    errors.Add("Category", "Category_Id");

                if (string.IsNullOrEmpty(caseSolution.Caption.ToString()) && Convert.ToBoolean((MandatoryFields.Where(i => i.Name == "Caption").First().Required)))
                    errors.Add("Caption", "Caption");

                if (string.IsNullOrEmpty(caseSolution.Description.ToString()) && Convert.ToBoolean((MandatoryFields.Where(i => i.Name == "Description").First().Required)))
                    errors.Add("Description", "Description");

                if (string.IsNullOrEmpty(caseSolution.Miscellaneous.ToString()) && Convert.ToBoolean((MandatoryFields.Where(i => i.Name == "Miscellaneous").First().Required)))
                    errors.Add("Miscellaneous", "Miscellaneous");

                if (string.IsNullOrEmpty(caseSolution.CaseWorkingGroup_Id.ToString()) && Convert.ToBoolean((MandatoryFields.Where(i => i.Name == "WorkingGroup_Id").First().Required)))
                    errors.Add("WorkingGroup", "WorkingGroup_Id");

                if (string.IsNullOrEmpty(caseSolution.PerformerUser_Id.ToString()) && Convert.ToBoolean((MandatoryFields.Where(i => i.Name == "Performer_User_Id").First().Required)))
                    errors.Add("PerformerUser", "Performer_User_Id");

                if (string.IsNullOrEmpty(caseSolution.Priority_Id.ToString()) && Convert.ToBoolean((MandatoryFields.Where(i => i.Name == "Priority_Id").First().Required)))
                    errors.Add("Priority", "Priority_Id");
            }

        }

        public void SaveCaseSolutionCategory(CaseSolutionCategory caseSolutionCategory, out IDictionary<string, string> errors)
        {
            if (caseSolutionCategory == null)
                throw new ArgumentNullException("casesolutioncategory");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(caseSolutionCategory.Name))
                errors.Add("CaseSolutionCategory.Name", "Du måste ange en ärendemallskategori");

            if (caseSolutionCategory.Id == 0)
                this._caseSolutionCategoryRepository.Add(caseSolutionCategory);
            else
                this._caseSolutionCategoryRepository.Update(caseSolutionCategory);


            if (caseSolutionCategory.IsDefault == 1)
                this._caseSolutionCategoryRepository.ResetDefault(caseSolutionCategory.Id);

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveEmptyForm(Guid formGuid, int caseId)
        {
            _formRepository.SaveEmptyForm(formGuid, caseId);
        }

        private void Commit()
        {
            this._unitOfWork.Commit();
        }

        public Dictionary<string, string> GetCaseSolutionConditions(int caseSolutionId)
        {
            // FYI, this item is cleared when the specific CaseSolution is saved (CaseSolutionController - Edit)
            var cacheKey = string.Format(Common.Constants.CacheKey.CaseSolutionConditionWithId, caseSolutionId);
            var caseSolutionConditions = this._cache.Get(cacheKey) as Dictionary<string, string>;
            if (caseSolutionConditions == null)
            {
                caseSolutionConditions = 
                    _caseSolutionConditionRepository.GetCaseSolutionConditions(caseSolutionId)
                        .Select(x => new
                        {
                            x.Property_Name,
                            x.Values
                        }).ToDictionary(x => x.Property_Name, x => x.Values);

                if (caseSolutionConditions.Any())
                    this._cache.Set(cacheKey, caseSolutionConditions, Common.Constants.Cache.Duration);
            }

            return caseSolutionConditions;
        }

        private IList<CaseSolutionOverview> GetCustomerCaseSolutionsCached(int customerId)
        {
            // FYI, this item is cleared when the specific CaseSolution is saved (CaseSolutionController - Edit)
            var cacheKey = string.Format(Common.Constants.CacheKey.CaseSolutionConditionWithId, customerId);

            var caseSolutions = this._cache.Get(cacheKey) as IList<CaseSolutionOverview>;
            if (caseSolutions == null)
            {
                caseSolutions = GetCustomerCaseSolutionsQuery(customerId).ToList();

                if (caseSolutions.Any())
                    this._cache.Set(cacheKey, caseSolutions, Common.Constants.Cache.Duration);
            }

            return caseSolutions;
        }

        public bool CheckIfExtendedFormExistForSolutionsInCategories(int customerId, List<int> ids)
        {
            var res = _computerUserCategoryRepository.CheckIfExtendedFormsExistForCategories(customerId, ids);
            return res;
        }

        private class WorkflowConditionsContext
        {
            public int CustomerId { get; set; }
            public Case Case { get; set; }
            public List<CaseSolutionConditionOverview> Conditions { get; set; }
            public UserOverview User { get; set; } 
            public ApplicationType ApplicationType { get; set; }
            public int? TemplateId { get; set; }
            public bool IsRelatedCase { get; set; }

            public IList<WorkingGroupInfo> WorkingGroups { get; set; }
            public CaseSolution Template { get; set; }
        }
    }
}