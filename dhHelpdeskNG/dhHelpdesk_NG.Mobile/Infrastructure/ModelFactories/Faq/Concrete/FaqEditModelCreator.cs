namespace dhHelpdesk_NG.Web.Infrastructure.ModelCreators.Faq.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Faq;
    using dhHelpdesk_NG.Web.Infrastructure.Converters;
    using dhHelpdesk_NG.Web.Infrastructure.InternalModels;
    using dhHelpdesk_NG.Web.Models.Faq;

    public sealed class FaqEditModelCreator : IFaqEditModelCreator
    {
        private readonly ICategoryConverter categoryConverter;

        public FaqEditModelCreator(ICategoryConverter categoryConverter)
        {
            this.categoryConverter = categoryConverter;
        }

        public EditModel Create(Faq faq, List<CategoryWithSubcategories> categories, List<FaqFile> files, List<WorkingGroup> workingGroups)
        {
            var categoriesGroup = new List<CategoriesModel>();
            categoriesGroup.Add(new CategoriesModel(new List<CategoryItemModel>
                                                        {
                                                            new CategoryItemModel(1, "Name 1", 0),
                                                            new CategoryItemModel(2, "Name 2", 0)
                                                        }, 2));

            categoriesGroup.Add(new CategoriesModel(new List<CategoryItemModel>
                                                        {
                                                            new CategoryItemModel(3, "Name 3", 1),
                                                            new CategoryItemModel(4, "Name 4", 2),
                                                            new CategoryItemModel(5, "Name 5", 1),
                                                            new CategoryItemModel(6, "Name 6", 2)
                                                        }, 5));

            var workingGroupDropDownItems =
                workingGroups.Select(g => new DropDownItem(g.Name, g.Id.ToString(CultureInfo.InvariantCulture)))
                             .ToList();

            DropDownSettings dropDownSettings;

            if (faq.WorkingGroup_Id.HasValue)
            {
                dropDownSettings = new DropDownSettings(
                    workingGroupDropDownItems, faq.WorkingGroup_Id.Value.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                dropDownSettings = new DropDownSettings(workingGroupDropDownItems);
            }

            return new EditModel
                       {
                           Id = faq.Id,
                           CategoriesGroup = categoriesGroup,
                           CurrentCategoryId = faq.CategoryId,
                           Question = faq.Question,
                           Answer = faq.Answer,
                           InternalAnswer = faq.InternalAnswer,
                           UrlOne = faq.UrlOne,
                           UrlTwo = faq.UrlTwo,
                           InformationIsAvailableForNotifiers = faq.InformationIsAvailableForNotifiers,
                           ShowOnStartPage = faq.ShowOnStartPage,
                           Files = files.Select(f => new FileModel(f.Id, f.Name)).ToList(),
                           WorkingGroupsDropDownSettings = dropDownSettings
                       };
        }
    }
}