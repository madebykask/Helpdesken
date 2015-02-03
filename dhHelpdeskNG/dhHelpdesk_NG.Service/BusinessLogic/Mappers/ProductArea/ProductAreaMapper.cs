namespace DH.Helpdesk.Services.BusinessLogic.Mappers.ProductArea
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.ProductArea;

    public static class ProductAreaMapper
    {
        public static void BuildRelations(this IEnumerable<ProductAreaItem> productAreas)
        {
            foreach (var productArea in productAreas)
            {
                productArea.Children.AddRange(productAreas.Where(a => a.ParentId == productArea.Id));
            }
        }
    }
}