namespace DH.Helpdesk.Domain.Computers
{
    using global::System;
    using global::System.Collections.Generic;

    public class ComputerUser : Entity
    {
        #region Public Properties

        public virtual ICollection<ComputerUserGroup> CUGs { get; set; }

        public string Cellphone { get; set; }

        public DateTime ChangeTime { get; set; }

        public string City { get; set; }

        public virtual ComputerUserGroup ComputerUserGroup { get; set; }

        public int? ComputerUserGroup_Id { get; set; }

        public int ComputerUserRole { get; set; }

        public virtual Customer Customer { get; set; }

        public int? Customer_Id { get; set; }

        public virtual Department Department { get; set; }

        public int? Department_Id { get; set; }

        public string DisplayName { get; set; }

        public virtual Division Division { get; set; }

        public int? Division_Id { get; set; }

        public virtual Domain Domain { get; set; }

        public int? Domain_Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string FullName { get; set; }

        public string Info { get; set; }

        public string Initials { get; set; }

        public string Location { get; set; }

        public string LogonName { get; set; }

        public virtual ComputerUser ManagerComputerUser { get; set; }

        public virtual Language Language { get; set; }

        public int? ManagerComputerUser_Id { get; set; }

        public string NDSpath { get; set; }

        public virtual OU OU { get; set; }

        public int? OU_Id { get; set; }

        public int OrderPermission { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string Phone2 { get; set; }

        public string PostalAddress { get; set; }

        public string Postalcode { get; set; }

        public DateTime RegTime { get; set; }

        public string SOU { get; set; }

        public int Status { get; set; }

        public string SurName { get; set; }

        public DateTime? SyncChangedDate { get; set; }

        public string Title { get; set; }

        public int Updated { get; set; }

        public string UserCode { get; set; }

        public string UserGUID { get; set; }

        public string UserId { get; set; }

        public string homeDirectory { get; set; }

        public string homeDrive { get; set; }

        public string CostCentre { get; set; }


        public int LanguageId { get; set; }

        #endregion
    }
}