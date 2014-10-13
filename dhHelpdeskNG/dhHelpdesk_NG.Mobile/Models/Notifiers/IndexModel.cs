namespace DH.Helpdesk.Mobile.Models.Notifiers
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class IndexModel
    {
        public IndexModel(
            NotifiersModel notifiers,
            bool isEmpty)
        {
            this.Notifiers = notifiers;
            this.IsEmpty = isEmpty;
        }

        [NotNull]
        public NotifiersModel Notifiers { get; private set; }

        public bool IsEmpty { get; private set; }
    }
}