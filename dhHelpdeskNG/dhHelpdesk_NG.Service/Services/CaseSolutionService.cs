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
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using System.Data;
    using System.Configuration;
    using System.Data.SqlClient;
    using BusinessData.Models.Case;

    public interface ICaseSolutionService
    {
        IList<CaseSolution> GetCaseSolutions(int customerId);
        IList<Application> GetAllApplications(int customerId);
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
        private readonly IApplicationRepository _applicationRepository;
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
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly ICaseService _caseService;

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
            IApplicationRepository applicationRepository,
            ICaseService caseService
            )
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
            this._caseSolutionConditionRepository = caseSolutionConditionRepository;
            this._stateSecondaryService = stateSecondaryService;
            this._applicationRepository = applicationRepository;
            this._caseService = caseService;
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

        public IList<Application> GetAllApplications(int customerId)
        {
            string sql = string.Empty;
            sql = "SELECT * FROM tblApplicationType  ";//WHERE Customer_Id= " + customerId + "";
            string ConnectionStringExt = ConfigurationManager.ConnectionStrings["HelpdeskSqlServerDbContext"].ConnectionString;
            DataTable dtExt = null;

            using (var connectionExt = new SqlConnection(ConnectionStringExt))
            {
                if (connectionExt.State == ConnectionState.Closed)
                {
                    connectionExt.Open();
                }
                using (var commandExt = new SqlCommand { Connection = connectionExt, CommandType = CommandType.StoredProcedure, CommandTimeout = 0 })
                {
                    commandExt.CommandType = CommandType.Text;
                    commandExt.CommandText = sql;
                    var reader = commandExt.ExecuteReader();
                    dtExt = new DataTable();
                    dtExt.Load(reader);

                }
            }

            List<Application> lapp = new List<Application>();

            foreach (DataRow rowExt in dtExt.Rows)
            {
                Application a = new Application();

                if (rowExt["Id"].ToString() != null)
                {
                    a.Id = Convert.ToInt32(rowExt["Id"].ToString());
                }
                if (rowExt["ApplicationType"].ToString() != null)
                {
                    a.Name = Convert.ToString(rowExt["ApplicationType"].ToString());
                }

                lapp.Add(a);
            }

            return lapp;
            //return this._applicationRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<WorkflowStepModel> GetGetWorkflowSteps(int customerId, Case _case, UserOverview user, ApplicationType applicationType, int? templateId)
        {
            var templates = GetCaseSolutions(customerId).Where(c => c.Status != 0 && c.ConnectedButton == 0).Select(c => new WorkflowStepModel()
            {
                CaseTemplateId = c.Id,
                Name = c.Name,
                SortOrder = c.SortOrder,
                //If value exist in NextStepState - use it. Otherwise check if caseSolution.StateSecondary_id have value, otherwise return 0;
                NextStep = (c.NextStepState != null ? c.NextStepState.Value : (c.StateSecondary_Id != null ? _stateSecondaryService.GetStateSecondary(c.StateSecondary_Id.Value).StateSecondaryId : 0))

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

            bool isRelatedCase = (_case != null ? this._caseService.IsRelated(_case.Id) : false);

            foreach (var condition in caseSolutionConditions)
            {
                var conditionValue = condition.Value.Tidy().ToLower();
                var conditionKey = condition.Key.Tidy();

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
                        if (isRelatedCase)
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
                        int appType = (int)((ApplicationType)Enum.Parse(typeof(ApplicationType), applicationType.ToString()));
                        value = appType.ToString();
                    }
                    //GET FROM USER
                    else if (conditionKey.ToLower().StartsWith("user_WorkingGroup.WorkingGroupGUID".ToLower()))
                    {
                        //Get working groups connected to "UserRole.Admin"
                        var workingGroups = this._workingGroupService.GetWorkingGroupsAdmin(customerId, user.Id);

                        bool wgShowWorkflowStep = false;

                        string[] conditionValues = conditionValue.Split(',').Select(sValue => sValue.Trim()).ToArray();

                        for (int i = 0; i < conditionValues.Length; i++)
                        {
                            var val = conditionValues[i];

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

                query = query.Where(x => (!string.IsNullOrEmpty(x.Caption) && x.Caption.ToLower().Contains(SearchCaseSolutions.SearchCss))
                                      || (!string.IsNullOrEmpty(x.Description) && x.Description.ToLower().Contains(SearchCaseSolutions.SearchCss))
                                      || (!string.IsNullOrEmpty(x.Miscellaneous) && x.Miscellaneous.ToLower().Contains(SearchCaseSolutions.SearchCss))
                                      || (!string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(SearchCaseSolutions.SearchCss))
                                      || (!string.IsNullOrEmpty(x.Text_External) && x.Text_External.ToLower().Contains(SearchCaseSolutions.SearchCss))
                                      || (!string.IsNullOrEmpty(x.Text_Internal) && x.Text_Internal.ToLower().Contains(SearchCaseSolutions.SearchCss))
                                   );
            }

            if (SearchCaseSolutions.CategoryIds != null && SearchCaseSolutions.CategoryIds.Any())
            {
                query = query.Where(x => x.CaseSolutionCategory_Id.HasValue && SearchCaseSolutions.CategoryIds.Contains(x.CaseSolutionCategory_Id.Value));
            }


            if (SearchCaseSolutions.OnlyActive == true)
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

            var t = (from cs in this._caseSolutionConditionRepository.GetAll().OrderBy(z => z.CaseSolution_Id).ThenBy(z => z.Property_Name).ThenBy(z => z.Values)
                     select cs);

            DataTable tableCond = new DataTable();
            if (t.Count() > 0)
            {
                tableCond.Columns.Add("Id_Cond", typeof(int));
                tableCond.Columns.Add("CaseSolution_Id", typeof(int));
                tableCond.Columns.Add("Property_Name", typeof(string));
                tableCond.Columns.Add("Values", typeof(string));

                foreach (var c in t)
                {
                    int id = 0;
                    if (c.Id != null)
                    {
                        id = c.Id;
                    }
                    int casedsolid = 0;
                    if (c.CaseSolution_Id != null)
                    {
                        casedsolid = c.CaseSolution_Id;
                    }

                    string pname = string.Empty;
                    if (c.Property_Name != null)
                    {
                        pname = c.Property_Name;
                    }
                    string values = string.Empty;
                    if (c.Values != null)
                    {
                        values = c.Values;
                    }
                    char[] delimiterChars = { ',' };
                    string[] words = values.Split(delimiterChars);

                    foreach (string s in words)
                    {

                        tableCond.Rows.Add(id, casedsolid, pname, s);
                    }


                }

            }

            List<CaseSolution> cresList = new List<CaseSolution>();
            List<CaseSolutionConditionModel> cmoList = new List<CaseSolutionConditionModel>();

            if (SearchCaseSolutions.ApplicationIds != null | SearchCaseSolutions.PriorityIds != null | SearchCaseSolutions.ProductAreaIds != null | SearchCaseSolutions.StatusIds != null | SearchCaseSolutions.SubStatusIds != null | SearchCaseSolutions.TemplateProductAreaIds != null | SearchCaseSolutions.UserWGroupIds != null | SearchCaseSolutions.WgroupIds != null)
            {
                if (SearchCaseSolutions.ApplicationIds.Count > 0 | SearchCaseSolutions.PriorityIds.Count > 0 | SearchCaseSolutions.ProductAreaIds.Count > 0 | SearchCaseSolutions.StatusIds.Count > 0 | SearchCaseSolutions.SubStatusIds.Count > 0 | SearchCaseSolutions.TemplateProductAreaIds.Count > 0 | SearchCaseSolutions.UserWGroupIds.Count > 0 | SearchCaseSolutions.WgroupIds.Count > 0)
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



                    if (SearchCaseSolutions.SubStatusIds != null && SearchCaseSolutions.SubStatusIds.Any())
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
                                           where SearchCaseSolutions.SubStatusIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                                           where SearchCaseSolutions.SubStatusIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                    if (SearchCaseSolutions.WgroupIds != null && SearchCaseSolutions.WgroupIds.Any())
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
                                           where SearchCaseSolutions.WgroupIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                                           where SearchCaseSolutions.WgroupIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                    if (SearchCaseSolutions.PriorityIds != null && SearchCaseSolutions.PriorityIds.Any())
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
                                           where SearchCaseSolutions.PriorityIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                                           where SearchCaseSolutions.PriorityIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                    if (SearchCaseSolutions.StatusIds != null && SearchCaseSolutions.StatusIds.Any())
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
                                           where SearchCaseSolutions.StatusIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                                           where SearchCaseSolutions.StatusIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                    if (SearchCaseSolutions.ProductAreaIds != null && SearchCaseSolutions.ProductAreaIds.Any())
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
                                           where SearchCaseSolutions.ProductAreaIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                                           where SearchCaseSolutions.ProductAreaIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                    if (SearchCaseSolutions.UserWGroupIds != null && SearchCaseSolutions.UserWGroupIds.Any())
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
                                           where SearchCaseSolutions.UserWGroupIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                                           where SearchCaseSolutions.UserWGroupIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                    if (SearchCaseSolutions.TemplateProductAreaIds != null && SearchCaseSolutions.TemplateProductAreaIds.Any())
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
                                           where SearchCaseSolutions.TemplateProductAreaIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                                           where SearchCaseSolutions.TemplateProductAreaIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                    if (SearchCaseSolutions.ApplicationIds != null && SearchCaseSolutions.ApplicationIds.Any())
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
                                           where SearchCaseSolutions.ApplicationIds.Contains(d.Values)
                                           select d;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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
                                           where SearchCaseSolutions.ApplicationIds.Contains(c.Values)
                                           select c;

                            if (results1 != null)
                            {
                                if (results1.Count() > 0)
                                {
                                    cresList = new List<CaseSolution>();
                                    foreach (var r in results1)
                                    {
                                        CaseSolution cres = _caseSolutionRepository.GetById(r.CaseSolution_Id);
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

                    if (SearchCaseSolutions.ApplicationIds == null && SearchCaseSolutions.PriorityIds == null && SearchCaseSolutions.ProductAreaIds == null && SearchCaseSolutions.StatusIds == null && SearchCaseSolutions.SubStatusIds == null && SearchCaseSolutions.TemplateProductAreaIds == null && SearchCaseSolutions.UserWGroupIds == null && SearchCaseSolutions.WgroupIds == null)
                    {
                        if (cresList.Count == 0 | cresList == null)
                        {
                            if (results != null)
                            {
                                foreach (var r in results)
                                {

                                    CaseSolution cres = _caseSolutionRepository.GetById(r.Id);

                                    cresList.Add(cres);
                                }
                            }
                        }
                    }

                    if (SearchCaseSolutions.ApplicationIds != null && SearchCaseSolutions.PriorityIds != null && SearchCaseSolutions.ProductAreaIds != null && SearchCaseSolutions.StatusIds != null && SearchCaseSolutions.SubStatusIds != null && SearchCaseSolutions.TemplateProductAreaIds != null && SearchCaseSolutions.UserWGroupIds != null && SearchCaseSolutions.WgroupIds != null)
                    {
                        if (SearchCaseSolutions.ApplicationIds.Count == 0 && SearchCaseSolutions.PriorityIds.Count == 0 && SearchCaseSolutions.ProductAreaIds.Count == 0 && SearchCaseSolutions.StatusIds.Count == 0 && SearchCaseSolutions.SubStatusIds.Count == 0 && SearchCaseSolutions.TemplateProductAreaIds.Count == 0 && SearchCaseSolutions.UserWGroupIds.Count == 0 && SearchCaseSolutions.WgroupIds.Count == 0)
                        {

                            if (cresList.Count == 0 | cresList == null)
                            {
                                if (results != null)
                                {
                                    foreach (var r in results)
                                    {

                                        CaseSolution cres = _caseSolutionRepository.GetById(r.Id);

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



                    if (SearchCaseSolutions.ApplicationIds == null && SearchCaseSolutions.PriorityIds == null && SearchCaseSolutions.ProductAreaIds == null && SearchCaseSolutions.StatusIds == null && SearchCaseSolutions.SubStatusIds == null && SearchCaseSolutions.TemplateProductAreaIds == null && SearchCaseSolutions.UserWGroupIds == null && SearchCaseSolutions.WgroupIds == null)
                    {
                        if (cresList.Count == 0 | cresList == null)
                        {
                            if (results != null)
                            {
                                foreach (var r in results)
                                {

                                    CaseSolution cres = _caseSolutionRepository.GetById(r.Id);

                                    cresList.Add(cres);
                                }
                            }
                        }
                    }

                    if (SearchCaseSolutions.ApplicationIds != null && SearchCaseSolutions.PriorityIds != null && SearchCaseSolutions.ProductAreaIds != null && SearchCaseSolutions.StatusIds != null && SearchCaseSolutions.SubStatusIds != null && SearchCaseSolutions.TemplateProductAreaIds != null && SearchCaseSolutions.UserWGroupIds != null && SearchCaseSolutions.WgroupIds != null)
                    {
                        if (SearchCaseSolutions.ApplicationIds.Count == 0 && SearchCaseSolutions.PriorityIds.Count == 0 && SearchCaseSolutions.ProductAreaIds.Count == 0 && SearchCaseSolutions.StatusIds.Count == 0 && SearchCaseSolutions.SubStatusIds.Count == 0 && SearchCaseSolutions.TemplateProductAreaIds.Count == 0 && SearchCaseSolutions.UserWGroupIds.Count == 0 && SearchCaseSolutions.WgroupIds.Count == 0)
                        {

                            if (cresList.Count == 0 | cresList == null)
                            {
                                if (results != null)
                                {
                                    foreach (var r in results)
                                    {

                                        CaseSolution cres = _caseSolutionRepository.GetById(r.Id);

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



                if (SearchCaseSolutions.ApplicationIds == null && SearchCaseSolutions.PriorityIds == null && SearchCaseSolutions.ProductAreaIds == null && SearchCaseSolutions.StatusIds == null && SearchCaseSolutions.SubStatusIds == null && SearchCaseSolutions.TemplateProductAreaIds == null && SearchCaseSolutions.UserWGroupIds == null && SearchCaseSolutions.WgroupIds == null)
                {
                    if (cresList.Count == 0 | cresList == null)
                    {
                        if (results != null)
                        {
                            foreach (var r in results)
                            {

                                CaseSolution cres = _caseSolutionRepository.GetById(r.Id);

                                cresList.Add(cres);
                            }
                        }
                    }
                }

                if (SearchCaseSolutions.ApplicationIds != null && SearchCaseSolutions.PriorityIds != null && SearchCaseSolutions.ProductAreaIds != null && SearchCaseSolutions.StatusIds != null && SearchCaseSolutions.SubStatusIds != null && SearchCaseSolutions.TemplateProductAreaIds != null && SearchCaseSolutions.UserWGroupIds != null && SearchCaseSolutions.WgroupIds != null)
                {
                    if (SearchCaseSolutions.ApplicationIds.Count == 0 && SearchCaseSolutions.PriorityIds.Count == 0 && SearchCaseSolutions.ProductAreaIds.Count == 0 && SearchCaseSolutions.StatusIds.Count == 0 && SearchCaseSolutions.SubStatusIds.Count == 0 && SearchCaseSolutions.TemplateProductAreaIds.Count == 0 && SearchCaseSolutions.UserWGroupIds.Count == 0 && SearchCaseSolutions.WgroupIds.Count == 0)
                    {

                        if (cresList.Count == 0 | cresList == null)
                        {
                            if (results != null)
                            {
                                foreach (var r in results)
                                {

                                    CaseSolution cres = _caseSolutionRepository.GetById(r.Id);

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

            if (!string.IsNullOrEmpty(SearchCaseSolutions.SortBy) && (SearchCaseSolutions.SortBy != "undefined"))
            {
                switch (SearchCaseSolutions.SortBy)
                {

                    case CaseSolutionIndexColumns.Name:


                        var query1 = (SearchCaseSolutions.Ascending) ?
                                cresList.OrderBy(l => (l.Name != null ? l.Name : string.Empty)) :
                                cresList.OrderByDescending(l => (l.Name != null ? l.Name : string.Empty));

                        return query1.ToList();

                        break;

                    case CaseSolutionIndexColumns.Category:
                        query1 = (SearchCaseSolutions.Ascending) ?
                                cresList.OrderBy(l => (l.CaseSolutionCategory != null ? l.CaseSolutionCategory.Name : string.Empty)) :
                                cresList.OrderByDescending(l => (l.CaseSolutionCategory != null ? l.CaseSolutionCategory.Name : string.Empty));

                        return query1.ToList();
                        break;

                    case CaseSolutionIndexColumns.Caption:
                        query1 = (SearchCaseSolutions.Ascending) ?
                                cresList.OrderBy(l => (l.Caption != null ? l.Caption : string.Empty)) :
                                cresList.OrderByDescending(l => (l.Caption != null ? l.Caption : string.Empty));

                        return query1.ToList();
                        break;

                    case CaseSolutionIndexColumns.Administrator:
                        if (SearchCaseSolutions.Ascending)
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
                        query1 = (SearchCaseSolutions.Ascending) ?
                                    cresList.OrderBy(l => (l.Priority != null ? l.Priority.Name : string.Empty)) :
                                    cresList.OrderByDescending(l => (l.Priority != null ? l.Priority.Name : string.Empty));

                        return query1.ToList();
                        break;


                    case CaseSolutionIndexColumns.Status:
                        query1 = (SearchCaseSolutions.Ascending) ?
                                cresList.OrderBy(l => l.Status) :
                                cresList.OrderByDescending(l => l.Status);

                        return query1.ToList();
                        break;

                    case CaseSolutionIndexColumns.ConnectedToButton:
                        query1 = (SearchCaseSolutions.Ascending) ?
                                cresList.OrderBy(l => l.ConnectedButton) :
                                cresList.OrderByDescending(l => l.ConnectedButton);

                        return query1.ToList();
                        break;
                    case CaseSolutionIndexColumns.SortOrder:
                        query1 = (SearchCaseSolutions.Ascending) ?
                                cresList.OrderBy(l => l.SortOrder) :
                                cresList.OrderByDescending(l => l.SortOrder);

                        return query1.ToList();
                        break;
                    default:
                        query1 = (SearchCaseSolutions.Ascending) ?
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