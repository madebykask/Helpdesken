namespace DH.Helpdesk.SelfService.Models.Notifiers
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

        public bool IsEmpty { get; private set; }

        public void MarkAsEmpty()
        {
            this.IsEmpty = true;
        }
    }
}