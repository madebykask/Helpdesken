namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange.Concrete
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.InventoryDialog;

    using iTextSharp.text;
    using iTextSharp.text.pdf.crypto;

    public sealed class NewGeneralModelFactory : INewGeneralModelFactory
    {
        #region Fields

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        #endregion

        #region Constructors and Destructors

        public NewGeneralModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public GeneralModel Create(GeneralEditSettings settings, ChangeEditOptions options)
        {
            var prioritisation = this.configurableFieldModelFactory.CreateIntegerField(settings.Priority, 0);
            var title = this.configurableFieldModelFactory.CreateStringField(settings.Title, null);

            var statuses = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Status,
                options.Statuses,
                null);

            var systems = this.configurableFieldModelFactory.CreateSelectListField(
                settings.System,
                options.Systems,
                null);

            var objects = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Object,
                options.Objects,
                null);

            //


            var inventoryDialog = new ConfigurableFieldModel<InventoryDialogModel>(
                settings.Inventory.Caption,
                new InventoryDialogModel(new List<InventoryTypeModel>
                                         {
                                             new InventoryTypeModel("Servers", new MultiSelectList(new List<object>{ new { Text = "gfgf", Value = 2 } }, "Value", "Text"))
                                         }),
                settings.Inventory.Required);

            //

            
            var workingGroups = this.configurableFieldModelFactory.CreateSelectListField(
                settings.WorkingGroup,
                options.WorkingGroups,
                null);

            var administrators = this.configurableFieldModelFactory.CreateSelectListField(
                settings.Administrator,
                options.Administrators,
                null);

            var finishingDate = this.configurableFieldModelFactory.CreateNullableDateTimeField(
                settings.FinishingDate,
                null);

            var rss = this.configurableFieldModelFactory.CreateBooleanField(settings.Rss, false);

            return new GeneralModel(
                true,
                prioritisation,
                title,
                statuses,
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