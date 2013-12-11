using System.Collections.Generic;
using System.Linq;

namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Faq.Concrete
{
    using System.Globalization;

    using dhHelpdesk_NG.DTO.DTOs.Faq.Output;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using dhHelpdesk_NG.Web.Models.Faq.Output;

    public sealed class IndexModelFactory : IIndexModelFactory
    {
        public IndexModel Create(List<CategoryWithSubcategories> categories, int selectedCategoryId, List<FaqOverview> firstCategoryFaqs)
        {
            var categoryTreeItems = categories.Select(this.CategoryToTreeItem).ToList();

            var categoriesTreeContent = new TreeContent(
                categoryTreeItems, selectedCategoryId.ToString(CultureInfo.InvariantCulture));

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