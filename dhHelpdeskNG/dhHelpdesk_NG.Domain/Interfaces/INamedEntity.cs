namespace DH.Helpdesk.Domain.Interfaces
{
    public interface INamedEntity
    {
        int Id { get; } 

        string Name { get; }
    }
}