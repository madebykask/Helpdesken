namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Faq.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Models.Faq.Output;

    public sealed class IndexModelFactory : IIndexModelFactory
    {
        public IndexModel Create(List<CategoryWithSubcategories> categories, 
            int? selectedCategoryId, 
            List<FaqOverview> firstCategoryFaqs,
            int languageId,
            bool userHasFaqAdminPermission)
        {
            if (categories == null)
            {
                return new IndexModel(new TreeContent(new List<TreeItem>(), null), new List<FaqOverviewModel>(), languageId, userHasFaqAdminPermission);
            }

            var categoryTreeItems = categories.Select(CategoryToTreeItem).ToList();

            var categoriesTreeContent = new TreeContent(
                categoryTreeItems, selectedCategoryId?.ToString(CultureInfo.InvariantCulture));

            var firstCategoryFaqModels = 
                firstCategoryFaqs?.Select(f => new FaqOverviewModel(f.Id, f.CreatedDate, f.ChangedDate, f.Text)).ToList();

            return new IndexModel(categoriesTreeContent, firstCategoryFaqModels, languageId, userHasFaqAdminPermission);
        }

        private TreeItem CategoryToTreeItem(CategoryWithSubcategories categoryWithSubcategories)
        {
            var item = new TreeItem(categoryWithSubcategories.Name, categoryWithSubcategories.Id.ToString(CultureInfo.InvariantCulture));

            if (categoryWithSubcategories.Subcategories.Any())
            {
                var subitems =
                    categoryWithSubcategories.Subcategories.Select(CategoryToTreeItem).ToList();

                item.Children.AddRange(subitems);
            }

            return item;
        }
    }
}