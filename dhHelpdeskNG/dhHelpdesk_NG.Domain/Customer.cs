namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.Domain.Faq;

    using global::System;
    using global::System.Collections.Generic;

    public class Customer : Entity
    {
        public int ControlTime { get; set; }
        public int? CustomerGroup_Id { get; set; }
        public int Days2WaitBeforeDelete { get; set; }
        public int EMailSendFromOrder { get; set; }
        public int Language_Id { get; set; }
        public int LockCaseToWorkingGroup { get; set; }
        public int MarkPassedWatchDateAcute { get; set; }
        public int OrderPermission { get; set; }
        public int OverwriteFromMasterDirectory { get; set; }
        public int PasswordRequiredOnExternalPage { get; set; }
        public int ResponsibleReminderControlTime { get; set; }
        public int ShowBulletinBoardOnExtPage { get; set; }
        public int ShowCalenderOnExtPage { get; set; }
        public int ShowOperationalLogOnExtPage { get; set; }
        public int ShowCaseOnExternalPage { get; set; }
        public int ShowDashboardOnExternalPage { get; set; }
        public int ShowFAQOnExternalPage { get; set; }
        public int WorkingDayEnd { get; set; }
        public int WorkingDayStart { get; set; }
        public string Address { get; set; }
        public string CaseStatisticsEmailList { get; set; }
        public string CustomerID { get; set; }
        public string CustomerNumber { get; set; }
        public string DailyReportEmail { get; set; }
        public string DirectoryPathExclude { get; set; }
        public string HelpdeskEmail { get; set; }
        public string Logo { get; set; }
        public string LogoBackColor { get; set; }
        public string Name { get; set; }
        public string NewCaseEmailList { get; set; }
        public string CloseCaseEmailList { get; set; }
        public string NDSPath { get; set; }
        public string OrderEMailList { get; set; }
        public string Phone { get; set; }
        public string PostalAddress { get; set; }
        public string PostalCode { get; set; }
        public string RegistrationMessage { get; set; }
        public string ResponsibleReminderEmailList { get; set; }
        public DateTime ChangeTime { get; set; }
        public DateTime RegTime { get; set; }
        public Guid CustomerGUID { get; set; }
        public int CommunicateWithNotifier { get; set; }
        public int ShowDocumentsOnExternalPage { get; set; }
        public int? ShowFAQOnExternalStartPage { get; set; }
        public int ShowCoWorkersOnExternalPage { get; set; }
        public int ShowHelpOnExternalPage { get; set; }
        public int UseInternalLogNoteOnExternalPage { get; set; }
        public int ShowCasesOnExternalPage { get; set; }
        public bool MyCasesInitiator { get; set; }
        public bool MyCasesRegistrator { get; set; } = true;
        public bool MyCasesFollower { get; set; }
        public bool MyCasesRegarding { get; set; }
        public bool MyCasesUserGroup { get; set; }        
        public bool FetchDataFromApiOnExternalPage { get; set; }
        public bool RestrictUserToGroupOnExternalPage { get; set; }
        public int GroupCaseTemplates { get; set; }
        public bool FetchPcNumber { get; set; }

        public bool ShowCaseActionsPanelOnTop { get; set; }
        public bool ShowCaseActionsPanelAtBottom { get; set; }

        public virtual Language Language { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<Case> Cases { get; set; }
        public virtual ICollection<CaseFieldSetting> CaseFieldSettings { get; set; }
        public virtual ICollection<ComputerUserFieldSettings> ComputerUserFieldSettings { get; set; }
        public virtual ICollection<ReportCustomer> ReportCustomers { get; set; }
        public virtual ICollection<ReportFavorite> ReportFavorites { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<FaqEntity> FAQs { get; set; }

        /// <summary>
        /// Users selected for this customer
        /// </summary>
        public virtual ICollection<User> Users { get; set; }

        /// <summary>
        /// Users available for this customer
        /// </summary>
        public virtual ICollection<User> UsersAvailable { get; set; }

        public virtual ICollection<Product> Products { get; set; } 

        public virtual ICollection<ChangeEntity> Changes { get; set; } 

        public virtual ICollection<Order> Orders { get; set; } 
    }
}
