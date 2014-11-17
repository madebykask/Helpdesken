namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UserFieldSettingsOverview
    {
        public UserFieldSettingsOverview(
                FieldOverviewSetting userId, 
                FieldOverviewSetting userFirstName, 
                FieldOverviewSetting userLastName)
        {
            this.UserLastName = userLastName;
            this.UserFirstName = userFirstName;
            this.UserId = userId;
        }

        [NotNull]
        public FieldOverviewSetting UserId { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserFirstName { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserLastName { get; private set; }         
    }
}