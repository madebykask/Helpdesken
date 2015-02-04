namespace DH.Helpdesk.Services.BusinessLogic.Mappers.ProductArea
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.ProductArea;

    public static class ProductAreaMapper
    {
        public static List<ProductAreaItem> BuildRelations(this IEnumerable<ProductAreaItem> productAreas)
        {
            return productAreas.BuildLineRelations().Where(a => !a.ParentId.HasValue).ToList();
        }

        public static List<ProductAreaItem> BuildLineRelations(this IEnumerable<ProductAreaItem> productAreas)
        {
            foreach (var productArea in productAreas)
            {
                if (productArea.ParentId.HasValue)
                {
                    productArea.Parent = productAreas.FirstOrDefault(a => a.Id == productArea.ParentId);
                }

                productArea.Children.AddRange(productAreas.Where(a => a.ParentId == productArea.Id));
            }

            return productAreas.ToList();
        }
    }
}