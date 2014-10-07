namespace DH.Helpdesk.BusinessData.Models.Shared
{
    public abstract class EntityBusinessModel
    {
        public int Id { get; set; }

        public bool IsNew()
        {
            return this.Id == 0;
        }
    }
}