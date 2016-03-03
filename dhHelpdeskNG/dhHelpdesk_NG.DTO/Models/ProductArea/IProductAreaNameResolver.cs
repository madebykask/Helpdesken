namespace DH.Helpdesk.BusinessData.Models.ProductArea
{
    using System.Collections.Generic;

    public interface IProductAreaNameResolver
    {
        IEnumerable<string> GetParentPath(int productAreaId, int customerId);
        IEnumerable<string> GetParentPathOnExternalPage(int productAreaId, int customerId, out bool checkShowOnExtenal);
    }
}
