namespace DH.Helpdesk.Services.Services.Users
{
    public interface IUsersPasswordHistoryService
    {
        int SaveHistory(int userId, string password);
    }
}