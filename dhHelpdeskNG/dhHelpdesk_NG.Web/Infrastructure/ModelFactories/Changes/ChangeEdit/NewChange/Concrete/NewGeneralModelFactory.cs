namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class NewGeneralModelFactory : INewGeneralModelFactory
    {
        #region Fields

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        #endregion

        #region Constructors and Destructors

        public NewGeneralModelFactory(
                    IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public GeneralModel Create(
                        GeneralEditSettings settings, 
                        ChangeEditOptions options,
                        OperationContext context,
                        IList<ChangeStatusEntity> statuses)
        {
            var prioritisation = this.configurableFieldModelFactory.CreateIntegerField(settings.Priority, 0);
            var title = this.configurableFieldModelFactory.CreateStringField(settings.Title, null);

            var defaultStatus = statuses.FirstOrDefault(s => s.isDefault.ToBool());

            var changeStatuses = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Status,
                options.Statuses,
                defaultStatus != null ? defaultStatus.Id.ToString(CultureInfo.InvariantCulture) : null,
                true);

            var systems = this.configurableFieldModelFactory.CreateSelectListField(
                settings.System,
                options.Systems,
                null);

            var objects = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Object,
                options.Objects,
                null,
                true);

            var inventoryDialog = this.configurableFieldModelFactory.CreateInventoryDialog(
                settings.Inventory,
                options.InventoryTypesWithInventories);
            
            var workingGroups = this.configurableFieldModelFactory.CreateSelectListField(
                settings.WorkingGroup,
                options.WorkingGroups,
                null);

            var administrators = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Administrator,
                options.Administrators,
                null,
                true);

            var finishingDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(
                settings.FinishingDate,
                null);

            var rss = this.configurableFieldModelFactory.CreateBooleanField(settings.Rss, false);

            return new GeneralModel(
                true,
                prioritisation,
                title,
                changeStatuses,
                systems,
                objects,
                inventoryDialog,
                workingGroups,
                administrators,
                finishingDate,
                null,
                null,
                null,
                rss);
        }

        #endregion
    }
}