namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.ChangeDetailedOverview
{
    public sealed class OrdererFieldGroupDto
    {
        public OrdererFieldGroupDto(
            string id, string name, string phone, string cellPhone, string email, string department)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Department = department;
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Phone { get; private set; }

        public string CellPhone { get; private set; }

        public string Email { get; private set; }

        public string Department { get; private set; }
    }
}
