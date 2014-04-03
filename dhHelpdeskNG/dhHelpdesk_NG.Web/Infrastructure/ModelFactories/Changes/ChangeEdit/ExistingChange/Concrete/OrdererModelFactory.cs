namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Responses.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.Shared;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class OrdererModelFactory : IOrdererModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public OrdererModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public OrdererModel Create(FindChangeResponse response, ChangeEditData editData, OrdererEditSettings settings)
        {
            var id = this.configurableFieldModelFactory.CreateStringField(settings.Id, response.Change.Orderer.Id);
            var name = this.configurableFieldModelFactory.CreateStringField(settings.Name, response.Change.Orderer.Name);

            var phone = this.configurableFieldModelFactory.CreateStringField(
                settings.Phone, response.Change.Orderer.Phone);

            var cellPhone = this.configurableFieldModelFactory.CreateStringField(
                settings.CellPhone, response.Change.Orderer.CellPhone);

            var email = this.configurableFieldModelFactory.CreateStringField(
                settings.Email, response.Change.Orderer.Email);

            var department = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Department, editData.Departments, response.Change.Orderer.DepartmentId);

            return new OrdererModel(id, name, phone, cellPhone, email, department);
        }
    }
}