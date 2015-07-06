namespace DH.Helpdesk.BusinessData.Models.User.Input
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;

    using UserGroup = DH.Helpdesk.BusinessData.Enums.Admin.Users.UserGroup;

    public class UserOverview
    {
        public UserOverview(
            int id,
            string userId,
            int customerId,
            int languageId,
            int userGroupId,
            int followUpPermission,
            int restrictedCasePermission,
            int showNotAssignedWorkingGroups,
            int createCasePermission,
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
            int setPriorityPermission,
            int invoicePermission,
            int dataSecurityPermission,
            int refreshContent,
            string firstName,
            string surName,
            string phone,
            string email,
            ICollection<UserWorkingGroup> wgs,
            int startPage,
            bool showSolutionTime,
            string timeZoneId)
        {
            this.StartPage = startPage;
            this.Id = id;
            this.UserId = userId;
            this.CustomerId = customerId;
            this.LanguageId = languageId;
            this.UserGroupId = userGroupId;
            this.FollowUpPermission = followUpPermission;
            this.RestrictedCasePermission = restrictedCasePermission;
            this.ShowNotAssignedWorkingGroups = showNotAssignedWorkingGroups;
            this.CreateCasePermission = createCasePermission;
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
            this.SetPriorityPermission = setPriorityPermission;
            this.InvoicePermission = invoicePermission;
            this.DataSecurityPermission = dataSecurityPermission;
            this.RefreshContent = refreshContent;
            this.FirstName = firstName;
            this.SurName = surName;
            this.Phone = phone;
            this.Email = email;
            this.UserWorkingGroups = wgs;
            this.ShowSolutionTime = showSolutionTime;
            this.TimeZoneId = timeZoneId;
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

        public int SetPriorityPermission { get; set; }

        public int InvoicePermission { get; set; }

        public int DataSecurityPermission { get; set; }

        public int RestrictedCasePermission { get; set; }

        public int ShowNotAssignedWorkingGroups { get; set; }

        public int RefreshContent { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public ICollection<UserWorkingGroup> UserWorkingGroups { get; set; }

        public int StartPage { get; private set; }

        public bool ShowSolutionTime { get; set; }

        public string TimeZoneId { get; set; }

        public bool IsAdministrator()
        {
            return this.UserGroupId > (int)UserGroup.User;
        }
    }
}
