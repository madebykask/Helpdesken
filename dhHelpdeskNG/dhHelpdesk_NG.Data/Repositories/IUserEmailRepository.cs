using System.Collections.Generic;
using System.Net.Mail;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseIntLog;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Dal.Dal;

namespace DH.Helpdesk.Dal.Repositories
{
    public interface IUserEmailRepository : INewRepository
    {
        List<MailAddress> FindUserEmails(int userId);
        List<CaseEmailSendOverview> SearchWorkingGroupEmails(int customerId, string searchText);
        IList<UserBasicOvierview> SearchUsersEmails(string searchText, int customerId, bool performersOnly, bool usersOnly);
    }
}