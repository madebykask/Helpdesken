namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields
{
    public sealed class UserHistoryFields
    {
        public UserHistoryFields(string userId, 
            string userFirstName, 
            string userLastName, 
            string userPhone, 
            string userEMail, 
            string userInitials, 
            string userPersonalIdentityNumber, 
            string userExtension, 
            string userTitle,
            string userLocation, 
            string userRoomNumber, 
            string userPostalAddress, 
            string responsibility, 
            string activity, 
            string manager, 
            string referenceNumber, 
            string infoUser, 
            int? userOuId, 
            string userOuName, 
            int? employmentTypeId, 
            string employmentTypeName, 
            int? userDepartmentId1, 
            string userDepartmentName, 
            int? userDepartmentId2, 
            string userDepartmentName2)
        {
            UserId = userId;
            UserFirstName = userFirstName;
            UserLastName = userLastName;
            UserPhone = userPhone;
            UserEMail = userEMail;
            UserInitials = userInitials;
            UserPersonalIdentityNumber = userPersonalIdentityNumber;
            UserExtension = userExtension;
            UserTitle = userTitle;
            UserLocation = userLocation;
            UserRoomNumber = userRoomNumber;
            UserPostalAddress = userPostalAddress;
            Responsibility = responsibility;
            Activity = activity;
            Manager = manager;
            ReferenceNumber = referenceNumber;
            InfoUser = infoUser;
            UserOU_Id = userOuId;
            UserOUName = userOuName;
            EmploymentType_Id = employmentTypeId;
            EmploymentTypeName = employmentTypeName;
            UserDepartment_Id1 = userDepartmentId1;
            UserDepartmentName = userDepartmentName;
            UserDepartment_Id2 = userDepartmentId2;
            UserDepartmentName2 = userDepartmentName2;
        }

        public string UserId { get; private set; }
        
        public string UserFirstName { get; private set; }
        
        public string UserLastName { get; private set; }

        public string UserPhone { get; private set; }

        public string UserEMail { get; private set; }

        public string UserInitials { get; private set; }

        public string UserPersonalIdentityNumber { get; private set; }

        public string UserExtension { get; private set; }

        public string UserTitle { get; private set; }

        public string UserLocation { get; private set; }

        public string UserRoomNumber { get; private set; }

        public string UserPostalAddress { get; private set; }

        public string Responsibility { get; private set; }

        public string Activity { get; private set; }

        public string Manager { get; private set; }

        public string ReferenceNumber { get; private set; }

        public string InfoUser { get; private set; }

        public int? UserOU_Id { get; private set; }

        public string UserOUName { get; private set; }

        public int? EmploymentType_Id { get; private set; }

        public string EmploymentTypeName { get; private set; }

        public int? UserDepartment_Id1 { get; private set; }

        public string UserDepartmentName { get; private set; }

        public int? UserDepartment_Id2 { get; private set; }

        public string UserDepartmentName2 { get; private set; }

    }
}