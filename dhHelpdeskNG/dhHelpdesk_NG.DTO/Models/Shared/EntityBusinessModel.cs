namespace DH.Helpdesk.BusinessData.Models.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class EntityBusinessModel
    {
        [IsId]
        public int Id { get; set; }

        public bool IsNew()
        {
            return this.Id == 0;
        }
    }
}