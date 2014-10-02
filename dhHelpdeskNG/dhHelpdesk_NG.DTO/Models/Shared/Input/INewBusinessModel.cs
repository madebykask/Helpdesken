namespace DH.Helpdesk.BusinessData.Models.Shared.Input
{
    public interface INewBusinessModel
    {
        int Id { get; set; }
    }

    public abstract class BusinessModel
    {
        public int Id { get; protected set; }
    }
}
