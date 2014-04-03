namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Responses.Changes;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.Shared;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class GeneralModelFactory : IGeneralModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public GeneralModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public GeneralModel Create(FindChangeResponse response, ChangeEditData editData, GeneralEditSettings settings)
        {
            var priority = this.configurableFieldModelFactory.CreateIntegerField(
                settings.Priority, response.Change.General.Priority);

            var title = this.configurableFieldModelFactory.CreateStringField(
                settings.Title, response.Change.General.Title);

            var status = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Status, editData.Statuses, response.Change.General.StatusId);

            var system = this.configurableFieldModelFactory.CreateSelectListField(
                settings.System, editData.Systems, response.Change.General.SystemId);

            var @object = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Object, editData.Systems, response.Change.General.ObjectId);

            var workingGroup = this.configurableFieldModelFactory.CreateSelectListField(
                settings.WorkingGroup, editData.WorkingGroups, response.Change.General.WorkingGroupId);

            var administrator = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Administrator, editData.Administrators, response.Change.General.AdministratorId);

            var finishingDate = this.configurableFieldModelFactory.CreateDateTimeField(
                settings.FinishingDate, response.Change.General.FinishingDate);

            var rss = this.configurableFieldModelFactory.CreateBooleanField(settings.Rss, response.Change.General.Rss);

            return new GeneralModel(
                priority,
                title,
                status,
                system,
                @object,
                workingGroup,
                administrator,
                finishingDate,
                response.Change.General.CreatedDate,
                response.Change.General.ChangedDate,
                rss);
        }
    }
}