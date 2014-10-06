namespace DH.Helpdesk.Mobile.Models.Changes.ChangeEdit
{
    using System.Web.Mvc;

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
            ConfigurableFieldModel<string> email,
            ConfigurableFieldModel<SelectList> departments)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Departments = departments;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public ConfigurableFieldModel<string> CellPhone { get; set; }

        [IsId]
        public int? DepartmentId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Departments { get; set; }

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