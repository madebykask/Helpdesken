namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.DTO.DTOs.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

    public interface IChangeContactRepository : INewRepository<Contact, Contact>
    {
        List<Contact> FindByChangeId(int changeId);
    }
}
