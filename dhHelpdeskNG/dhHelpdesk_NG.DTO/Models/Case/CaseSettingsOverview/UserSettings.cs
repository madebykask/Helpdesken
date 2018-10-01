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
            FieldOverviewSetting ordererCode,
            FieldOverviewSetting costcentre,
            FieldOverviewSetting isabout_user,
            FieldOverviewSetting isabout_persons_name,
            FieldOverviewSetting isabout_persons_phone,
            FieldOverviewSetting isabout_persons_cellphone,
            FieldOverviewSetting isabout_persons_email,
            FieldOverviewSetting isabout_department,
            FieldOverviewSetting isabout_region,
            FieldOverviewSetting isabout_ou,
            FieldOverviewSetting isabout_costcentre,
            FieldOverviewSetting isabout_place,
            FieldOverviewSetting isabout_usercode)
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
            this.CostCentre = costcentre;
            this.IsAbout_User = isabout_user;
            this.IsAbout_Persons_Name = isabout_persons_name;
            this.IsAbout_Persons_Phone = isabout_persons_phone;
            this.IsAbout_Persons_CellPhone = isabout_persons_cellphone;
            this.IsAbout_Persons_Email = isabout_persons_email;
            this.IsAbout_Department = isabout_department;
            this.IsAbout_Region = isabout_region;
            this.IsAbout_OU = isabout_ou;
            this.IsAbout_CostCentre = isabout_costcentre;
            this.IsAbout_Place = isabout_place;
            this.IsAbout_UserCode = isabout_usercode;
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

        [NotNull]
        public FieldOverviewSetting CostCentre { get; private set; }

        [NotNull]
        public FieldOverviewSetting IsAbout_User { get; private set; }

        [NotNull]
        public FieldOverviewSetting IsAbout_Persons_Name { get; private set; }

        [NotNull]
        public FieldOverviewSetting IsAbout_Persons_Phone { get; private set; }

        [NotNull]
        public FieldOverviewSetting IsAbout_Persons_CellPhone { get; private set; }

        [NotNull]
        public FieldOverviewSetting IsAbout_Persons_Email { get; private set; }

        [NotNull]
        public FieldOverviewSetting IsAbout_Department { get; private set; }

        [NotNull]
        public FieldOverviewSetting IsAbout_Region { get; private set; }

        [NotNull]
        public FieldOverviewSetting IsAbout_OU { get; private set; }

        [NotNull]
        public FieldOverviewSetting IsAbout_CostCentre { get; private set; }

        [NotNull]
        public FieldOverviewSetting IsAbout_Place { get; private set; }

        [NotNull]
        public FieldOverviewSetting IsAbout_UserCode { get; private set; }
    }
}