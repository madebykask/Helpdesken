namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit
{
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

        public FieldSettingMultipleChoices Ids { get; private set; }

        public FieldSetting FirstName { get; private set; }

        public FieldSetting Initials { get; private set; }

        public FieldSetting LastName { get; private set; }

        public FieldSettingMultipleChoices PersonalIdentityNumber { get; private set; }

        public FieldSetting Phone { get; private set; }

        public FieldSetting Extension { get; private set; }

        public FieldSetting EMail { get; private set; }

        public FieldSetting Title { get; private set; }

        public FieldSetting Location { get; private set; }

        public FieldSetting RoomNumber { get; private set; }

        public FieldSetting PostalAddress { get; private set; }

        public FieldSetting EmploymentType { get; private set; }

        public FieldSetting DepartmentId { get; private set; }

        public FieldSetting UnitId { get; private set; }

        public FieldSetting DepartmentId2 { get; private set; }

        public FieldSetting Info { get; private set; }

        public FieldSetting Responsibility { get; private set; }

        public FieldSetting Activity { get; private set; }

        public FieldSetting Manager { get; private set; }

        public FieldSetting ReferenceNumber { get; private set; }
    }
}