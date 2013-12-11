namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Faq
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Faq.Output;
    using dhHelpdesk_NG.Web.Models.Faq.Output;

    public interface INewFaqModelFactory
    {
        NewFaqModel Create(string temporaryId, List<CategoryWithSubcategories> categories, int categoryId, List<WorkingGroupOverview> workingGroups);
    }
}