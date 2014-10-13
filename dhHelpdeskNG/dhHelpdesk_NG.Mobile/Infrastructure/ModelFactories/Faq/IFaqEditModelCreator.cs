namespace dhHelpdesk_NG.Web.Infrastructure.ModelCreators.Faq
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Faq;
    using dhHelpdesk_NG.Web.Models.Faq;

    public interface IFaqEditModelCreator
    {
        EditModel Create(Faq faq, List<CategoryWithSubcategories> categories, List<FaqFile> files, List<WorkingGroup> workingGroups);
    }
}