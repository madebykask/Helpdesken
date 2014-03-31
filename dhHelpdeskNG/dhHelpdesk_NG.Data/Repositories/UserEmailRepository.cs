namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public sealed class UserEmailRepository : Repository, IUserEmailRepository
    {
        public UserEmailRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<string> FindUserEmails(int userId)
        {
            var emails = this.DbContext.Users.Where(u => u.Id == userId).Select(u => u.Email).Single();
            return emails.Split(";").ToList();
        }
    }
}