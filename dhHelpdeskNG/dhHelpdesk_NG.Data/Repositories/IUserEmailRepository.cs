using DH.Helpdesk.BusinessData.Models.Case.CaseIntLog;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Net.Mail;

    using DH.Helpdesk.Dal.Dal;

    public interface IUserEmailRepository : INewRepository
    {
        List<MailAddress> FindUserEmails(int userId);
        List<CaseEmailSendOverview> GetEmailsListForIntLogSend(int customerId, string searchText, bool isWgChecked, bool isInitChecked, bool isAdmChecked, bool isEgChecked);
    }
}