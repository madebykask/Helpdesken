namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.Shared;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewGeneralModelFactory : INewGeneralModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public NewGeneralModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public GeneralViewModel Create(ChangeEditData editData, GeneralEditSettings settings)
        {
            var priority = this.configurableFieldModelFactory.CreateIntegerField(settings.Priority, 0);
            var title = this.configurableFieldModelFactory.CreateStringField(settings.Title, null);

            var statuses = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Status,
                editData.Statuses,
                (int?)null);

            var systems = this.configurableFieldModelFactory.CreateSelectListField(
                settings.System,
                editData.Systems,
                (int?)null);

            var objects = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Object,
                editData.Objects,
                (int?)null);

            var workingGroups = this.configurableFieldModelFactory.CreateSelectListField(
                settings.WorkingGroup,
                editData.WorkingGroups,
                (int?)null);

            var administrators = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Administrator,
                editData.Administrators,
                (int?)null);

            var finishingDate = this.configurableFieldModelFactory.CreateDateTimeField(settings.FinishingDate, null);

            var rss = this.configurableFieldModelFactory.CreateBooleanField(settings.Rss, false);

            var generalModel = new GeneralModel(priority, title, finishingDate, null, null, rss);
            return new GeneralViewModel(statuses, systems, objects, workingGroups, administrators, generalModel);
        }
    }
}