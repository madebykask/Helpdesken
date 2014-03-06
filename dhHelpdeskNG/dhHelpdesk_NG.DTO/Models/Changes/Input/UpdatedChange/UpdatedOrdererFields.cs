namespace DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedOrdererFields
    {
        public UpdatedOrdererFields(
            string id,
            string name,
            string phone,
            string cellPhone,
            string email,
            int? departmentId)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.DepartmentId = departmentId;
        }

        public string Id { get; internal set; }

        public string Name { get; internal set; }

        public string Phone { get; internal set; }

        public string CellPhone { get; internal set; }

        public string Email { get; internal set; }

        [IsId]
        public int? DepartmentId { get; internal set; }

        public static UpdatedOrdererFields CreateDefault()
        {
            return new UpdatedOrdererFields(null, null, null, null, null, null);
        }
    }
}
