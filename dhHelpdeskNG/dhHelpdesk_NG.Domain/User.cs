namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Accounts;
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;
    using global::System.Collections.Generic;

    public class User : Entity, ICustomerEntity, IActiveEntity
    {
        public int AccountType { get; set; }
        public int ActivateCasePermission { get; set; }

        /// <summary>
        /// Send e-mail when the user is assigned a case
        /// </summary>
        public int AllocateCaseMail { get; set; }

        /// <summary>
        /// Send SMS when the user is assigned a case
        /// </summary>
        public int AllocateCaseSMS { get; set; }

        public int BulletinBoardPermission { get; set; }

        public int DocumentPermission { get; set; }

        public int InventoryPermission { get; set; }
        /// <summary>
        /// Allows user to manage events in calendar    
        /// </summary>
        public int CalendarPermission { get; set; }
        public int CaseInfoMail { get; set; }
        public int CaseSolutionPermission { get; set; }
        public int CloseCasePermission { get; set; }
        public int CreateCasePermission { get; set; }

        /// <summary>
        /// Allows user to create subcases
        /// </summary>
        public int CreateSubCasePermission { get; set; }
        public int CopyCasePermission { get; set; }
        public int Customer_Id { get; set; }
        public int DailyReportReminder { get; set; }

        // obsolete field http://redmine.fastdev.se/issues/10997
        public int? Default_WorkingGroup_Id { get; set; }
        
        public int DeleteCasePermission { get; set; }
        public int DeleteAttachedFilePermission { get; set; }
        public int? Domain_Id { get; set; }
        public int ExternalUpdateMail { get; set; }
        public int FAQPermission { get; set; }
        public int FollowUpPermission { get; set; }
        public int InvoicePermission { get; set; }
        public int IsActive { get; set; }
        public int Language_Id { get; set; }
        public int Layout_Id { get; set; }
        public int MarkRequiredFields { get; set; }
        public int MoveCasePermission { get; set; }
        public int OrderPermission { get; set; }
        public int Performer { get; set; }
        public int PlanDateMail { get; set; }
        public int RefreshContent { get; set; }
        public int ReportPermission { get; set; }

        /// <summary>
        /// User has permission to see own cases only
        /// </summary>
        public int RestrictedCasePermission { get; set; }
        public int SessionTimeout { get; set; }
        public int SetPriorityPermission { get; set; }
        public int ShowNotAssignedCases { get; set; }
        public int ShowNotAssignedWorkingGroups { get; set; }
        public int ShowQuickMenuOnStartPage { get; set; }
        public int StartPage { get; set; }
        public int TimeAutoAuthorize { get; set; }
        public int TimeRegistration { get; set; }
        public int UserGroup_Id { get; set; }
        public int WatchDateMail { get; set; }
        public int DataSecurityPermission { get; set; }
        public string Address { get; set; }
        public string ArticleNumber { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Logo { get; set; }
        public string LogoBackColor { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string PostalAddress { get; set; }
        public string PostalCode { get; set; }
        public string SurName { get; set; }
        public string UserID { get; set; }
        public DateTime? BulletinBoardDate { get; set; }
        public DateTime ChangeTime { get; set; }
        public DateTime PasswordChangedDate { get; set; }
        public DateTime RegTime { get; set; }
        public Guid UserGUID { get; set; }
        public int CaseUnlockPermission { get; set; }
        public int CaseInternalLogPermission { get; set; }

        /// <summary>
        /// Flag to display "Solution time" grid on case overview page
        /// </summary>
        public int ShowSolutionTime { get; set; }
        public int ShowCaseStatistics { get; set; }
        public int SettingForNoMail { get; set; }


        public string TimeZoneId { get; set; }

        public virtual Domain Domain { get; set; }
        public virtual Language Language { get; set; }
        public virtual User ChangedByUser { get; set; }
        public virtual UserGroup UserGroup { get; set; }

        /// <summary>
        /// Cusomers selected for user. Should be equal to/included by this.CusomersAvailable
        /// </summary>
        public virtual ICollection<Customer> Cs { get; set; }

        /// <summary>
        /// Customers available for this user
        /// </summary>
        public virtual ICollection<Customer> CusomersAvailable { get; set; }

        public virtual ICollection<CustomerUser> CustomerUsers { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<DepartmentUser> DUs { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserWorkingGroup> UserWorkingGroups { get; set; }
        public virtual ICollection<AccountActivity> AAs { get; set; }
        public virtual ICollection<OrderType> OTs { get; set; }
        public virtual ICollection<OperationLog> OLs { get; set; }
		public virtual ICollection<ReportFavorite> ReportFavorites { get; set; }
	}
}
