namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UserFieldSettingsOverview
    {
        public UserFieldSettingsOverview(FieldOverviewSetting userId, FieldOverviewSetting userFirstName, FieldOverviewSetting userLastName, FieldOverviewSetting userPhone, FieldOverviewSetting userEMail)
        {
            UserId = userId;
            UserFirstName = userFirstName;
            UserLastName = userLastName;
            UserPhone = userPhone;
            UserEMail = userEMail;
        }

        [NotNull]
        public FieldOverviewSetting UserId { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserFirstName { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserLastName { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserPhone { get; private set; }

        [NotNull]
        public FieldOverviewSetting UserEMail { get; private set; }


    }
}