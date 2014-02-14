namespace DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewOrdererFields
    {
        public NewOrdererFields(string id, string name, string phone, string cellPhone, string email, int? departmentId)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.DepartmentId = departmentId;
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Phone { get; private set; }

        public string CellPhone { get; private set; }

        public string Email { get; private set; }

        [IsId]
        public int? DepartmentId { get; private set; }
    }
}
