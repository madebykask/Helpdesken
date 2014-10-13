namespace DH.Helpdesk.Mobile.Infrastructure.Session
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ActiveTab
    {
        public ActiveTab(string topic, string tab)
        {
            this.Topic = topic;
            this.Tab = tab;
        }

        [NotNullAndEmpty]
        public string Topic { get; private set; }

        [NotNullAndEmpty]
        public string Tab { get; private set; }
    }
}