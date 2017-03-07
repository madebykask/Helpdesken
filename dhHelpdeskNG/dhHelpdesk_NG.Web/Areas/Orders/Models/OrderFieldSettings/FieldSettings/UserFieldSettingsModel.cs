namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class UserFieldSettingsModel
    {
        public UserFieldSettingsModel()
        {
        }

        public UserFieldSettingsModel(TextFieldSettingsModel userId, TextFieldSettingsModel userFirstName, TextFieldSettingsModel userLastName, TextFieldSettingsModel userPhone, TextFieldSettingsModel userEMail, TextFieldSettingsModel personalIdentityNumber, TextFieldSettingsModel initials, TextFieldSettingsModel extension, TextFieldSettingsModel title, TextFieldSettingsModel location, TextFieldSettingsModel roomNumber, TextFieldSettingsModel postalAddress, TextFieldSettingsModel employmentType, TextFieldSettingsModel departmentId1, TextFieldSettingsModel unitId, TextFieldSettingsModel departmentId2, TextFieldSettingsModel info, TextFieldSettingsModel responsibility, TextFieldSettingsModel activity, TextFieldSettingsModel manager, TextFieldSettingsModel referenceNumber)
        {
            UserId = userId;
            UserFirstName = userFirstName;
            UserLastName = userLastName;
            UserPhone = userPhone;
            UserEMail = userEMail;
            PersonalIdentityNumber = personalIdentityNumber;
            Initials = initials;
            Extension = extension;
            Title = title;
            Location = location;
            RoomNumber = roomNumber;
            PostalAddress = postalAddress;
            EmploymentType = employmentType;
            DepartmentId1 = departmentId1;
            UnitId = unitId;
            DepartmentId2 = departmentId2;
            Info = info;
            Responsibility = responsibility;
            Activity = activity;
            Manager = manager;
            ReferenceNumber = referenceNumber;
        }

        [LocalizedStringLength(30)]
        public string Header { get; set; }

        [NotNull]
        [LocalizedDisplay("Användar ID")]
        public TextFieldSettingsModel UserId { get; set; }
         
        [NotNull]
        [LocalizedDisplay("Förnamn")]
        public TextFieldSettingsModel UserFirstName { get; set; }
         
        [NotNull]
        [LocalizedDisplay("Efternamn")]
        public TextFieldSettingsModel UserLastName { get; set; }

        [NotNull]
        [LocalizedDisplay("Telefon")]
        public TextFieldSettingsModel UserPhone { get; set; }
        
        [NotNull]
        [LocalizedDisplay("E-post")]
        public TextFieldSettingsModel UserEMail { get; set; }

        [LocalizedDisplay("Personnummer")]
        public TextFieldSettingsModel PersonalIdentityNumber { get; set; }

        [LocalizedDisplay("Initialer")]
        public TextFieldSettingsModel Initials { get; set; }

        [LocalizedDisplay("Anknytning")]
        public TextFieldSettingsModel Extension { get; set; }

        [LocalizedDisplay("Titel")]
        public TextFieldSettingsModel Title { get; set; }

        [LocalizedDisplay("Placering")]
        public TextFieldSettingsModel Location { get; set; }

        [LocalizedDisplay("Rum")]
        public TextFieldSettingsModel RoomNumber { get; set; }

        [LocalizedDisplay("Besöksadress")]
        public TextFieldSettingsModel PostalAddress { get; set; }

        [LocalizedDisplay("Anställningstyp")]
        public TextFieldSettingsModel EmploymentType { get; set; }

        [LocalizedDisplay("Avdelning")]
        public TextFieldSettingsModel DepartmentId1 { get; set; }

        [LocalizedDisplay("Enhet")]
        public TextFieldSettingsModel UnitId { get; set; }

        [LocalizedDisplay("Avdelning 2")]
        public TextFieldSettingsModel DepartmentId2 { get; set; }

        [LocalizedDisplay("Övrigt")]
        public TextFieldSettingsModel Info { get; set; }

        [LocalizedDisplay("Ansvar")]
        public TextFieldSettingsModel Responsibility { get; set; }

        [LocalizedDisplay("Verksamhet")]
        public TextFieldSettingsModel Activity { get; set; }

        [LocalizedDisplay("Chef")]
        public TextFieldSettingsModel Manager { get; set; }

        [LocalizedDisplay("Referensnummer")]
        public TextFieldSettingsModel ReferenceNumber { get; set; }
    }
}