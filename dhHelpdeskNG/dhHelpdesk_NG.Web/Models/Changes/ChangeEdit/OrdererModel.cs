namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdererModel
    {
        #region Constructors and Destructors

        public OrdererModel()
        {
        }

        public OrdererModel(
            ConfigurableFieldModel<string> id,
            ConfigurableFieldModel<string> name,
            ConfigurableFieldModel<string> phone,
            ConfigurableFieldModel<string> cellPhone,
            ConfigurableFieldModel<string> email)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public ConfigurableFieldModel<string> CellPhone { get; set; }

        [IsId]
        public int? DepartmentId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Email { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Id { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Name { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Phone { get; set; }

        #endregion
    }
}