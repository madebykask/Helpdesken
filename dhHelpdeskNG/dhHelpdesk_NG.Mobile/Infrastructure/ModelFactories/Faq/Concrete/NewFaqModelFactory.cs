﻿namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Faq.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Mobile.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Mobile.Models.Faq.Output;

    public sealed class NewFaqModelFactory : INewFaqModelFactory
    {
        public NewFaqModel Create(string temporaryId, List<CategoryWithSubcategories> categories, int categoryId, List<ItemOverview> workingGroups)
        {
            var categoryDropDownItems =
               categories.Select(this.CategoryToDropDownItem).ToList();

            var categoryDropDownContent = new DropDownWithSubmenusContent(
                categoryDropDownItems, categoryId.ToString(CultureInfo.InvariantCulture));

            var workingGroupDropDownItems = workingGroups.Select(g => new DropDownItem(g.Name, g.Value)).ToList();
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