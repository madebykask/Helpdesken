namespace DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview
{
    using System.Collections.Generic;

    public sealed class User
    {
        public User(
            List<string> ids,
            string firstName,
            string initials,
            string lastName,
            string personalIdentityNumber,
            string phone,
            string extension,
            string eMail,
            string title,
            string location,
            string roomNumber,
            string postalAddress,
            string employmentType,
            string departmentId,
            string unitId,
            string departmentId2,
            string info,
            string responsibility,
            string activity,
            string manager,
            string referenceNumber)
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

        public List<string> Ids { get; private set; }

        public string FirstName { get; private set; }

        public string Initials { get; private set; }

        public string LastName { get; private set; }

        public string PersonalIdentityNumber { get; private set; }

        public string Phone { get; private set; }

        public string Extension { get; private set; }

        public string EMail { get; private set; }

        public string Title { get; private set; }

        public string Location { get; private set; }

        public string RoomNumber { get; private set; }

        public string PostalAddress { get; private set; }

        public string EmploymentType { get; private set; }

        public string DepartmentId { get; private set; }

        public string UnitId { get; private set; }

        public string DepartmentId2 { get; private set; }

        public string Info { get; private set; }

        public string Responsibility { get; private set; }

        public string Activity { get; private set; }

        public string Manager { get; private set; }

        public string ReferenceNumber { get; private set; }
    }
}