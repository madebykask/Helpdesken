namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using DH.Helpdesk.Dal.Dal;

    public interface IChangeCouncilRepository : INewRepository
    {
        void DeleteByChangeId(int changeId);
    }
}
