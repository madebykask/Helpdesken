namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs;

    public interface IEmailGroupEmailRepository
    {
        List<ItemWithEmails> FindEmailGroupsEmails(List<int> emailGroupIds);
    }
}
