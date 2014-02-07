namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

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
