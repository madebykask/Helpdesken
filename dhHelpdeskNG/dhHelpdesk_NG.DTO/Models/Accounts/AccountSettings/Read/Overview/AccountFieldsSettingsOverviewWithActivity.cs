namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class AccountFieldsSettingsOverviewWithActivity
    {
        public AccountFieldsSettingsOverviewWithActivity(
            int activityId,
            AccountFieldsSettingsOverview accountFieldsSettingsOverview)
        {
            this.ActivityId = activityId;
            this.AccountFieldsSettingsOverview = accountFieldsSettingsOverview;
        }

        [IsId]
        public int ActivityId { get; set; }

        public AccountFieldsSettingsOverview AccountFieldsSettingsOverview { get; set; }
    }
}