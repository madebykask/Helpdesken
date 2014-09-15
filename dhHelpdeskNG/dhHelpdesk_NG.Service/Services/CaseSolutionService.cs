using DH.Helpdesk.BusinessData.Models.CaseSolution;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Cases;
    using DH.Helpdesk.Domain;

    public interface ICaseSolutionService
    {
        IList<CaseSolution> GetCaseSolutions(int customerId);
        IList<CaseSolutionCategory> GetCaseSolutionCategories(int customerId);
        IList<CaseSolution> SearchAndGenerateCaseSolutions(int customerId, ICaseSolutionSearch SearchCaseSolutions);

        //int GetAntal(int customerId, int userid);

        CaseSolution GetCaseSolution(int id);
        CaseSolutionCategory GetCaseSolutionCategory(int id);
        CaseSolutionSchedule GetCaseSolutionSchedule(int id);

        DeleteMessage DeleteCaseSolution(int id);
        DeleteMessage DeleteCaseSolutionCategory(int id);

        List<CaseTemplateCategoryNode> GetCaseSolutionCategoryTree(int customerId, int userId);
        void SaveCaseSolution(CaseSolution caseSolution, CaseSolutionSchedule caseSolutionSchedule, IList<CaseFieldSetting> CaseFieldSetting, out IDictionary<string, string> errors);
        void SaveCaseSolutionCategory(CaseSolutionCategory caseSolutionCategory, out IDictionary<string, string> errors);
        void Commit();
    }

    public class CaseSolutionService : ICaseSolutionService
    {
        private readonly ICaseSolutionRepository _caseSolutionRepository;
        private readonly ICaseSolutionCategoryRepository _caseSolutionCategoryRepository;
        private readonly ICaseSolutionScheduleRepository _caseSolutionScheduleRepository;

        private readonly ICaseSolutionSettingRepository caseSolutionSettingRepository;

        private readonly IUnitOfWork _unitOfWork;

        public CaseSolutionService(
            ICaseSolutionRepository caseSolutionRepository,
            ICaseSolutionCategoryRepository caseSolutionCategoryRepository,
            ICaseSolutionScheduleRepository caseSolutionScheduleRepository,
            ICaseSolutionSettingRepository caseSolutionSettingRepository,
            IUnitOfWork unitOfWork)
        {
            this._caseSolutionRepository = caseSolutionRepository;
            this._caseSolutionCategoryRepository = caseSolutionCategoryRepository;
            this._caseSolutionScheduleRepository = caseSolutionScheduleRepository;
            this.caseSolutionSettingRepository = caseSolutionSettingRepository;
            this._unitOfWork = unitOfWork;
        }

        //public int GetAntal(int customerId, int userid)
        //{
        //    return _caseSolutionRepository.GetAntal(customerId, userid);
        //}

        public List<CaseTemplateCategoryNode> GetCaseSolutionCategoryTree(int customerId, int userId)
        {
            List<CaseTemplateCategoryNode> ret1 = new List<CaseTemplateCategoryNode>();

            List<CaseTemplateCategoryNode> ret2 = new List<CaseTemplateCategoryNode>();

            var noneCatCaseSolutions = _caseSolutionRepository.GetMany(s => s.Customer_Id == customerId && s.CaseSolutionCategory_Id == null &&
                                                                        (s.WorkingGroup.UserWorkingGroups.Select(
                                                                         x => x.User_Id).Contains(userId) ||
                                                                         s.WorkingGroup_Id == null)).OrderBy(cs => cs.Name);

            foreach (var casetemplate in noneCatCaseSolutions)
            {
                CaseTemplateCategoryNode noneCategory = new CaseTemplateCategoryNode();
                noneCategory.CategoryId = casetemplate.Id;
                noneCategory.CategoryName = casetemplate.Name;
                noneCategory.IsRootTemplate = true;
                ret1.Add(noneCategory);
            }




            var allCategory =
                _caseSolutionCategoryRepository.GetMany(c => c.Customer_Id == customerId).OrderBy(c => c.Name);

            foreach (var category in allCategory)
            {
                CaseTemplateCategoryNode curCategory = new CaseTemplateCategoryNode();

                curCategory.CategoryId = category.Id;
                curCategory.CategoryName = category.Name;
                curCategory.IsRootTemplate = false;

                var caseSolutions = _caseSolutionRepository.GetMany(s => s.CaseSolutionCategory_Id == category.Id &&
                                                                         (s.WorkingGroup.UserWorkingGroups.Select(
                                                                             x => x.User_Id).Contains(userId) || s.WorkingGroup_Id == null));
                if (caseSolutions != null)
                {
                    curCategory.CaseTemplates = new List<CaseTemplateNode>();
                    foreach (var casetemplate in caseSolutions)
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

        public IList<CaseSolution> SearchAndGenerateCaseSolutions(int customerId, ICaseSolutionSearch SearchCaseSolutions)
        {
            var query = (from cs in this._caseSolutionRepository.GetAll().Where(x => x.Customer_Id == customerId)
                         select cs);




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
            //|| x.ReportedBy.Contains(SearchCaseSolutions.SearchCss) 

            if (!string.IsNullOrEmpty(SearchCaseSolutions.SortBy) && (SearchCaseSolutions.SortBy != "undefined"))
            {
                switch (SearchCaseSolutions.SortBy)
                {
                    // For fields which are from other Entities 
                    case "CaseType":
                        if (SearchCaseSolutions.Ascending)
                            query = query.OrderBy(l => (l.CaseType != null ? l.CaseType.Name : string.Empty));
                        else
                            query = query.OrderByDescending(l => (l.CaseType != null ? l.CaseType.Name : string.Empty));
                        break;

                    case "PerformerUser":
                        if (SearchCaseSolutions.Ascending)
                            query = query.OrderBy(l => (l.PerformerUser != null ? l.PerformerUser.FirstName : string.Empty));
                        else
                            query = query.OrderByDescending(l => (l.PerformerUser != null ? l.PerformerUser.FirstName : string.Empty));
                        break;

                    case "Priority":
                        if (SearchCaseSolutions.Ascending)
                            query = query.OrderBy(l => (l.Priority != null ? l.Priority.Name : string.Empty));
                        else
                            query = query.OrderByDescending(l => (l.Priority != null ? l.Priority.Name : string.Empty));
                        break;

                    case "FinishingCause":
                        if (SearchCaseSolutions.Ascending)
                            query = query.OrderBy(l => (l.FinishingCause != null ? l.FinishingCause.Name : string.Empty));
                        else
                            query = query.OrderByDescending(l => (l.FinishingCause != null ? l.FinishingCause.Name : string.Empty));
                        break;

                    case "Category":
                        if (SearchCaseSolutions.Ascending)
                            query = query.OrderBy(l => (l.CaseSolutionCategory != null ? l.CaseSolutionCategory.Name : string.Empty));
                        else
                            query = query.OrderByDescending(l => (l.CaseSolutionCategory != null ? l.CaseSolutionCategory.Name : string.Empty));
                        break;

                    default: // Primary Fields (Dosen't have relation to other Entities)                       
                        if (SearchCaseSolutions.Ascending)
                            query = query.OrderBy(x => x.GetType().GetProperty(SearchCaseSolutions.SortBy).GetValue(x, null));
                        else
                            query = query.OrderByDescending(x => x.GetType().GetProperty(SearchCaseSolutions.SortBy).GetValue(x, null));
                        break;
                }
            }

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

        public DeleteMessage DeleteCaseSolution(int id)
        {
            var caseSolution = this._caseSolutionRepository.GetById(id);

            if (caseSolution != null)
            {
                try
                {
                    this.caseSolutionSettingRepository.DeleteByCaseSolutionId(id);
                    this.caseSolutionSettingRepository.Commit();

                    var caseSolutionSchedule = this._caseSolutionScheduleRepository.GetById(id);

                    if (caseSolutionSchedule != null)
                        this._caseSolutionScheduleRepository.Delete(caseSolutionSchedule);

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

            //this.CheckRequiredFields(caseSolution, CaseFieldSetting, out errors);

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

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}