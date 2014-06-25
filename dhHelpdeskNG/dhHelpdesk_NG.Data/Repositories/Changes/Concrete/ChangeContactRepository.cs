namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeContactRepository : Repository, IChangeContactRepository
    {
        public ChangeContactRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void DeleteChangeContacts(int changeId)
        {
            var contacts = this.DbContext.ChangeContacts.Where(c => c.Change_Id == changeId).ToList();
            contacts.ForEach(c => this.DbContext.ChangeContacts.Remove(c));
        }

        public void AddContacts(List<Contact> contacts)
        {
            foreach (var contact in contacts)
            {
                var entity = new ChangeContactEntity
                             {
                                 Change_Id = contact.ChangeId,
                                 ContactCompany = contact.Company,
                                 ContactEMail = contact.Email,
                                 ContactName = contact.Name,
                                 ContactPhone = contact.Phone,
                                 CreatedDate = contact.CreatedDateAndTime,
                             };

                this.DbContext.ChangeContacts.Add(entity);
                this.InitializeAfterCommit(contact, entity);
            }
        }

        public void UpdateContacts(List<Contact> contacts)
        {
            foreach (var contact in contacts)
            {
                var entity = this.DbContext.ChangeContacts.Find(contact.Id);

                entity.ChangedDate = contact.ChangedDateAndTime;
                entity.ContactCompany = contact.Company;
                entity.ContactEMail = contact.Email;
                entity.ContactName = contact.Name;
                entity.ContactPhone = contact.Phone;
            }
        }

        public List<Contact> FindChangeContacts(int changeId)
        {
            return
                this.DbContext.ChangeContacts.Where(c => c.Change_Id == changeId)
                    .Select(c => new { c.Id, c.ContactName, c.ContactPhone, c.ContactEMail, c.ContactCompany })
                    .ToList()
                    .Select(
                        c =>
                            Contact.CreateExisting(
                                c.Id,
                                c.ContactName,
                                c.ContactPhone,
                                c.ContactEMail,
                                c.ContactCompany))
                    .ToList();
        }

        public List<Contact> FindChangeContacts(string phrase)
        {
            return
                this.DbContext.ChangeContacts
                    .Where(c => (c.ContactName != null && c.ContactName.ToLower().Contains(phrase.ToLower()))
                            || (c.ContactPhone != null && c.ContactPhone.ToLower().Contains(phrase.ToLower()))
                            || (c.ContactEMail != null && c.ContactEMail.ToLower().Contains(phrase.ToLower()))
                            || (c.ContactCompany != null && c.ContactCompany.ToLower().Contains(phrase.ToLower())))
                    .Select(c => new { c.Id, c.Change_Id, c.ContactName, c.ContactPhone, c.ContactEMail, c.ContactCompany })
                    .ToList()
                    .Select(
                        c =>
                            Contact.Create(
                                c.Change_Id,
                                c.ContactName,
                                c.ContactPhone,
                                c.ContactEMail,
                                c.ContactCompany))
                    .ToList();            
        }
    }
}