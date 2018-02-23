namespace DH.Helpdesk.Domain.Interfaces
{
    public interface IUserCommon
    {
        int Id { get; }
        string Email { get; }
        string FirstName { get; }
        string SurName { get; }
        int IsActive { get; }
    }
}