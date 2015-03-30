namespace DH.Helpdesk.Dal.Repositories.ProductArea
{
    using System.Collections.Generic;

    public interface IProductAreaNameResolver
    {
        IEnumerable<string> GetParentPath(int productAreaId, int customerId);
    }
}
