namespace DH.Helpdesk.Dal.Repositories.Servers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.Dal.Dal;

    public interface IServerRepository : INewRepository
    {
        void Add(ServerForInsert businessModel);

        void Update(ServerForUpdate businessModel);

        void DeleteById(int id);

        string FindOperationObjectName(int id);

        ServerForRead FindById(int id);

        void RemoveReferenceOnNic(int id);

        void RemoveReferenceOnRam(int id);

        void RemoveReferenceOnProcessor(int id);

        void RemoveReferenceOnOs(int id);

        int GetServerCount(int customerId);

        List<ReportModel> FindConnectedToServerLocationOverviews(int customerId, string searchFor);
        int GetIdByName(string serverName, int customerId);
    }
}