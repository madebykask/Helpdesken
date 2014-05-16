namespace DH.Helpdesk.BusinessData.Models.Shared.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class IdAndNameOverview
    {
        public IdAndNameOverview(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        [IsId]
        public int Id { get; private set; }

        public string Name { get; private set; }
    }
}
