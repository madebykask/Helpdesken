namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    using System;

    public sealed class DisplayNotifierDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int? DomainId { get; set; }

        public string LoginName { get; set; }

        public string FirstName { get; set; }

        public string Initials { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string Place { get; set; }

        public string Phone { get; set; }

        public string CellPhone { get; set; }

        public string Email { get; set; }

        public string Code { get; set; }

        public string PostalAddress { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Title { get; set; }

        public int? DepartmentId { get; set; }

        public string Unit { get; set; }

        public int? OrganizationUnitId { get; set; }

        public int? DivisionId { get; set; }

        public int? ManagerId { get; set; }

        public int? GroupId { get; set; }

        public string Password { get; set; }

        public string Other { get; set; }

        public bool Ordered { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime? SynchronizationDate { get; set; }
    }
}
