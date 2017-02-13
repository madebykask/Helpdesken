namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UserFieldSettings : HeaderSettings
    {
        public UserFieldSettings(TextFieldSettings userId, TextFieldSettings userFirstName, TextFieldSettings userLastName, TextFieldSettings userPhone, TextFieldSettings userEMail, TextFieldSettings initials, TextFieldSettings personalIdentityNumber, TextFieldSettings extension, TextFieldSettings title, TextFieldSettings location, TextFieldSettings roomNumber, TextFieldSettings postalAddress, TextFieldSettings employmentType, TextFieldSettings departmentId1, TextFieldSettings unitId, TextFieldSettings departmentId2, TextFieldSettings info, TextFieldSettings responsibility, TextFieldSettings activity, TextFieldSettings manager, TextFieldSettings referenceNumber)
        {
            UserId = userId;
            UserFirstName = userFirstName;
            UserLastName = userLastName;
            UserPhone = userPhone;
            UserEMail = userEMail;
            Initials = initials;
            PersonalIdentityNumber = personalIdentityNumber;
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

        [NotNull]
        public TextFieldSettings UserId { get; private set; }
         
        [NotNull]
        public TextFieldSettings UserFirstName { get; private set; }
         
        [NotNull]
        public TextFieldSettings UserLastName { get; private set; }
        [NotNull]
        public TextFieldSettings UserPhone { get; private set; }

        [NotNull]
        public TextFieldSettings UserEMail { get; private set; }

        public TextFieldSettings Initials { get; private set; }

        public TextFieldSettings PersonalIdentityNumber { get; private set; }

        public TextFieldSettings Extension { get; private set; }

        public TextFieldSettings Title { get; private set; }

        public TextFieldSettings Location { get; private set; }

        public TextFieldSettings RoomNumber { get; private set; }

        public TextFieldSettings PostalAddress { get; private set; }

        public TextFieldSettings EmploymentType { get; private set; }

        public TextFieldSettings DepartmentId1 { get; private set; }

        public TextFieldSettings UnitId { get; private set; }

        public TextFieldSettings DepartmentId2 { get; private set; }

        public TextFieldSettings Info { get; private set; }

        public TextFieldSettings Responsibility { get; private set; }

        public TextFieldSettings Activity { get; private set; }

        public TextFieldSettings Manager { get; private set; }

        public TextFieldSettings ReferenceNumber { get; private set; }
    }
}