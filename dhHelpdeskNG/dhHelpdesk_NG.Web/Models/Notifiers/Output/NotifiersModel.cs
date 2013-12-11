namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class NotifiersModel
    {
        public NotifiersModel(SearchModel searchModel, NotifiersGridModel gridModel)
        {
            ArgumentsValidator.NotNull(searchModel, "searchModel");
            ArgumentsValidator.NotNull(gridModel, "gridModel");

            this.SearchModel = searchModel;
            this.GridModel = gridModel;
        }

        public SearchModel SearchModel { get; private set; }

        public NotifiersGridModel GridModel { get; private set; }
    }
}