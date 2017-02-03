using DH.Helpdesk.BusinessData.Models.Case.CaseIntLog;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Net.Mail;

    using DH.Helpdesk.Dal.Dal;

    public interface IUserEmailRepository : INewRepository
    {
        List<MailAddress> FindUserEmails(int userId);
        List<CaseEmailSendOverview> GetUserEmailsListForCaseSend(int customerId, string searchText, bool searchInWorkingGrs, bool searchInInitiators, bool searchInAdmins, bool searchInEmailGrs);
    }
}