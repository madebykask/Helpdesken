namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public class AnalyzeFieldsFactory
    {
        #region Fields

        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        #endregion

        #region Constructors and Destructors

        public AnalyzeFieldsFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        #endregion

        #region Public Methods and Operators

        public ConfigurableFieldModel<DateTime?> CreateEndDate(AnalyzeFieldEditSettings editSettings, Change change)
        {
            return this.configurableFieldModelFactory.CreateNullableDateTimeField(
                editSettings.FinishDate,
                change.Analyze.FinishDate);
        }

        public ConfigurableFieldModel<DateTime?> CreateEndDate(AnalyzeFieldEditSettings editSettings)
        {
            return this.configurableFieldModelFactory.CreateNullableDateTimeField(editSettings.FinishDate, null);
        }

        #endregion
    }
}