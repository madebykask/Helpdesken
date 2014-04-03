namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.Shared;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewOrdererModelFactory : INewOrdererModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public NewOrdererModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public OrdererModel Create(ChangeEditData editData, OrdererEditSettings settings)
        {
            var id = this.configurableFieldModelFactory.CreateStringField(settings.Id, null);
            var name = this.configurableFieldModelFactory.CreateStringField(settings.Name, null);
            var phone = this.configurableFieldModelFactory.CreateStringField(settings.Phone, null);
            var cellPhone = this.configurableFieldModelFactory.CreateStringField(settings.CellPhone, null);
            var email = this.configurableFieldModelFactory.CreateStringField(settings.Email, null);

            var department = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Department, editData.Departments, (int?)null);

            return new OrdererModel(id, name, phone, cellPhone, email, department);
        }
    }
}