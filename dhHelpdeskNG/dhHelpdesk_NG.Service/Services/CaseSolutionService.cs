using DH.Helpdesk.BusinessData.Models.CaseSolution;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Common.Enums.CaseSolution;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Cases;
    using DH.Helpdesk.Domain;
    using System.Reflection;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.BusinessData.Models.User.Input;

    public interface ICaseSolutionService
    {
        IList<CaseSolution> GetCaseSolutions(int customerId);
        IList<CaseSolutionCategory> GetCaseSolutionCategories(int customerId);
        IList<CaseSolution> SearchAndGenerateCaseSolutions(int customerId, ICaseSolutionSearch SearchCaseSolutions, bool isFirstNamePresentation);

        //int GetAntal(int customerId, int userid);

        CaseSolution GetCaseSolution(int id);
        CaseSolutionCategory GetCaseSolutionCategory(int id);
        CaseSolutionSchedule GetCaseSolutionSchedule(int id);

        DeleteMessage DeleteCaseSolution(int id, int customerId);
        DeleteMessage DeleteCaseSolutionCategory(int id);

        List<CaseTemplateCategoryNode> GetCaseSolutionCategoryTree(int customerId, int userId, CaseSolutionLocationShow location);
        void SaveCaseSolution(CaseSolution caseSolution, CaseSolutionSchedule caseSolutionSchedule, IList<CaseFieldSetting> CaseFieldSetting, out IDictionary<string, string> errors);
        void SaveCaseSolutionCategory(CaseSolutionCategory caseSolutionCategory, out IDictionary<string, string> errors);
        void SaveEmptyForm(Guid formGuid, int caseId);
        void Commit();
        IList<WorkflowStepModel> GetGetWorkflowSteps(int customerId, Case _case, UserOverview user, ApplicationType applicationType, int? templateId);
    }

    public class CaseSolutionService : ICaseSolutionService
    {
        private readonly ICaseSolutionRepository _caseSolutionRepository;
        private readonly ICaseSolutionCategoryRepository _caseSolutionCategoryRepository;
        private readonly ICaseSolutionScheduleRepository _caseSolutionScheduleRepository;

        private readonly ICaseSolutionSettingRepository caseSolutionSettingRepository;
        private readonly ICaseSolutionConditionRepository caseSolutionConditionRepository;
        private readonly IFormRepository _formRepository;
        private readonly ILinkService _linkService;
        private readonly ILinkRepository _linkRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheProvider _cache;

        private readonly ICaseSolutionConditionRepository _caseSolutionConditionRepository;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ICaseSolutionConditionService _caseSolutionConditionService;

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
            ICaseSolutionConditionService caseSolutionCondtionService)
        {
            this._caseSolutionRepository = caseSolutionRepository;
            this._caseSolutionCategoryRepository = caseSolutionCategoryRepository;
            this._caseSolutionScheduleRepository = caseSolutionScheduleRepository;
            this.caseSolutionSettingRepository = caseSolutionSettingRepository;
            this._linkRepository = linkRepository;
            this._linkService = linkService;
            _formRepository = formRepository;
            this._unitOfWork = unitOfWork;
            this._caseSolutionConditionRepository = caseSolutionConditionRepository;
            this._cache = cache;
            this._workingGroupService = workingGroupService;
            this._caseSolutionConditionService = caseSolutionCondtionService;
            this.caseSolutionConditionRepository = caseSolutionConditionRepository;
        }

        //public int GetAntal(int customerId, int userid)
        //{
        //    return _caseSolutionRepository.GetAntal(customerId, userid);
        //}

        public List<CaseTemplateCategoryNode> GetCaseSolutionCategoryTree(int customerId, int userId, CaseSolutionLocationShow location)
        {
            List<CaseTemplateCategoryNode> ret1 = new List<CaseTemplateCategoryNode>();

            List<CaseTemplateCategoryNode> ret2 = new List<CaseTemplateCategoryNode>();

            var noneCatCaseSolutions = _caseSolutionRepository.GetMany(s => s.Customer_Id == customerId && s.CaseSolutionCategory_Id == null &&
                                                                            s.Status != 0 && s.ConnectedButton != 0 &&
                                                                        (s.WorkingGroup.UserWorkingGroups.Select(
                                                                         x => x.User_Id).Contains(userId) ||
                                                                         s.WorkingGroup_Id == null)).OrderBy(cs => cs.Name);

            var canShow = false;
            foreach (var casetemplate in noneCatCaseSolutions)
            {

                canShow = (location == CaseSolutionLocationShow.BothCaseOverviewAndInsideCase) ||
                          (location == CaseSolutionLocationShow.OnCaseOverview && casetemplate.ShowOnCaseOverview != 0) ||
                          (location == CaseSolutionLocationShow.InsideTheCase && casetemplate.ShowInsideCase != 0);

                if (canShow)
                {
                    CaseTemplateCategoryNode noneCategory = new CaseTemplateCategoryNode();
                    noneCategory.CategoryId = casetemplate.Id;
                    noneCategory.CategoryName = casetemplate.Name;
                    noneCategory.IsRootTemplate = true;
                    ret1.Add(noneCategory);
                }
            }




            var allCategory =
                _caseSolutionCategoryRepository.GetMany(c => c.Customer_Id == customerId).OrderBy(c => c.Name);

            foreach (var category in allCategory)
            {
                CaseTemplateCategoryNode curCategory = new CaseTemplateCategoryNode();

                curCategory.CategoryId = category.Id;
                curCategory.CategoryName = category.Name;
                curCategory.IsRootTemplate = false;

                var caseSolutions = _caseSolutionRepository.GetMany(s => s.Customer_Id == customerId && s.CaseSolutionCategory_Id == category.Id &&
                                                                         s.Status != 0 && s.ConnectedButton != 0 &&
                                                                         (s.WorkingGroup.UserWorkingGroups.Select(
                                                                             x => x.User_Id).Contains(userId) || s.WorkingGroup_Id == null)).OrderBy(cs => cs.Name);
                if (caseSolutions != null)
                {
                    curCategory.CaseTemplates = new List<CaseTemplateNode>();
                    foreach (var casetemplate in caseSolutions)
                    {
                        canShow = (location == CaseSolutionLocationShow.BothCaseOverviewAndInsideCase) ||
                          (location == CaseSolutionLocationShow.OnCaseOverview && casetemplate.ShowOnCaseOverview != 0) ||
                          (location == CaseSolutionLocationShow.InsideTheCase && casetemplate.ShowInsideCase != 0);

                        if (canShow)
                        {
                            CaseTemplateNode curCaseTemplate = new CaseTemplateNode();
                            curCaseTemplate.CaseTemplateId = casetemplate.Id;
                            curCaseTemplate.CaseTemplateName = casetemplate.Name;
                            curCaseTemplate.WorkingGroup = casetemplate.WorkingGroup == null
                                ? ""
                                : casetemplate.WorkingGroup.WorkingGroupName;

                            curCategory.CaseTemplates.Add(curCaseTemplate);
                        }
                    }
                }

                ret2.Add(curCategory);
            }

            int maxLen = 0;
            int curLen;
            foreach (var node in ret1)
            {
                curLen = node.CategoryName.Length;
                if (curLen > maxLen)
                    maxLen = curLen;
            }

            foreach (var node in ret2)
            {
                curLen = node.CategoryName.Length;
                if (curLen > maxLen)
                    maxLen = curLen;
            }

            string line = "";
            if (ret1.Count > 0 && ret2.Count > 0)
            {
                CaseTemplateCategoryNode separateLine = new CaseTemplateCategoryNode();
                separateLine.CategoryId = 0;
                separateLine.CategoryName = line.PadLeft(maxLen + 5, '_');
                separateLine.IsRootTemplate = false;
                ret1.Add(separateLine);
            }

            foreach (var ret in ret2)
                ret1.Add(ret);

            return ret1;
        }

        public IList<CaseSolution> GetCaseSolutions(int customerId)
        {
            return this._caseSolutionRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<WorkflowStepModel> GetGetWorkflowSteps(int customerId, Case _case, UserOverview user, ApplicationType applicationType, int? templateId)
        {
            var templates = GetCaseSolutions(customerId).Where(c => c.Status != 0 && c.ConnectedButton == 0).Select(c => new WorkflowStepModel()
            {
                CaseTemplateId = c.Id,
                Name = c.Name,
                SortOrder = c.SortOrder
            }).OrderBy(c => c.SortOrder).ToList();


            return templates.Where(c => showWorkflowStep(customerId, _case, c.CaseTemplateId, user, applicationType, templateId) == true).ToList();
        }

        private bool showWorkflowStep(int customerId, Case _case, int caseSolution_Id, UserOverview user, ApplicationType applicationType, int? templateId)
        {
            //ALL conditions must be met
            bool showWorkflowStep = false;

            //If no conditions are set in (CaseSolutionCondition) for the template, do not show step in list
            var caseSolutionConditions = this.GetCaseSolutionConditions(caseSolution_Id);

            if (caseSolutionConditions == null || caseSolutionConditions.Count() == 0)
                return false;

            foreach (var condition in caseSolutionConditions)
            {
                var conditionValue = condition.Value.Tidy().ToLower();
                var conditionKey = condition.Key.Tidy();

                try
                {
                    var value = "";

                    //GET FROM APPLICATION
                    if (conditionKey.ToLower() == "application_type")
                    {
                        int appType = (int)((ApplicationType)Enum.Parse(typeof(ApplicationType), applicationType.ToString()));
                        value = appType.ToString();
                    }
                    //GET FROM USER
                    else if (conditionKey.ToLower().StartsWith("user_WorkingGroup.WorkingGroupGUID".ToLower()))
                    {
                        var workingGroups = this._workingGroupService.GetWorkingGroups(customerId, user.Id);
                        bool wgShowWorkflowStep = false;

                      //  conditionKey = conditionKey.Replace("user_workinggroup", "");

                        string[] conditionValues = conditionValue.Split(',').Select(sValue => sValue.Trim()).ToArray();

                        for (int i = 0; i < conditionValues.Length; i++)
                        {
                            var val = conditionValues[i];

                            //if (workingGroups.Where(x => x.Id == int.Parse(conditionValues[i])).Count() > 0)
                            if (workingGroups.Where(x => x.WorkingGroupGUID.ToString().ToLower() == conditionValues[i]).Count() > 0)
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
                        value = user.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(user, null))[conditionKey].ToString();
                    }
                    //Get the specific property of Case in "Property_Name"
                    else if (_case != null && _case.Id != 0 && conditionKey.ToLower().StartsWith("case_"))
                    {
                        conditionKey = conditionKey.Replace("case_", "");

                        if (!conditionKey.Contains("."))
                        {
                            //Get value from Case Model by casting to dictionary and look for property name
                            value = _case.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(_case, null))[conditionKey].ToString();
                        }
                        else
                        {
                            var parentClassName = string.Concat(conditionKey.TakeWhile((c) => c != '.'));
                            var propertyName = conditionKey.Substring(conditionKey.LastIndexOf('.') + 1);

                            var parentClass = _case.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(_case, null))[parentClassName];
                            value = parentClass.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(parentClass, null))[propertyName].ToString();
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

                        int tempId = (templateId.HasValue) ? templateId.Value : 0;

                        if (tempId > 0)
                        {
                            var caseTemplate = GetCaseSolution(tempId);

                            if (!conditionKey.Contains("."))
                            {
                                //Get value from Case Solution Model by casting to dictionary and look for property name
                                value = caseTemplate.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(caseTemplate, null))[conditionKey].ToString();
                            }
                            else
                            {
                                var parentClassName = string.Concat(conditionKey.TakeWhile((c) => c != '.'));
                                var propertyName = conditionKey.Substring(conditionKey.LastIndexOf('.') + 1);

                                var parentClass = caseTemplate.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(caseTemplate, null))[parentClassName];
                                value = parentClass.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(parentClass, null))[propertyName].ToString();
                            }
                        }
                    }

                    // Check conditions
                    if (value.Length > 0 && conditionValue.IndexOf(value.ToLower()) > -1)
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
                    //Remove caching of conditions for this specific template that is used in Case
                    string cacheKey = string.Format(DH.Helpdesk.Common.Constants.CacheKey.CaseSolutionConditionWithId, caseSolution_Id);
                    this._cache.Invalidate(cacheKey);

                    //throw;
                    //TODO?
                    return false;
                }
            }

            //To be true all conditions needs to be fulfilled
            return showWorkflowStep;
        }

        public IList<CaseSolution> SearchAndGenerateCaseSolutions(int customerId, ICaseSolutionSearch SearchCaseSolutions, bool isFirstNamePresentation)
        {
            var query = (from cs in this._caseSolutionRepository.GetAll().Where(x => x.Customer_Id == customerId)
                         select cs);

            #region Search

            if (!string.IsNullOrEmpty(SearchCaseSolutions.SearchCss))
            {
                SearchCaseSolutions.SearchCss = SearchCaseSolutions.SearchCss.ToLower();

                query = query.Where(x => x.Caption.ToLower().Contains(SearchCaseSolutions.SearchCss)
                                      || x.Description.ToLower().Contains(SearchCaseSolutions.SearchCss)
                                      || x.Miscellaneous.ToLower().Contains(SearchCaseSolutions.SearchCss)
                                      || x.Name.ToLower().Contains(SearchCaseSolutions.SearchCss)
                                      || x.Text_External.ToLower().Contains(SearchCaseSolutions.SearchCss)
                                      || x.Text_Internal.ToLower().Contains(SearchCaseSolutions.SearchCss)
                                   );
            }

            if (SearchCaseSolutions.CategoryIds != null && SearchCaseSolutions.CategoryIds.Any())
            {
                query = query.Where(x => x.CaseSolutionCategory_Id.HasValue && SearchCaseSolutions.CategoryIds.Contains(x.CaseSolutionCategory_Id.Value));
            }

            if (SearchCaseSolutions.SubStatusIds != null && SearchCaseSolutions.SubStatusIds.Any())
            {
                query = query.Where(x => x.Status_Id.HasValue && SearchCaseSolutions.SubStatusIds.Contains(x.Status_Id.Value));
            }

            #endregion

            #region Sort

            if (!string.IsNullOrEmpty(SearchCaseSolutions.SortBy) && (SearchCaseSolutions.SortBy != "undefined"))
            {
                switch (SearchCaseSolutions.SortBy)
                {

                    case CaseSolutionIndexColumns.Name:
                        query = (SearchCaseSolutions.Ascending) ?
                                query.OrderBy(l => (l.Name != null ? l.Name : string.Empty)) :
                                query.OrderByDescending(l => (l.Name != null ? l.Name : string.Empty));
                        break;

                    case CaseSolutionIndexColumns.Category:
                        query = (SearchCaseSolutions.Ascending) ?
                                query.OrderBy(l => (l.CaseSolutionCategory != null ? l.CaseSolutionCategory.Name : string.Empty)) :
                                query.OrderByDescending(l => (l.CaseSolutionCategory != null ? l.CaseSolutionCategory.Name : string.Empty));
                        break;

                    case CaseSolutionIndexColumns.Caption:
                        query = (SearchCaseSolutions.Ascending) ?
                                query.OrderBy(l => (l.Caption != null ? l.Caption : string.Empty)) :
                                query.OrderByDescending(l => (l.Caption != null ? l.Caption : string.Empty));
                        break;

                    case CaseSolutionIndexColumns.Administrator:
                        if (SearchCaseSolutions.Ascending)
                            query = isFirstNamePresentation ?
                                        query.OrderBy(l => (l.PerformerUser != null ? l.PerformerUser.FirstName : string.Empty))
                                            .ThenBy(l => (l.PerformerUser != null ? l.PerformerUser.SurName : string.Empty)) :

                                        query.OrderBy(l => (l.PerformerUser != null ? l.PerformerUser.SurName : string.Empty))
                                        .ThenBy(l => (l.PerformerUser != null ? l.PerformerUser.FirstName : string.Empty));
                        else
                            query = isFirstNamePresentation ?
                                       query.OrderByDescending(l => (l.PerformerUser != null ? l.PerformerUser.FirstName : string.Empty))
                                           .ThenByDescending(l => (l.PerformerUser != null ? l.PerformerUser.SurName : string.Empty)) :

                                       query.OrderByDescending(l => (l.PerformerUser != null ? l.PerformerUser.SurName : string.Empty))
                                       .ThenByDescending(l => (l.PerformerUser != null ? l.PerformerUser.FirstName : string.Empty));
                        break;

                    case CaseSolutionIndexColumns.Priority:
                        query = (SearchCaseSolutions.Ascending) ?
                                    query.OrderBy(l => (l.Priority != null ? l.Priority.Name : string.Empty)) :
                                    query.OrderByDescending(l => (l.Priority != null ? l.Priority.Name : string.Empty));
                        break;


                    case CaseSolutionIndexColumns.Status:
                        query = (SearchCaseSolutions.Ascending) ?
                                query.OrderBy(l => l.Status) :
                                query.OrderByDescending(l => l.Status);
                        break;

                    case CaseSolutionIndexColumns.ConnectedToButton:
                        query = (SearchCaseSolutions.Ascending) ?
                                query.OrderBy(l => l.ConnectedButton) :
                                query.OrderByDescending(l => l.ConnectedButton);
                        break;
                    case CaseSolutionIndexColumns.SortOrder:
                        query = (SearchCaseSolutions.Ascending) ?
                                query.OrderBy(l => l.SortOrder) :
                                query.OrderByDescending(l => l.SortOrder);
                        break;
                    default:
                        query = (SearchCaseSolutions.Ascending) ?
                                query.OrderBy(l => (l.Name != null ? l.Name : string.Empty)) :
                                query.OrderByDescending(l => (l.Name != null ? l.Name : string.Empty));
                        break;
                }
            }

            #endregion

            return query.ToList();
        }

        public IList<CaseSolutionCategory> GetCaseSolutionCategories(int customerId)
        {
            return this._caseSolutionCategoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public CaseSolution GetCaseSolution(int id)
        {
            return this._caseSolutionRepository.GetById(id);
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
            var caseSolution = this._caseSolutionRepository.GetById(id);

            if (caseSolution != null)
            {
                try
                {

                    this.caseSolutionConditionRepository.DeleteByCaseSolutionId(id);
                    //this.caseSolutionConditionRepository.Commit();

                    this.caseSolutionSettingRepository.DeleteByCaseSolutionId(id);
                    this.caseSolutionSettingRepository.Commit();

                    var caseSolutionSchedule = this._caseSolutionScheduleRepository.GetById(id);

                    if (caseSolutionSchedule != null)
                        this._caseSolutionScheduleRepository.Delete(caseSolutionSchedule);

                    var caseSolutionLinks = this._linkService.GetLinksBySolutionIdAndCustomer(id, customerId);
                    if (caseSolutionLinks.Count > 0)
                        foreach (var link in caseSolutionLinks)
                            this._linkRepository.Delete(x => x.Id == link.Id);

                    this._caseSolutionRepository.Delete(caseSolution);

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
                this._caseSolutionRepository.Add(caseSolution);
            else
            {
                this._caseSolutionRepository.Update(caseSolution);
                this._caseSolutionScheduleRepository.Delete(x => x.CaseSolution_Id == caseSolution.Id);
            }

            if (caseSolutionSchedule != null)
            {
                this._caseSolutionScheduleRepository.Add(caseSolutionSchedule);
            }

            if (errors.Count == 0)
                this.Commit();
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

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public Dictionary<string, string> GetCaseSolutionConditions(int caseSolution_Id)
        {
            //FYI, this item is cleared when the specific CaseSolution is saved (CaseSolutionController - Edit)
            Dictionary<string, string> caseSolutionConditions = this._cache.Get(string.Format(DH.Helpdesk.Common.Constants.CacheKey.CaseSolutionCondition, caseSolution_Id)) as Dictionary<string, string>;

            if (caseSolutionConditions == null)
            {
                caseSolutionConditions = _caseSolutionConditionRepository.GetCaseSolutionConditions(caseSolution_Id).Select(x => new { x.Property_Name, x.Values }).ToDictionary(x => x.Property_Name, x => x.Values);
                
                if (caseSolutionConditions.Any())
                    this._cache.Set(string.Format(DH.Helpdesk.Common.Constants.CacheKey.CaseSolutionConditionWithId, caseSolution_Id), caseSolutionConditions, DH.Helpdesk.Common.Constants.Cache.Duration);
            }

            return caseSolutionConditions;
        }


    }
}