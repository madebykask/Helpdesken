namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Mobile.Models.Changes.ChangeEdit;

    public sealed class NewOrdererModelFactory : INewOrdererModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public NewOrdererModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public OrdererModel Create(OrdererEditSettings settings, ChangeEditOptions options)
        {
            var id = this.configurableFieldModelFactory.CreateStringField(settings.Id, null);
            var name = this.configurableFieldModelFactory.CreateStringField(settings.Name, null);
            var phone = this.configurableFieldModelFactory.CreateStringField(settings.Phone, null);
            var cellPhone = this.configurableFieldModelFactory.CreateStringField(settings.CellPhone, null);
            var email = this.configurableFieldModelFactory.CreateStringField(settings.Email, null);

            var departments = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Department,
                options.Departments,
                null,
                true);

            return new OrdererModel(id, name, phone, cellPhone, email, departments);
        }
    }
}