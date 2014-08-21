namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class OrdererModelFactory : IOrdererModelFactory
    {
        #region Fields

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        #endregion

        #region Constructors and Destructors

        public OrdererModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public OrdererModel Create(FindChangeResponse response)
        {
            var settings = response.EditSettings.Orderer;
            var fields = response.EditData.Change.Orderer;
            var options = response.EditOptions;

            var id = this.configurableFieldModelFactory.CreateStringField(settings.Id, fields.Id);
            var name = this.configurableFieldModelFactory.CreateStringField(settings.Name, fields.Name);
            var phone = this.configurableFieldModelFactory.CreateStringField(settings.Phone, fields.Phone);
            var cellPhone = this.configurableFieldModelFactory.CreateStringField(settings.CellPhone, fields.CellPhone);
            var email = this.configurableFieldModelFactory.CreateStringField(settings.Email, fields.Email);

            var departments = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Department,
                options.Departments,
                fields.DepartmentId.ToString(),
                true);

            return new OrdererModel(id, name, phone, cellPhone, email, departments);
        }

        #endregion
    }
}