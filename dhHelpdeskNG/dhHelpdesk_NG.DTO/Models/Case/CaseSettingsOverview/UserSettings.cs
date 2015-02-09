namespace DH.Helpdesk.BusinessData.Models.Case.CaseSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UserSettings
    {
        public UserSettings(
            FieldOverviewSetting user, 
            FieldOverviewSetting notifier, 
            FieldOverviewSetting email, 
            FieldOverviewSetting phone, 
            FieldOverviewSetting cellPhone, 
            FieldOverviewSetting customer, 
            FieldOverviewSetting region, 
            FieldOverviewSetting department, 
            FieldOverviewSetting unit, 
            FieldOverviewSetting place, 
            FieldOverviewSetting ordererCode)
        {
            this.OrdererCode = ordererCode;
            this.Place = place;
            this.Unit = unit;
            this.Department = department;
            this.Region = region;
            this.Customer = customer;
            this.CellPhone = cellPhone;
            this.Phone = phone;
            this.Email = email;
            this.Notifier = notifier;
            this.User = user;
        }

        [NotNull]
        public FieldOverviewSetting User { get; private set; }

        [NotNull]
        public FieldOverviewSetting Notifier { get; private set; }

        [NotNull]
        public FieldOverviewSetting Email { get; private set; }

        [NotNull]
        public FieldOverviewSetting Phone { get; private set; }

        [NotNull]
        public FieldOverviewSetting CellPhone { get; private set; }

        [NotNull]
        public FieldOverviewSetting Customer { get; private set; }

        [NotNull]
        public FieldOverviewSetting Region { get; private set; }

        [NotNull]
        public FieldOverviewSetting Department { get; private set; }

        [NotNull]
        public FieldOverviewSetting Unit { get; private set; }

        [NotNull]
        public FieldOverviewSetting Place { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrdererCode { get; private set; }
    }
}