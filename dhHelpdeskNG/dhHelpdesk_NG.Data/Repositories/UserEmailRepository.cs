namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;

    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public sealed class UserEmailRepository : Repository, IUserEmailRepository
    {
        public UserEmailRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<MailAddress> FindUserEmails(int userId)
        {
            var emails = this.DbContext.Users.Where(u => u.Id == userId).Select(u => u.Email).FirstOrDefault();
            
            if (string.IsNullOrEmpty(emails))
            {
                return new List<MailAddress>();
            }

            return emails.Split(";").Select(e => new MailAddress(e)).ToList();
        }
    }
}