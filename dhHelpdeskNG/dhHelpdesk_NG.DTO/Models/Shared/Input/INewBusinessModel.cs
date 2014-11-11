namespace DH.Helpdesk.BusinessData.Models.Shared.Input
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public interface INewBusinessModel
    {
        int Id { get; set; }
    }

    public abstract class BusinessModel
    {
        [MinValue(0)]
        public int Id { get; protected set; }

        public bool IsNew()
        {
            return this.Id == 0;
        }
    }
}
