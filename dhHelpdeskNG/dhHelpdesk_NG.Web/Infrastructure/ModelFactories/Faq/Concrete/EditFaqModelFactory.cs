namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Faq.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Faq.Output;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using dhHelpdesk_NG.Web.Models.Faq.Output;

    public sealed class EditFaqModelFactory : IEditFaqModelFactory
    {
        public EditFaqModel Create(Faq faq, List<CategoryWithSubcategories> categories, List<string> fileNames, List<WorkingGroupOverview> workingGroups)
        {
            var categoryDropDownItems = categories.Select(this.CategoryToDropDownItem).ToList();

            var categoryDropDownContent = new DropDownWithSubmenusContent(
                categoryDropDownItems, faq.FaqCategoryId.ToString(CultureInfo.InvariantCulture));

            DropDownContent workingGroupDropDownContent;

            var workingGroupDropDownItems =
                workingGroups.Select(g => new DropDownItem(g.Name, g.Id.ToString(CultureInfo.InvariantCulture)))
                             .ToList();

            if (faq.WorkingGroupId.HasValue)
            {
                workingGroupDropDownContent = new DropDownContent(
                    workingGroupDropDownItems, faq.WorkingGroupId.Value.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                workingGroupDropDownContent = new DropDownContent(workingGroupDropDownItems);
            }

            return new EditFaqModel(
                faq.Id,
                categoryDropDownContent,
                faq.Question,
                faq.Answer,
                faq.InternalAnswer,
                fileNames,
                faq.UrlOne,
                faq.UrlTwo,
                workingGroupDropDownContent,
                faq.InformationIsAvailableForNotifiers,
                faq.ShowOnStartPage);
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