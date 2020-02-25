using System.Web.Mvc;

namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Faq.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Models.Faq.Output;

    public sealed class EditFaqModelFactory : IEditFaqModelFactory
    {
        public EditFaqModel Create(
            Faq faq, 
            List<CategoryWithSubcategories> categories, 
            List<string> fileNames, 
            List<ItemOverview> workingGroups,
            bool userHasFaqAdminPermission,
            SelectList languages,
            int languageId,
            bool showDetails,
			List<string> fileUploadWhiteList)
        {
            var categoryDropDownItems = categories.Select(this.CategoryToDropDownItem).ToList();

            var categoryDropDownContent = new DropDownWithSubmenusContent(
                categoryDropDownItems, faq.FaqCategoryId.ToString(CultureInfo.InvariantCulture));

            DropDownContent workingGroupDropDownContent;
            var workingGroupDropDownItems = workingGroups.Select(g => new DropDownItem(g.Name, g.Value)).ToList();

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
                faq.ShowOnStartPage,
                userHasFaqAdminPermission,
                languageId,
                languages,
                showDetails,
				fileUploadWhiteList);
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