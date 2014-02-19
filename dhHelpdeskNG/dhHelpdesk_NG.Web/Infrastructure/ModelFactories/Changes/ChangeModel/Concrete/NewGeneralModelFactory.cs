namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewGeneralModelFactory : INewGeneralModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public NewGeneralModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public GeneralModel Create(ChangeEditData editData, GeneralEditSettings settings)
        {
            var priority = this.configurableFieldModelFactory.CreateIntegerField(settings.Priority, 0);
            var title = this.configurableFieldModelFactory.CreateStringField(settings.Title, null);

            var status = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Status, editData.Statuses, (int?)null);

            var system = this.configurableFieldModelFactory.CreateSelectListField(
                settings.System, editData.Systems, (int?)null);

            var @object = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Object, editData.Objects, (int?)null);

            var workingGroup = this.configurableFieldModelFactory.CreateSelectListField(
                settings.WorkingGroup, editData.WorkingGroups, (int?)null);

            var administrator = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Administrator, editData.Administrators, (int?)null);

            var finishingDate = this.configurableFieldModelFactory.CreateDateTimeField(
                settings.FinishingDate, null);

            var rss = this.configurableFieldModelFactory.CreateBooleanField(settings.Rss, false);

            return new GeneralModel(
                priority, title, status, system, @object, workingGroup, administrator, finishingDate, null, null, rss);
        }
    }
}