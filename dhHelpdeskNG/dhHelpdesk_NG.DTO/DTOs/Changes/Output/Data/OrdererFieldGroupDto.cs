namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data
{
    public sealed class OrdererFieldGroupDto
    {
        public OrdererFieldGroupDto(
            string id, string name, string phone, string cellPhone, string email, string department)
        {
            Id = id;
            Name = name;
            Phone = phone;
            CellPhone = cellPhone;
            Email = email;
            Department = department;
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Phone { get; private set; }

        public string CellPhone { get; private set; }

        public string Email { get; private set; }

        public string Department { get; private set; }
    }
}
