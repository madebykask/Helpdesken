namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview
{
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdererOverviewSettings
    {
        public OrdererOverviewSettings(
            FieldOverviewSetting id,
            FieldOverviewSetting name,
            FieldOverviewSetting phone,
            FieldOverviewSetting cellPhone,
            FieldOverviewSetting email,
            FieldOverviewSetting department)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Department = department;
        }

        [NotNull]
        public FieldOverviewSetting Id { get; private set; }

        [NotNull]
        public FieldOverviewSetting Name { get; private set; }

        [NotNull]
        public FieldOverviewSetting Phone { get; private set; }

        [NotNull]
        public FieldOverviewSetting CellPhone { get; private set; }

        [NotNull]
        public FieldOverviewSetting Email { get; private set; }

        [NotNull]
        public FieldOverviewSetting Department { get; private set; }
    }
}
