namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Faq.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Faq.Output;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using dhHelpdesk_NG.Web.Models.Faq.Output;

    public sealed class NewFaqModelFactory : INewFaqModelFactory
    {
        public NewFaqModel Create(string temporaryId, List<CategoryWithSubcategories> categories, int categoryId, List<WorkingGroupOverview> workingGroups)
        {
            var categoryDropDownItems =
               categories.Select(this.CategoryToDropDownItem).ToList();

            var categoryDropDownContent = new DropDownWithSubmenusContent(
                categoryDropDownItems, categoryId.ToString(CultureInfo.InvariantCulture));

            var workingGroupDropDownItems =
                workingGroups.Select(g => new DropDownItem(g.Name, g.Id.ToString(CultureInfo.InvariantCulture)))
                             .ToList();

            var workingGroupDropDownContent = new DropDownContent(workingGroupDropDownItems);
            return new NewFaqModel(temporaryId, categoryDropDownContent, workingGroupDropDownContent);
        }

        private DropDownWithSubmenusItem CategoryToDropDownItem(CategoryWithSubcategories categoryWithSubcategories)
        {
            var item = new DropDownWithSubmenusItem(
                categoryWithSubcategories.Name, categoryWithSubcategories.Id.ToString(CultureInfo.InvariantCulture));

            if (categoryWithSubcategories.Subcategories.Any())
            {
                var subitems =
                    categoryWithSubcategories.Subcategories.Select(
                        this.CategoryToDropDownItem).ToList();

                item.Subitems.AddRange(subitems);
            }

            return item;
        }
    }
}