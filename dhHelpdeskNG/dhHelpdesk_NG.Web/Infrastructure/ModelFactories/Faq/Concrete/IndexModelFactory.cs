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
        public IndexModel Create(List<CategoryWithSubcategories> categories, int? selectedCategoryId, List<FaqOverview> firstCategoryFaqs)
        {
            if (categories == null)
            {
                return new IndexModel(new TreeContent(new List<TreeItem>(), null), new List<FaqOverviewModel>());
            }

            var categoryTreeItems = categories.Select(this.CategoryToTreeItem).ToList();

            var categoriesTreeContent = new TreeContent(
                categoryTreeItems, selectedCategoryId.HasValue ? selectedCategoryId.Value.ToString(CultureInfo.InvariantCulture) : null);

            var firstCategoryFaqModels =
                firstCategoryFaqs.Select(
                    f => new FaqOverviewModel(f.Id, f.CreatedDate.ToString(CultureInfo.InvariantCulture), f.Text))
                                 .ToList();

            return new IndexModel(categoriesTreeContent, firstCategoryFaqModels);
        }

        private TreeItem CategoryToTreeItem(CategoryWithSubcategories categoryWithSubcategories)
        {
            var item = new TreeItem(
                categoryWithSubcategories.Name, categoryWithSubcategories.Id.ToString(CultureInfo.InvariantCulture));

            if (categoryWithSubcategories.Subcategories.Any())
            {
                var subitems =
                    categoryWithSubcategories.Subcategories.Select(
                        this.CategoryToTreeItem).ToList();

                item.Children.AddRange(subitems);
            }

            return item;
        }
    }
}