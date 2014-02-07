namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;

    public interface IEmailGroupEmailRepository
    {
        List<ItemWithEmails> FindEmailGroupsEmails(List<int> emailGroupIds);
    }
}
