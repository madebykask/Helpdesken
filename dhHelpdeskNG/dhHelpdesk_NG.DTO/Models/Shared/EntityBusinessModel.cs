namespace DH.Helpdesk.BusinessData.Models.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class EntityBusinessModel
    {
        [MinValue(0)]
        public int Id { get; set; }

        public bool IsNew()
        {
            return this.Id == 0;
        }
    }
}