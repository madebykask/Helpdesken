namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.Shared;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class OrdererModelFactory : IOrdererModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public OrdererModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public OrdererViewModel Create(
            FindChangeResponse response,
            ChangeEditData editData,
            OrdererEditSettings settings)
        {
            var id = this.configurableFieldModelFactory.CreateStringField(settings.Id, response.Change.Orderer.Id);
            var name = this.configurableFieldModelFactory.CreateStringField(settings.Name, response.Change.Orderer.Name);

            var phone = this.configurableFieldModelFactory.CreateStringField(
                settings.Phone,
                response.Change.Orderer.Phone);

            var cellPhone = this.configurableFieldModelFactory.CreateStringField(
                settings.CellPhone,
                response.Change.Orderer.CellPhone);

            var email = this.configurableFieldModelFactory.CreateStringField(
                settings.Email,
                response.Change.Orderer.Email);

            var departments = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Department,
                editData.Departments,
                response.Change.Orderer.DepartmentId);

            var ordererModel = new OrdererModel(id, name, phone, cellPhone, email);
            return new OrdererViewModel(departments, ordererModel);
        }
    }
}