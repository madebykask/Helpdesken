namespace DH.Helpdesk.BusinessData.Models.User.Input
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class UserOverview
    {
        public UserOverview(int id, string userId, int customerId, int languageId, int userGroupId, int followUpPermission, int restrictedCasePermission, int showNotAssignedWorkingGroups, string firstName, string surName)
        {
            this.Id = id;
            this.UserId = userId;
            this.CustomerId = customerId;
            this.LanguageId = languageId;
            this.UserGroupId = userGroupId;
            this.FollowUpPermission = followUpPermission;
            this.RestrictedCasePermission = restrictedCasePermission;
            this.ShowNotAssignedWorkingGroups = showNotAssignedWorkingGroups;
            this.FirstName = firstName;
            this.SurName = surName;
        }

        [IsId]
        public int Id { get; set; }

        public string UserId { get; set; }

        [IsId]
        public int CustomerId { get; set; }

        [IsId]
        public int LanguageId { get; set; }

        [IsId]
        public int UserGroupId { get; set; }

        public int FollowUpPermission { get; set; }

        public int RestrictedCasePermission { get; set; }

        public int ShowNotAssignedWorkingGroups { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }
    }
}