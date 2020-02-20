using DH.Helpdesk.BusinessData.Models.Case.CaseSections;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.SelfService.Models.Case
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Domain;
    using Shared;

    public class NewCaseModel
    {
        public Case NewCase { get; set; }

        public int CustomerId { get; set; }

        public int CurrentLanguageId { get; set; }

        public ApplicationType ApplicationType { get; set; }

        public int CaseTemplateId { get; set; }

        public string CaseTypeParantPath { get; set; }

        public string ProductAreaParantPath { get; set; }

        public string CategoryParentPath { get; set; }

        public string CaseFileKey { get; set; }

        public string ExLogFileGuid { get; set; }

        public int DepartmentFilterFormat { get; set; }

        public string FollowerUsers { get; set; }

        public string Information { get; set; }

        public bool ShowCaseActionsPanelOnTop { get; set; }

        public bool ShowCaseActionsPanelAtBottom { get; set; }

        public int AttachmentPlacement { get; set; }

        public bool ShowCommunicationForSelfService { get; set; }

        public int? SelectedWorkflowStep { get; set; }

        public CaseLog CaseLog { get; set; }

        public FilesModel CaseFilesModel { get; set; }

        public CaseMailSetting CaseMailSetting { get; set; }

        public List<CaseListToCase> FieldSettings { get; set; }

        public IList<CaseFieldSetting> CaseFieldSettings { get; set; }

        public IEnumerable<CaseSectionModel> CaseSectionSettings { get; set; }

        public IEnumerable<CaseFieldSettingsWithLanguage> CaseFieldSettingWithLangauges { get; set; }

        public SendToDialogModel SendToDialogModel { get; set; }

        public List<KeyValuePair<int, string>> CaseTypeRelatedFields { get; set; }

        #region Options Lists

        public IList<CaseType> CaseTypes { get; set; }

        public IList<ProductArea> ProductAreas { get; set; }

        public IList<Region> Regions { get; set; }

        public IList<Department> Departments { get; set; }

        public IList<OU> OrganizationUnits { get; set; }

        public IList<System> Systems { get; set; }

        public IList<Urgency> Urgencies { get; set; }

        public IList<Impact> Impacts { get; set; }

        public IList<Category> Categories { get; set; }

        public IList<Currency> Currencies { get; set; }

        public IList<Supplier> Suppliers { get; set; }

        public List<string> CaseFieldGroups { get; set; }

        public List<ProductAreaChild> ProductAreaChildren { get; set; }
        
        #endregion

        public JsApplicationOptions JsApplicationOptions { get; set; }

        public List<FieldSettingJSModel> JsFieldSettings { get; set; }

		public List<string> FileUploadWhiteList { get; set; }

		#region Methods

		public CaseControlsPanelModel CreateCaseControlsPanelModel(int position = 1)
        {
            return new CaseControlsPanelModel(position, false);
        }

        #endregion
    }

    public sealed class ProductAreaChild
    {
        public ProductAreaChild(int productAreaId, bool hasChildren)
        {
            ProductAreaId = productAreaId;
            HasChildren = hasChildren;
        }

        public int ProductAreaId { get; private set; }

        public bool HasChildren { get; private set; }
    }

    public class JsApplicationOptions
    {
        public int customerId;

        public int departmentFilterFormat;

        public string departmentsURL;

        public string orgUnitURL;
    }
}