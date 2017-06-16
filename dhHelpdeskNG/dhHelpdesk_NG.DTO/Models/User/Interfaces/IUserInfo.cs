namespace DH.Helpdesk.BusinessData.Models.User.Interfaces
{
    public interface IUserInfo
    {
        int Id { get; }
        string FirstName { get; }
        string SurName { get; }
    }
}