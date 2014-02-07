namespace DH.Helpdesk.Web.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

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