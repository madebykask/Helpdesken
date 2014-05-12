namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using DH.Helpdesk.Services.DisplayValues;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class GeneralModelFactory : IGeneralModelFactory
    {
        #region Fields

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        #endregion

        #region Constructors and Destructors

        public GeneralModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public GeneralModel Create(FindChangeResponse response)
        {
            var settings = response.EditSettings.General;
            var fields = response.EditData.Change.General;
            var options = response.EditOptions;

            var prioritisation = this.configurableFieldModelFactory.CreateIntegerField(
                settings.Priority,
                fields.Priority);

            var title = this.configurableFieldModelFactory.CreateStringField(settings.Title, fields.Title);

            var statuses = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Status,
                options.Statuses,
                fields.StatusId.ToString());

            var systems = this.configurableFieldModelFactory.CreateSelectListField(
                settings.System,
                options.Systems,
                fields.SystemId.ToString());

            var objects = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Object,
                options.Objects,
                fields.ObjectId.ToString());

            var inventoryDialog = this.configurableFieldModelFactory.CreateInventoryDialog(
                settings.Inventory,
                options.InventoryTypesWithInventories,
                fields.Inventories);

            var workingGroups = this.configurableFieldModelFactory.CreateSelectListField(
                settings.WorkingGroup,
                options.WorkingGroups,
                fields.WorkingGroupId.ToString());

            var administrators = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Administrator,
                options.Administrators,
                fields.AdministratorId.ToString());

            var finishingDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(
                settings.FinishingDate,
                fields.FinishingDate);

            var rss = this.configurableFieldModelFactory.CreateBooleanField(settings.Rss, fields.Rss);

            return new GeneralModel(
                false,
                prioritisation,
                title,
                statuses,
                systems,
                objects,
                inventoryDialog,
                workingGroups,
                administrators,
                finishingDate,
                fields.CreatedDate,
                fields.ChangedDate,
                fields.ChangedByUser,
                rss);
        }

        #endregion
    }
}