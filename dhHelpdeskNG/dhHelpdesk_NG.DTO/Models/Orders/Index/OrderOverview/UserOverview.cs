﻿namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    public sealed class UserOverview
    {
        public UserOverview(string userId, string userFirstName,
            string userLastName, string userPhone,
            string userEMail, string userInitials,
            string userPersonalIdentityNumber, string userExtension,
            string userTitle, string userLocation, string userRoomNumber,
            string userPostalAddress, string responsibility,
            string activity, string manager,
            string referenceNumber, string infoUser,
            string userOuId, string employmentType,
            string userDepartmentId1, string userDepartmentId2)
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
            EmploymentType = employmentType;
            UserDepartment_Id1 = userDepartmentId1;
            UserDepartment_Id2 = userDepartmentId2;
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

        public string UserOU_Id { get; private set; }

        public string EmploymentType { get; private set; }

        public string UserDepartment_Id1 { get; private set; }

        public string UserDepartment_Id2 { get; private set; }
    }
}