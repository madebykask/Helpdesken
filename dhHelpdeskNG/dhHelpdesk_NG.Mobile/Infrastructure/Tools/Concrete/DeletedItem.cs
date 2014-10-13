namespace DH.Helpdesk.Mobile.Infrastructure.Tools.Concrete
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class DeletedItem
    {
        public DeletedItem(int id, string key)
        {
            this.Id = id;
            this.Key = key;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNullAndEmpty]
        public string Key { get; private set; }
    }
}