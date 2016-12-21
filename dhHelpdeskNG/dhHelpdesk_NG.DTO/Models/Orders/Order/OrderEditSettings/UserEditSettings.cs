namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UserEditSettings
    {
        public UserEditSettings(TextFieldEditSettings userId, TextFieldEditSettings userFirstName, TextFieldEditSettings userLastName, TextFieldEditSettings userPhone, TextFieldEditSettings userEMail, TextFieldEditSettings initials, TextFieldEditSettings personalIdentityNumber, TextFieldEditSettings extension, TextFieldEditSettings title, TextFieldEditSettings location, TextFieldEditSettings roomNumber, TextFieldEditSettings postalAddress, TextFieldEditSettings employmentType, TextFieldEditSettings departmentId1, TextFieldEditSettings unitId, TextFieldEditSettings departmentId2, TextFieldEditSettings info, TextFieldEditSettings responsibility, TextFieldEditSettings activity, TextFieldEditSettings manager, TextFieldEditSettings referenceNumber)
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
        public TextFieldEditSettings UserId { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings UserFirstName { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings UserLastName { get; private set; }

        [NotNull]
        public TextFieldEditSettings UserPhone { get; private set; }

        [NotNull]
        public TextFieldEditSettings UserEMail { get; private set; }

        public TextFieldEditSettings Initials { get; private set; }

        public TextFieldEditSettings PersonalIdentityNumber { get; private set; }

        public TextFieldEditSettings Extension { get; private set; }

        public TextFieldEditSettings Title { get; private set; }

        public TextFieldEditSettings Location { get; private set; }

        public TextFieldEditSettings RoomNumber { get; private set; }

        public TextFieldEditSettings PostalAddress { get; private set; }

        public TextFieldEditSettings EmploymentType { get; private set; }

        public TextFieldEditSettings DepartmentId1 { get; private set; }

        public TextFieldEditSettings UnitId { get; private set; }

        public TextFieldEditSettings DepartmentId2 { get; private set; }

        public TextFieldEditSettings Info { get; private set; }

        public TextFieldEditSettings Responsibility { get; private set; }

        public TextFieldEditSettings Activity { get; private set; }

        public TextFieldEditSettings Manager { get; private set; }

        public TextFieldEditSettings ReferenceNumber { get; private set; }

    }
}