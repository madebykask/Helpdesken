namespace dhHelpdesk_NG.DTO.DTOs.User.Input
{
    public class UserOverview
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int CustomerId { get; set; }

        public int LanguageId { get; set; }

        public int UserGroupId { get; set; }

        public int FollowUpPermission { get; set; }

        public int RestrictedCasePermission { get; set; }

        public int ShowNotAssignedWorkingGroups { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }
    }
}