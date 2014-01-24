namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.DTO.DTOs.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;

    public sealed class ChangeContactRepository : Repository, IChangeContactRepository
    {
        public ChangeContactRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<Contact> FindByChangeId(int changeId)
        {
            return
                this.DbContext.ChangeContacts.Where(c => c.Change_Id == changeId)
                    .Select(
                        c =>
                            new Contact
                            {
                                Name = c.ContactName,
                                Phone = c.ContactPhone,
                                Email = c.ContactEMail,
                                Company = c.ContactCompany
                            })
                    .ToList();
        }
    }
}
