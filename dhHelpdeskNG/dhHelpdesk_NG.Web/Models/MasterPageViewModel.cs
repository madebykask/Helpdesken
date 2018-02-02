namespace DH.Helpdesk.Web.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Infrastructure;

    using UserGroup = DH.Helpdesk.BusinessData.Enums.Admin.Users.UserGroup;

    public class MasterPageViewModel
    {
        public int SelectedCustomerId { get; set; }

        public int SelectedLanguageId { get; set; }

        public Setting CustomerSetting { get; set; }

        public IList<Customer> Customers { get; set; }
        
        public IList<Language> Languages { get; set; }

        public IEnumerable<GlobalSetting> GlobalSettings { get; set; }

        public int UsersDefaultLanguage
        {
            get
            {
                return SessionFacade.CurrentUser.LanguageId;
            }
        }   

        public bool IsAdministrator()
        {
            return SessionFacade.CurrentUser.UserGroupId > (int)UserGroup.User;
        }

        public bool IsCustomerOrSystemAdministrator()
        {
            return SessionFacade.CurrentUser.UserGroupId > (int)UserGroup.Administrator;
        }

        public bool IsCaseVisible()
        {
            return this.CustomerSetting.ModuleCase.ToBool();
        }

        public bool IsProblemVisible()
        {
            return this.CustomerSetting.ModuleProblem == 1 && this.IsAdministrator();
        }

        public bool IsChangeManagementVisible()
        {
            return this.CustomerSetting.ModuleChangeManagement.ToBool() && this.IsCustomerOrSystemAdministrator();
        }

        public bool IsCaseSolutionVisible()
        {
            return this.IsCustomerOrSystemAdministrator() || SessionFacade.CurrentUser.CaseSolutionPermission.ToBool();
        }

        public bool IsReportVisible()
        {
            return SessionFacade.CurrentUser.ReportPermission.ToBool();
        }

        public bool IsFaqVisible()
        {
            return this.CustomerSetting.ModuleFAQ.ToBool();
        }

        public bool IsCalendarVisible()
        {
            return this.CustomerSetting.ModuleCalendar.ToBool()
                   && (SessionFacade.CurrentUser.CalendarPermission.ToBool() || this.IsCustomerOrSystemAdministrator());
        }

        public bool IsOrderVisible()
        {
            return this.CustomerSetting.ModuleOrder.ToBool()
                   && (SessionFacade.CurrentUser.OrderPermission > 0 || this.IsCustomerOrSystemAdministrator());
        }

        public bool IsAccountVisible()
        {
            return this.CustomerSetting.ModuleAccount.ToBool()
                   && (SessionFacade.CurrentUser.OrderPermission == 2 || this.IsCustomerOrSystemAdministrator());
        }

        public bool IsCheckListVisuble()
        {
            return this.CustomerSetting.ModuleChecklist.ToBool() && this.IsAdministrator();
        }

        public bool IsDailyReportVisible()
        {
            return this.CustomerSetting.ModuleDailyReport.ToBool() && this.IsAdministrator();
        }

        public bool IsOperationLogVisible()
        {
            return this.CustomerSetting.ModuleOperationLog.ToBool() && this.IsAdministrator();
        }

        public bool IsInventoryVisible()
        {
            return this.CustomerSetting.ModuleInventory.ToBool() && this.IsAdministrator();
        }
             
        public bool IsBulletinBoardVisible()
        {
            return this.CustomerSetting.ModuleBulletinBoard.ToBool();
        }

        public bool IsPlanningVisible()
        {
            return this.CustomerSetting.ModulePlanning.ToBool() && this.IsAdministrator();
        }

        public bool IsProjectVisible()
        {
            return this.CustomerSetting.ModuleProject.ToBool() && this.IsAdministrator();
        }

        public bool IsQuestionVisible()
        {
            return this.CustomerSetting.ModuleQuestion.ToBool() && this.IsCustomerOrSystemAdministrator();
        }

        public bool IsQuestionnaireVisible()
        {
            return this.CustomerSetting.ModuleQuestionnaire.ToBool() && this.IsCustomerOrSystemAdministrator();
        }

        public bool IsLicenseVisible()
        {
            return this.CustomerSetting.ModuleLicense.ToBool() && this.IsAdministrator();
        }

        public bool IsDocumentVisible()
        {
            return this.CustomerSetting.ModuleDocument.ToBool();
        }

        public bool IsComputerUserVisible()
        {
            return this.CustomerSetting.ModuleComputerUser.ToBool() && this.IsAdministrator();
        }

        public bool IsContractVisible()
        {
            return this.CustomerSetting.ModuleContract.ToBool();
        }

		public bool IsInvoicesVisible()
		{
			return CustomerSetting.ModuleInvoice.ToBool() && this.IsCustomerOrSystemAdministrator();
		}

		public bool IsSettingsModulesVisible()
        {
            return                 
                this.IsCheckListVisuble() ||
                this.IsInventoryVisible() ||
                this.IsLicenseVisible() ||
                this.IsAccountVisible() ||
                this.IsContractVisible() ||
                this.IsOrderVisible() ||
                this.IsInvoicesVisible();
        }

        public bool IsModulesVisible()
        {
            return                 
                this.IsBulletinBoardVisible() ||
                this.IsFaqVisible() ||
                this.IsCalendarVisible() ||
                this.IsQuestionnaireVisible() ||
                this.IsDocumentVisible();                
        }

        public bool IsReportsLogsModulesVisible()
        {
            return 
                   this.IsReportVisible() ||
                   this.IsDailyReportVisible() ||        
                   this.IsOperationLogVisible();
        }

        public bool IsCaseHandlingModulesVisible()
        {
            return
                this.IsCaseVisible() ||
                this.IsCaseSolutionVisible() ||
                this.IsComputerUserVisible() ||
                this.IsChangeManagementVisible() ||
                this.IsProblemVisible() ||
                this.IsProjectVisible();
        }

        public bool IsModuleExtendedCaseVisible()
        {
            return this.CustomerSetting.ModuleExtendedCase.ToBool() && this.IsCustomerOrSystemAdministrator();
        }
    }
}