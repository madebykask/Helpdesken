using DH.Helpdesk.BusinessData.Enums.Orders.Fields;

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

        [LocalizedStringLength(50)]
        public string Header { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.UserId)]
        public TextFieldSettingsModel UserId { get; set; }
         
        [NotNull]
        [LocalizedDisplay(OrderLabels.UserFirstName)]
        public TextFieldSettingsModel UserFirstName { get; set; }
         
        [NotNull]
        [LocalizedDisplay(OrderLabels.UserLastName)]
        public TextFieldSettingsModel UserLastName { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.UserPhone)]
        public TextFieldSettingsModel UserPhone { get; set; }
        
        [NotNull]
        [LocalizedDisplay(OrderLabels.UserEMail)]
        public TextFieldSettingsModel UserEMail { get; set; }

        [LocalizedDisplay(OrderLabels.UserPersonalIdentityNumber)]
        public TextFieldSettingsModel PersonalIdentityNumber { get; set; }

        [LocalizedDisplay(OrderLabels.UserInitials)]
        public TextFieldSettingsModel Initials { get; set; }

        [LocalizedDisplay(OrderLabels.UserExtension)]
        public TextFieldSettingsModel Extension { get; set; }

        [LocalizedDisplay(OrderLabels.UserTitle)]
        public TextFieldSettingsModel Title { get; set; }

        [LocalizedDisplay(OrderLabels.UserLocation)]
        public TextFieldSettingsModel Location { get; set; }

        [LocalizedDisplay(OrderLabels.UserRoomNumber)]
        public TextFieldSettingsModel RoomNumber { get; set; }

        [LocalizedDisplay(OrderLabels.UserPostalAddress)]
        public TextFieldSettingsModel PostalAddress { get; set; }

        [LocalizedDisplay(OrderLabels.UserEmploymentType)]
        public TextFieldSettingsModel EmploymentType { get; set; }

        [LocalizedDisplay(OrderLabels.UserDepartment_Id1)]
        public TextFieldSettingsModel DepartmentId1 { get; set; }

        [LocalizedDisplay(OrderLabels.UserOU_Id)]
        public TextFieldSettingsModel UnitId { get; set; }

        [LocalizedDisplay(OrderLabels.UserDepartment_Id2)]
        public TextFieldSettingsModel DepartmentId2 { get; set; }

        [LocalizedDisplay(OrderLabels.UserInfo)]
        public TextFieldSettingsModel Info { get; set; }

        [LocalizedDisplay(OrderLabels.UserResponsibility)]
        public TextFieldSettingsModel Responsibility { get; set; }

        [LocalizedDisplay(OrderLabels.UserActivity)]
        public TextFieldSettingsModel Activity { get; set; }

        [LocalizedDisplay(OrderLabels.UserManager)]
        public TextFieldSettingsModel Manager { get; set; }

        [LocalizedDisplay(OrderLabels.UserReferenceNumber)]
        public TextFieldSettingsModel ReferenceNumber { get; set; }
    }
}