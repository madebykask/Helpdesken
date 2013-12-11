namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Faq
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Faq.Output;
    using dhHelpdesk_NG.Web.Models.Faq.Output;

    public interface IEditFaqModelFactory
    {
        EditFaqModel Create(Faq faq, List<CategoryWithSubcategories> categories, List<string> fileNames, List<WorkingGroupOverview> workingGroups);
    }
}