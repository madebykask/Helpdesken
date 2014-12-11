namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class UserFieldSettings
    {
        public UserFieldSettings()
        {
        }

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
        public FieldSettingMultipleChoices Ids { get;  set; }

        [LocalizedDisplay("Förnamn")]
        public FieldSetting FirstName { get; set; }

        [LocalizedDisplay("Initialer")]
        public FieldSetting Initials { get;  set; }

        [LocalizedDisplay("Efternamn")]
        public FieldSetting LastName { get;  set; }

        [LocalizedDisplay("Personnummer")]
        public FieldSettingMultipleChoices PersonalIdentityNumber { get;  set; }

        [LocalizedDisplay("Telefon")]
        public FieldSetting Phone { get;  set; }

        [LocalizedDisplay("Anknytning")]
        public FieldSetting Extension { get;  set; }

        [LocalizedDisplay("E-post")]
        public FieldSetting EMail { get;  set; }

        [LocalizedDisplay("Titel")]
        public FieldSetting Title { get;  set; }

        [LocalizedDisplay("Placering")]
        public FieldSetting Location { get;  set; }

        [LocalizedDisplay("Rum")]
        public FieldSetting RoomNumber { get;  set; }

        [LocalizedDisplay("Besöksadress")]
        public FieldSetting PostalAddress { get;  set; }

        [LocalizedDisplay("Anställningstyp")]
        public FieldSetting EmploymentType { get;  set; }

        [LocalizedDisplay("Avdelning")]
        public FieldSetting DepartmentId { get;  set; }

        [LocalizedDisplay("Enhet")]
        public FieldSetting UnitId { get;  set; }

        [LocalizedDisplay("Avdelning 2")]
        public FieldSetting DepartmentId2 { get;  set; }

        [LocalizedDisplay("Övrigt")]
        public FieldSetting Info { get;  set; }

        [LocalizedDisplay("Ansvar")]
        public FieldSetting Responsibility { get;  set; }

        [LocalizedDisplay("Verksamhet")]
        public FieldSetting Activity { get;  set; }

        [LocalizedDisplay("Chef")]
        public FieldSetting Manager { get;  set; }

        [LocalizedDisplay("Referensnummer")]
        public FieldSetting ReferenceNumber { get;  set; }
    }
}