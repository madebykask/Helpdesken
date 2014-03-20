namespace DH.Helpdesk.Web.Models.Notifiers
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class IndexModel
    {
        public IndexModel(NotifiersModel notifiers)
        {
            this.Notifiers = notifiers;
        }

        [NotNull]
        public NotifiersModel Notifiers { get; private set; }
    }
}