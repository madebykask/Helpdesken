namespace DH.Helpdesk.BusinessData.Models.User.Input
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;

    using UserGroup = DH.Helpdesk.BusinessData.Enums.Admin.Users.UserGroup;

    public class UserOverview
    {
        public UserOverview()
        {
        }

        public UserOverview(
            int id,
            string userId,
            int customerId,
            int languageId,
            int userGroupId,
            int followUpPermission,
            //int restrictedCasePermission,
            int showNotAssignedWorkingGroups,
            int createCasePermission,
            int createSubCasePermission,
            int mergeCasePermission,
            int copyCasePermission,
            int orderPermission,
            int caseSolutionPermission,
            int deleteCasePermission,
            int deleteAttachedFilePermission,
            int moveCasePermission,
            int activateCasePermission,
            int reportPermission,
            int closeCasePermission,
            int calendarPermission,
            int faqPermission,
            int bulletinBoardPermission,
            int documentPermission,
            int inventoryAdminPermission,
            int inventoryViewPermission,
            int contractPermission,
            int setPriorityPermission,
            int invoicePermission,
            int caseUnlockPermission,
            int refreshContent,
            string firstName,
            string surName,
            string phone,
            string email,
            ICollection<UserWorkingGroup> wgs,
            int startPage,
            bool showSolutionTime,
            bool showCaseStatistics,
            string timeZoneId,
            Guid? userGuid,
            int caseInternalLogPermission,
            int invoiceTimePermission)
        {
            this.StartPage = startPage;
            this.Id = id;
            this.UserId = userId;
            this.CustomerId = customerId;
            this.LanguageId = languageId;
            this.UserGroupId = userGroupId;
            //this.RestrictedCasePermission = restrictedCasePermission;
            this.ShowNotAssignedWorkingGroups = showNotAssignedWorkingGroups;
            this.CreateCasePermission = createCasePermission;
            this.MergeCasePermission = mergeCasePermission;
            this.CreateSubCasePermission = createSubCasePermission;
            this.CopyCasePermission = copyCasePermission;
            this.OrderPermission = orderPermission;
            this.CaseSolutionPermission = caseSolutionPermission;
            this.DeleteCasePermission = deleteCasePermission;
            this.DeleteAttachedFilePermission = deleteAttachedFilePermission;
            this.MoveCasePermission = moveCasePermission;
            this.ActivateCasePermission = activateCasePermission;
            this.ReportPermission = reportPermission;
            this.CloseCasePermission = closeCasePermission;
            this.CalendarPermission = calendarPermission;
            this.FAQPermission = faqPermission;
            this.FollowUpPermission = followUpPermission;
            this.BulletinBoardPermission = bulletinBoardPermission;
            this.DocumentPermission = documentPermission;
            this.InventoryAdminPermission = inventoryAdminPermission;
            this.InventoryViewPermission = inventoryViewPermission;
            this.ContractPermission = contractPermission;
            this.SetPriorityPermission = setPriorityPermission;
            this.InvoicePermission = invoicePermission;
            this.CaseUnlockPermission = caseUnlockPermission;
            this.RefreshContent = refreshContent;
            this.FirstName = firstName;
            this.SurName = surName;
            this.Phone = phone;
            this.Email = email;
            this.UserWorkingGroups = wgs;
            this.ShowSolutionTime = showSolutionTime;
            this.ShowCaseStatistics = showCaseStatistics;
            this.TimeZoneId = timeZoneId;
            this.UserGUID = userGuid;
            this.CaseInternalLogPermission = caseInternalLogPermission;
            this.InvoiceTimePermission = invoiceTimePermission;
        }

        [IsId]
        public int Id { get; set; }

        public string UserId { get; set; }

        [IsId]
        public int CustomerId { get; set; }

        [IsId]
        public int LanguageId { get; set; }

        /// <summary>
        /// One of DH.Helpdesk.BusinessData.Enums.Admin.Users.UserGroup enum
        /// </summary>
        [IsId]
        public int UserGroupId { get; set; }

        public int FollowUpPermission { get; set; }

        public int CreateCasePermission { get; set; }

        /// <summary>
        /// This permissions allows to user create sub cases for case
        /// </summary>
        public int CreateSubCasePermission { get; set; }

        public int MergeCasePermission { get; set; }

        public int CopyCasePermission { get; set; }

        public int OrderPermission { get; set; }

        public int CaseSolutionPermission { get; set; }

        public int DeleteCasePermission { get; set; }

        public int DeleteAttachedFilePermission { get; set; }

        public int MoveCasePermission { get; set; }

        public int ActivateCasePermission { get; set; }

        public int ReportPermission { get; set; }

        public int CloseCasePermission { get; set; }

        public int CalendarPermission { get; set; }

        public int FAQPermission { get; set; }

        public int BulletinBoardPermission { get; set; }

        public int DocumentPermission { get; set; }

        public int SetPriorityPermission { get; set; }

        public int InvoicePermission { get; set; }

        //public int RestrictedCasePermission { get; set; }

        public int ShowNotAssignedWorkingGroups { get; set; }

        public int RefreshContent { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public ICollection<UserWorkingGroup> UserWorkingGroups { get; set; }

        public int StartPage { get; set; }

        public bool ShowSolutionTime { get; set; }

        public bool ShowCaseStatistics { get; set; }

        public string TimeZoneId { get; set; }

        public int InventoryAdminPermission { get; set; }

        public int InventoryViewPermission { get; internal set; }

        public int ContractPermission { get; set; }

        public int CaseUnlockPermission { get; set; }

        public int CaseInternalLogPermission { get; set; }
        public int InvoiceTimePermission { get; set; }

        public bool IsAdministrator()
        {
            return this.UserGroupId > (int)UserGroup.User;
        }
        public bool IsSystemAdministrator()
        {
            return this.UserGroupId > (int)UserGroup.CustomerAdministrator;
        }

        public Guid? UserGUID { get; set; }
    }
}
