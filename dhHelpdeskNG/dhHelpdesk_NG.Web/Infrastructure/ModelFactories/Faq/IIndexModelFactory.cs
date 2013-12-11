namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Faq
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Faq.Output;
    using dhHelpdesk_NG.Web.Models.Faq.Output;

    public interface IIndexModelFactory
    {
        IndexModel Create(List<CategoryWithSubcategories> categories, int selectedCategoryId, List<FaqOverview> firstCategoryFaqs);
    }
}