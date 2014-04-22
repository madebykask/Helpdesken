namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.Dal.Dal;

    public interface IChangeContactRepository : INewRepository
    {
        void DeleteChangeContacts(int changeId);

        void AddContacts(List<Contact> contacts);

        void UpdateContacts(List<Contact> contacts);

        List<Contact> FindChangeContacts(int changeId);
    }
}