namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;

    using DH.Helpdesk.Dal.Dal;

    public interface IUserEmailRepository : INewRepository
    {
        List<string> FindUserEmails(int userId);
    }
}
