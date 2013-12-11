namespace dhHelpdesk_NG.Service.Validators.Notifier.Settings
{
    public sealed class ValidatableNotifier
    {
        public string UserId { get; set; }

        public int? DomainId { get; set; }

        public string LoginName { get; set; }

        public string FirstName { get; set; }

        public string Initials { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string Location { get; set; }

        public string Phone { get; set; }

        public string CellPhone { get; set; }

        public string Email { get; set; }

        public string Number { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Title { get; set; }

        public int? RegionId { get; set; }

        public int? DepartmentId { get; set; }

        public string Unit { get; set; }

        public int? Division { get; set; }

        public bool ExtendedInfo { get; set; }

        public int? GroupId { get; set; }

        public string Password { get; set; }

        public string Other { get; set; }

        public bool Orderer { get; set; }

        public bool IsActive { get; set; }
    }
}
