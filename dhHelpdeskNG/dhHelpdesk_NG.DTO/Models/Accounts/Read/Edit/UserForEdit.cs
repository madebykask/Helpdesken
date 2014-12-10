namespace DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit
{
    using System.Collections.Generic;

    public class UserForEdit : User
    {
        public UserForEdit(
            List<string> ids,
            string firstName,
            string initials,
            string lastName,
            List<string> personalIdentityNumber,
            string phone,
            string extension,
            string eMail,
            string title,
            string location,
            string roomNumber,
            string postalAddress,
            int employmentType,
            int? departmentId,
            int? unitId,
            int? departmentId2,
            string info,
            string responsibility,
            string activity,
            string manager,
            string referenceNumber,
            int? regionId)
            : base(
                ids,
                firstName,
                initials,
                lastName,
                personalIdentityNumber,
                phone,
                extension,
                eMail,
                title,
                location,
                roomNumber,
                postalAddress,
                employmentType,
                departmentId,
                unitId,
                departmentId2,
                info,
                responsibility,
                activity,
                manager,
                referenceNumber)
        {
            this.RegionId = regionId;
        }

        public int? RegionId { get; set; }
    }
}