namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class NotifiersModel
    {
        public NotifiersModel(SearchModel searchModel, NotifiersGridModel gridModel)
        {
            this.SearchModel = searchModel;
            this.GridModel = gridModel;
        }

        [NotNull]
        public SearchModel SearchModel { get; private set; }

        [NotNull]
        public NotifiersGridModel GridModel { get; private set; }
    }
}