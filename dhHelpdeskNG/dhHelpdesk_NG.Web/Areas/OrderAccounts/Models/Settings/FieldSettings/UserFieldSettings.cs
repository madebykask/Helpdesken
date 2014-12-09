namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class UserFieldSettings
    {
        public UserFieldSettings(
            FieldSettingMultipleChoices ids,
            FieldSetting firstName,
            FieldSetting initials,
            FieldSetting lastName,
            FieldSettingMultipleChoices personalIdentityNumber,
            FieldSetting phone,
            FieldSetting extension,
            FieldSetting eMail,
            FieldSetting title,
            FieldSetting location,
            FieldSetting roomNumber,
            FieldSetting postalAddress,
            FieldSetting employmentType,
            FieldSetting departmentId,
            FieldSetting unitId,
            FieldSetting departmentId2,
            FieldSetting info,
            FieldSetting responsibility,
            FieldSetting activity,
            FieldSetting manager,
            FieldSetting referenceNumber)
        {
            this.Ids = ids;
            this.FirstName = firstName;
            this.Initials = initials;
            this.LastName = lastName;
            this.PersonalIdentityNumber = personalIdentityNumber;
            this.Phone = phone;
            this.Extension = extension;
            this.EMail = eMail;
            this.Title = title;
            this.Location = location;
            this.RoomNumber = roomNumber;
            this.PostalAddress = postalAddress;
            this.EmploymentType = employmentType;
            this.DepartmentId = departmentId;
            this.UnitId = unitId;
            this.DepartmentId2 = departmentId2;
            this.Info = info;
            this.Responsibility = responsibility;
            this.Activity = activity;
            this.Manager = manager;
            this.ReferenceNumber = referenceNumber;
        }

        [LocalizedDisplay("Användar ID")]
        public FieldSettingMultipleChoices Ids { get; private set; }

        [LocalizedDisplay("Personnummer")]
        public FieldSetting FirstName { get; private set; }

        [LocalizedDisplay("Förnamn")]
        public FieldSetting Initials { get; private set; }

        [LocalizedDisplay("Initialer")]
        public FieldSetting LastName { get; private set; }

        [LocalizedDisplay("Efternamn")]
        public FieldSettingMultipleChoices PersonalIdentityNumber { get; private set; }

        [LocalizedDisplay("Telefon")]
        public FieldSetting Phone { get; private set; }

        [LocalizedDisplay("Anknytning")]
        public FieldSetting Extension { get; private set; }

        [LocalizedDisplay("E-post")]
        public FieldSetting EMail { get; private set; }

        [LocalizedDisplay("Titel")]
        public FieldSetting Title { get; private set; }

        [LocalizedDisplay("Placering")]
        public FieldSetting Location { get; private set; }

        [LocalizedDisplay("Rum")]
        public FieldSetting RoomNumber { get; private set; }

        [LocalizedDisplay("Besöksadress")]
        public FieldSetting PostalAddress { get; private set; }

        [LocalizedDisplay("Anställningstyp")]
        public FieldSetting EmploymentType { get; private set; }

        [LocalizedDisplay("Avdelning")]
        public FieldSetting DepartmentId { get; private set; }

        [LocalizedDisplay("Enhet")]
        public FieldSetting UnitId { get; private set; }

        [LocalizedDisplay("Avdelning 2")]
        public FieldSetting DepartmentId2 { get; private set; }

        [LocalizedDisplay("Övrigt")]
        public FieldSetting Info { get; private set; }

        [LocalizedDisplay("Ansvar")]
        public FieldSetting Responsibility { get; private set; }

        [LocalizedDisplay("Verksamhet")]
        public FieldSetting Activity { get; private set; }

        [LocalizedDisplay("Chef")]
        public FieldSetting Manager { get; private set; }

        [LocalizedDisplay("Referensnummer")]
        public FieldSetting ReferenceNumber { get; private set; }
    }
}