namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdererModel
    {
        public OrdererModel()
        {
        }

        public OrdererModel(
            ConfigurableFieldModel<string> id,
            ConfigurableFieldModel<string> name,
            ConfigurableFieldModel<string> phone,
            ConfigurableFieldModel<string> cellPhone,
            ConfigurableFieldModel<string> email,
            ConfigurableFieldModel<SelectList> department)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Department = department;
        }

        [NotNull]
        public ConfigurableFieldModel<string> Id { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Name { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Phone { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> CellPhone { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Email { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Department { get; private set; }

        [IsId]
        public int? DepartmentId { get; set; }
    }
}