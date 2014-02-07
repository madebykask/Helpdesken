namespace DH.Helpdesk.Web.Models.Notifiers.Input
{
    using System.ComponentModel.DataAnnotations;

    using DataAnnotationsExtensions;

    public sealed class NewNotifierInputModel
    {
        [StringLength(50)]
        public string UserId { get; set; }

        public int? DomainId { get; set; }

        [StringLength(50)]
        public string LoginName { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(10)]
        public string Initials { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string DisplayName { get; set; }

        [StringLength(100)]
        public string Place { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string CellPhone { get; set; }

        [Email]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(13)]
        public string Code { get; set; }

        [StringLength(50)]
        public string PostalAddress { get; set; }

        [StringLength(13)]
        public string PostalCode { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        public int? DepartmentId { get; set; }

        [StringLength(100)]
        public string Unit { get; set; }

        public int? DivisionId { get; set; }

        public int? GroupId { get; set; }

        [StringLength(20)]
        public string Password { get; set; }

        [StringLength(500)]
        public string Other { get; set; }

        public bool Ordered { get; set; }

        public bool IsActive { get; set; }

        public int? OrganizationUnitId { get; set; }

        public int? ManagerId { get; set; }
    }
}