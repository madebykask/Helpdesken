namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;

    public interface IEmailGroupEmailRepository
    {
        List<ItemWithEmails> FindEmailGroupsEmails(List<int> emailGroupIds);
    }
}
